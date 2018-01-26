using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using PlateRecognitionSystem.Extension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PlateRecognitionSystem.Plate
{
    public static class FilterPlate
    {
        public static List<UMat> GetCharacters(UMat plate)
        {
            bool resultThreshWork = false;
            double thresholdValue = 100;
            /////ten blok jest używany jak na sztywno chcesz podzielić jakąś mape bitową na osobne literki
            //var path = Path.GetFullPath("C:\\Users\\jurek993\\Desktop\\images.bmp");
            //BitmapImage image = new Bitmap(
            //    new Bitmap(path)).ToBitmapImage();
            //Image<Bgr, Byte> imageCV = new Image<Bgr, byte>(image.ToBitmap());
            //Mat ItsWrong_ChangeItToPlate = imageCV.Mat;
            /////


            UMat plateThresh = new UMat();
            List<UMat> characters = new List<UMat>();
            do
            {
                CvInvoke.Threshold(plate, plateThresh, thresholdValue, 255, ThresholdType.BinaryInv);
                resultThreshWork = CheckHowManyBlackColor(plateThresh.Bitmap, 20);
                thresholdValue-=10;
            } while (!resultThreshWork && thresholdValue > 0);

            
            Size plateSize = plate.Size;
            List<Rectangle> rectangles = new List<Rectangle>();
            rectangles = FindCounturedCharacters(plateThresh, plateSize);
            if (rectangles.Count <= 5)
            {
                using (UMat plateCanny = new UMat())
                {
                    CvInvoke.Canny(plate, plateCanny, 100, 50);
                    rectangles = FindCounturedCharacters(plateCanny, plateSize);
                    if (rectangles.Count < 5)
                        rectangles = new List<Rectangle>();
                }
            }
            var orderedRectangles = rectangles.OrderBy(x => x.X).ToList();
            foreach (var rect in orderedRectangles)
            {
                var cloneTresh = plateThresh.Clone();
                var character = new UMat(cloneTresh, rect);
                CvInvoke.BitwiseNot(character, character);
                var result = CheckHowManyBlackColor(character.Bitmap,10);
                if (result)
                {
                    characters.Add(character);
                }
                //CvInvoke.Erode(character, character, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
                //CvInvoke.Dilate(character, character, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);

                //character.Bitmap.Save("C:\\Users\\jurek993\\Desktop\\test\\" + rect.Size.ToString() + rect.Location.ToString()+ ".bmp", ImageFormat.Bmp);
            }
            return characters;
        }

        private static List<Rectangle> FindCounturedCharacters(UMat plate, Size size)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(plate, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                for (int i = 1; i < contours.Size; i++)
                {
                    using (VectorOfPoint contour = contours[i]) 
                    {
                        Rectangle rect = CvInvoke.BoundingRectangle(contour);
                        if (rect.Height > (size.Height >> 1) && rect.Width < size.Width / 4)
                        {
                            rect.X -= 1; rect.Y -= 1; rect.Width += 3; rect.Height += 3;
                            Rectangle roi = new Rectangle(Point.Empty, plate.Size);
                            rect.Intersect(roi);
                            rectangles.Add(rect);
                        }
                    }
                }
            }
            return rectangles;
        }

        private static bool CheckHowManyBlackColor(Bitmap image, int percent)
        {
            int colorInc = 0;
            for (int column = 0; column < image.Height - 1; column++)
            {
                for (int row = 0; row < image.Width - 1; row++)
                {
                    var color = image.GetPixel(row, column);
                    var xx = color.R;
                    if (color.R != 255)
                    {
                        colorInc++;
                    }
                }
            }
            int percentValue = ((image.Height * image.Width * 20)) / 100;
            if (colorInc > percentValue && colorInc < (image.Height * image.Width) - percentValue) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
