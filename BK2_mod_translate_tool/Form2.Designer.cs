namespace BK2_mod_translate_tool
{
    partial class Form2
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
            OKButton = new Button();
            CancelButton = new Button();
            textBox1 = new TextBox();
            FilesIndexesList = new ListView();
            label1 = new Label();
            ClearResultsButton = new Button();
            SuspendLayout();
            // 
            // OKButton
            // 
            OKButton.Location = new Point(410, 402);
            OKButton.Name = "OKButton";
            OKButton.Size = new Size(134, 36);
            OKButton.TabIndex = 0;
            OKButton.Text = "OK";
            OKButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            CancelButton.Location = new Point(270, 402);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(134, 36);
            CancelButton.TabIndex = 1;
            CancelButton.Text = "Cancel";
            CancelButton.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(90, 19);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(587, 23);
            textBox1.TabIndex = 5;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // FilesIndexesList
            // 
            FilesIndexesList.Location = new Point(12, 52);
            FilesIndexesList.Name = "FilesIndexesList";
            FilesIndexesList.Size = new Size(776, 344);
            FilesIndexesList.TabIndex = 6;
            FilesIndexesList.UseCompatibleStateImageBehavior = false;
            FilesIndexesList.View = View.Details;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 22);
            label1.Name = "label1";
            label1.Size = new Size(72, 15);
            label1.TabIndex = 7;
            label1.Text = "Search path:";
            // 
            // ClearResultsButton
            // 
            ClearResultsButton.Location = new Point(683, 19);
            ClearResultsButton.Name = "ClearResultsButton";
            ClearResultsButton.Size = new Size(105, 23);
            ClearResultsButton.TabIndex = 8;
            ClearResultsButton.Text = "Clear Results";
            ClearResultsButton.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ClearResultsButton);
            Controls.Add(label1);
            Controls.Add(FilesIndexesList);
            Controls.Add(textBox1);
            Controls.Add(CancelButton);
            Controls.Add(OKButton);
            Name = "Form2";
            Text = "Pick file index";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button OKButton;
        private Button CancelButton;
        private TextBox textBox1;
        private ListView FilesIndexesList;
        private Label label1;
        private Button ClearResultsButton;
    }
}