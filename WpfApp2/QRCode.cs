// ReSharper disable All

using ReedSolomon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
//using System.Range;
//using System.Index;

namespace WpfApp2
{
    public class QRCode : MyImage
    {
        
        #region Constructeur basique et Attributs

        private int _version;//Version du QRCode
        private int[] _modeindicator;//Indicateur de mode de codage (alphanumerique ici)
        private int _contours;//Nombre de contours
        private static Dictionary<char, int> _alphanum = new Dictionary<char, int>() { { '0', 0 }, { '1', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 }, { '7', 7 }, { '8', 8 }, { '9', 9 }, { 'A', 10 }, { 'B', 11 }, { 'C', 12 }, { 'D', 13 }, { 'E', 14 }, { 'F', 15 }, { 'G', 16 }, { 'H', 17 }, { 'I', 18 }, { 'J', 19 }, { 'K', 20 }, { 'L', 21 }, { 'M', 22 }, { 'N', 23 }, { 'O', 24 }, { 'P', 25 }, { 'Q', 26 }, { 'R', 27 }, { 'S', 28 }, { 'T', 29 }, { 'U', 30 }, { 'V', 31 }, { 'W', 32 }, { 'X', 33 }, { 'Y', 34 }, { 'Z', 35 }, { ' ', 36 }, { '$', 37 }, { '%', 38 }, { '*', 39 }, { '+', 40 }, { '-', 41 }, { '.', 42 }, { '/', 43 }, { ':', 44 } };
        //Dictionnaire des caractères alphanumériques et leur valeur
        private int _taillemessage;//Taille du message entré
        private List<int> _chainbits;//Liste des bits du message (sans les bits d'erreurs)
        private int _mask;//Numéro du masque de correction
        private int _taillemodule;//Taille d'un module
        private int[] _bitwords;//Liste des bits du message (avec les bits d'erreurs)
        private int _nivcorrection;//Niveau de correction (de 1 à 4)
        private int _datacode;//taille de _chainbits en bits total pour cette version et ce niveau de correction
        private int _errordata;//taille des bits d'erreurs pour cette version et ce niveau de correction
        private string _message;//message entré
        private int _nbecblock;//nombre d'EC codewords par block
        private int _nbblocksG1;//nombre de blocks du groupe 1
        private int _nbblocksG2;//nombre de blocks du groupe 2
        private int _tailleblock1;//taille d'un block du groupe 1
        private int _tailleblock2;//taille d'un block du groupe 2


        public int Version
        {
            get => _version;
            set => _version = value;
        }

        public int[] ModeIndicator
        {
            get => _modeindicator;
            set => _modeindicator = value ?? throw new ArgumentNullException(nameof(value));
        }

        public int Contours
        {
            get => _contours;
            set => _contours = value;
        }

        public Dictionary<char, int> Alphanum
        {
            get => _alphanum;
            set => _alphanum = value ?? throw new ArgumentNullException(nameof(value));
        }

        public List<int> Chainbits
        {
            get => _chainbits;
            set => _chainbits = value;
        }

        public int Mask
        {
            get => _mask;
            set => _mask = value;
        }

        public int Taillemodule
        {
            get => _taillemodule;
            set => _taillemodule = value;
        }

        public int Nivcorrection
        {
            get => _nivcorrection;
            set => _nivcorrection = value;
        }

        public int Taillemessage
        {
            get => _taillemessage;
            set => _taillemessage = value;
        }

        public int Datacode
        {
            get => _datacode;
            set => _datacode = value;
        }

        public int ErrorData
        {
            get => _errordata;
            set => _errordata = value;
        }

        public int[] Bitwords
        {
            get => _bitwords;
            set => _bitwords = value;
        }

        public string Message
        {
            get => _message;
            set => _message = value;
        }

        public int Ecblock
        {
            get => _nbecblock;
            set => _nbecblock = value;
        }

        public int NbblocksG1
        {
            get => _nbblocksG1;
            set => _nbblocksG1 = value;
        }

        public int NbblocksG2
        {
            get => _nbblocksG2;
            set => _nbblocksG2 = value;
        }

        public int Tailleblock1
        {
            get => _tailleblock1;
            set => _tailleblock1 = value;
        }

        public int Tailleblock2
        {
            get => _tailleblock2;
            set => _tailleblock2 = value;
        }

        public QRCode(int version, int contours, List<int> chainbits, int mask, int taillemodule, int nivcorrection,
            Pixel[,] imageData, string paires, int[] modeindicator, int height, int width, string typeImage, int numberRgb,
            int offset)
        {
            _version = version;
            _contours = contours;
            _chainbits = chainbits;
            _mask = mask;
            _taillemodule = taillemodule;
            _nivcorrection = nivcorrection;
            ImageData = imageData;
            _modeindicator = modeindicator;
            Height = height;
            Width = width;
            TypeImage = typeImage;
            NumberRgb = numberRgb;
            Offset = offset;
        }

        #endregion
        
        
        #region Constructeurs test et final et écriture du QRCode

        /// <summary>
        /// Constructeur du QRCode
        /// </summary>
        /// <param name="version"></param>
        /// <param name="taillemodule"></param>
        /// <param name="contours"></param>
        /// <param name="masque"></param>
        /// <param name="message"></param>
        /// <param name="nivcorrection"></param>
        /// version du QRCode
        /// taille du module
        /// taille des contours
        /// niveau de correction
        /// masque
        /// message à coder (string)
        //Peut être séparé plus tard
        
        
        #region Constructeur test
        public QRCode(int version, int taillemodule, int contours, int masque, string message, int nivcorrection = 0)
        {

            _message = message;
            if (nivcorrection == 0)
            {
                MeilleurVersionEtNiveauDeCorrection();
            }
            else
            {
                _version = version;
                _nivcorrection = nivcorrection;
            }

            int bordsQR = (8 * 2 + (4 * _version + 1)) * taillemodule + 2 * contours;
            Height = bordsQR;
            Width = bordsQR;
            ImageData = new Pixel[Height, Width];
            MyImage QRCode = new MyImage(Height, Width, ImageData);
            _contours = contours;
            SizeFile = Height * Width * 3 + Offset;
            _taillemodule = taillemodule;
            Offset = 54;
            Ecriture = 1;
            NumberRgb = 24;
            //_version =version;
            var rdm = new Random();
            int mask = rdm.Next(8);
            _mask = mask;
            //_nivcorrection = nivcorrection;
            ModulesDeRecherches(0 + _contours, 0 + _contours);
            ModulesDeRecherches(0 + Height - (7 * _taillemodule) - _contours, 0 + _contours);
            ModulesDeRecherches(0 + _contours, 0 + Width - (7 * _taillemodule) - _contours);
            Separateurs(0 + _contours, 0 + _contours);
            Separateurs(0 + Height - 8 * _taillemodule - _contours, 0 + _contours);
            Separateurs(0 + _contours, 0 + Width - 8 * _taillemodule - _contours);
            EcritureMotifsAlignement();
            MotifsDeSynchro();
            DarkModule();
            //Dico();
            EcritureInfoVersionQRCode();
            EcritureInfoFormat();
            DataCodeAndErrorDataWords();

            MessageData(_message);
            EncodeErrorData();
            //ErrorCorrectionQRCode();


            MessageQRCode(_bitwords);

            //DeterminerMasqueFinal();

            QRCode.FillImageWithWhite(); //pour voir les modules non remplis


            this.From_Image_To_File($"../../../Images/QRCode_V{_version}_N{_nivcorrection}_M{_mask}.bmp");
            Console.WriteLine($"V:" + _version + " N:" + _nivcorrection + " M:" + _mask);

        }
        #endregion

        #region Constructeur final
        public QRCode(string message)
        {
            _version = 1;
            _nivcorrection = 0;
            var taillemodule = 5;
            var contours = 5;

            _message = message;

            MeilleurVersionEtNiveauDeCorrection();

            int bordsQR = (8 * 2 + (4 * _version + 1)) * taillemodule + 2 * contours;
            Height = bordsQR;
            Width = bordsQR;
            ImageData = new Pixel[Height, Width];
            MyImage QRCode = new MyImage(Height, Width, ImageData);
            _contours = contours;
            SizeFile = Height * Width * 3 + Offset;
            _taillemodule = taillemodule;
            Offset = 54;
            Ecriture = 1;
            NumberRgb = 24;
            //_version =version;
            //_mask = masque;
            //_nivcorrection = nivcorrection;
            var rdm = new Random();
            int mask = rdm.Next(8);
            _mask = mask;
            ModulesDeRecherches(0 + _contours, 0 + _contours);
            ModulesDeRecherches(0 + Height - (7 * _taillemodule) - _contours, 0 + _contours);
            ModulesDeRecherches(0 + _contours, 0 + Width - (7 * _taillemodule) - _contours);
            Separateurs(0 + _contours, 0 + _contours);
            Separateurs(0 + Height - 8 * _taillemodule - _contours, 0 + _contours);
            Separateurs(0 + _contours, 0 + Width - 8 * _taillemodule - _contours);
            EcritureMotifsAlignement();
            MotifsDeSynchro();
            DarkModule();
            //Dico();
            EcritureInfoVersionQRCode();
            EcritureInfoFormat();
            DataCodeAndErrorDataWords();

            MessageData(_message);
            EncodeErrorData();
            //ErrorCorrectionQRCode();


            MessageQRCode(_bitwords);

            //DeterminerMasqueFinal();

            QRCode.FillImageWithWhite(); //pour voir les modules non remplis


            //this.From_Image_To_File($"{filepath}");
            //Console.WriteLine($"V:" + _version + " N:" + _nivcorrection + " M:" + _mask);
        }

        #endregion
        
        #endregion
        

        #region Modules de Recherches

        /// <summary>
        /// Méthode qui permet d'écrire les modules de recherches dans le QRCode
        /// </summary>
        /// <param name="ligne"></param> Ligne où écrire les modules de recherches
        /// <param name="colonne"></param> Colonne où écrire les modules de recherches
        public void ModulesDeRecherches(int ligne, int colonne)
        {
            for (int i = ligne; i < _taillemodule * 7 + ligne; i++)
            {
                for (int j = colonne; j < _taillemodule * 7 + colonne; j++)
                {
                    if (i < _taillemodule + ligne || j < _taillemodule + colonne)
                    {
                        ImageData[i, j] = new Pixel(0, 0, 0);
                    }
                    else if (i > ligne + 6 * _taillemodule - 1 || j > colonne + 6 * _taillemodule - 1)
                    {
                        ImageData[i, j] = new Pixel(0, 0, 0);
                    }
                    else if (i >= ligne + 2 * _taillemodule && j >= colonne + 2 * _taillemodule &&
                             i <= ligne + 6 * _taillemodule - 2 * _taillemodule + (_taillemodule - 1) &&
                             j <= colonne + 6 * _taillemodule - 2 * _taillemodule + (_taillemodule - 1))
                    {
                        ImageData[i, j] = new Pixel(0, 0, 0);
                    }
                    else
                    {
                        ImageData[i, j] ??= new Pixel(255, 255, 255);
                    }
                }
            }
        }

        #endregion
        
        
        #region Séparateurs

        /// <summary>
        /// Méthode qui permet d'écrire les séparateurs dans le QRCode
        /// </summary>
        /// <param name="ligne"></param> Ligne où écrire les séparateurs
        /// <param name="colonne"></param> Colonne où écrire les séparateurs
        public void Separateurs(int ligne, int colonne)
        {
            for (int i = ligne; i < 8 * _taillemodule + ligne; i++)
            {
                for (int j = colonne; j < 8 * _taillemodule + colonne; j++)
                {
                    ImageData[i, j] ??= new Pixel(255, 255, 255);
                }
            }
        }

        #endregion
        
        
        #region Motifs de Synchronisation

        /// <summary>
        /// Méthode qui permet d'écrire les motifs de synchronisation dans le QRCode
        /// </summary>
        public void MotifsDeSynchro()
        {
            for (int i = 7 * _taillemodule + _contours; i <= Height - 7 * _taillemodule - _contours; i++)
            {
                for (int j = 6 * _taillemodule + _contours; j < 7 * _taillemodule + _contours; j++)
                {
                    if ((i - _contours) / _taillemodule % 2 == 0)
                    {
                        ImageData[i, j] ??= new Pixel(0, 0, 0);
                    }
                    else
                    {
                        ImageData[i, j] ??= new Pixel(255, 255, 255);
                    }
                    //On utilise le else pour vérifie l'écriture
                    /*
                    else
                    {
                        ImageData[i, j] = new Pixel(0,0,255);
                    }
                    */
                }
            }

            for (int j = 7 * _taillemodule + _contours; j <= Height - 7 * _taillemodule - _contours; j++)
            {
                for (int i = 6 * _taillemodule + _contours; i < 7 * _taillemodule + _contours; i++)
                {
                    if ((j - _contours) / _taillemodule % 2 == 0)
                    {
                        ImageData[i, j] = new Pixel(0, 0, 0);
                    }
                    else
                    {
                        ImageData[i, j] = new Pixel(255, 255, 255);
                    }

                }
            }



        }

        #endregion
        
        
        #region Dark Module

        /// <summary>
        /// Méthode qui permet d'écrire le module noir dans le QRCode
        /// </summary>
        public void DarkModule()
        {
            for (int j = 8 * _taillemodule + _contours; j < _taillemodule * 9 + _contours; j++)
            {
                for (int i = (4 * _version + 9) * _taillemodule + _contours;
                     i < _taillemodule + (4 * _version + 9) * _taillemodule + _contours;
                     i++)
                {
                    ImageData[i, j] = new Pixel(0, 0, 0);
                }
            }
        }

        #endregion
        
        
        #region Motifs d'Alignements

        #region Ecriture des motifs d'alignements

        /// <summary>
        /// Méthode qui permet d'écrire les motifs d'alignements dans le QRCode
        /// </summary>
        public void EcritureMotifsAlignement()
        {

            //voir https://askcodez.com/generer-toutes-les-combinaisons-pour-une-liste-de-chaines-de-caracteres.html
            //Il faut qu'on réussisse à faire une combinaison de toutes les coordonées des Motifs d'Alignements
            //le problème est de savoir ensuite quels motifs il faut mettre et lequels il faut enlever(superposition)
            if (_version < 2) return;

            var coordonees = Coordonees(@"../../Coordonées.txt", _version);

            foreach (var intArray in coordonees)
            {
                MotifsAlignement(intArray[0], intArray[0]);
                MotifsAlignement(intArray[0], intArray[1]);
                MotifsAlignement(intArray[1], intArray[0]);
                MotifsAlignement(intArray[1], intArray[1]);

            }


        }

        #endregion

        #region Méthode qui permet d'écrire les motifs d'alignements en fonction des coordonées

        /// <summary>
        /// Méthode qui permet d'écrire les motifs d'alignements en fonction des coordonées
        /// </summary>
        /// <param name="ligne"></param> Ligne où écrire les motifs d'alignements
        /// <param name="colonne"></param> Colonne où écrire les motifs d'alignements
        public void MotifsAlignement(int ligne, int colonne)
        {
            if (ImageData[ligne, colonne] != null) return;
            for (int i = ligne - 2 * _taillemodule; i <= ligne + 2 * _taillemodule + (_taillemodule - 1); i++)
            {
                for (int j = colonne - 2 * _taillemodule; j <= colonne + 2 * _taillemodule + (_taillemodule - 1); j++)
                {

                    int newligne = ((2 * _taillemodule - ligne) + i) / _taillemodule;
                    int newcolonne = ((2 * _taillemodule - colonne) + j) / _taillemodule;

                    if (newligne == 0 || newcolonne == 0 || newligne == 4 || newcolonne == 4)
                    {
                        ImageData[i, j] = new Pixel(0, 0, 0);
                    }
                    else if (newligne != 0 && newcolonne != 0 && newligne != 4 && newcolonne != 4)
                    {
                        if (newligne == 2 && newcolonne == 2)
                        {
                            ImageData[i, j] = new Pixel(0, 0, 0);
                        }
                        else
                        {
                            ImageData[i, j] = new Pixel(255, 255, 255);
                        }
                    }



                }
            }

        }

        #endregion

        #region Méthode qui permet de récupérer les coordonées des motifs d'alignements

        /// <summary>
        /// Méthode qui permet de récupérer les coordonées des motifs d'alignements
        /// </summary>
        /// <param name="nomfichier"></param> nom du fichier
        /// <param name="version"></param> version du QRCode
        /// <returns></returns> Liste des coordonées des motifs d'alignements
        public int[][] Coordonees(string nomfichier, int version) // lecture du fichier pour determiner les coordonnées
        {
            StreamReader? flux = null;
            string? lines;
            int i = 0;
            int[] coordonee = new int[] { };
            try
            {
                flux = new StreamReader(nomfichier);
                while ((lines = flux.ReadLine()) != null)
                {

                    if (i == _version - 2)
                    {
                        string[] ligne = lines.Split(' ');
                        coordonee = new int[ligne.Length - 1];
                        for (int j = 0; j < ligne.Length - 1; j++)
                        {
                            coordonee[j] = (Convert.ToInt32(ligne[j + 1])) * _taillemodule + _contours;
                        }
                    }

                    i++;
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(" Le fichier spécifié " + nomfichier +
                                  " est introuvable, veuillez réessayer.\nMessage d'erreur : \n" + e.Message);
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
            finally
            {
                if (flux != null) flux.Close();
            }

            var donees = Combinaisons(coordonee);

            return donees.Where(x => x.Length == 2).ToArray();
        }

        #endregion

        #endregion
        
        
        #region Version du QRCode et Ecriture Version

        #region Méthode qui permet de récupérer la version du QRCode

        /// <summary>
        /// Méthode qui permet de récupérer la version du QRCode
        /// </summary>
        /// <returns></returns> version du QRCode
        public int[] InfoVersionQRCode()
        {

            StreamReader? flux = null;
            int i = 0;
            string? lines;
            int[] final = new int[] { };
            if (_version < 7)
            {
                return Array.Empty<int>();
            }

            try
            {
                flux = new StreamReader(@"../../InfosVersionQRCode.txt");
                while ((lines = flux.ReadLine()) != null)
                {
                    if (i == _version - 7)
                    {
                        string[] ligne = lines.Split(' ');
                        final = new int[ligne[1].Length];
                        for (int j = 0; j < ligne[1].Length; j++)
                        {
                            final[j] = (int)ligne[1][j] - 48;
                            //Console.WriteLine(final[j]);debug
                        }
                    }

                    i++;
                }


            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(" Le fichier spécifié " + "InfosVersionQRCode.txt" +
                                  " est introuvable, veuillez réessayer.\nMessage d'erreur : \n" + e.Message);
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
            finally
            {
                if (flux != null) flux.Close();
            }

            return final;
        }

        #endregion

        #region Méthode qui permet d'écrire la version du QRCode dans le QRCode

        /// <summary>
        /// Méthode qui permet d'écrire la version du QRCode dans le QRCode
        /// </summary>
        public void EcritureInfoVersionQRCode()
        {
            if (_version < 7)
            {
                return;
            }
            else
            {
                int[] couleur = InfoVersionQRCode();
                for (int i = 0; i < couleur.Length; i++)
                {
                    Pixel pixel = couleur[i] == 1 ? new Pixel(0, 0, 0) : new Pixel(255, 255, 255);
                    int ligne = (2 - i % 3) * _taillemodule + ImageData.GetLength(0) - 11 * _taillemodule - _contours;
                    int colonne = (5 - i / 3) * _taillemodule + _contours;
                    for (int a = ligne; a < _taillemodule + ligne; a++)
                    {
                        for (int b = colonne; b < _taillemodule + colonne; b++)
                        {
                            ImageData[a, b] = pixel;
                            ImageData[b, a] = pixel;
                        }
                    }
                }
            }
        }

        #endregion

        #endregion
        
        
        #region Format QRCode et Ecriture des Info du Format


        #region Récupération des informations du format du QRCode sous un int[]

        /// <summary>
        /// Méthode qui permet de récupérer les informations du format du QRCode sous un int[]
        /// </summary>
        /// <returns></returns> informations du format du QRCode sous un int[]
        public int[] InfoFormatQRcode()
        {
            StreamReader? flux = null;
            int i = 0;
            int k = 0;
            if (_nivcorrection == 2) k = 8;
            if (_nivcorrection == 3) k = 16;
            if (_nivcorrection == 4) k = 24;

            string? lines;
            int[] final = new int[] { };
            try
            {
                flux = new StreamReader(@"../../InfosFormatQRCode.txt");
                while ((lines = flux.ReadLine()) != null)
                {
                    if (i == _mask + k)
                    {
                        string[] ligne = lines.Split(' ');
                        final = new int[ligne[2].Length];
                        for (int j = 0; j < ligne[2].Length; j++)
                        {
                            final[j] = (int)ligne[2][j] - 48;
                            //Console.Write(final[j]);
                        }
                    }

                    i++;
                }


            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(" Le fichier spécifié " + "InfosFormatQRCode.txt" +
                                  " est introuvable, veuillez réessayer.\nMessage d'erreur : \n" + e.Message);
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
            finally
            {
                if (flux != null) flux.Close();
            }

            return final;



        }

        #endregion


        #region Ecriture du Format sur le QRCode

        /// <summary>
        /// Méthode qui permet d'écrire le format du QRCode dans le QRCode
        /// </summary>
        public void EcritureInfoFormat()
        {
            int[] infoformat = InfoFormatQRcode();
            int colonne = 8 * _taillemodule + _contours;
            int ligne = 8 * _taillemodule + _contours;

            int i = Height - _contours - _taillemodule;
            int j = 0 + _contours;
            foreach (int n in infoformat)
            {
                if (ImageData[i, colonne] != null) i -= _taillemodule;
                if (ImageData[ligne, j] != null) j += _taillemodule;

                for (int l = 0; l < _taillemodule; l++)
                {
                    for (int c = 0; c < _taillemodule; c++)
                    {
                        ImageData[ligne + l, j + c] = n == 0 ? new Pixel(255, 255, 255) : new Pixel(0, 0, 0);
                        ImageData[i + l, colonne + c] = n == 0 ? new Pixel(255, 255, 255) : new Pixel(0, 0, 0);
                    }
                }

                if (i - _taillemodule == (4 * _version + 9) * _taillemodule + _contours)
                {
                    i = 8 * _taillemodule + _contours;
                    continue;
                }

                if (j == 8 * _taillemodule + _contours)
                {
                    j = ImageData.GetLength(0) - 7 * _taillemodule - _contours;
                    continue;
                }

                for (int l = 0; l < _taillemodule; l++)
                {
                    for (int c = 0; c < _taillemodule; c++)
                    {
                        ImageData[ligne + l, ImageData.GetLength(0) - 8 * _taillemodule - _contours + c] =
                            ImageData[ligne + l, ImageData.GetLength(0) - 7 * _taillemodule - _contours + c];
                    }
                }

                i -= _taillemodule;
                j += _taillemodule;
            }




        }


        #endregion


        #endregion
        
        
        #region Identification de la meilleur version et du meilleur masque pour le QRCode

        /// <summary>
        /// Méthode qui permet de trouver la meilleure version du QRCode et le meilleur niveau de correction pour le QRCode
        /// </summary>
        public void MeilleurVersionEtNiveauDeCorrection()
        {
            int taillemessage = _message.Length;
            var taille = CatchFile("../../CharacterCapacitiesQRCodeOrdered.txt").ToArray();
            int i = 0;
            //find the best version for the message length in Char CharacterCapacitiesQRCodeOrdered.txt
            foreach (var n in taille)
            {
                var readligne = taille[i].Split(',');

                var tailletest = Convert.ToInt32(readligne[0]);
                if (tailletest >= taillemessage)
                {
                    _version = Convert.ToInt32(readligne[1]);
                    _nivcorrection = Convert.ToInt32(readligne[2]);
                    break;
                }

                i++;


            }



        }

        #endregion
        
        
        #region Récupération de la taille de la chaine de cractère dans CharacterCapacitiesQRCode

        /// <summary>
        /// Méthode qui permet de récupérer la taille de la chaine de caractère dans CharacterCapacitiesQRCode
        /// </summary>
        public void DataCodeAndErrorDataWords()
        {
            var ligne = CatchFile($"../../CharacterCapacitiesQRCode.txt").ToArray();
             //vérifier si ce n'est pas la valeur entiere correspondant plutôt

             var readligne = ligne[(_version - 1) * 4 + _nivcorrection - 1];
            var finalinfo = readligne.Split(';');
            var final = new int[finalinfo.Length - 2];
            for (int n = 1; n < finalinfo.Length - 1; n++)
            {
                final[n - 1] = Convert.ToInt32(finalinfo[n]);
            }

            _datacode = final[0];
            _errordata = final[1] * final[2] + final[1] * final[4];
            _nbecblock = final[1];
            _nbblocksG1 = final[2];
            _nbblocksG2 = final[4];
            _tailleblock1 = final[3];
            _tailleblock2 = final[5];
        }

        #endregion
        
        
        #region Calcul des Datas et Initialisation du Dictionary

        #region Calcul des Datas Totales

        /// <summary>
        /// Méthode qui permet de calculer les datas totales
        /// </summary>
        /// <returns></returns> nombre de bits total (int)
        public int TotalData()
        {
            return _datacode + _errordata;
        }

        #endregion

        #region Passage des datas en array

        /// <summary>
        /// Méthode qui permet de passer les datas en array
        /// </summary>
        /// <returns></returns> array des datas
        public int[] DataByteEncoding()
        {
            return BitToByte(_chainbits.ToArray());
        }

        #endregion

        #region Initialisation du Dictionary

        /// <summary>
        /// Méthode qui permet d'initialiser le Dictionary
        /// </summary>
        public static void Dico()
        {

            var Dico = CatchFile("../../AlphanumQRCode.txt");
            foreach (var n in Dico)
            {
                var args = n.Split('=');
                _alphanum.Add(Convert.ToChar(args[0]), Convert.ToInt32(args[1]));
            }
        }

        #endregion

        #endregion
        
        
        #region Masques EncodageQRCode

        /// <summary>
        /// Méthode qui permet de calculer les masques pour chaque bit
        /// </summary>
        /// <param name="j"></param> hauteur
        /// <param name="i"></param> largeur
        /// <returns></returns> test du masque (bool) (true or false)
        public bool MasqueQRCode(int j, int i)
        {
            bool boole = _mask
                switch
            {
                0 => (j + i) % 2 == 0,
                1 => (j) % 2 == 0,
                2 => (i) % 3 == 0,
                3 => (j + i) % 3 == 0,
                4 => ((j / 2) + (i / 3)) % 2 == 0,
                5 => ((j * i) % 2) + ((j * i) % 3) == 0,
                6 => (((j * i) % 2) + ((j * i) % 3)) % 2 == 0,
                7 => (((j + i) % 2) + ((j * i) % 3)) % 2 == 0,
                _ => (j) % 2 == 0,
            };
            return boole;

        }

        #endregion
        
        
        #region Encodage du message

        /// <summary>
        /// Méthode qui permet d'encoder le message sous forme de bits
        /// </summary>
        /// <param name="message"></param> message à encoder
        public void MessageData(string message)
        {
            message = message.ToUpper();
            _modeindicator = new int[] { 0, 0, 1, 0 };
            List<int> final = new List<int>(_modeindicator);
            var messagelength = message.Length;
            var bin = Convert.ToString(messagelength, 2);
            var term = new int[bin.Length];
            for (int i = 0; i < bin.Length; i++)
            {
                term[i] = bin[i] - 48;
            }

            var modif = UnShift(term, _version < 10 ? 9 : _version < 27 ? 11 : 13);
            final.AddRange(modif);

            for (int k = 0; k <= message.Length - 1; k += 2)
            {
                if (k % 2 == 0 && k != message.Length - 1)
                {
                    _alphanum.TryGetValue(message[k], out var value1);
                    _alphanum.TryGetValue(message[k + 1], out var value2);
                    var value = value1 * 45 + value2;
                    var bin1 = Convert.ToString(value, 2);
                    var term1 = new int[bin1.Length];
                    for (int i = 0; i < bin1.Length; i++)
                    {
                        term1[i] = bin1[i] - 48;
                    }

                    var modif1 = UnShift(term1, 11);
                    final.AddRange(modif1);

                }

                if (k == message.Length - 1 && message.Length % 2 == 1)
                {
                    _alphanum.TryGetValue(message[k], out var index);
                    var bin2 = Convert.ToString(index, 2);
                    var term2 = new int[bin2.Length];
                    for (int i = 0; i < bin2.Length; i++)
                    {
                        term2[i] = bin2[i] - 48;
                    }

                    var modif2 = UnShift(term2, 6);
                    final.AddRange(modif2);
                }




            }

            var n = 1;

            while (final.Count < _datacode * 8 && n <= 4)
            {
                final.Add(0);
                n++;
            }

            final = final.Count % 8 != 0 ? Pad(final.ToArray(), final.Count + (8 - final.Count % 8)).ToList() : final;
            if (final.Count < _datacode * 8)
            {
                var add1 = Convert.ToString(236, 2);
                var add2 = Convert.ToString(17, 2);
                var term3 = new int[add1.Length];
                for (int i = 0; i < add1.Length; i++)
                {
                    term3[i] = add1[i] - 48;
                }

                var modif3 = UnShift(term3, 8);

                var term4 = new int[add2.Length];
                for (int i = 0; i < add2.Length; i++)
                {
                    term4[i] = add2[i] - 48;
                }

                var modif4 = UnShift(term4, 8);

                var val2 = (_datacode * 8 - final.Count);

                for (int j = 0; j < val2 / 8; j++) //a revoir
                {
                    final.AddRange(j % 2 == 0 ? modif3 : modif4);
                }


            }

            _chainbits = final;



        }

        #endregion

        
        #region ErrorCorrection

        //A terminer
        /// <summary>
        /// Méthode qui permet de calculer le nombre de bits d'erreur à ajouter
        /// </summary>
        public void ErrorCorrectionQRCode()
        {
            var ggf = new GenericGF(285, 256, 0);
            var reed = new ReedSolomonEncoder(ggf);
            var data = _chainbits.ToArray();

            var errordata = _errordata;

            var array1 = Enumerable.Repeat(0, errordata);
            var bytes = DataByteEncoding().Concat(array1).ToArray();
            reed.Encode(bytes, errordata);
            _bitwords = ByteToBit(bytes);
        }

        #endregion
        
        
        #region Ecriture du message

        /// <summary>
        /// Methode qui ecrit le message convertis en bit dans le QRCode
        /// </summary>
        /// <param name="tab"></param> le tableau de bit
        public void MessageQRCode(int[] tab)
        {

            for (int i = 0; i < _bitwords.Length; i++)
            {
                Console.Write(_bitwords[i]);
            }

            Console.WriteLine("");

            var bas = true;
            var spec = true;
            var compteur = 0;


            for (var i = Width - _taillemodule - _contours; i > _contours; i -= 2 * _taillemodule)
            {
                if (i <= (7 * _taillemodule + _contours) && spec == true)
                {
                    i = i - _taillemodule;
                    spec = false;
                }

                if (compteur >= tab.Length) break;
                if (bas)
                {
                    if (compteur >= tab.Length) break;
                    for (var j = Height - _taillemodule - _contours; j > _contours; j -= _taillemodule)
                    {

                        if (compteur >= tab.Length) break;
                        if (ImageData[j, i] == null)
                        {

                            if (tab[compteur] == 0) //blanc
                            {

                                var testbool = MasqueQRCode(j - _contours, i - _contours);

                                if (testbool == true)
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c] = new Pixel(0, 0, 0); //switch en noir
                                        }
                                    }

                                    Console.Write("1");
                                }
                                else
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c] = new Pixel(255, 255, 255); //reste blanc
                                        }
                                    }

                                    Console.Write("0");
                                }


                            }
                            else if (tab[compteur] == 1) //noir
                            {

                                var testbool = MasqueQRCode(j - _contours, i - _contours);


                                if (testbool == true)
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c] = new Pixel(255, 255, 255); //witch en blanc
                                        }
                                    }

                                    Console.Write("0");
                                }
                                else
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c] = new Pixel(0, 0, 0); //reste noir
                                        }
                                    }

                                    Console.Write("1");
                                }


                            }

                            compteur++;
                        }


                        if (ImageData[j, i - _taillemodule] == null)
                        {
                            if (tab[compteur] == 0) //blanc
                            {
                                var testbool = MasqueQRCode(j - _contours, i - _taillemodule - _contours);


                                if (testbool == true)
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c - _taillemodule] = new Pixel(0, 0, 0); //switch en noir
                                        }
                                    }

                                    Console.Write("1");
                                }
                                else
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c - _taillemodule] =
                                                new Pixel(255, 255, 255); //reste blanc
                                        }
                                    }

                                    Console.Write("0");
                                }


                            }
                            else if (tab[compteur] == 1) //noir
                            {
                                var testbool = MasqueQRCode(j - _contours, i - _taillemodule - _contours);

                                if (testbool == true)
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c - _taillemodule] =
                                                new Pixel(255, 255, 255); //switch en blanc
                                        }
                                    }

                                    Console.Write("0");
                                }
                                else
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c - _taillemodule] = new Pixel(0, 0, 0); //reste noir
                                        }
                                    }

                                    Console.Write("1");
                                }

                            }

                            compteur++;
                        }



                    }
                    //Console.Write(" ");

                }

                if (compteur >= tab.Length) break;


                else
                {

                    if (compteur >= tab.Length) break;
                    for (var j = _contours; j < Height - _contours; j += _taillemodule)
                    {

                        if (compteur >= tab.Length) break;

                        if (ImageData[j, i] == null)
                        {
                            if (tab[compteur] == 0)
                            {

                                var testbool = MasqueQRCode(j - _contours, i - _contours);


                                if (testbool == true) //blanc
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c] = new Pixel(0, 0, 0); //switch en noir
                                        }
                                    }

                                    Console.Write("1");

                                }
                                else
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c] = new Pixel(255, 255, 255); //reste blanc
                                        }
                                    }

                                    Console.Write("0");
                                }


                            }
                            else if (tab[compteur] == 1) //noir
                            {

                                var testbool = MasqueQRCode(j - _contours, i - _contours);


                                if (testbool == true)
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c] = new Pixel(255, 255, 255); //switch en blanc
                                        }
                                    }

                                    Console.Write("0");
                                }
                                else
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c] = new Pixel(0, 0, 0); //reste noir

                                        }
                                    }

                                    Console.Write("1");
                                }

                            }

                            compteur++;
                        }

                        if (compteur >= tab.Length) break;

                        if (ImageData[j, i - _taillemodule] == null)
                        {
                            if (tab[compteur] == 0) //blanc
                            {
                                var testbool = MasqueQRCode(j - _contours, i - _taillemodule - _contours);

                                if (testbool == true)
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c - _taillemodule] = new Pixel(0, 0, 0); //switch en noir
                                        }
                                    }

                                    Console.Write("1");
                                }
                                else
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c - _taillemodule] =
                                                new Pixel(255, 255, 255); //reste blanc
                                        }
                                    }

                                    Console.Write("0");
                                }

                            }
                            else if (tab[compteur] == 1) //noir
                            {

                                var testbool = MasqueQRCode(j - _contours, i - _taillemodule - _contours);


                                if (testbool == true)
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c - _taillemodule] =
                                                new Pixel(255, 255, 255); //switch en blanc
                                        }
                                    }

                                    Console.Write("0");
                                }
                                else
                                {
                                    for (int l = 0; l < _taillemodule; l++)
                                    {
                                        for (int c = 0; c < _taillemodule; c++)
                                        {
                                            ImageData[j + l, i + c - _taillemodule] = new Pixel(0, 0, 0); //reste noir
                                        }
                                    }

                                    Console.Write("1");
                                }

                            }

                            compteur++;
                        }


                    }
                    //Console.Write(" ");


                }

                bas = !bas;

            }

            Console.Write("\n");
        }

        #endregion
        
        
        #region Autres méthodes

        /// <summary>
        /// Métode qui permet d'effectuer des combinaisons de int
        /// </summary>
        /// <param name="source"></param> Array de int à combiner
        /// <typeparam name="T"></typeparam> Type de l'array
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception> Si l'array est null
        public static IEnumerable<T[]> Combinaisons<T>(IEnumerable<T> source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            var data = source.ToArray();

            return Enumerable
                .Range(0, 1 << (data.Length))
                .Select(index => data
                    .Where((v, i) => (index & (1 << i)) != 0)
                    .ToArray());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="array"></param>
        /// <param name="desiredLength"></param> Taille du tableau voulu
        /// <returns></returns>
        public static int[] TrimAndPad(int[] array, int desiredLength)
        {
            while (array[0] == 0) array = array.Skip(1).ToArray();
            var zerosArray = Enumerable.Repeat(0, desiredLength - array.Length);
            return array.Concat(zerosArray).ToArray();
        }

        /// <summary>
        /// Méthode qui permet d'ajouter des 0 à gauche d'un tableau d'int
        /// </summary>
        /// <param name="array"></param> Tableau d'int à modifier
        /// <param name="desiredLength"></param> Taille finale du tableau
        /// <returns></returns>
        public static int[] Pad(int[] array, int desiredLength)
        {
            var zerosArray = Enumerable.Repeat(0, desiredLength - array.Length);
            return array.Concat(zerosArray).ToArray();
        }

        /// <summary>
        /// Méthode qui permet d'ajouter des 0 à droite d'un tableau d'int
        /// </summary>
        /// <param name="array"></param> Tableau d'int à modifier
        /// <param name="desiredLength"></param> Taille finale du tableau
        /// <returns></returns>
        public static int[] UnShift(int[] array, int desiredLength)
        {
            var zerosArray = Enumerable.Repeat(0, desiredLength - array.Length);
            return zerosArray.Concat(array).ToArray();
        }

        /// <summary>
        /// Méthode qui saute les 0 d'un tableau d'int
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int[] Trim(int[] array)
        {
            while (array[0] == 0) array = array.Skip(1).ToArray();
            return array;
        }

        /// <summary>
        /// Méthode ou exclusif
        /// </summary>
        /// <param name="x"></param> Tableau à tester
        /// <param name="y"></param> Tableau à tester
        /// <returns></returns>
        public static int[] XOR(int[] x, int[] y)
        {
            var result = new int[x.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = x[i] == 1 && y[i] == 1 ? 0 : x[i] != y[i] ? 1 : 0;
            }

            return result;
        }

        /// <summary>
        /// Méthode qui transforme la data d'un fichier pour la lire
        /// </summary>
        /// <param name="path"></param> Chemin du fichier
        /// <returns></returns>
        /// <exception cref="IOException"></exception> Si le fichier n'existe pas
        public static IEnumerable<string> CatchFile(string path)
        {
            var lignes = new Stack<string>();
            
                using var sr = new StreamReader(path);
                string line;
                while ((line = sr.ReadLine()!) != null)
                {
                    lignes.Push(line);
                }
            
            

            return lignes.ToArray().Reverse();
        }

        /// <summary>
        /// Méthode qui transforme un tableau d'int(bit) en int(Byte)
        /// </summary>
        /// <param name="data"></param> Tableau d'int(bit)
        /// <returns></returns>
        public static int[] BitToByte(int[] data)
        {
            var final = new int[data.Length / 8];
            var bytes = 0;
            for (int i = 0; i < data.Length; i++)
            {
                bytes += (int)(Math.Pow(2, Math.Abs(i % 8 - 7)) * data[i]);
                if (i != 0 && (i + 1) % 8 == 0)
                {
                    final[i / 8] = bytes;
                    bytes = 0;
                }
            }

            return final;
        }

        /// <summary>
        /// Méthode qui transforme un tableau d'int(Byte) en int(bit)
        /// </summary>
        /// <param name="data"></param> Tableau d'int(Byte)
        /// <returns></returns>
        public static int[] ByteToBit(int[] data)
        {
            var result = new int[data.Length * 8];
            for (int i = 0; i < result.Length; i++)
            {
                var division = (int)(data[i / 8] / Math.Pow(2, Math.Abs(i % 8 - 7)));
                result[i] = division;
                data[i / 8] -= (int)(Math.Pow(2, Math.Abs(i % 8 - 7)) * division);
            }

            return result;
        }

        #endregion
        
        
        #region Méthode pour déterminer le meilleur masque( pas terminée )
        /// <summary>
        /// Méthode qui détermine le meilleur masque
        /// </summary>
        /// <returns></returns>
        public bool DeterminerMasqueFinal()
        {


            var total = 0;
            var totfinal = Math.Pow(10, 1000);
            var masquefinal = 0;
            for (int i = 0; i < 8; i++)
            {
                _mask = i;
                QRCode test = new QRCode(_version, _taillemodule, _contours, _mask, _message, _nivcorrection);
                total = test.Regle1() + test.Regle2() + test.Regle3() + test.Regle4();
                if (totfinal > total)
                {
                    masquefinal = i;
                    totfinal = total;
                }
            }

            _mask = masquefinal;
            return true;


        }

        #region Regle1
        /// <summary>
        /// Méthode qui permet de calculer le total d'erreurs de la règle 1
        /// </summary>
        /// <returns></returns> Le total d'erreurs
        public int Regle1() //marche en théorie
        {
            int countNC = 0;
            int countBC = 0;
            int countNL = 0;
            int countBL = 0;
            var total = 0;

            for (int i = _contours; i < Width - _contours; i++)
            {
                for (int j = _contours; j < Height - _contours; j++)
                {
                    if (new Pixel(0, 0, 0) == ImageData[i, j])
                    {
                        countNC++;
                    }

                    if (new Pixel(255, 255, 255) == ImageData[i, j])
                    {
                        countBC++;
                    }

                    if (new Pixel(0, 0, 0) == ImageData[j, i])
                    {
                        countNL++;
                    }

                    if (new Pixel(255, 255, 255) == ImageData[j, i])
                    {
                        countBL++;
                    }

                    for (int k = 5; k < Height - _contours; k++)
                    {
                        if (countNC + k == countBC || countBC + k == countNC)
                        {
                            total += k - 2;
                        }

                        if (countNL + k == countBL || countBL + k == countNL)
                        {
                            total += k - 2;
                        }
                    }

                }

            }

            return total;
        }

        #endregion

        #region Regle2
        /// <summary>
        /// Méthode qui permet de calculer le total d'erreurs de la règle 2
        /// </summary>
        /// <returns></returns> Le total d'erreurs
        public int Regle2()
        {
            int total = 0;
            for (int i = _contours; i < Height - _contours - _taillemodule; i++)
            {
                for (int j = _contours; j < Width - _contours - _taillemodule; j++)
                {
                    int countN = 0;
                    int countB = 0;
                    for (int k = i; k < i + 2; k++)
                    {
                        for (int l = j; l < j + 2; l++)
                        {
                            if (ImageData[k, l] == new Pixel(0, 0, 0))
                            {
                                countN++;
                            }

                            if (ImageData[k, l] == new Pixel(255, 255, 255))
                            {
                                countB++;
                            }
                        }
                    }

                    if (countN == 4)
                    {
                        total += 3;
                    }

                    if (countB == 4)
                    {
                        total += 3;
                    }



                }
            }

            return total;
        }

        #endregion

        #region Regle3
        /// <summary>
        /// Méthode qui permet de calculer le total d'erreurs de la règle 3
        /// </summary>
        /// <returns></returns> Le total d'erreurs
        public int Regle3()
        {

            int total = 0;


            var pattern = new Pixel[]
            {
            new Pixel(0, 0, 0), new Pixel(255, 255, 255), new Pixel(0, 0, 0), new Pixel(0, 0, 0), new Pixel(0, 0, 0),
            new Pixel(255, 255, 255), new Pixel(0, 0, 0), new Pixel(255, 255, 255), new Pixel(255, 255, 255),
            new Pixel(255, 255, 255), new Pixel(255, 255, 255)
            };
            var reversepattern = new Pixel[]
            {
            new Pixel(255, 255, 255), new Pixel(255, 255, 255), new Pixel(255, 255, 255), new Pixel(255, 255, 255),
            new Pixel(0, 0, 0), new Pixel(255, 255, 255), new Pixel(0, 0, 0), new Pixel(0, 0, 0), new Pixel(0, 0, 0),
            new Pixel(255, 255, 255), new Pixel(0, 0, 0)
            };
            var row = new Pixel[Height - _contours];
            var column = new Pixel[Width - _contours];
            int patterns = 0;
            for (int index = _contours; index < Height - _taillemodule - _contours; index++)
            {
                for (int i = _contours; i < Height - _taillemodule - _contours; i++)
                {
                    row[i] = ImageData[i, index];
                }

                for (int j = _contours; j < Width - _taillemodule - _contours; j++)
                {
                    column[j] = ImageData[index, j];
                }

                for (int columnIndex = _contours; columnIndex < Width - _contours - 11 * _taillemodule; columnIndex++)
                {
                    if (column[columnIndex] == pattern[columnIndex] || column[columnIndex] == reversepattern[columnIndex])
                    {
                        patterns++;
                    }

                }

                for (int rowIndex = _contours; rowIndex < Height - _contours - 11; rowIndex++)
                {
                    if (row[rowIndex] == pattern[rowIndex] || row[rowIndex] == reversepattern[rowIndex])
                    {
                        patterns++;
                    }
                }
            }

            total += patterns * 40;
            return total;
        }

        #endregion

        #region Regle4
        /// <summary>
        /// Méthode qui permet de calculer le total d'erreurs de la règle 4
        /// </summary>
        /// <returns></returns> Le total d'erreurs
        public int Regle4()
        {
            var totB = 0;
            var totalmod = (Height - 2 * _contours) / _taillemodule;
            for (int i = _contours; i < Height - _contours; i++)
            {
                for (int j = _contours; j < Width - _contours; j++)
                {
                    if (new Pixel(0, 0, 0) == ImageData[i, j])
                    {
                        totB++;
                    }
                }
            }

            var percent = (int)(totB / totalmod) * 100;
            var step1 = percent % 5;
            var val1 = percent - step1;
            var val2 = val1 + 5;
            var abs1 = Math.Abs(val1 - 50);
            var abs2 = Math.Abs(val2 - 50);
            var final1 = abs1 / 5;
            var final2 = abs2 / 5;
            var total = final1 <= final2 ? abs1 : abs2;
            return total;


        }

        #endregion

        #endregion


        #region Pour encoder les versions supérieures du QRCode
        /// <summary>
        /// Méthode qui permet d'encoder les informations du QRCode sous blocks
        /// </summary>
        public void EncodeErrorData()
        {
            var ggf = new GenericGF(285, 256, 0);
            var reed = new ReedSolomonEncoder(ggf);
            var maxdata = _tailleblock1 >= _tailleblock2 ? _tailleblock1 : _tailleblock2;
            var data = new int[_nbblocksG1 + _nbblocksG2, maxdata];
            var ecdata = new int[_nbblocksG1 + _nbblocksG2, _nbecblock];
            var dataencoded = new List<int>();

            for (int i = 0; i < _nbblocksG1; i++)
            {
                var datablock1 = DataByteEncoding().ToList().GetRange(i*_tailleblock1, (i + 1)*_tailleblock1 - i * _tailleblock1);
                //[(i * _tailleblock1)..((i + 1) * _tailleblock1)];
                var array = Enumerable.Repeat(0, _nbecblock);
                //var array2 = Enumerable.Repeat(0,maxdata).ToArray();

                var final = datablock1.Concat(array).ToArray();
                reed.Encode(final, _nbecblock);

                for (int j = 0; j < maxdata; j++)
                {
                    data[i, j] = j >= _tailleblock1 ? -1 : final[j];
                }
                for (int k = 0; k < _nbecblock; k++)
                {
                    ecdata[i, k] = final[k + _tailleblock1];
                }
            }

            for (int i = 0; i < _nbblocksG2; i++)
            {
                var datablock2 = DataByteEncoding().ToList().GetRange(i*_tailleblock2 + _tailleblock1*_nbblocksG1, (i + 1)*_tailleblock2 + _tailleblock1*_nbblocksG1 - (i*_tailleblock2 + _tailleblock1*_nbblocksG1));
                var array = Enumerable.Repeat(0, _nbecblock);

                var final = datablock2.Concat(array).ToArray();
                reed.Encode(final, _nbecblock);

                for (int j = 0; j < maxdata; j++)
                {
                    data[i + _nbblocksG1, j] = j >= _tailleblock2 ? -1 : final[j];
                }
                for (int k = 0; k < _nbecblock; k++)
                {
                    ecdata[i + _nbblocksG1, k] = final[k + _tailleblock2];
                }
            }

            for (int i = 0; i < maxdata; i++)
            {
                for (int j = 0; j < _nbblocksG1 + _nbblocksG2; j++)
                {
                    if (data[j, i] != -1)
                    {
                        dataencoded.Add(data[j, i]);
                    }
                    else continue;

                }
            }

            for (int i = 0; i < _nbecblock; i++)
            {
                for (int j = 0; j < _nbblocksG1 + _nbblocksG2; j++)
                {
                    dataencoded.Add(ecdata[j, i]);
                }
            }
            var end = ByteToBit(dataencoded.ToArray()).ToList();
            _bitwords = end.ToArray();
        }


        #endregion
        

        
        
        
        
        #region Lecture du QRCode (pas finie)

        public static string LectQRCode(MyImage qrcode)
        {
            var contours = 0;
            while (qrcode.ImageData[contours, contours] == new Pixel(255, 255, 255))
            {
                contours++;
            }
            var tailleQR = qrcode.Width - 2 * contours;
            var version = (tailleQR / 4) - (17 / 4);
            var taillemodule = 0;
            while (qrcode.ImageData[contours, contours] == new Pixel(0, 0, 0))
            {
                contours++;
            }


            return "kjedi";
        }


        #endregion

    }
}