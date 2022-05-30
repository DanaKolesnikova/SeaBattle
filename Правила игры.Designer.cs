
namespace Морской_бой
{
    partial class ПравилаИгры
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ПравилаИгры));
            this.labelПравила = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelПравила
            // 
            this.labelПравила.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelПравила.Location = new System.Drawing.Point(35, 21);
            this.labelПравила.Name = "labelПравила";
            this.labelПравила.Size = new System.Drawing.Size(724, 400);
            this.labelПравила.TabIndex = 1;
            this.labelПравила.Text = resources.GetString("labelПравила.Text");
            // 
            // ПравилаИгры
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelПравила);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ПравилаИгры";
            this.Text = "Правила игры";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelПравила;
    }
}