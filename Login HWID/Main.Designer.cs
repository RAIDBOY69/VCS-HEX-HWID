
namespace Login_HWID
{
    partial class Main
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
            this.LoginBTN = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // LoginBTN
            // 
            this.LoginBTN.BorderRadius = 10;
            this.LoginBTN.CheckedState.Parent = this.LoginBTN;
            this.LoginBTN.CustomImages.Parent = this.LoginBTN;
            this.LoginBTN.FillColor = System.Drawing.Color.Red;
            this.LoginBTN.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.LoginBTN.ForeColor = System.Drawing.Color.White;
            this.LoginBTN.HoverState.Parent = this.LoginBTN;
            this.LoginBTN.Location = new System.Drawing.Point(-6, -4);
            this.LoginBTN.Name = "LoginBTN";
            this.LoginBTN.ShadowDecoration.Parent = this.LoginBTN;
            this.LoginBTN.Size = new System.Drawing.Size(402, 195);
            this.LoginBTN.TabIndex = 5;
            this.LoginBTN.Text = "ERROR CONTACT ADMIN @ezvocabsize";
            this.LoginBTN.Click += new System.EventHandler(this.LoginBTN_Click);
            // 
            // Main
            // 
            this.ClientSize = new System.Drawing.Size(395, 189);
            this.Controls.Add(this.LoginBTN);
            this.MaximumSize = new System.Drawing.Size(411, 228);
            this.MinimumSize = new System.Drawing.Size(411, 228);
            this.Name = "Main";
            this.Text = "Error";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }


        #endregion
        private Guna.UI2.WinForms.Guna2Button LoginBTN;
    }
}