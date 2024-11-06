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

namespace WpfApp2
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }

        private void Button_trait_image(object sender, RoutedEventArgs e)
        {
            trait_image trait_image = new trait_image();
            trait_image.Show();
            this.Close();
        }

        private void Button_Fractale(object sender, RoutedEventArgs e)
        {
            fractale fractale = new fractale();
            fractale.Show();
            this.Close();
        }

        private void Button_QRCode(object sender, RoutedEventArgs e)
        {
            Choix_QR QR_fenetre = new Choix_QR();
            QR_fenetre.Show();
            this.Close();
        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Retour(object sender, RoutedEventArgs e)
        {
            MainWindow Main = new MainWindow();
            Main.Show();
            this.Close();
        }

    }
}
