// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ReedSolomon;

namespace WpfApp2
{
    #region Projet PSI S4
    public class MyImage
    {

        //TD2
        #region Attributs

        private string _typeImage = "BMP";
        private int _height;
        private int _width;
        private int _sizeFile;
        private int _numberRgb;
        private int _offset;
        private Pixel[,] _imageData = null!;
        private int _ecriture;

        #endregion


        #region Les 4 constructeurs

        /// <summary>
        /// Constructeur naturel avec paramètres saisis manuellement
        /// </summary>
        /// <param name="typeImage"> type de l'image </param>
        /// <param name="height"> hauteur de l'image </param>
        /// <param name="width"> largeur de l'image </param>
        /// <param name="size"> taille du fichier </param>
        /// <param name="numberRgb"> nombre de bits par couleur </param>
        /// <param name="offset"> taille du header + headerinfo </param>
        /// <param name="imageData"> matrice RGB de l'image elle-même </param>
        /// <param name="ecriture"> type d'écriture (haut en bas) (bas en haut)</param>

        #region Premier constructeur

        public MyImage(string typeImage, int height, int width, int size, int numberRgb, int offset, Pixel[,] imageData, int ecriture)
        {
            this._typeImage = typeImage;
            this._height = height;
            this._width = width;
            this._sizeFile = size;
            _numberRgb = numberRgb;
            this._offset = offset;
            _imageData = imageData;
            _ecriture = ecriture = 0;
        }


        #endregion


        /// <summary>
        /// Constructeur prenant en entrée un fichier
        /// </summary>

        #region Deuxième constructeur

        public MyImage(string path)
        {
            byte[] myfile = File.ReadAllBytes(path);

            //if (myfile[0] == 66 && myfile[1] == 77)
            //{
            //this._typeImage ---> Initialisé directement dans le constructeur puique BMP ne change pasa
            //}

            byte[] tabLargeur = new byte[] { myfile[18], myfile[19], myfile[20], myfile[21] };
            this._width = Convertir_Endian_To_Int(tabLargeur);

            byte[] tabHauteur = new byte[] { myfile[22], myfile[23], myfile[24], myfile[25] };
            this._height = Convertir_Endian_To_Int(tabHauteur);

            byte[] tabTaille = new byte[] { myfile[2], myfile[3], myfile[4], myfile[5] };
            this._sizeFile = Convertir_Endian_To_Int(tabTaille);

            byte[] tabBits = new byte[] { myfile[28], myfile[29] };
            this._numberRgb = Convertir_Endian_To_Int(tabBits);

            byte[] tabOffset = new byte[] { myfile[10], myfile[11], myfile[12], myfile[13] };
            this._offset = Convertir_Endian_To_Int(tabOffset);

            this._imageData = new Pixel[_height, _width];
            int k = _offset;
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    this._imageData[i, j] = new Pixel(myfile[k], myfile[k + 1], myfile[k + 2]); //creation d'un pixel pour chaque set de 3 bytes du fichier
                    k += 3;
                }
                /*
                int reste = _weight % 4;
                if (reste == 1)
                {
                    k = k + 3;
                } 
                if (reste == 2)
                {
                    k = k + 2;
                }
                if (reste == 3)
                {
                    k = k + 1;
                }
                */
            }

        }



        #endregion

        #region Constructeur null

        #endregion

        #region Constructeur nouvelle Image
        public MyImage(int height, int width, Pixel[,] imageData)
        {
            this._imageData = imageData;
            this._height = height;
            this._width = width;
        }
        #endregion

        #region Constructeur clone

        public MyImage(MyImage image)
        {
            this._typeImage = image._typeImage;
            this._height = image._height;
            this._width = image._width;
            this._sizeFile = Height * Width * 3 + Offset;
            _numberRgb = image._numberRgb;
            this._offset = image._offset;
            _imageData = image._imageData;
        }

        public MyImage()
        {
            _numberRgb = 24;
            _offset = 54;
            _typeImage = "BMP";
            _sizeFile = Height * Width * 3 + Offset;

        }

        #endregion

        #endregion


        #region Propriétés

        public string TypeImage
        {
            get => _typeImage;
            set => _typeImage = value;
        }

        public int Height
        {
            get => _height;
            set => _height = value;
        }

        public int Width
        {
            get => _width;
            set => _width = value;
        }

        public int SizeFile
        {
            get => _sizeFile;
            set => _sizeFile = Height * Width * 3 + Offset;
        }

        public int NumberRgb
        {
            get => _numberRgb;
            set => _numberRgb = value;
        }

        public int Offset
        {
            get => _offset;
            set => _offset = value;
        }

        public Pixel[,] ImageData
        {
            get => _imageData;
            set => _imageData = value;
        }

        public int Ecriture
        {
            get => _ecriture;
            set => _ecriture = value;
        }

        #endregion


        #region Méthodes de conversion

        /// <summary>
        /// Méthode permettant de convertir un tableau de bytes en int
        /// </summary>
        /// <param name="tab"></param>Tableau de bytes à convertir
        /// <returns></returns>
        #region Endian --> Entier

        public static int Convertir_Endian_To_Int(byte[] tab)
        {
            int result = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                result = (int)(result + tab[i] * Math.Pow(256, i));
            }

            return result;
        }


        #endregion

        #region Entier --> Endian
        /// <summary>
        /// Méthode permettant de convertir un entier en tableau de bytes
        /// </summary>
        /// <param name="val"></param>Valeur à convertir
        /// <param name="size"></param>Taille du tableau de bytes à créer
        /// <returns></returns>
        public static byte[] Convertir_Int_To_Endian(int val, int size)
        {
            byte[] newone = new byte[size];
            for (int i = size - 1; i >= 0; i--)
            {
                newone[i] = (byte)(val / Math.Pow(256, i));
                val -= newone[i] * ((int)Math.Pow(256, i));
            }

            return newone;
        }


        #endregion

        #endregion


        #region Méthode pour afficher les caractéristiques d'une image
        /// <summary>
        /// Méthode permettant d'afficher les caractéristiques d'une image
        /// </summary>
        public void toString()
        {
            string s = "Type de l'image : " + TypeImage + "\n" + "Hauteur de l'image : " + Height + "\n" +
                       "Largeur de l'image : " + Width + "\n" + "Taille du fichier : " + SizeFile + "\n" +
                       "Nombre de bits par couleur : " + NumberRgb + "\n" + "Taille du bandeau : " + Offset + "\n" +
                       "\n";
            if (ImageData != null)
            {
                s += "Données de l'image : " + "\n";
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        s += ImageData[i, j].toStringP();

                    }

                    s += "\n";
                }
            }

            Console.WriteLine(s);
        }

        #endregion


        #region Méthode qui tranforme une image en fichier binaire
        /// <summary>
        /// Méthode qui tranforme une image en fichier binaire
        /// </summary>
        /// <param name="path"></param>Nom du fichier à convertir
        public void From_Image_To_File(string path)
        {
            // creation de 3 lists, une pour chaque catégorie, où on y ajoute les données une à une 
            #region Header

            List<byte> header = new List<byte>(); //header
            if (this.TypeImage == "BMP")
            {
                header.Add(Convert.ToByte(66));
                header.Add(Convert.ToByte(77));
            }


            header.AddRange(Convertir_Int_To_Endian(this.SizeFile, 4));

            for (int i = 0; i < 4; i++)
            {
                header.Add(Convert.ToByte(0));
            }

            header.Add(Convert.ToByte(54));
            for (int i = 0; i < 3; i++)
            {
                header.Add(Convert.ToByte(0));
            }

            #endregion

            #region HeaderInfo

            List<byte> headerInfo = new List<byte>(); //HeaderInfos

            headerInfo.Add(40);

            for (int i = 1; i < 4; i++)
            {
                headerInfo.Add(0);
            }

            headerInfo.AddRange(Convertir_Int_To_Endian(this.Width, 4));

            headerInfo.AddRange(Convertir_Int_To_Endian(this.Height, 4));

            headerInfo.Add(1);
            headerInfo.Add(0);

            headerInfo.AddRange(Convertir_Int_To_Endian(this.NumberRgb, 2));


            for (int i = 0; i < 4; i++)
            {
                headerInfo.Add(0);
            }


            headerInfo.Add(176);
            headerInfo.Add(4);

            for (int i = 0; i < 18; i++)
            {
                headerInfo.Add(0);
            }

            #endregion

            #region Image

            if (_ecriture == 1)
            {
                List<byte> image = new List<byte>(); //QRCODE
                for (int i = Height - 1; i >= 0; i--)//Lecture inverse donc de i = height-1; i>=0;i--) changer et tester tt les méthdodes depuis le début
                {
                    for (int j = 0; j < this.Width; j++)
                    {
                        image.Add(this.ImageData[i, j].Red);
                        image.Add(this.ImageData[i, j].Green);
                        image.Add(this.ImageData[i, j].Blue);
                    }

                    for (int l = 0; l < this._imageData.GetLength(1) % 4; l++)
                    {
                        image.Add(Convert.ToByte(0));
                    }
                }
                var output = header.Concat(headerInfo).Concat(image); //fusionne les 3 listes
                File.WriteAllBytes(path, output.ToArray());
            }

            if (_ecriture == 0)
            {
                List<byte> image = new List<byte>(); //IMAGE
                for (int i = 0; i < this.Height; i++)
                {
                    for (int j = 0; j < this.Width; j++)
                    {
                        image.Add(this.ImageData[i, j].Red);
                        image.Add(this.ImageData[i, j].Green);
                        image.Add(this.ImageData[i, j].Blue);
                    }

                    for (int l = 0; l < this._imageData.GetLength(1) % 4; l++)
                    {
                        image.Add(Convert.ToByte(0));
                    }
                }
                var output = header.Concat(headerInfo).Concat(image); //fusionne les 3 listes
                File.WriteAllBytes(path, output.ToArray());
            }


            #endregion


        }

        #endregion

        //TD3
        #region Méthode Couleur --> Noir&Blanc/Inversion
        /// <summary>
        /// Méthode qui permet de convertir une image en nuances de gris
        /// </summary>
        public void NuancesGris()
        {
            Pixel[,] nuancgris = new Pixel[this._height, this._width];

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    byte moyenne = Convert.ToByte((this.ImageData[i, j].Red + this.ImageData[i, j].Green + this.ImageData[i, j].Blue) / 3);
                    nuancgris[i, j] = new Pixel(moyenne, moyenne, moyenne);
                }
            }

            _imageData = nuancgris;
        }


        /// <summary>
        ///Méthode qui permet de convertir une image en noir et blanc
        /// </summary>
        public void NoirEtBlanc()
        {
            Pixel[,] neb = new Pixel[this._height, this._width];

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    byte moyenne = Convert.ToByte((this.ImageData[i, j].Red + this.ImageData[i, j].Green + this.ImageData[i, j].Blue) / 3);
                    if (moyenne > 127)
                    {
                        neb[i, j] = new Pixel(255, 255, 255);
                    }
                    else
                    {
                        neb[i, j] = new Pixel(0, 0, 0);

                    }
                }
            }

            _imageData = neb;
        }


        /// <summary>
        /// Méthode qui permet d'inverser les couleurs d'une image
        /// </summary>
        public void Negatif()
        {
            Pixel[,] neg = new Pixel[this._height, this._width];

            for (int i = 0; i < this._imageData.GetLength(0); i++)
            {
                for (int j = 0; j < this._imageData.GetLength(1); j++)
                {
                    neg[i, j] = new Pixel(Convert.ToByte(255 - this._imageData[i, j].Red), Convert.ToByte(255 - this._imageData[i, j].Green), Convert.ToByte(255 - this._imageData[i, j].Blue));

                }
            }

            _imageData = neg;
        }

        #endregion


        #region Méthodes pour agrandir et retrecir et décalage
        /// <summary>
        /// Cette méthode permet d'agrandir l'image voulue
        /// </summary>
        /// <param name="facteur"></param>Le facteur d'agrandissement de l'image
        public void Agrandir(double facteur)//Voir dans le dossier directement, l'affichage ne se fait pas sur Riders
        {
            _height = (int)(_height * facteur);
            _width = (int)(_width * facteur);

            Pixel[,] grand = new Pixel[_height, _width];

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    grand[i, j] = new Pixel(_imageData[(int)(i / facteur), (int)(j / facteur)]);
                }
            }
            _imageData = grand;

        }


        /// <summary>
        /// Cette méthode permet de rétrecir l'image voulue
        /// </summary>
        /// <param name="facteur"></param>Facteur de rétrecissement de l'image
        public void Retrecir(double facteur) //Voir dans le dossier directement, l'affichage ne se fait pas sur Riders
        {

            _height = (int)(_height / facteur);
            _width = (int)(_width / facteur);
            Pixel[,] petit = new Pixel[_height, _width];
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    petit[i, j] = new Pixel(this._imageData[(int)(i * facteur), (int)(j * facteur)]);
                }
            }

            _imageData = petit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>Direction du décalage
        /// <param name="defilement1"></param>Valeur du décalage vertical
        /// <param name="defilement2"></param>Valeur du décalage horizontal
        public void Decaler(string direction, int defilement1, int defilement2)
        {

            if (direction == "gauche")
            {
                var defilement = defilement1;
                Pixel[,] decalage = new Pixel[this._height, this._width];
                for (int i = 0; i < _height; i++)
                {
                    for (int j = 0; j < _width; j++)
                    {
                        decalage[i, j] = new Pixel(this._imageData[i, (j + defilement) % _width]);
                    }
                }
                _imageData = decalage;
            }

            else if (direction == "haut")
            {
                var defilement = defilement2;
                Pixel[,] decalage = new Pixel[this._height, this._width];
                for (int i = 0; i < _height; i++)
                {
                    for (int j = 0; j < _width; j++)
                    {
                        decalage[i, j] = new Pixel(this._imageData[(i + defilement) % _height, j]);
                    }
                }
                _imageData = decalage;
            }

            else if (direction == "diagonal")
            {
                Pixel[,] decalage = new Pixel[this._height, this._width];
                for (int i = 0; i < _height; i++)
                {
                    for (int j = 0; j < _width; j++)
                    {
                        decalage[i, j] = new Pixel(this._imageData[(i + defilement2) % _height, (j + defilement1) % _width]);
                    }
                }
                _imageData = decalage;
            }


        }




        #endregion


        #region Méthodes mirroir(Horizontal/Vertical)
        /// <summary>
        /// Méthode permettant de faire un mirroir horizontal
        /// </summary>
        public void MirroirHorizontal()
        {
            Pixel[,] mirh = new Pixel[_height, _width];
            for (int i = 0; i < this._imageData.GetLength(0); i++)
            {
                for (int j = 0; j < this._imageData.GetLength(1); j++)
                {
                    mirh[i, j] = this._imageData[i, this._imageData.GetLength(1) - 1 - j];
                }
            }

            _imageData = mirh;
        }
        /// <summary>
        /// Métode permettant de faire un mirroir vertical
        /// </summary>
        public void MirroirVertical()
        {
            Pixel[,] mirv = new Pixel[_height, _width];
            for (int i = 0; i < this._imageData.GetLength(0); i++)
            {
                for (int j = 0; j < this._imageData.GetLength(1); j++)
                {
                    mirv[i, j] = this._imageData[this._imageData.GetLength(0) - 1 - i, j];
                }
            }

            _imageData = mirv;
        }
        #endregion


        #region Méthodes pour la rotation d'une Image

        #region Méthode complémentaire rotation
        /// <summary>
        /// Méthode permettant de compléter la rotation d'image pour plus d'efficacité
        /// </summary>
        /// <param name="deg"></param>Degré de rotation supplémentaire après les rotations de 90°
        public void Rotatesupp(int deg)
        {
            //première méthode faite avec les méthodes miroirs pour plus d'efficacité dans la méthode Rotate()

            while (deg < 0) deg += 360;
            while (deg >= 360) deg -= 360;

            if (deg % 180 != 0)
            {
                Pixel[,] rot = new Pixel[this._imageData.GetLength(1), this._imageData.GetLength(0)];
                _height = this._imageData.GetLength(1);
                _width = this._imageData.GetLength(0);
                int k = deg / 90;
                if (k < 2)
                {
                    for (int i = 0; i < this._imageData.GetLength(0); i++)
                    {
                        for (int j = 0; j < this._imageData.GetLength(1); j++)
                        {
                            rot[j, this._imageData.GetLength(0) - 1 - i] = this._imageData[i, j];
                        }
                    }

                    _imageData = rot;
                }
                else
                {
                    this.Rotatesupp(90);
                    this.MirroirHorizontal();
                    this.MirroirVertical();
                }
            }
            else
            {
                Pixel[,] rot = new Pixel[this._imageData.GetLength(0), this._imageData.GetLength(1)];

                _height = this._imageData.GetLength(0);
                _width = this._imageData.GetLength(1);
                int k = deg / 180;

                if (k == 1)
                {
                    for (int i = 0; i < this._imageData.GetLength(0); i++)
                    {
                        for (int j = 0; j < this._imageData.GetLength(1); j++)
                        {
                            rot[i, j] = this._imageData[this._imageData.GetLength(0) - 1 - i, this._imageData.GetLength(1) - 1 - j];
                        }
                    }

                    _imageData = rot;
                }
            }
        }
        #endregion

        #region Méthode Rotate90 basique
        /// <summary>
        /// Méthode permettant de faire une rotation de 90°
        /// </summary>
        public void Rotate90()
        {
            Pixel[,] rot = new Pixel[this._width, this._height];
            for (int i = 0; i < _width - 1; i++)
            {

                for (int j = 0; j < _height; j++)
                {
                    rot[i, _imageData.GetLength(0) - j - 1] = _imageData[j, i];
                }
            }

            (_height, _width) = (_width, _height);
            _imageData = rot;
        }

        #endregion

        #region Méthode de Rotation pour tous les degrés
        /// <summary>
        /// Méthode permettant de faire des rotations de tous les angles (degrés
        /// </summary>
        /// <param name="deg"></param> Dégré de rotation
        /// <returns></returns>
        public MyImage Rotate(int deg)
        {
            MyImage imagerot = new MyImage(this);


            //On remet l'angle entre 0 et 360°
            while (deg < 0) deg += 360;
            while (deg >= 360) deg -= 360;

            int prerot = deg / 90;
            int rotation = deg % 90;

            if (deg == 90 || deg == 180 || deg == 270)
            {
                for (int k = 0; k < prerot; k++)
                {
                    imagerot.Rotate90();
                }

            }

            //On fait plusieurs rotation majeures
            Rotatesupp(deg - rotation);


            if (rotation > 0)
            {
                // On met l'angle de rotation en radians
                double rad = (double)rotation * (Math.PI / 180.0);

                // On calcul la hauteur et la largeur de la nouvelle image 

                imagerot._height = (int)(Math.Abs(Math.Sin(rad) * (double)this._imageData.GetLength(1)) + Math.Abs(Math.Cos(rad) * (double)this._imageData.GetLength(0)));
                imagerot._width = (int)(Math.Abs(Math.Cos(rad) * (double)this._imageData.GetLength(1)) + Math.Abs(Math.Sin(rad) * (double)this._imageData.GetLength(0)));
                imagerot._imageData = new Pixel[imagerot._height, imagerot._width];

                // Pour chaque pixel de la NOUVELLE image
                for (int i = 0; i < imagerot._height; i++)
                {
                    for (int j = 0; j < imagerot._width; j++)
                    {

                        // On initilaise les coordonnées cartésiennes de chaque point
                        double X = j;
                        double Y = (double)(imagerot._height - i) - (double)(Math.Sin(rad) * _imageData.GetLength(1));

                        // On passe en coordonees polaires
                        double r = Math.Sqrt(X * X + Y * Y);
                        double angle = Math.Atan2(Y, X) + rad;

                        // On calcule les nouvelles coordonnées de l'image
                        double x = r * Math.Cos(angle);
                        double y = r * Math.Sin(angle);

                        int nvlhauteur = (int)(this._imageData.GetLength(0) - y);
                        int nvllargeur = (int)x;

                        if (nvlhauteur >= 0 && nvllargeur >= 0 && nvlhauteur < this._imageData.GetLength(0) && nvllargeur < this._imageData.GetLength(1))
                        {
                            //Console.WriteLine($"({nvllargeur}, {nvlhauteur}) ==> ({i}, {j})");//Pour voir ancienne/nouvelle coordonées
                            imagerot._imageData[i, j] = this._imageData[nvlhauteur, nvllargeur];
                        }
                    }
                }
            }
            //On complète l'image avec des Pixels blancs
            imagerot.FillImageWithWhite();
            return imagerot;
        }
        #endregion

        #region Méthodes de remplissage de l'image
        /// <summary>
        /// Méthode permettant de remplir l'image avec des pixels blancs
        /// </summary>
        public void FillImageWithWhite()
        {
            for (int i = 0; i < this._imageData.GetLength(0); i++)
            {
                for (int j = 0; j < this._imageData.GetLength(1); j++)
                {
                    _imageData[i, j] ??= new Pixel(255, 255, 255);
                }
            }
        }
        /// <summary>
        /// Méthode permettant de remplir l'image avec des pixels gris
        /// </summary>
        public void FillImageWithGrey()
        {
            for (int i = 0; i < this._imageData.GetLength(0); i++)
            {
                for (int j = 0; j < this._imageData.GetLength(1); j++)
                {
                    _imageData[i, j] ??= new Pixel(177, 177, 177);
                }
            }
        }
        #endregion


        #endregion

        //TD4
        #region Matrice de Convolution  

        /// <summary>
        /// Méthode permettant d'effectuer une convolution sur l'image
        /// </summary>
        /// <param name="matrice1"></param> Matrice de convolution 1
        /// <param name="matrice2"></param> Matrice de convolution 2
        /// <param name="factor"></param> Facteur de multiplication
        /// <exception cref="ArgumentException"></exception> Si les matrices ne sont pas de même taille
        public void Convolution(int[,] matrice1, int[,] matrice2, double factor = 1.0)
        {
            Pixel[,] pix = new Pixel[_height, _width];

            var midPoint = matrice1.GetLength(0) / 2;
            if (matrice1.GetLength(0) != matrice2.GetLength(0))
            {
                throw new ArgumentException("Vérifier la taille des matrices");
            }

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    pix[i, j] = new Pixel(0, 0, 0);//creer trois somme de int pour chaque pixel
                    int cr1 = 0;
                    int cg1 = 0;
                    int cb1 = 0;

                    int cr2 = 0;
                    int cg2 = 0;
                    int cb2 = 0;

                    for (int k = 0; k < matrice1.GetLength(0); k++)
                    {
                        for (int l = 0; l < matrice1.GetLength(0); l++)
                        {
                            var line = (i + (k - midPoint) + _imageData.GetLength(0)) % _imageData.GetLength(0);
                            var col = (j + (l - midPoint) + _imageData.GetLength(1)) % _imageData.GetLength(1);
                            //récupère les informations des lignes et colonnes des Pixels checké par la matrice puis la passe en posiif

                            cr1 += ((int)_imageData[line, col].Red * matrice1[k, l]);//changer le premier en somme
                            cg1 += ((int)_imageData[line, col].Green * matrice1[k, l]);
                            cb1 += ((int)_imageData[line, col].Blue * matrice1[k, l]);

                            cr2 += ((int)_imageData[line, col].Red * matrice2[k, l]);//changer le premier en somme
                            cg2 += ((int)_imageData[line, col].Green * matrice2[k, l]);
                            cb2 += ((int)_imageData[line, col].Blue * matrice2[k, l]);
                        }
                    }

                    cr1 = (int)Math.Abs((double)factor * cr1);
                    cg1 = (int)Math.Abs((double)factor * cg1);
                    cb1 = (int)Math.Abs((double)factor * cb1);

                    cr2 = (int)Math.Abs((double)factor * cr2);
                    cg2 = (int)Math.Abs((double)factor * cg2);
                    cb2 = (int)Math.Abs((double)factor * cb2);

                    var redValue = Math.Sqrt(cr1 * cr1 + cr2 * cr2);
                    var greenValue = Math.Sqrt(cg1 * cg1 + cg2 * cg2);
                    var blueValue = Math.Sqrt(cb1 * cb1 + cb2 * cb2);


                    pix[i, j] = new Pixel(Convert.ToByte(redValue > 255 ? 255 : redValue), Convert.ToByte(greenValue > 255 ? 255 : greenValue), Convert.ToByte(blueValue > 255 ? 255 : blueValue));
                }
            }

            this._imageData = pix;

        }

        #endregion

        //TD5
        #region Dessiner une fractale (3 versions de Mandelbrot/ Fractale de Julia)

        /// <summary>
        /// Méthodes permettant de dessiner une fractale de Mandelbrot(Type A,B,C)
        /// </summary>
        #region Fractale Mandelbrot (A,B,C)
         public static MyImage DrawMandelbrotA()
       {
            //MyImage fract = new MyImage("../../../Images/Fractale.bmp");
            //il faut créer une nouvelle image puis partir de celle ci
            int height = 4320;
            int width = 7680;
            MyImage mand = new MyImage();
            mand._imageData = new Pixel[height, width];
            mand.FillImageWithWhite();
            mand.Height = height;
            mand.Width = width;
            
            int lines = mand._imageData.GetLength(0);//hauteur
            int column = mand._imageData.GetLength(1);//largeur

            double xmin = -2;//bornes du repère
            double xmax = 0.5;
            double ymin = -1.25;
            double ymax = 1.25;
           
            int count = 200;//à faire avec chemin et 20000 pour count en avance++
           
            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < column; j++)
                {
 
                    double xn = 0;
                    double yn = 0;
                    double prec = 0;
                    double xnstocke = 0;
                   
                    double cx = j * ((Math.Abs(ymax) + Math.Abs(ymin)) / column);//association des coordonnées du plan (i,j) à des coordonnées (cx,cy) dans le repère (xmin,xmax) et (ymin,ymax)
                    double cy = i * ((Math.Abs(xmax) + Math.Abs(xmin)) / lines);
 
                    for (int k = 0; k < count; k++)
                    {
                        xnstocke = xn;
                        xn = xn * xn - yn * yn + cx + 1.5 * ymin;
                        yn = 2 * yn * xnstocke +  cy + 0.6 * xmin;
                        prec = xn * xn + yn * yn;//on peut mettre ça sous racine pour plus de cercles et de lignes
                        
                        if (prec > 25)
                        {
                            {
                                goto recuperer;// on va au sortir de la boucle pour itérer en gardant l'ancienne valeur de prec ( équivalent d'une fenêtre graphique tournant à l'infinie mais certe fixe)
                            }
                        }
                    }

                    recuperer:
                        { 
                            if ((prec) < 4.0)//on teste si le carré de la distance est inférieure à 4 on gagne en performance
                            {
                                mand._imageData[i, j].Red = (byte) (Convert.ToByte((byte)((prec)%255))); 
                                mand._imageData[i, j].Green = 0;
                                mand._imageData[i, j].Blue = 0;
                            }
                            else
                            {
                                prec = ((prec) % 255);
                                mand._imageData[i, j].Red = 0;//Convert.ToByte(prec);
                                mand._imageData[i, j].Green = 0; //Convert.ToByte(prec);
                                mand._imageData[i, j].Blue = Convert.ToByte(prec);
 
                            }
                                
                        }
                    
                    
                }
            }
            return mand;
        }

         public static MyImage DrawMandelbrotB() //on peut s'amuser un peu avec les valeurs des Pixels rouge et bleus pour dessiner d'autre sorte de forme
        {
            int height = 4320;
            int width = 7680;
            MyImage mand = new MyImage();
            mand._imageData = new Pixel[height, width];
            mand.FillImageWithWhite();
            mand.Height = height;
            mand.Width = width;

            int lines = mand._imageData.GetLength(0);
            int column = mand._imageData.GetLength(1);

            double xmin = -2;
            double xmax = 0.5;
            double ymin = -1.25;
            double ymax = 1.25;
           
            int count = 200;
           
            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < column; j++)
                {

                    double xn = 0;
                    double yn = 0;
                    double prec = 0;
                    double xnstocked = 0;
                   
                    double cx = j * ((Math.Abs(ymax) + Math.Abs(ymin)) / column);
                    double cy = i * ((Math.Abs(xmax) + Math.Abs(xmin)) / lines);

                    for (int k = 0; k < count; k++)
                    {
                        xnstocked = xn;
                        xn = xn * xn - yn * yn + cx + 1.5 * ymin;
                        yn = 2 * yn * xnstocked +  cy + 0.6 * xmin;
                        prec = xn * xn + yn * yn;
                       
                        if (prec > 25)
                        {
                            {
                                goto recuperer;
                            }
                        }
                    }

                    recuperer:
                        { 
                            if ((prec) < 4.0)
                            {
                                mand._imageData[i, j].Red = 0; 
                                mand._imageData[i, j].Green = 0;
                                mand._imageData[i, j].Blue = 0; //(byte) (Convert.ToByte((byte)(1000*(prec-yn/xn)%255)));
                            }
                            else
                            {
                                prec = ((xn) % 255);
                                mand._imageData[i, j].Red = Convert.ToByte((byte)(prec));
                                mand._imageData[i, j].Green = Convert.ToByte((byte)(prec));
                                mand._imageData[i, j].Blue = Convert.ToByte((byte)(prec));
                                //_imageData[i, j].Blue = Convert.ToByte((byte)(100*prec-xn)%255);//-- à tester aussi
                            }
                               
                        }
                   
                   
                }
            }
            return mand;
        }
       
        public static MyImage DrawMandelbrotC() //on peut s'amuser un peu avec les valeurs des Pixels rouge et bleus pour dessiner d'autre sorte de forme
        {
            int height = 4320;
            int width = 7680;
            MyImage mand = new MyImage();
            mand._imageData = new Pixel[height, width];
            mand.FillImageWithWhite();
            mand.Height = height;
            mand.Width = width;

            int lines = mand._imageData.GetLength(0);
            int column = mand._imageData.GetLength(1);

            double xmin = -2;
            double xmax = 0.5;
            double ymin = -1.25;
            double ymax = 1.25;
           
            int countmax = 10000;
           
            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < column; j++)
                {

                    double xn = 0;
                    double yn = 0;
                    double prec = 0;
                    double xnstocked = 0;
                    double count = 0;
                   
                    double cx = j * ((Math.Abs(ymax) + Math.Abs(ymin)) / column);
                    double cy = i * ((Math.Abs(xmax) + Math.Abs(xmin)) / lines);

                    for (int k = 0; k < countmax; k++)
                    {
                        xnstocked = xn;
                        xn = xn * xn - yn * yn + cx + 1.5 * ymin;
                        yn = 2 * yn * xnstocked +  cy + 0.6 * xmin;
                        prec = xn * xn + yn * yn;
                       
                        if (prec > countmax)
                        {
                            {
                                goto recuperer;
                            }
                        }

                        count++;
                    }

                    recuperer:
                        { 
                            if ((prec) < 4.0)
                            {
                                mand._imageData[i, j].Red = 0; 
                                mand._imageData[i, j].Green = 0;
                                mand._imageData[i, j].Blue = 0; //(byte) (Convert.ToByte((byte)(1000*(prec-yn/xn)%255)));
                            }
                            else
                            {
                                prec = ((xn) % 255);
                                mand._imageData[i, j].Red = Convert.ToByte((byte)(count % 16 *14));
                                mand._imageData[i, j].Green = Convert.ToByte((byte)(count % 32 *7));
                                mand._imageData[i, j].Blue = Convert.ToByte((byte)(count %128 *2));
                                //_imageData[i, j].Blue = Convert.ToByte((byte)(100*prec-xn)%255);//-- à tester aussi
                            }
                               
                        }
                   
                   
                }
            }
            return mand;
        }
       
       
         
         #endregion


        #region Fractale de Julia

        /// <summary>
        /// Méthode qui dessine la fractale de Julia
        /// </summary>
        /// <param name="numero"></param> Numéro de la fractale(0 à 7)
        /// <returns></returns>
        public static MyImage DrawJulia(int numero)
        {

            int height = 4320;
            int width = 7680;
            MyImage julia = new MyImage();
            julia._imageData = new Pixel[height, width];
            julia.FillImageWithWhite();
            julia.Height = height;
            julia.Width = width;
            ComplexNumber c;
            RectangleF rect = new RectangleF(0, 0, width, height);

            #region Les différentes valeurs de la fractale (Nombre Complexe)

            c = numero switch
            {
                1 => new ComplexNumber(0.3f, 0.5f),
                2 => new ComplexNumber(0.285f, 0.01f),
                3 => new ComplexNumber(-0.038088f, 0.9754633f),
                4 => new ComplexNumber(0.285f, 0.013f),
                5 => new ComplexNumber(-1.416f, 0f),
                6 => new ComplexNumber(-0.4f, 0.6f),
                7 => new ComplexNumber(-0.8f, 0.156f),
                _ => new ComplexNumber(-1.0f, 0f),
            };
            #endregion

            RectangleF rectF = new RectangleF(-2.0f, -2.0f, 4.0f, 4.0f);
            for (int x = 0; x < height; x++)
            {

                for (int y = 0; y < width; y++)
                {
                    // Create a complex number for the current pixel
                    ComplexNumber z = new ComplexNumber(
                        rectF.Left + x * (rectF.Right - rectF.Left) / (height),
                        rectF.Top + y * (rectF.Bottom - rectF.Top) / (width));

                    // Iterate through the pixels
                    for (x = 0; x < height; x++)
                    {
                        // Create a complex number for the current pixel
                        z = new ComplexNumber(
                            rectF.Left + x * (rectF.Right - rectF.Left) / (height),
                            rectF.Top + y * (rectF.Bottom - rectF.Top) / (width));
                        // Itere sur le nomrbe complexe
                        int i = 0;
                        while (i < 255 && z.Magnitude < 8.0f)
                        {
                            z = z * z + c;
                            i++;
                        }

                        julia._imageData[x, y].Red = (byte)((9 * i) % 255); //8
                        julia._imageData[x, y].Green = (byte)((6 * i) % 255);//16
                        julia._imageData[x, y].Blue = (byte)((3 * i) % 255);//32
                    }
                }

            }
            return julia;

        }


        #region ComplexNumber
        // Complex number class 
        class ComplexNumber
        {
            // Real and imaginary parts
            public float Real;
            public float Imaginary;

            // Constructor
            public ComplexNumber(float real, float imaginary)
            {
                Real = real;
                Imaginary = imaginary;
            }

            // Magnitude
            public float Magnitude
            {
                get { return (float)Math.Sqrt(Real * Real + Imaginary * Imaginary); }
            }

            // Addition
            public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
            {
                return new ComplexNumber(a.Real + b.Real, a.Imaginary + b.Imaginary);
            }

            // Subtraction
            public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
            {
                return new ComplexNumber(a.Real - b.Real, a.Imaginary - b.Imaginary);
            }

            // Multiplication
            public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
            {
                return new ComplexNumber(a.Real * b.Real - a.Imaginary * b.Imaginary,
                    a.Real * b.Imaginary + a.Imaginary * b.Real);
            }

        }

        #endregion






        #endregion

        #endregion
        
        
        #region Histogramme des couleurs d'une photo
        /// <summary>
        /// Methode qui permet de calculer l'histogramme d'une image
        /// </summary>

        //fait une méthode qui cacule les valeurs de l'histogramme d'une image et qui le renvoie dans _imageData
        public void DrawHistogram() //histogramme des couleurs d'une photo
        {
            //Initialisation des variables
            Pixel[,] pix = new Pixel[_height, _width];
            //il faudrait trouver comment calculer les facteurs automatiquement

            //On parcourt l'image
            double coeflargeur = 1.245;   //coco 1.245      lac 3
            double coefhauteur = 0.086;//coco 0.086      lac 0.09
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    pix[i, j] = new Pixel(0, 0, 0);
                }
            }
            for (int r = 0; r < 256; r++)
            {
                int countR = 0;
                for (int k = 0; k < _height; k++)
                {
                    for (int l = 0; l < _width; l++)
                    {

                        if (_imageData[k, l].Red == r)
                        {
                            countR++;
                        }
                    }

                }

                for (int i = 0; i < Convert.ToInt32(countR * coefhauteur); i++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        pix[i, Convert.ToInt32((int)(r * coeflargeur)) + k].Red = 255;
                    }

                }
            }
            for (int g = 0; g < 256; g++)
            {
                int countG = 0;
                for (int k = 0; k < _height; k++)
                {
                    for (int l = 0; l < _width; l++)
                    {

                        if (_imageData[k, l].Green == g)
                        {
                            countG++;
                        }

                    }

                }

                for (int i = 0; i < Convert.ToInt32(countG * coefhauteur); i++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        pix[i, Convert.ToInt32((int)(g * coeflargeur)) + k].Blue = 255;
                    }


                }
            }
            for (int b = 0; b < 256; b++)
            {
                int countB = 0;
                for (int k = 0; k < _height; k++)
                {
                    for (int l = 0; l < _width; l++)
                    {

                        if (_imageData[k, l].Blue == b)
                        {
                            countB++;
                        }



                    }

                }

                for (int i = 0; i < Convert.ToInt32(countB * coefhauteur); i++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        pix[i, Convert.ToInt32((int)(b * coeflargeur)) + k].Green = 255;
                    }


                }
            }
            _imageData = pix;

        }


        #endregion


        #region Cacher/Décoder une image dans une image
        //réflexion faite, on pourrait juste travailler en Hexadécimal, faire un F0 (=11110000) & la valeur en octet de l'image pour Red, Green, Blue et c'est tt

        #region Cacher l'image
        /// <summary>
        /// Methode qui permet de cacher une image dans une autre
        /// </summary>
        /// <param name="imagecach"></param>
        public void CacherImage(MyImage imagecach)
        {

            if (imagecach._height > _height)
            {
                imagecach.Retrecir(Math.Ceiling((double)imagecach._height / (double)_height));
            }
            if (imagecach._width > _width)
            {
                imagecach.Retrecir(Math.Ceiling((double)imagecach._width / (double)_width));
            }

            if (imagecach._height < 2 * _height && imagecach._width < 2 * _width)
            {
                imagecach.Agrandir(2);
            }


            for (int i = 0; i < _height; i++)
            {
                //Console.WriteLine($"{i}");
                for (int j = 0; j < _width; j++)
                {
                    if (i < imagecach._height && j < imagecach._width)
                    {
                        byte[] octet = new byte[3] { _imageData[i, j].Red, _imageData[i, j].Green, _imageData[i, j].Blue };
                        byte[] octetcach = new byte[3] { imagecach._imageData[i, j].Red, imagecach._imageData[i, j].Green, imagecach._imageData[i, j].Blue };

                        for (int k = 0; k < 3; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                octet[k] = ValCachee(octet[k], Decalage(octetcach[k], l + 4), l + 4);//remplace la valeur de l'octet par sa nouvelle valeur (avec la valeur de l'image a cacher)
                            }
                        }
                        _imageData[i, j] = new Pixel(octet[0], octet[1], octet[2]);
                    }
                    else
                    {
                        byte[] octet = new byte[3] { _imageData[i, j].Red, _imageData[i, j].Green, _imageData[i, j].Blue };
                        for (int k = 0; k < 3; k++)
                        {
                            for (int l = 0; l < 4; l++)
                            {
                                octet[k] = ValCachee(octet[k], Decalage(255, l + 4), l + 4);//remplace la valeur de l'octet par 255 (couleur neutre et arbitraire) lorsque l'on a dépassé la hauteur ou la largeur de l'image à cacher
                            }
                        }
                        _imageData[i, j] = new Pixel(octet[0], octet[1], octet[2]);//on applique les nouvelles valeurs du pixel après incrustation de l'image a cacher
                    }
                }

            }
        }
        #region Méthodes pour convertir et utiliser les octets et valeurs en base de 2
        static int Base2aInt(string s)//ok
        {
            int a = 0;
            for (int i = 0; i < 8; i++)
            {
                a += (int)((s[i] - 48) * Math.Pow(2, 7 - i));//passage de binaire à int (<255)
            }
            return a;
        }
        public static int Decalage(byte name, int position)
        {
            int decalage = 1 << position;
            return (name & decalage) >> position;//retourne la valeur rajoutée à chaque pixel en base de 2 (plusieurs valeurs)
        }
        public static string CompleterBits(byte name, int length)
        {
            string c = Convert.ToString(name, toBase: 2);//passage de int en base de 2
            while (c.Length != length) { c = "0" + c; }//complète la chaine de caractère en rajoutant des bits de poids forts nuls
            return c;
        }
        public static byte ValCachee(byte name, int val, int position)
        {
            string cache = CompleterBits(name, 8);
            string nouvs = "";
            for (int i = 0; i < 8; i++)
            {
                nouvs += (i != position) ? cache[i] - 48 : val; //cache la valeur de du pixel dans la collection
            }
            return (byte)Base2aInt(nouvs);//passage de la collection en int (on se rend compyte que la valeur ne diverge pas énormément (15 maximum) sur les 4 bits de poids faibles
        }

        #endregion
        #endregion


        #region Decoder l'image 
        /// <summary>
        /// Méthode qui décode l'image en fonction de la taille de l'image à cacher
        /// </summary>
        /// <returns></returns>
        public MyImage DecoderImage()
        {

            MyImage imagecach = new MyImage("../../../Images/Test5.bmp");
            Pixel[,] matrice = new Pixel[_height, _width];
            imagecach._height = _height;
            imagecach._width = _width;

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    byte[] octet = new byte[3] { 0, 0, 0 };
                    byte[] octetcach = new byte[3] { _imageData[i, j].Red, _imageData[i, j].Green, _imageData[i, j].Blue };

                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 4; l++)
                        {
                            octet[k] = ValCachee(octet[k], Decalage(octetcach[k], l), l);
                        }

                        _imageData[i, j] = new Pixel(octet[0], octet[1], octet[2]);
                    }
                }
            }

            imagecach._imageData = matrice;

            return imagecach;
        }
        #endregion

        #endregion


    }
    #endregion

}

