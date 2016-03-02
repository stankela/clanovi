namespace Soko.UI
{
    partial class DuplikatiClanovaForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnMergeSelected = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Location = new System.Drawing.Point(25, 23);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(712, 372);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnMergeSelected);
            this.panel1.Location = new System.Drawing.Point(25, 401);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(712, 109);
            this.panel1.TabIndex = 4;
            // 
            // btnMergeSelected
            // 
            this.btnMergeSelected.Location = new System.Drawing.Point(17, 16);
            this.btnMergeSelected.Name = "btnMergeSelected";
            this.btnMergeSelected.Size = new System.Drawing.Size(100, 23);
            this.btnMergeSelected.TabIndex = 2;
            this.btnMergeSelected.Text = "Merge selected";
            this.btnMergeSelected.UseVisualStyleBackColor = true;
            this.btnMergeSelected.Click += new System.EventHandler(this.btnMergeSelected_Click);
            // 
            // DuplikatiClanovaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 522);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listView1);
            this.Name = "DuplikatiClanovaForm";
            this.Text = "DuplikatiClanovaForm";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnMergeSelected;
    }
}