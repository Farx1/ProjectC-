using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//using Microsoft.Win32;

namespace WpfApp2
{
    /// <summary>
    /// Logique d'interaction pour trait_image.xaml
    /// </summary>
    public partial class trait_image : Window
    {
        public trait_image()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }

        private void Button_netb (object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\";
            file.Filter = "BMP Image|*.bmp";
            //file.FilterIndex = 2;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyImage image = new MyImage(file.FileName);
                image.NoirEtBlanc();
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

        private void Button_mir_nuances_gris(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\";
            file.Filter = "BMP Image|*.bmp";
            //file.FilterIndex = 2;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyImage image = new MyImage(file.FileName);
                image.NuancesGris();
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

        private void Button_negatif(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\";
            file.Filter = "BMP Image|*.bmp";
            //file.FilterIndex = 2;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyImage image = new MyImage(file.FileName);
                image.Negatif();
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

        private void Button_agrandir(object sender, RoutedEventArgs e)
        {
            Valeur_agrandir coef = new Valeur_agrandir();
            coef.Show();

        }

        private void Button_retrecir(object sender, RoutedEventArgs e)
        {
            Valeur_retrecir coef = new Valeur_retrecir();
            coef.Show();
        }

        private void Button_mir_ver(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\";
            file.Filter = "BMP Image|*.bmp";
            //file.FilterIndex = 2;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyImage image = new MyImage(file.FileName);
                image.MirroirVertical();
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

        private void Button_mir_hor(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\";
            file.Filter = "BMP Image|*.bmp";
            //file.FilterIndex = 2;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyImage image = new MyImage(file.FileName);
                image.MirroirHorizontal();
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

        private void Button_renforcement(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\";
            file.Filter = "BMP Image|*.bmp";
            //file.FilterIndex = 2;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyImage image = new MyImage(file.FileName);
                image.NuancesGris();
                image.Convolution(Matrice.Renforcement,Matrice.Null);
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

        private void Button_detection(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\";
            file.Filter = "BMP Image|*.bmp";
            //file.FilterIndex = 2;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyImage image = new MyImage(file.FileName);
                image.NuancesGris();
                image.Convolution(Matrice.Sobel1, Matrice.Sobel2);
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

        private void Button_flou(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\";
            file.Filter = "BMP Image|*.bmp";
            //file.FilterIndex = 2;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyImage image = new MyImage(file.FileName);
                image.Convolution(Matrice.Flou, Matrice.Null, 0.11111);
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

        private void Button_Rotation(object sender, RoutedEventArgs e)
        {
            angle_rotation angle = new angle_rotation();
            angle.Show();
        }

        private void Button_histogramme(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\";
            file.Filter = "BMP Image|*.bmp";
            //file.FilterIndex = 2;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyImage image = new MyImage(file.FileName);
                image.DrawHistogram();
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

        private void Button_repoussage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\";
            file.Filter = "BMP Image|*.bmp";
            //file.FilterIndex = 2;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyImage image = new MyImage(file.FileName);
                image.Convolution(Matrice.Repoussage, Matrice.Null);
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

        private void Button_Cacher(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = "c:\\";
            file.Filter = "BMP Image|*.bmp";
            file.FilterIndex = 2;
            file.RestoreDirectory = true;

            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyImage image = new MyImage(file.FileName);
                OpenFileDialog file2 = new OpenFileDialog();
                file2.InitialDirectory = "c:\\";
                file2.Filter = "BMP Image|*.bmp";
                file2.FilterIndex = 2;
                file2.RestoreDirectory = true;
                if (file2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    MyImage image2 = new MyImage(file2.FileName);
                    image.CacherImage(image2);
                    image.Agrandir(3);
                }

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

        private void Button_decoder(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.InitialDirectory = "c:\\";
            file.Filter = "BMP Image|*.bmp";
            //file.FilterIndex = 2;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyImage image = new MyImage(file.FileName);
                image.DecoderImage();
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

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Retour(object sender, RoutedEventArgs e)
        {
            Window1 f1 = new Window1();
            f1.Show();
            this.Close();
        }
    }
}
