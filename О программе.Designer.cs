
namespace Морской_бой
{
    partial class О_программе
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(О_программе));
            this.labelОпрограмме = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelОпрограмме
            // 
            this.labelОпрограмме.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelОпрограмме.Location = new System.Drawing.Point(33, 27);
            this.labelОпрограмме.Name = "labelОпрограмме";
            this.labelОпрограмме.Size = new System.Drawing.Size(724, 400);
            this.labelОпрограмме.TabIndex = 2;
            this.labelОпрограмме.Text = resources.GetString("labelОпрограмме.Text");
            // 
            // О_программе
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelОпрограмме);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "О_программе";
            this.Text = "О программе";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelОпрограмме;
    }
}