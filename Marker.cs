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

    class Marker
    {
        private Contour<Point> contourInternal;
        private Contour<Point> contourExternal;
        private Image<Gray, Byte> image;
        private bool rot4Position;
        public double angle { get; set; }
        public Marker(Contour<Point> cInternal,
                   Contour<Point> cExternal, Image<Gray, Byte> bin)
        {
            contourExternal = cExternal;
            contourInternal = cInternal;
            image = bin;
            rot4Position = true;
   
        }
        public Marker(Contour<Point> cInternal,
                  Contour<Point> cExternal)
        {
            contourExternal = cExternal;
            contourInternal = cInternal;
            rot4Position = true;
        }
        public bool isRot4Position() {
            return rot4Position; 
        }
        public bool isRotBasePosition() {
            return !rot4Position;
        }
        public Contour<Point> getContourInternal() {
            return contourInternal;
        }
        public Contour<Point> getContourExternal() {
            return contourExternal;
        }
        public Image<Gray, Byte> getImage() {
            return image;
        }
        public void setImage(Image<Gray,Byte> img){
             image =img;
        }
        public void thresholdMarker()
        {
            image = image.ThresholdBinary(new Gray(127), new Gray(255));
        }
        public void rotateMarker(int angle)
        {
            image = image.Rotate(
               angle, new Gray(0), false);
            rot4Position = false;
        }
        public void rotateMarker()
        {
            image = image.Rotate(
               angle, new Gray(0), false);
            rot4Position = false;
        }
    }
}