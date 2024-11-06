using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;


namespace WpfApp2
{
    /// <summary>
    /// Logique d'interaction pour fractale.xaml
    /// </summary>
    public partial class fractale : Window
    {
        public fractale()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Retour(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
            this.Close();
        }

        private void Button_Choix(object sender, RoutedEventArgs e)
        {
            fenetre_choix_frac choix = new fenetre_choix_frac();
            choix.Show();
        }

        private void Button_Aleatoire(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            int randInt = rand.Next(1, 12);
            //int randInt = 4;
            switch (randInt)
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
