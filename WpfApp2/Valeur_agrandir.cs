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
    public partial class Valeur_agrandir : Form
    {
        public Valeur_agrandir()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        // Boutton OK
        private void button1_Click(object sender, EventArgs e)
        {
            double val = Convert.ToDouble(textBox1.Text);
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = "c:\\";
            file.Filter = "BMP Image|*.bmp";
            file.FilterIndex = 2;
            file.RestoreDirectory = true;

            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyImage image = new MyImage(file.FileName);
                image.Agrandir(val);
                image.Agrandir(3);
                SaveFileDialog newfile = new SaveFileDialog();
                newfile.Filter = "bmp image|*.bmp";
                if (newfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string chemin = newfile.FileName;
                    image.From_Image_To_File(chemin);
                    Affichage affichage = new Affichage(chemin);
                    affichage.Show();
                }
            }
        }
    }
}
