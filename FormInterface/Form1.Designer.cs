namespace FormInterface
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.Output = new System.Windows.Forms.RichTextBox();
            this.Input = new System.Windows.Forms.TextBox();
            this.VariablesList = new System.Windows.Forms.ListBox();
            this.Run = new System.Windows.Forms.Button();
            this.FunctionsList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CleanButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Output
            // 
            this.Output.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.Output.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Output.Font = new System.Drawing.Font("Cascadia Code", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Output.ForeColor = System.Drawing.Color.SpringGreen;
            this.Output.Location = new System.Drawing.Point(15, 15);
            this.Output.Name = "Output";
            this.Output.ReadOnly = true;
            this.Output.Size = new System.Drawing.Size(700, 400);
            this.Output.TabIndex = 0;
            this.Output.Text = "";
            // 
            // Input
            // 
            this.Input.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.Input.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Input.Font = new System.Drawing.Font("Cascadia Code", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Input.ForeColor = System.Drawing.Color.SpringGreen;
            this.Input.Location = new System.Drawing.Point(15, 435);
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(600, 27);
            this.Input.TabIndex = 1;
            this.Input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Input_KeyDown);
            // 
            // VariablesList
            // 
            this.VariablesList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.VariablesList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VariablesList.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.VariablesList.ForeColor = System.Drawing.Color.SpringGreen;
            this.VariablesList.FormattingEnabled = true;
            this.VariablesList.ItemHeight = 21;
            this.VariablesList.Location = new System.Drawing.Point(730, 38);
            this.VariablesList.Name = "VariablesList";
            this.VariablesList.Size = new System.Drawing.Size(240, 170);
            this.VariablesList.TabIndex = 2;
            // 
            // Run
            // 
            this.Run.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.Run.FlatAppearance.BorderSize = 0;
            this.Run.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Run.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.Run.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Run.ForeColor = System.Drawing.Color.SpringGreen;
            this.Run.Location = new System.Drawing.Point(621, 435);
            this.Run.Name = "Run";
            this.Run.Size = new System.Drawing.Size(94, 27);
            this.Run.TabIndex = 3;
            this.Run.Text = "|> Run Line";
            this.Run.UseVisualStyleBackColor = false;
            this.Run.Click += new System.EventHandler(this.Run_Click);
            // 
            // FunctionsList
            // 
            this.FunctionsList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.FunctionsList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FunctionsList.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FunctionsList.ForeColor = System.Drawing.Color.SpringGreen;
            this.FunctionsList.FormattingEnabled = true;
            this.FunctionsList.ItemHeight = 21;
            this.FunctionsList.Location = new System.Drawing.Point(730, 253);
            this.FunctionsList.Name = "FunctionsList";
            this.FunctionsList.Size = new System.Drawing.Size(240, 149);
            this.FunctionsList.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.SpringGreen;
            this.label1.Location = new System.Drawing.Point(730, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "Variables";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.SpringGreen;
            this.label2.Location = new System.Drawing.Point(730, 230);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 21);
            this.label2.TabIndex = 6;
            this.label2.Text = "Functions";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CleanButton
            // 
            this.CleanButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.CleanButton.FlatAppearance.BorderSize = 0;
            this.CleanButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.CleanButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.CleanButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CleanButton.ForeColor = System.Drawing.Color.SpringGreen;
            this.CleanButton.Location = new System.Drawing.Point(730, 435);
            this.CleanButton.Name = "CleanButton";
            this.CleanButton.Size = new System.Drawing.Size(94, 27);
            this.CleanButton.TabIndex = 7;
            this.CleanButton.Text = "Clean All";
            this.CleanButton.UseVisualStyleBackColor = false;
            this.CleanButton.Click += new System.EventHandler(this.CleanButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(982, 483);
            this.Controls.Add(this.CleanButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FunctionsList);
            this.Controls.Add(this.Run);
            this.Controls.Add(this.VariablesList);
            this.Controls.Add(this.Input);
            this.Controls.Add(this.Output);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "H.U.L.K Interface";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RichTextBox Output;
        private TextBox Input;
        private ListBox VariablesList;
        private Button Run;
        private ListBox FunctionsList;
        private Label label1;
        private Label label2;
        private Button CleanButton;
    }
}