using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using Soko.Dao;
using Soko.Exceptions;

namespace Soko.UI
{
    public partial class EntityListForm : Form
    {
        // NOTE: Za tip je izabrana List<object> a ne List<DomainObject> zato sto 
        // u drugom slucaju grid nece da prikaze listu jer tumaci elemente liste da 
        // su tipa DomainObject a on nema atribute.
        protected List<object> entities;

        protected string sortProperty = String.Empty;
        private ListSortDirection sortDirection;
        private Type entityType;
        protected bool allowSorting = true;

        public EntityListForm()
        {
            InitializeComponent();

            // TODO, NOTE: Za forme sa DataGridView kontrolama je korisceno automatsko 
            // skaliranje iz NET 1.1 zato sto 2.0 ne radi dobro (tj. nisam skontao 
            // zasto nece da radi. Greska se javlja kada se skalira na velicine pri
            // kojima prozor postaje veci od ekrana, inace radi dobro. Kada skontas 
            // vrati na 2.0.)
            // Izvrsene su sledece promene: Izbacene su sledece dve linije iz
            // Designer.cs fajlova formi sa DataGrid kontrolama (NET 2.0 nacin)

            // this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            // this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            // i zamenjene sa sledecom linijom (NET 1.1 nacin) koja je dodata samo
            // u EntityListForm zato sto je ostale forme nasledjuju

            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        }

        protected void initialize(Type entityType)
        {
            initUI();
            this.entityType = entityType;
            try
            {
                MapperRegistry.initialize();
                setEntities(loadEntities());
            }
            catch (InfrastructureException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(
                    Strings.getFullDatabaseAccessExceptionMessage(ex.Message), ex);
            }
        }

        protected virtual void initUI()
        {
            Font designFont = Font;
            Font = Options.Instance.Font;

            initalizeGrid();
            // scale columns widths
            // TODO: Mozda bi trebala neka druga mera umesto Font.SizeInPoints, mada 
            // i ovo radi dobro
            float scale = Font.SizeInPoints / designFont.SizeInPoints;
            foreach (DataGridViewColumn col in getDataGridView().Columns)
            {
                col.Width = (int)Math.Ceiling(scale * col.Width);
            }

        }

        protected virtual DataGridView getDataGridView()
        {
            throw new Exception("Derived class should implement this method.");
        }

        private void initalizeGrid()
        {
            DataGridView dgw = getDataGridView();
            dgw.AllowUserToAddRows = false;
            dgw.AllowUserToDeleteRows = false;
            dgw.AutoGenerateColumns = false;
            dgw.ReadOnly = true;
            dgw.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgw.ColumnHeaderMouseClick +=
                new DataGridViewCellMouseEventHandler(dataGridView_ColumnHeaderMouseClick);

            // NOTE: Row height information cannot be stored in DataGridViewCellStyle,
            // so row template is the only way to change the default height used by 
            // all rows.
            // The row template is used only when rows are added. You cannot 
            // change existing rows by changing the row template.
            DataGridViewRow row = getDataGridView().RowTemplate;
            row.Height = (int)(Font.Height * 1.5);
            
            addGridColumns();
        }

        private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            onColumnHeaderClicked(e.ColumnIndex);
        }
        
        protected virtual void addGridColumns()
        {
            throw new Exception("Derived class should implement this method.");
        }

        protected void AddColumn(string columnTitle, string boundPropertyName)
        {
            DataGridViewColumn column = CreateGridColumn(columnTitle, boundPropertyName);
            getDataGridView().Columns.Add(column);
        }

        private DataGridViewColumn CreateGridColumn(string columnTitle, string boundPropertyName)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = boundPropertyName;
            column.HeaderText = columnTitle;
            column.SortMode = DataGridViewColumnSortMode.Programmatic;
            return column;
        }

        protected void AddColumn(string columnTitle, string boundPropertyName, 
            int width)
        {
            DataGridViewColumn column = CreateGridColumn(columnTitle, boundPropertyName);
            if (width == int.MaxValue)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            else
                column.Width = width;
            getDataGridView().Columns.Add(column);
        }

        protected void AddColumn(string columnTitle, string boundPropertyName,
            int width, string format)
        {
            DataGridViewColumn column = CreateGridColumn(columnTitle, boundPropertyName);
            if (width == int.MaxValue)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            else
                column.Width = width;
            getDataGridView().CellFormatting += new DataGridViewCellFormattingEventHandler(
                delegate(object sender, DataGridViewCellFormattingEventArgs e)
                {
                    DataGridView dgw = (DataGridView)sender;
                    if (dgw.Columns[e.ColumnIndex].DataPropertyName == boundPropertyName)
                    {
                        e.Value = string.Format(format, e.Value);
                        e.FormattingApplied = true;
                    }
                });
            getDataGridView().Columns.Add(column);
        }

        protected void AddColumn(string columnTitle, string boundPropertyName, 
            int width, DataGridViewContentAlignment alignment, string format)
        {
            DataGridViewColumn column = CreateGridColumn(columnTitle, boundPropertyName);
            if (width == int.MaxValue)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            else
                column.Width = width;
            column.DefaultCellStyle.Alignment = alignment;
            getDataGridView().CellFormatting += new DataGridViewCellFormattingEventHandler(
                delegate(object sender, DataGridViewCellFormattingEventArgs e)
                {
                    DataGridView dgw = (DataGridView)sender;
                    if (dgw.Columns[e.ColumnIndex].DataPropertyName == boundPropertyName)
                    {
                        e.Value = string.Format(format, e.Value);
                        e.FormattingApplied = true;
                    }
                });
            getDataGridView().Columns.Add(column);
        }

        protected void AddColumn(string columnTitle, 
            string boundPropertyName, DataGridViewContentAlignment alignment)
        {
            DataGridViewColumn column = CreateGridColumn(columnTitle, boundPropertyName);
            column.DefaultCellStyle.Alignment = alignment;
            getDataGridView().Columns.Add(column);
        }

        protected void AddColumn(string columnTitle, string boundPropertyName, 
            int width, DataGridViewContentAlignment alignment)
        {
            DataGridViewColumn column = CreateGridColumn(columnTitle, boundPropertyName);
            if (width == int.MaxValue)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            else
                column.Width = width;
            column.DefaultCellStyle.Alignment = alignment;
            getDataGridView().Columns.Add(column);
        }

        protected void AddColumn(string columnTitle, string boundPropertyName, 
            DataGridViewContentAlignment alignment, string format)
        {
            DataGridViewColumn column = CreateGridColumn(columnTitle, boundPropertyName);
            column.DefaultCellStyle.Alignment = alignment;
            getDataGridView().CellFormatting += new DataGridViewCellFormattingEventHandler(
                delegate(object sender, DataGridViewCellFormattingEventArgs e)
                {
                    DataGridView dgw = (DataGridView)sender;
                    if (dgw.Columns[e.ColumnIndex].DataPropertyName == boundPropertyName)
                    {
                        e.Value = string.Format(format, e.Value);
                        e.FormattingApplied = true;
                    }
                });
            getDataGridView().Columns.Add(column);
        }

        protected virtual List<object> loadEntities()
        {
            throw new Exception("Derived class should implement this method.");
        }

        protected void setEntities(List<object> entities)
        {
            this.entities = entities;
            getDataGridView().DataSource = null;
            getDataGridView().DataSource = entities;
        }

        protected void addCommand()
        {
            MapperRegistry.initialize();  // nova sesija
            try
            {
                EntityDetailForm form = createEntityDetailForm(null);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    onEntityAdded(form.Entity);
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        protected virtual EntityDetailForm createEntityDetailForm(Key entityId)
        {
            throw new Exception("Derived class should override this method.");
        }

        protected virtual void onEntityAdded(DomainObject entity)
        {
            entities.Add(entity);
            if (entities.Count == 1)
            {
                // Lista je bila prazna, sto znaci da mora ponovo da se uradi data
                // binding da bi grid znao tipove objekata
                setEntities(entities);
            }
            if (sortProperty != String.Empty)
                sort(sortProperty);
            else
                refreshView();
            setSelectedEntity(entity);
        }

        protected void refreshView()
        {
            getCurrencyManager().Refresh();
        }

        protected CurrencyManager getCurrencyManager()
        {
            return (CurrencyManager)this.BindingContext[entities];
        }

        protected object getSelectedEntity()
        {
            if (getCurrencyManager().Count == 0)
                return null;
            else
                return getCurrencyManager().Current;
        }

        private void setSelectedEntity(object entity)
        {
            int index = entities.IndexOf(entity);
            if (index >= 0)
            {
                getCurrencyManager().Position = index;
            }
        }

        protected void sort(string propertyName)
        {
            sort(propertyName, false);
        }

        protected void sort(string propertyName, ListSortDirection direction)
        {
            if (!allowSorting)
                return;
            PropertyDescriptor propDesc = TypeDescriptor.GetProperties(entityType)[propertyName];
            if (propDesc == null)
                return;

            doSort(propDesc, direction);
        }

        private void sort(string propertyName, bool toggleDirection)
        {
            if (!allowSorting)
                return;
            PropertyDescriptor propDesc = TypeDescriptor.GetProperties(entityType)[propertyName];
            if (propDesc == null)
                return;

            ListSortDirection direction = ListSortDirection.Ascending;
            if (propertyName == sortProperty)
            {
                direction = sortDirection;
                if (toggleDirection)
                {
                    direction = (sortDirection == ListSortDirection.Ascending) ?
                        ListSortDirection.Descending : ListSortDirection.Ascending;
                }
            }
            doSort(propDesc, direction);
        }

        private void doSort(PropertyDescriptor propDesc, ListSortDirection direction)
        {
            entities.Sort(new SortComparer<object>(propDesc, direction));
            refreshView();

            sortProperty = propDesc.Name;
            sortDirection = direction;
        }

        protected void editCommand()
        {
            MapperRegistry.initialize();  // nova sesija
            DomainObject entity = (DomainObject)getSelectedEntity();
            if (entity == null)
                return;

            try
            {
                EntityDetailForm form = createEntityDetailForm(entity.Key);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    onEntityUpdated(form.Entity);
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        protected virtual void onEntityUpdated(DomainObject entity)
        {
            entities[getSelectedRowIndex()] = entity;
            if (sortProperty != String.Empty)
                sort(sortProperty);
            else
                refreshView();
            setSelectedEntity(entity);
        }

        private int getSelectedRowIndex()
        {
            return getCurrencyManager().Position;
        }

        protected void setSelectedRowIndex(int index)
        {
            getCurrencyManager().Position = index;
        }

        protected void deleteCommand()
        {
            MapperRegistry.initialize();  // nova sesija
            DomainObject entity = (DomainObject)getSelectedEntity();
            if (entity == null)
                return;
            if (!MessageDialogs.queryConfirmation(deleteConfirmationMessage(entity), this.Text))
                return;

            try
            {
                if (refIntegrityDeleteDlg(entity))
                {
                    if (delete(entity))
                    {
                        onEntityDeleted(entity);
                    }
                    else
                    {
                        // Greska zbog konkurentnosti
                        string errMsg = deleteConcurrencyErrorMessage(entity);
                        MessageDialogs.showError(errMsg, this.Text);
                    }
                }
            }
            catch (InfrastructureException ex)
            {
                string errMsg = deleteErrorMessage(entity);
                MessageDialogs.showError(
                    String.Format("{0} \n\n{1}", errMsg, ex.Message),
                    this.Text);
            }
        }

        protected virtual void onEntityDeleted(DomainObject entity)
        {
            int row = getSelectedRowIndex();
            entities.Remove(entity);
            refreshView();
            if (row < entities.Count)
                setSelectedEntity(entities[row]);
            else if (entities.Count > 0)
                setSelectedEntity(entities[entities.Count - 1]);
        }

        protected virtual string deleteConfirmationMessage(DomainObject entity)
        {
            throw new Exception("Derived class should implement this method.");
        }

        protected virtual bool refIntegrityDeleteDlg(DomainObject entity)
        {
            throw new Exception("Derived class should implement this method.");
        }

        protected virtual bool delete(DomainObject entity)
        {
            throw new Exception("Derived class should implement this method.");
        }

        protected virtual string deleteErrorMessage(DomainObject entity)
        {
            throw new Exception("Derived class should implement this method.");
        }

        protected virtual string deleteConcurrencyErrorMessage(DomainObject entity)
        {
            throw new Exception("Derived class should implement this method.");
        }

        private void EntityListForm_Resize(object sender, System.EventArgs e)
        {
            Size monitorSize = SystemInformation.PrimaryMonitorMaximizedWindowSize;
            this.Size = new Size(Math.Min(this.Size.Width, monitorSize.Width),
                                 Math.Min(this.Size.Height, monitorSize.Height));
        }

        private void onColumnHeaderClicked(int col)
        {
            string propertyName = getColumnProperty(col);
            if (propertyName == String.Empty)
                return;
            sort(propertyName, true);
        }

        private string getColumnProperty(int colIndex)
        {
            return getDataGridView().Columns[colIndex].DataPropertyName;
        }
    }
}