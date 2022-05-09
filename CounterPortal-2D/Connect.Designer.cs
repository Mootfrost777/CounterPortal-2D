namespace CounterPortal_2D
{
    partial class Connect
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
            this.NameTB = new System.Windows.Forms.TextBox();
            this.IPTB = new System.Windows.Forms.TextBox();
            this.PortTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PlayBtn = new System.Windows.Forms.Button();
            this.ExitBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NameTB
            // 
            this.NameTB.Location = new System.Drawing.Point(10, 24);
            this.NameTB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NameTB.Name = "NameTB";
            this.NameTB.Size = new System.Drawing.Size(196, 23);
            this.NameTB.TabIndex = 0;
            this.NameTB.Text = "test";
            // 
            // IPTB
            // 
            this.IPTB.Location = new System.Drawing.Point(10, 64);
            this.IPTB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.IPTB.Name = "IPTB";
            this.IPTB.Size = new System.Drawing.Size(196, 23);
            this.IPTB.TabIndex = 1;
            this.IPTB.Text = "127.0.0.1";
            // 
            // PortTB
            // 
            this.PortTB.Location = new System.Drawing.Point(10, 104);
            this.PortTB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PortTB.Name = "PortTB";
            this.PortTB.Size = new System.Drawing.Size(196, 23);
            this.PortTB.TabIndex = 2;
            this.PortTB.Text = "7632";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "IP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Port:";
            // 
            // PlayBtn
            // 
            this.PlayBtn.Location = new System.Drawing.Point(10, 126);
            this.PlayBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PlayBtn.Name = "PlayBtn";
            this.PlayBtn.Size = new System.Drawing.Size(96, 22);
            this.PlayBtn.TabIndex = 6;
            this.PlayBtn.Text = "Play";
            this.PlayBtn.UseVisualStyleBackColor = true;
            this.PlayBtn.Click += new System.EventHandler(this.PlayBtn_Click);
            // 
            // ExitBtn
            // 
            this.ExitBtn.Location = new System.Drawing.Point(112, 128);
            this.ExitBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(94, 22);
            this.ExitBtn.TabIndex = 7;
            this.ExitBtn.Text = "Exit";
            this.ExitBtn.UseVisualStyleBackColor = true;
            // 
            // Connect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 159);
            this.Controls.Add(this.ExitBtn);
            this.Controls.Add(this.PlayBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PortTB);
            this.Controls.Add(this.IPTB);
            this.Controls.Add(this.NameTB);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Connect";
            this.Text = "Launcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NameTB;
        private System.Windows.Forms.TextBox IPTB;
        private System.Windows.Forms.TextBox PortTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button PlayBtn;
        private System.Windows.Forms.Button ExitBtn;
    }
}