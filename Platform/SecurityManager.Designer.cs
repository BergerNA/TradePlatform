namespace Platform
{
    partial class SecurityManager
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
            this.listClass = new System.Windows.Forms.ListBox();
            this.listName = new System.Windows.Forms.ListBox();
            this.listSelect = new System.Windows.Forms.ListBox();
            this.lbSec = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.lbClass = new System.Windows.Forms.Label();
            this.lbSecName = new System.Windows.Forms.Label();
            this.lbSelect = new System.Windows.Forms.Label();
            this.btAdd = new System.Windows.Forms.Button();
            this.btDel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listClass
            // 
            this.listClass.BackColor = System.Drawing.Color.Black;
            this.listClass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listClass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listClass.ForeColor = System.Drawing.Color.White;
            this.listClass.FormattingEnabled = true;
            this.listClass.ItemHeight = 16;
            this.listClass.Location = new System.Drawing.Point(1, 47);
            this.listClass.Name = "listClass";
            this.listClass.Size = new System.Drawing.Size(164, 386);
            this.listClass.Sorted = true;
            this.listClass.TabIndex = 1;
            this.listClass.SelectedIndexChanged += new System.EventHandler(this.listClass_SelectedIndexChanged);
            // 
            // listName
            // 
            this.listName.BackColor = System.Drawing.Color.Black;
            this.listName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listName.ForeColor = System.Drawing.Color.White;
            this.listName.FormattingEnabled = true;
            this.listName.ItemHeight = 16;
            this.listName.Location = new System.Drawing.Point(171, 47);
            this.listName.Name = "listName";
            this.listName.Size = new System.Drawing.Size(210, 386);
            this.listName.Sorted = true;
            this.listName.TabIndex = 2;
            this.listName.SelectedIndexChanged += new System.EventHandler(this.listName_SelectedIndexChanged);
            // 
            // listSelect
            // 
            this.listSelect.BackColor = System.Drawing.Color.Black;
            this.listSelect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listSelect.ForeColor = System.Drawing.Color.White;
            this.listSelect.FormattingEnabled = true;
            this.listSelect.ItemHeight = 16;
            this.listSelect.Location = new System.Drawing.Point(413, 47);
            this.listSelect.Name = "listSelect";
            this.listSelect.Size = new System.Drawing.Size(210, 338);
            this.listSelect.TabIndex = 3;
            this.listSelect.SelectedIndexChanged += new System.EventHandler(this.listSelect_SelectedIndexChanged);
            // 
            // lbSec
            // 
            this.lbSec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSec.BackColor = System.Drawing.Color.Red;
            this.lbSec.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbSec.ForeColor = System.Drawing.Color.White;
            this.lbSec.Location = new System.Drawing.Point(0, 0);
            this.lbSec.Name = "lbSec";
            this.lbSec.Size = new System.Drawing.Size(625, 23);
            this.lbSec.TabIndex = 4;
            this.lbSec.Text = "Security Manager";
            this.lbSec.Paint += new System.Windows.Forms.PaintEventHandler(this.lbSec_Paint);
            // 
            // lbName
            // 
            this.lbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbName.BackColor = System.Drawing.Color.DarkOrange;
            this.lbName.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbName.ForeColor = System.Drawing.Color.White;
            this.lbName.Location = new System.Drawing.Point(0, 23);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(625, 23);
            this.lbName.TabIndex = 5;
            // 
            // lbClass
            // 
            this.lbClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbClass.AutoSize = true;
            this.lbClass.BackColor = System.Drawing.Color.DarkOrange;
            this.lbClass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbClass.Location = new System.Drawing.Point(1, 26);
            this.lbClass.Name = "lbClass";
            this.lbClass.Size = new System.Drawing.Size(46, 16);
            this.lbClass.TabIndex = 6;
            this.lbClass.Text = "Класс";
            // 
            // lbSecName
            // 
            this.lbSecName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSecName.AutoSize = true;
            this.lbSecName.BackColor = System.Drawing.Color.DarkOrange;
            this.lbSecName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbSecName.Location = new System.Drawing.Point(168, 26);
            this.lbSecName.Name = "lbSecName";
            this.lbSecName.Size = new System.Drawing.Size(56, 16);
            this.lbSecName.TabIndex = 7;
            this.lbSecName.Text = "Бумага";
            // 
            // lbSelect
            // 
            this.lbSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSelect.AutoSize = true;
            this.lbSelect.BackColor = System.Drawing.Color.DarkOrange;
            this.lbSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbSelect.Location = new System.Drawing.Point(410, 26);
            this.lbSelect.Name = "lbSelect";
            this.lbSelect.Size = new System.Drawing.Size(133, 16);
            this.lbSelect.TabIndex = 8;
            this.lbSelect.Text = "Выбранные бумаги";
            // 
            // btAdd
            // 
            this.btAdd.Location = new System.Drawing.Point(388, 168);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(19, 23);
            this.btAdd.TabIndex = 9;
            this.btAdd.Text = "A";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // btDel
            // 
            this.btDel.Location = new System.Drawing.Point(387, 220);
            this.btDel.Name = "btDel";
            this.btDel.Size = new System.Drawing.Size(19, 23);
            this.btDel.TabIndex = 10;
            this.btDel.Text = "D";
            this.btDel.UseVisualStyleBackColor = true;
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(478, 406);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 11;
            this.btOk.Text = "Ok";
            this.btOk.UseVisualStyleBackColor = true;
            // 
            // SecurityManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.btDel);
            this.Controls.Add(this.btAdd);
            this.Controls.Add(this.lbSelect);
            this.Controls.Add(this.lbSecName);
            this.Controls.Add(this.lbClass);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.lbSec);
            this.Controls.Add(this.listSelect);
            this.Controls.Add(this.listName);
            this.Controls.Add(this.listClass);
            this.Name = "SecurityManager";
            this.Text = "Security Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listClass;
        private System.Windows.Forms.ListBox listName;
        private System.Windows.Forms.ListBox listSelect;
        private System.Windows.Forms.Label lbSec;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbClass;
        private System.Windows.Forms.Label lbSecName;
        private System.Windows.Forms.Label lbSelect;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btDel;
        private System.Windows.Forms.Button btOk;
    }
}