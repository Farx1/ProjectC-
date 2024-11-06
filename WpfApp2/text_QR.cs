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
    public partial class text_QR : Form
    {
        public text_QR()
        {
            InitializeComponent();
        }

        //Boutton OK
        private void button1_Click(object sender, EventArgs e)
        {
            string text = Convert.ToString(textBox1.Text);
            QRCode QR = new QRCode(text);
            SaveFileDialog newfile = new SaveFileDialog();
            newfile.Filter = "bmp image|*.bmp";
            if (newfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string chemin = newfile.FileName;
                QR.From_Image_To_File(chemin);
                Affichage affichage = new Affichage(chemin);
                affichage.Show();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
