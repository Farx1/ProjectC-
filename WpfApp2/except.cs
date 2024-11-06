using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfApp2
{
    public partial class except : Form
    {
        public except()
        {
            InitializeComponent();
        }

        //Boutton OK
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            fenetre_choix_frac f1 = new fenetre_choix_frac();
            f1.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
