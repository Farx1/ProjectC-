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
    public partial class SMS_QR : Form
    {
        public SMS_QR()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        //Boutton OK
        private void button1_Click(object sender, EventArgs e)
        {
            string numero = Convert.ToString(textBox2.Text);
            string texte = Convert.ToString(textBox1.Text);
            string text = ($"SMSTO:+33{numero}:{texte}");//SMSTO:+33620330631:Les bits de donnees sont places en commencant en bas a droite 
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
