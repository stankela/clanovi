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
    public partial class EntityDetailForm : Form
    {
        protected DomainObject entity;
        protected bool editMode;
        private bool persistEntity;
        private bool closedByOK;
        private bool closedByCancel;

        public DomainObject Entity
        {
            get { return entity; }
        }

        public EntityDetailForm()
        {
            InitializeComponent();
        }

        protected void initialize(Key entityId, bool persistEntity)
        {
            this.persistEntity = persistEntity;
            MapperRegistry.initialize();
            try
            {
                if (entityId != null)
                {
                    editMode = true;
                    initUpdateMode(entityId);
                }
                else
                {
                    editMode = false;
                    initAddMode();
                }
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(
                    Strings.getFullDatabaseAccessExceptionMessage(ex.Message), ex);
            }
        }

        protected virtual void initAddMode()
        {
            entity = createNewEntity();
            loadData();
            initUI();
        }

        protected virtual DomainObject createNewEntity()
        {
            throw new Exception("Derived class should implement this method.");
        }

        protected virtual void loadData()
        {
            // Empty
        }

        protected virtual void initUI()
        {
            Font = Options.Instance.Font;
        }

        protected virtual void initUpdateMode(Key entityId)
        {
            // Najpre se ucitava objekt, zatim ostali podaci potrebni za UI,
            // i tek nakon toga se inicijalizuje UI. Razlog za ovakav redosled je
            // slucaj kada neko svojstvo objekta utice na UI (npr. koje ce
            // opcije prisutne u combo box-u.)
            entity = getEntityById(entityId);
            saveOriginalData(entity);
            loadData();
            initUI();
            updateUIFromEntity(entity);
        }

        protected virtual DomainObject getEntityById(Key id)
        {
            throw new Exception("Derived class should implement this method.");
        }

        protected virtual void saveOriginalData(DomainObject entity)
        {
            // Empty
        }

        protected virtual void updateUIFromEntity(DomainObject entity)
        {
            throw new Exception("Derived class should implement this method.");
        }

        protected void handleOkClick()
        {
            try
            {
                Notification notification = new Notification();
                requiredFieldsAndFormatValidation(notification);
                if (!notification.IsValid())
                    throw new BusinessException(notification);

                if (!beforePersistDlg(entity))
                {
                    this.DialogResult = DialogResult.Cancel;
                    closedByCancel = true;
                    return;
                }

                bool success;
                if (editMode)
                    success = update();
                else
                    success = add();

                if (!success)
                {
                    string errMsg;
                    if (editMode)
                        errMsg = "Neuspesna promena mesta.";
                    else
                        errMsg = "Neuspesno dodavanje mesta.";
                    MessageDialogs.showMessage(errMsg, this.Text);
                    this.DialogResult = DialogResult.Cancel;
                    closedByCancel = true;
                }
                else
                    closedByOK = true;
            }
            catch (InvalidPropertyException ex)
            {
                MessageDialogs.showMessage(ex.Message, this.Text);
                setFocus(ex.InvalidProperty);
                this.DialogResult = DialogResult.None;
            }
            catch (BusinessException ex)
            {
                if (ex.Notification != null)
                {
                    NotificationMessage msg = ex.Notification.FirstMessage;
                    MessageDialogs.showMessage(msg.Message, this.Text);
                    setFocus(msg.FieldName);
                }
                else
                {
                    MessageDialogs.showMessage(ex.Message, this.Text);
                }
                this.DialogResult = DialogResult.None;
            }
            catch (InfrastructureException ex)
            {
                //discardChanges();
                MessageDialogs.showMessage(ex.Message, this.Text);
                this.DialogResult = DialogResult.Cancel;
                closedByCancel = true;
            }
            catch (Exception ex)
            {
                //discardChanges();
                MessageDialogs.showMessage(
                    Strings.getFullDatabaseAccessExceptionMessage(ex.Message), this.Text);
                this.DialogResult = DialogResult.Cancel;
                closedByCancel = true;
            }
        }

        protected virtual void requiredFieldsAndFormatValidation(Notification notification)
        {
            // Empty
        }

        protected virtual void setFocus(string propertyName)
        {
            // Empty
        }

        private bool add()
        {
            updateEntityFromUI(entity);
            validateEntity(entity);
            checkBusinessRulesOnAdd(entity);
            if (persistEntity)
                return insertEntity(entity);

            return true;
        }

        protected virtual void updateEntityFromUI(DomainObject entity)
        {
            throw new Exception("Derived class should implement this method.");
        }

        private void validateEntity(DomainObject entity)
        {
            Notification notification = new Notification();
            entity.validate(notification);
            if (!notification.IsValid())
                throw new BusinessException(notification);
        }

        protected virtual bool beforePersistDlg(DomainObject entity)
        {
            return true;
        }

        protected virtual void checkBusinessRulesOnAdd(DomainObject entity)
        {
            // Empty
        }

        protected virtual bool insertEntity(DomainObject entity)
        {
            throw new Exception("Derived class should implement this method.");
        }

        private bool update()
        {
            updateEntityFromUI(entity);
            validateEntity(entity);
            checkBusinessRulesOnUpdate(entity);
            if (persistEntity)
                return updateEntity(entity);

            return true;
        }

        protected virtual void checkBusinessRulesOnUpdate(DomainObject entity)
        {
            // Empty
        }

        protected virtual bool updateEntity(DomainObject entity)
        {
            throw new Exception("Derived class should implement this method.");
        }

        protected virtual void discardChanges()
        {
            // Empty
        }

        protected void handleCancelClick()
        {
            discardChanges();
            closedByCancel = true;
        }

        private bool isDirty()
        {
            // TODO
            return true;
        }

        private void EntityDetailForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closedByOK && !closedByCancel)
            {
                // zatvoreno pomocu X
                if (isDirty())
                {
                    bool canClose = MessageBox.Show(
                        "Izmene koje ste uneli nece biti sacuvane?", "Klub",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) ==
                        DialogResult.OK;
                    e.Cancel = !canClose;
                }
            }
        }

        private void EntityDetailForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!closedByOK && !closedByCancel)
            {
                discardChanges();
            }
        }
    }
}