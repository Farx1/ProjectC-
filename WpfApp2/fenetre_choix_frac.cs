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
    public partial class fenetre_choix_frac : Form
    {
        public fenetre_choix_frac()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            int val = Convert.ToInt32(textBox1.Text);
            switch (val)
            {
                case 1:
                    
                    var image1 = MyImage.DrawMandelbrotA();
                    System.Windows.Forms.SaveFileDialog newfile1 = new System.Windows.Forms.SaveFileDialog();
                    newfile1.Filter = "bmp image|*.bmp";
                    if (newfile1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string chemin = newfile1.FileName;
                        image1.From_Image_To_File(chemin);
                        Affichage affichage = new Affichage(chemin);
                        affichage.Show();
                    }
                    
                    break;
                case 2:
                    
                    var image2 = MyImage.DrawMandelbrotB();
                    System.Windows.Forms.SaveFileDialog newfile2 = new System.Windows.Forms.SaveFileDialog();
                    newfile2.Filter = "bmp image|*.bmp";
                    if (newfile2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string chemin = newfile2.FileName;
                        image2.From_Image_To_File(chemin);
                        Affichage affichage = new Affichage(chemin);
                        affichage.Show();
                    }
                    break;
                case 3:
                    var image3 = MyImage.DrawMandelbrotC();
                    System.Windows.Forms.SaveFileDialog newfile3 = new System.Windows.Forms.SaveFileDialog();
                    newfile3.Filter = "bmp image|*.bmp";
                    if (newfile3.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string chemin = newfile3.FileName;
                        image3.From_Image_To_File(chemin);
                        Affichage affichage = new Affichage(chemin);
                        affichage.Show();
                    }
                    break;
                case 4:
                    ShowJuliafract(0);
                    break;
                case 5:
                    ShowJuliafract(1);
                    break;
                case 6:
                    ShowJuliafract(2);
                    break;
                case 7:
                    ShowJuliafract(3);
                    break;
                case 8:
                    ShowJuliafract(4);
                    break;
                case 9:
                    ShowJuliafract(5);
                    break;
                case 10:
                    ShowJuliafract(6);
                    break;
                case 11:
                    ShowJuliafract(7);
                    break;
                
                
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        

        private void choix_Click(object sender, EventArgs e)
        {

        }

        private void fenetre_choix_frac_Load(object sender, EventArgs e)
        {

        }
        public void ShowJuliafract(int numero)
        {
            var image = MyImage.DrawJulia(numero);
            System.Windows.Forms.SaveFileDialog newfile = new System.Windows.Forms.SaveFileDialog();
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
