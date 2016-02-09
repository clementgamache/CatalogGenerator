namespace CatalogGenerator
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelMoulding = new System.Windows.Forms.Label();
            this.labelHeight = new System.Windows.Forms.Label();
            this.labelWidth = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxMoulding = new System.Windows.Forms.TextBox();
            this.textBoxWidth = new System.Windows.Forms.TextBox();
            this.buttonFolder = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(37, 269);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(403, 61);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.labelMoulding, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelHeight, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelWidth, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxTitle, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxHeight, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxMoulding, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxWidth, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonFolder, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(37, 54);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(406, 209);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // labelMoulding
            // 
            this.labelMoulding.AutoSize = true;
            this.labelMoulding.Location = new System.Drawing.Point(3, 123);
            this.labelMoulding.Name = "labelMoulding";
            this.labelMoulding.Size = new System.Drawing.Size(144, 20);
            this.labelMoulding.TabIndex = 6;
            this.labelMoulding.Text = "Moulding Width (in)";
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(3, 82);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(129, 20);
            this.labelHeight.TabIndex = 5;
            this.labelHeight.Text = "Inside Height (in)";
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(3, 41);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(123, 20);
            this.labelWidth.TabIndex = 4;
            this.labelWidth.Text = "Inside Width (in)";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(206, 3);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(197, 26);
            this.textBoxTitle.TabIndex = 0;
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(206, 85);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(197, 26);
            this.textBoxHeight.TabIndex = 2;
            this.textBoxHeight.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Catalog Title";
            // 
            // textBoxMoulding
            // 
            this.textBoxMoulding.Location = new System.Drawing.Point(206, 126);
            this.textBoxMoulding.Name = "textBoxMoulding";
            this.textBoxMoulding.Size = new System.Drawing.Size(197, 26);
            this.textBoxMoulding.TabIndex = 7;
            this.textBoxMoulding.Text = "0";
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.Location = new System.Drawing.Point(206, 44);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(197, 26);
            this.textBoxWidth.TabIndex = 1;
            this.textBoxWidth.Text = "0";
            // 
            // buttonFolder
            // 
            this.buttonFolder.Location = new System.Drawing.Point(3, 167);
            this.buttonFolder.Name = "buttonFolder";
            this.buttonFolder.Size = new System.Drawing.Size(197, 39);
            this.buttonFolder.TabIndex = 8;
            this.buttonFolder.Text = "Select Image Folder";
            this.buttonFolder.UseVisualStyleBackColor = true;
            this.buttonFolder.Click += new System.EventHandler(this.buttonFolder_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(206, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "No folder chosen...";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 342);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Catalog Generator";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelMoulding;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMoulding;
        private System.Windows.Forms.TextBox textBoxWidth;
        private System.Windows.Forms.Button buttonFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

