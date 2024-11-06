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
    /// <summary>
    /// Logique d'interaction pour Choix_QR.xaml
    /// </summary>
    public partial class Choix_QR : Window
    {
        public Choix_QR()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void Button_Txt(object sender, RoutedEventArgs e)
        {
            text_QR text = new text_QR();
            text.Show();
        }

        private void Button_Retour(object sender, RoutedEventArgs e)
        {
            Window1 f1 = new Window1();
            f1.Show();
            this.Close();
        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_SMS(object sender, RoutedEventArgs e)
        {
            SMS_QR sms = new SMS_QR();
            sms.Show();
        }
    } 
}
