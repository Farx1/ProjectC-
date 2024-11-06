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
    public partial class angle_rotation : Form
    {
        public angle_rotation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int val = Convert.ToInt32(textBox1.Text);
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = "c:\\";
            file.Filter = "BMP Image|*.bmp";
            file.FilterIndex = 2;
            file.RestoreDirectory = true;

            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyImage image = new MyImage(file.FileName);
                MyImage rotImage = image.Rotate(val);
                image.Agrandir(3);
                SaveFileDialog newfile = new SaveFileDialog();
                newfile.Filter = "bmp image|*.bmp";
                if (newfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string chemin = newfile.FileName;
                    rotImage.From_Image_To_File(chemin);
                    Affichage affichage = new Affichage(chemin);
                    affichage.Show();
                }
            }
        }
    }
}
