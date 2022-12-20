namespace Compilers
{
    partial class Form1 :Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.outputFile = new System.Windows.Forms.RichTextBox();
            this.inputFile = new System.Windows.Forms.RichTextBox();
            this.Btn_Browse = new System.Windows.Forms.Button();
            this.Btn_Scan = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Tokens = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Black;
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Location = new System.Drawing.Point(539, 84);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 48);
            this.button1.TabIndex = 1;
            this.button1.Text = "Parse";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // outputFile
            // 
            this.outputFile.Location = new System.Drawing.Point(667, 84);
            this.outputFile.Margin = new System.Windows.Forms.Padding(4);
            this.outputFile.Name = "outputFile";
            this.outputFile.Size = new System.Drawing.Size(398, 426);
            this.outputFile.TabIndex = 7;
            this.outputFile.Text = "";
            this.outputFile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.outputFile_KeyPress);
            // 
            // inputFile
            // 
            this.inputFile.Location = new System.Drawing.Point(114, 84);
            this.inputFile.Margin = new System.Windows.Forms.Padding(4);
            this.inputFile.Name = "inputFile";
            this.inputFile.Size = new System.Drawing.Size(398, 426);
            this.inputFile.TabIndex = 6;
            this.inputFile.Text = "";
            this.inputFile.TextChanged += new System.EventHandler(this.inputFile_TextChanged);
            this.inputFile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputFile_KeyPress_1);
            // 
            // Btn_Browse
            // 
            this.Btn_Browse.BackColor = System.Drawing.Color.Black;
            this.Btn_Browse.ForeColor = System.Drawing.Color.White;
            this.Btn_Browse.Location = new System.Drawing.Point(539, 217);
            this.Btn_Browse.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_Browse.Name = "Btn_Browse";
            this.Btn_Browse.Size = new System.Drawing.Size(100, 48);
            this.Btn_Browse.TabIndex = 5;
            this.Btn_Browse.Text = "Browse Code";
            this.Btn_Browse.UseVisualStyleBackColor = false;
            this.Btn_Browse.Click += new System.EventHandler(this.Btn_Browse_Click);
            // 
            // Btn_Scan
            // 
            this.Btn_Scan.BackColor = System.Drawing.Color.Black;
            this.Btn_Scan.ForeColor = System.Drawing.Color.White;
            this.Btn_Scan.Location = new System.Drawing.Point(539, 462);
            this.Btn_Scan.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_Scan.Name = "Btn_Scan";
            this.Btn_Scan.Size = new System.Drawing.Size(100, 48);
            this.Btn_Scan.TabIndex = 4;
            this.Btn_Scan.Text = "Scan";
            this.Btn_Scan.UseVisualStyleBackColor = false;
            this.Btn_Scan.Click += new System.EventHandler(this.Btn_Scan_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(316, 544);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 16);
            this.label1.TabIndex = 8;
            // 
            // btn_Tokens
            // 
            this.btn_Tokens.BackColor = System.Drawing.Color.Black;
            this.btn_Tokens.ForeColor = System.Drawing.Color.White;
            this.btn_Tokens.Location = new System.Drawing.Point(539, 338);
            this.btn_Tokens.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Tokens.Name = "btn_Tokens";
            this.btn_Tokens.Size = new System.Drawing.Size(100, 48);
            this.btn_Tokens.TabIndex = 9;
            this.btn_Tokens.Text = "Browse Tokens";
            this.btn_Tokens.UseVisualStyleBackColor = false;
            this.btn_Tokens.Click += new System.EventHandler(this.btn_Tokens_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AntiqueWhite;
            this.ClientSize = new System.Drawing.Size(1178, 594);
            this.Controls.Add(this.btn_Tokens);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outputFile);
            this.Controls.Add(this.inputFile);
            this.Controls.Add(this.Btn_Browse);
            this.Controls.Add(this.Btn_Scan);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Tiny Compiler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button button1;
        private RichTextBox outputFile;
        private RichTextBox inputFile;
        private Button Btn_Browse;
        private Button Btn_Scan;
        private Label label1;
        private Button btn_Tokens;
    }
}