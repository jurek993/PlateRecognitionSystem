using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Image
{
    class ImageProcessing
    {
        //TODO: maybe better make extension?
        public static double[] ToMatrix(Bitmap BM, int MatrixRowNumber, int MatrixColumnNumber)
        {
            try
            {
                double HRate = ((Double)MatrixRowNumber / BM.Height);
                double WRate = ((Double)MatrixColumnNumber / BM.Width);
                double[] result = new double[MatrixColumnNumber * MatrixRowNumber];

                for (int r = 0; r < MatrixRowNumber; r++)
                {
                    for (int c = 0; c < MatrixColumnNumber; c++)
                    {
                        Color color = BM.GetPixel((int)(c / WRate), (int)(r / HRate));
                        result[r * MatrixColumnNumber + c] = 1 - (color.R * .3 + color.G * .59 + color.B * .11) / 255;
                    }
                }
                return result;
            } catch(Exception)
            {
                return null;
            }

        }
    }
}
