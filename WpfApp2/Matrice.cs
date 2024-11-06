namespace WpfApp2
{
    public static class Matrice
    {
        public static int[,] Flou
        {
            get
            {
                return new int[,]
                {
                {1, 1, 1}, {1, 1, 1}, {1, 1, 1}
                };
            }
        }

        public static int[,] Contour
        {
            get
            {
                return new int[,]
                {
                {0, 1, 0}, {1, -4, 1}, {0, 1, 0}
                };
            }
        }

        public static int[,] Renforcement
        {
            get
            {
                return new int[,]
                {
                {0, 0, 0}, {-1, 1, 0}, {0, 0, 0}
                };
            }
        }

        public static int[,] Repoussage
        {
            get
            {
                return new int[,]
                {
                {-2, -1, 0}, {-1, 1, 1}, {0, 1, 2}
                };
            }
        }

        public static int[,] Sobel1//les matrices de Sobel permettent de faire la détection de contour plus efficacement
        {
            get
            {
                return new int[,]
                {
                {-1, 0, 1}, {-2, 0, 2}, {-1, 0, 1}
                };
            }
        }

        public static int[,] Sobel2
        {
            get
            {
                return new int[,]
                {
                {-1, -2, -1}, {0, 0, 0}, {1, 2, 1}
                };
            }
        }

        public static int[,] Null
        {
            get
            {
                return new int[,]
                {
                {0, 0, 0}, {0, 0, 0}, {0, 0, 0}
                };
            }

        }
        public static int[,] Contraste
        {
            get
            {
                return new int[,]
                {
                {0, -1, 0}, {-1, 5, -1}, {0, -1, 0}
                };
            }
        }

    }
}

