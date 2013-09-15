using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.Util;
using System.Drawing;
namespace Pikto
{
    class ExtendContours 
    {
        private List<Contour<Point>> contour;
        public ExtendContours()
        {
            contour = new List<Contour<Point>>();
        }
        public Contour<Point> getContourAt(int index) {
            return contour.ElementAt(index); 
        }
        public void addContour(Contour<Point> c) {
            contour.Add(c);
        }
        public  double getContourAreaAt(int index){
            return contour.ElementAt(index).Area;
        }
        public PointF getCenterAt(int index) {
            return contour.ElementAt(index).GetMinAreaRect().center;
        }
        public double getAngleAt(int index) {
            return contour.ElementAt(index).GetMinAreaRect().angle;
        }
        public int getCount() { return contour.Count; }
        public void swapPoint13At(int index)
        {
            Point temp1 = contour.Last().Pop();
            Point temp2 = contour.Last().Pop();
            Point temp3 = contour.Last().Pop();
            contour.Last().Push(temp1);
            contour.Last().Push(temp2);
            contour.Last().Push(temp3);
        }
        public void swapPoint13Last() {

            swapPoint13At(contour.Count - 1);
        }
        public Point subTwoPointLast(int i, int j)
        {
            return new Point(contour.Last()[i].X - contour.Last()[j].X,
                             contour.Last()[i].Y - contour.Last()[j].Y);

        }
        public bool includeContour(int i,int j)
        {
            if (contour.ElementAt(i).InContour(contour.ElementAt(j)[0]) < 0) return false;
            if (contour.ElementAt(i).InContour(contour.ElementAt(j)[1]) < 0) return false;
            if (contour.ElementAt(i).InContour(contour.ElementAt(j)[2]) < 0) return false;
            return true;
        }
        public void contourClear() { contour.Clear(); }
    }
}
