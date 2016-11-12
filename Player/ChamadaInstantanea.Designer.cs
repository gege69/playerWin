namespace Player
{
    partial class ChamadaInstantanea
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
            this.listBoxChamadasInstant = new System.Windows.Forms.ListBox();
            this.btnFechar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxChamadasInstant
            // 
            this.listBoxChamadasInstant.FormattingEnabled = true;
            this.listBoxChamadasInstant.Items.AddRange(new object[] {
            "Vazio"});
            this.listBoxChamadasInstant.Location = new System.Drawing.Point(12, 12);
            this.listBoxChamadasInstant.Name = "listBoxChamadasInstant";
            this.listBoxChamadasInstant.Size = new System.Drawing.Size(596, 290);
            this.listBoxChamadasInstant.TabIndex = 0;
            // 
            // btnFechar
            // 
            this.btnFechar.Location = new System.Drawing.Point(533, 321);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(75, 23);
            this.btnFechar.TabIndex = 1;
            this.btnFechar.Text = "Fechar";
            this.btnFechar.UseVisualStyleBackColor = true;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // ChamadaInstantanea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(620, 358);
            this.Controls.Add(this.btnFechar);
            this.Controls.Add(this.listBoxChamadasInstant);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChamadaInstantanea";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chamada Instantânea";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxChamadasInstant;
        private System.Windows.Forms.Button btnFechar;
    }
}