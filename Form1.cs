using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Морской_бой
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonStartPlay_Click(object sender, EventArgs e)  //открытие формы с игрой
        {
            
            Игра f2 = new Игра();
            f2.FormClosed += formClosed;
            this.Hide();
            f2.Show();


        }
        void formClosed(object sender, FormClosedEventArgs e) // Закрытие формы
                     {
                    this.Show();
                      }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void правилаИгрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ПравилаИгры register_ПравилаИгры = new ПравилаИгры();
            register_ПравилаИгры.ShowDialog();
        }       

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            О_программе register_Опрограмме = new О_программе();
            register_Опрограмме.ShowDialog();
        }

        private void справочнаяИнформацияToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Справочная_информация register_справочнаяИнформация = new Справочная_информация();
            register_справочнаяИнформация.ShowDialog();
        }
    }
}
