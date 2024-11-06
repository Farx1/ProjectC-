namespace WpfApp2
{
    // Creation d'une classe "Pixel" pour facilités le remplissage des données de l'image
    public class Pixel
    {

        #region Attributs 
        private byte _red;
        private byte _green;
        private byte _blue;
        #endregion


        #region Constructeur
        /// <summary>
        /// Constructeur naturel
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public Pixel(byte red, byte green, byte blue)
        {
            this._red = red;
            this._green = green;
            this._blue = blue;
        }

        public Pixel(Pixel pixel)
        {
            _red = pixel._red;
            _green = pixel._green;
            _blue = pixel._blue;
        }

        #endregion


        #region Propriétés

        public byte Red
        {
            get => _red;
            set => _red = value;
        }

        public byte Green
        {
            get => _green;
            set => _green = value;
        }

        public byte Blue
        {
            get => _blue;
            set => _blue = value;
        }

        #endregion


        #region Méthode d'affichage des Pixels
        public string toStringP()
        {
            return _red + " " + _green + " " + _blue + " ";
        }
        #endregion


    }
}
