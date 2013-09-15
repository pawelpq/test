using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.Util;
using System.Drawing;
using System.Collections.Generic;
namespace Pikto
{
    static class EmguTools
    {
        static public void draw4ContourAndCircle(Image<Bgr, Byte> img, Contour<Point> contour) {
            img.Draw(contour, new Bgr(255, 0, 0), 3);
            for (int i = 0; i < contour.Total; i++)
            {
                PointF pkt = new PointF(contour[0].X,
                    contour[0].Y);
                img.Draw(new CircleF(pkt, 4), new Bgr(i*50, i*50, 250), 4);
            }
        
        }

        static public void drawContour(Image<Bgr, Byte> img,
          Contour<Point> c, Bgr color)
        {
            img.Draw(c, color, 3);
        }
        static public void drawAllContour(Image<Bgr, Byte> img, 
            List<Contour<Point>> c)
        {
            for (int i = 0; i < c.Count; i++)
            {
                img.Draw(c.ElementAt(i), new Bgr(0,255,255), 1);
               
                 }
        }
    }
}