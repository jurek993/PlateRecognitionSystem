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
            /////ten blok jest używany jak na sztywno chcesz podzielić jakąś mape bitową na osobne literki
            //var path = Path.GetFullPath("C:\\Users\\jurek993\\Desktop\\images.bmp");
            //BitmapImage image = new Bitmap(
            //    new Bitmap(path)).ToBitmapImage();
            //Image<Bgr, Byte> imageCV = new Image<Bgr, byte>(image.ToBitmap());
            //Mat ItsWrong_ChangeItToPlate = imageCV.Mat;
            /////


            UMat thresh = new UMat();
            List<UMat> characters = new List<UMat>();
            CvInvoke.Threshold(plate, thresh, 60, 255, ThresholdType.BinaryInv);

            Size plateSize = plate.Size;
            using (Mat plateCanny = new Mat())
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                List<Rectangle> rectangles = new List<Rectangle>();
                CvInvoke.Canny(plate, plateCanny, 100, 50);
                CvInvoke.FindContours(plateCanny, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                for (int i = 1; i < contours.Size; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    {
                        Rectangle rect = CvInvoke.BoundingRectangle(contour);
                        if (rect.Height > (plateSize.Height >> 1) && rect.Width < plateSize.Width / 4)
                        {
                            rect.X -= 1; rect.Y -= 1; rect.Width += 3; rect.Height += 3;
                            Rectangle roi = new Rectangle(Point.Empty, plate.Size);
                            rect.Intersect(roi);
                            rectangles.Add(rect);
                        }
                    }
                }
                var orderedRectangles = rectangles.OrderBy(x => x.X).ToList();
                foreach (var rect in orderedRectangles)
                {
                    var cloneTresh = thresh.Clone();
                    var character = new UMat(cloneTresh, rect);
                    //CvInvoke.Erode(character, character, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
                    //CvInvoke.Dilate(character, character, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
                    CvInvoke.BitwiseNot(character, character);
                    characters.Add(character);
                    //character.Bitmap.Save("C:\\Users\\jurek993\\Desktop\\test\\" + rect.Size.ToString() + rect.Location.ToString()+ ".bmp", ImageFormat.Bmp);
                }
            }
            return characters;
        }
    }
}
