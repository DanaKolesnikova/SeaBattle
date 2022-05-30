
namespace Морской_бой
{
    partial class Справочная_информация
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Справочная_информация));
            this.labelИнфа = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelИнфа
            // 
            this.labelИнфа.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelИнфа.Location = new System.Drawing.Point(38, 25);
            this.labelИнфа.Name = "labelИнфа";
            this.labelИнфа.Size = new System.Drawing.Size(724, 400);
            this.labelИнфа.TabIndex = 2;
            this.labelИнфа.Text = resources.GetString("labelИнфа.Text");
            // 
            // Справочная_информация
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelИнфа);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Справочная_информация";
            this.Text = "Справочная информация";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelИнфа;
    }
}