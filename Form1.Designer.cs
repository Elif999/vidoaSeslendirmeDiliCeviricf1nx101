namespace WindowsFormsApp3
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.lblDurum = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTranskript = new System.Windows.Forms.RichTextBox();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.btnCalistir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "YT url";
            // 
            // lblDurum
            // 
            this.lblDurum.AutoSize = true;
            this.lblDurum.Location = new System.Drawing.Point(97, 171);
            this.lblDurum.Name = "lblDurum";
            this.lblDurum.Size = new System.Drawing.Size(16, 13);
            this.lblDurum.TabIndex = 11;
            this.lblDurum.Text = "...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 173);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "durum:";
            // 
            // txtTranskript
            // 
            this.txtTranskript.Location = new System.Drawing.Point(58, 189);
            this.txtTranskript.Name = "txtTranskript";
            this.txtTranskript.Size = new System.Drawing.Size(328, 143);
            this.txtTranskript.TabIndex = 9;
            this.txtTranskript.Text = "";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(100, 119);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(646, 20);
            this.txtURL.TabIndex = 8;
            // 
            // btnCalistir
            // 
            this.btnCalistir.Location = new System.Drawing.Point(100, 145);
            this.btnCalistir.Name = "btnCalistir";
            this.btnCalistir.Size = new System.Drawing.Size(75, 23);
            this.btnCalistir.TabIndex = 7;
            this.btnCalistir.Text = "çalıştır";
            this.btnCalistir.UseVisualStyleBackColor = true;
            this.btnCalistir.Click += new System.EventHandler(this.btnCalistir_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblDurum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTranskript);
            this.Controls.Add(this.txtURL);
            this.Controls.Add(this.btnCalistir);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDurum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox txtTranskript;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Button btnCalistir;
    }
}

