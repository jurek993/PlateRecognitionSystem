using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using PlateRecognitionSystem.Extension;
using PlateRecognitionSystem.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlateRecognitionSystem.Plate
{
    internal class ProcessingPlate
    {
        private PlateViewModel _plateViewModel;

        public List<string> DetectLicensePlate(PlateViewModel plateViewModel)
        {
            List<string> licenses;
            int mode = 0;
            do
            {
                _plateViewModel = plateViewModel;
                _plateViewModel.filteredCharatersInSinglePlate = new List<List<UMat>>();
                _plateViewModel.FilteredDetectedCharacters = new ObservableCollection<ImageSource>();
                _plateViewModel.DetectedPlates = new ObservableCollection<ImageSource>();
                licenses = new List<string>();
                using (Mat gray = new Mat())
                using (Mat canny = new Mat())
                using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
                {
                    CvInvoke.CvtColor(plateViewModel.Mat, gray, ColorConversion.Bgr2Gray);
                    Image<Bgr, Byte> grayCVcontrasted = PreTreatment(gray, mode);
                    CvInvoke.Canny(grayCVcontrasted, canny, 250, 100, 3, false);
                    plateViewModel.MonoImage = grayCVcontrasted.Bitmap.ToBitmapImage();
                    plateViewModel.CannyImage = canny.Bitmap.ToBitmapImage();
                    int[,] hierachy = CvInvoke.FindContourTree(canny, contours, ChainApproxMethod.ChainApproxSimple);
                    FindLicensePlateAndCharacters(contours, hierachy, 0, gray, canny, licenses);
                }
                mode++;
            } while (_plateViewModel.FilteredDetectedCharacters.Count <= 5 && mode <= 3);
            return licenses;
        }
        public void FindLicensePlateAndCharacters(VectorOfVectorOfPoint contours, int[,] hierachy, int idx, Mat gray, Mat canny, List<string> licenses)
        {
            for (; idx >= 0; idx = hierachy[idx, 0])
            {
                int numberOfChildren = GetNumberOfChildren(hierachy, idx);
                //if it does not contains any children (charactor), it is not a license plate region
                if (numberOfChildren == 0) continue;

                using (VectorOfPoint contour = contours[idx])
                {
                    if (CvInvoke.ContourArea(contour) > 200)
                    {
                        if (numberOfChildren < 3)
                        {
                            //    If the contour has less than 3 children, it is not a license plate (assuming license plate has at least 3 charactor)
                            //    However we should search the children of this contour to see if any of them is a license plate
                            FindLicensePlateAndCharacters(contours, hierachy, hierachy[idx, 2], gray, canny, licenses);
                            continue;
                        }

                        RotatedRect box = CvInvoke.MinAreaRect(contour);
                        if (box.Angle < -45.0)
                        {
                            float tmp = box.Size.Width;
                            box.Size.Width = box.Size.Height;
                            box.Size.Height = tmp;
                            box.Angle += 90.0f;
                        }
                        else if (box.Angle > 45.0)
                        {
                            float tmp = box.Size.Width;
                            box.Size.Width = box.Size.Height;
                            box.Size.Height = tmp;
                            box.Angle -= 90.0f;
                        }

                        double whRatio = (double)box.Size.Width / box.Size.Height;
                        if (!(3.0 < whRatio && whRatio < 10.0))
                        //if (!(1.0 < whRatio && whRatio < 2.0))
                        {
                            //if the width height ratio is not in the specific range,it is not a license plate 
                            //However we should search the children of this contour to see if any of them is a license plate
                            //Contour<Point> child = contours.VNext;
                            if (hierachy[idx, 2] > 0)
                                FindLicensePlateAndCharacters(contours, hierachy, hierachy[idx, 2], gray, canny, licenses);
                            continue;
                        }

                        using (UMat tmp1 = new UMat())
                        using (UMat tmp2 = new UMat())
                        {
                            PointF[] srcCorners = box.GetVertices();

                            PointF[] destCorners = new PointF[] {
                                new PointF(0, box.Size.Height - 1),
                                new PointF(0, 0),
                                new PointF(box.Size.Width - 1, 0),
                                new PointF(box.Size.Width - 1, box.Size.Height - 1)
                            };

                            using (Mat rot = CameraCalibration.GetAffineTransform(srcCorners, destCorners))
                                CvInvoke.WarpAffine(gray, tmp1, rot, Size.Round(box.Size));

                            //resize the license plate such that the front is ~ 10-12. This size of front results in better accuracy from tesseract
                            Size approxSize = new Size(240, 180);
                            double scale = Math.Min(approxSize.Width / box.Size.Width, approxSize.Height / box.Size.Height);
                            Size newSize = new Size((int)Math.Round(box.Size.Width * scale), (int)Math.Round(box.Size.Height * scale));
                            CvInvoke.Resize(tmp1, tmp2, newSize, 0, 0, Inter.Cubic);
                            //removes some pixels from the edge
                            int edgePixelSize = 2;
                            Rectangle newRoi = new Rectangle(new Point(edgePixelSize, edgePixelSize),
                            tmp2.Size - new Size(2 * edgePixelSize, 2 * edgePixelSize));
                            UMat plate = new UMat(tmp2, newRoi);
                            List<UMat> filteredCharaters = FilterPlate.GetCharacters(plate);


                            _plateViewModel.filteredCharatersInSinglePlate.Add(filteredCharaters);
                            _plateViewModel.DetectedPlates.Add(plate.Bitmap.ToBitmapImage());
                            foreach (var character in filteredCharaters)
                            {
                                _plateViewModel.FilteredDetectedCharacters.Add(character.Bitmap.ToBitmapImage());
                            }
                        }
                    }
                }
            }
        }
        private static int GetNumberOfChildren(int[,] hierachy, int idx)
        {
            //first child
            idx = hierachy[idx, 2];
            if (idx < 0)
                return 0;

            int count = 1;
            while (hierachy[idx, 0] > 0)
            {
                count++;
                idx = hierachy[idx, 0];
            }
            return count;
        }


        private static Image<Bgr, Byte> PreTreatment(Mat gray, int mode)
        {

            switch (mode)
            {
                case 0:
                    var contrastedBitmap2 = gray.Bitmap.Contrast(80);
                    return new Image<Bgr, byte>(contrastedBitmap2);
                    break;
                case 1:
                    CvInvoke.CLAHE(gray, 2, new Size(8, 8), gray);
                    var contrastedBitmap = gray.Bitmap.Contrast(30);
                    return new Image<Bgr, byte>(contrastedBitmap);
                    break;
                case 2:
                    CvInvoke.CLAHE(gray, 6, new Size(8, 8), gray);
                    return new Image<Bgr, byte>(gray.Bitmap);
                    break;
                default:
                    return new Image<Bgr, byte>(gray.Bitmap);
            }
        }

    }
}

