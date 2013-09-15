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
    class MarkerDetector
    {
        Image<Gray, Byte> grayImage;
        Image<Gray, Byte> binaryImage;
        public List<Contour<Point>> contours;
        public ExtendContours contourPassibleMarkers;
        public PossibleMarkers possibleMarkers;
        public List<Marker> markers;
        public List<PointF> listPkt;
        public PointF waznePkt;
        private double minArea;
        private double epsParam;
        public MarkerDetector()
        {
            contours = new List<Contour<Point>>();
            contourPassibleMarkers = new ExtendContours();
            possibleMarkers = new PossibleMarkers();
            waznePkt = new PointF();
            markers = new List<Marker>();
            listPkt = new List<PointF>();
            minArea = 100.0;       
            epsParam = 7;
        }
        public void findMarkers(Image<Gray, Byte> imgGray)
        {
            markers.Clear();
            contours.Clear();
            contourPassibleMarkers.contourClear();
            possibleMarkers.Clear();
            setGrayImage(imgGray);
            binaryImage = threshold(imgGray, 127, 255);
            findContours(binaryImage,contours, binaryImage.Cols /5);
            findCandidates(contours,minArea);
            findRelationAndDeletePassibleMarker(possibleMarkers);
            findInternalFrame(possibleMarkers);
            findTopRectangle(possibleMarkers);
            
        for (int i = 0; i < possibleMarkers.getCount(); i++)
          {
              if (possibleMarkers.isMarkerAt(i)) {
               
                //  funpkt(i); 
                  rot4MarkerAndCreateMarkerElem(i);
              }
          }
        }
        private void findContours(Image<Gray, Byte> imgBin,
            List<Contour<Point>> c,int minContourPointsAllowed)
        {       
           for( Contour<Point> allContours =
                imgBin.FindContours(
                Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE,
                Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST)
             ;allContours != null;allContours =allContours.HNext)
            {   
              if (allContours.Total > minContourPointsAllowed)
                 c.Add(allContours);
            }   
        }
        private void findCandidates(List<Contour<Point>> c, double minArea)
        {
            double o;
            Contour<Point> approxCurve;
            foreach (Contour<Point> temp in c)
            {
               approxCurve = temp.ApproxPoly(epsParam);
               if (approxCurve.Count() != 4) 
                   continue;
               if (!approxCurve.Convex)
                   continue;
               if (approxCurve.Area < minArea) 
                   continue;
               contourPassibleMarkers.addContour(approxCurve);
                 
               Point v1 = contourPassibleMarkers.subTwoPointLast(1, 0);
               Point v2 = contourPassibleMarkers.subTwoPointLast(2, 0);
               
                o = (v1.X * v2.Y) - (v1.Y * v2.X);
                if (o > 0.0)
                  contourPassibleMarkers.swapPoint13Last();             
            }
        }
        private void findRelationAndDeletePassibleMarker(PossibleMarkers pM)
        {
            bool constructor = true;
            for (int i = 0; i < contourPassibleMarkers.getCount(); i++)
            {
                for (int j = 0; j < contourPassibleMarkers.getCount(); j++){
                    if (i != j){
                        if (contourPassibleMarkers.includeContour(i, j))
                        {
                            if (constructor) { 
                                constructor = false; 
                                pM.addPossibleMarker(i); 
                            }
                            pM.addIncludeContourAtLast(j);
                        }
                    }
                }
                constructor = true;
            }
        }
        private void findInternalFrame(PossibleMarkers pM)
        {
            for (int i = 0; i < pM.getCount(); i++)
            {
                for (int j = 0; j < pM.getCountIncludeContourAt(i); j++)
                {
                    int numIncludeFrame = pM.getNumberIncludeContourAt(i, j);
                    double wsp = contourPassibleMarkers.
                                    getContourAreaAt(pM.getNumberBaseAt(i)) /
                            (double)contourPassibleMarkers.
                                    getContourAreaAt(numIncludeFrame);
                    if (wsp > 3.8 && wsp < 4.2)
                    {
                        pM.setInternalFrameAt(i, numIncludeFrame);
                        break;
                    }
                }
            }
        }
        private void findTopRectangle(PossibleMarkers pM)
        {
            for (int i = 0; i < pM.getCount(); i++) {
                int tempBase = pM.getNumberBaseAt(i);                
                    if (pM.getNumberInternalFrameAt(i) != -1) {
                        int internalFrame = pM.getNumberInternalFrameAt(i);
                        for (int j = 0; j < pM.getCountIncludeContourAt(i); j++)
                        {
                            int contourNumber=pM.getNumberIncludeContourAt(i,j);
                            if (contourPassibleMarkers.
                                includeContour(tempBase, contourNumber) &&
                                !contourPassibleMarkers.
                                includeContour(internalFrame, contourNumber))
                                pM.setTopRectAt(i, contourNumber);             
                        }   
                    }
            }
        }
        public int calculateAngle(PointF markerCenter, PointF rectCenter, double angle)
        {
            listPkt.Clear();
            listPkt.Add(new PointF(markerCenter.X - rectCenter.X,
                                       markerCenter.Y - rectCenter.Y));
            PointF pktPrim = listPkt.First();
            listPkt.Add(new PointF(-pktPrim.Y, pktPrim.X)); //90
            listPkt.Add(new PointF(-pktPrim.X, -pktPrim.Y)); //180
            listPkt.Add(new PointF(pktPrim.Y, -pktPrim.X)); //270

            float temp = listPkt.First().Y;
            int wsk = 0;
            for (int i = 1; i < 4; i++)
            {
                if (listPkt.ElementAt(i).Y < temp)
                {
                    wsk = i;
                }
            }
            if (angle > -45)
            {
                switch (wsk)
                {
                    case 0: return 180;
                    case 1: return 180;
                    case 2: return -90;
                    case 3: return 0;
                }
            }
            else
            {
                switch (wsk)
                {
                    case 0: return 90;
                    case 1: return 180;
                    case 2: return -90;
                    case 3: return -90;
                }

            }
            return 0;
        }

        public void rot4MarkerAndCreateMarkerElem(int indexPM)
        {    
            Image<Gray, Byte> roi = grayImage.Copy(
           contourPassibleMarkers.getContourAt(
           possibleMarkers.getNumberInternalFrameAt(indexPM))
                                            .BoundingRectangle);          
           double angle =contourPassibleMarkers.getAngleAt(
                  possibleMarkers.getNumberInternalFrameAt(indexPM));
        
           listPkt.Clear();
           PointF center = contourPassibleMarkers.getCenterAt(
                    possibleMarkers.getNumberBaseAt(indexPM));
           PointF centerTopRect = contourPassibleMarkers.getCenterAt(
                    possibleMarkers.getNumberTopRectAt(indexPM));
          RotationMatrix2D<float> rotateBasePos = new RotationMatrix2D<float>(
              new PointF(roi.Width/2,roi.Height/2),angle,1);
          double d=   Math.Sqrt(contourPassibleMarkers.getContourAreaAt(indexPM) );
                     double x = 0.125 * d;
        Image<Gray, Byte> rot= roi.WarpAffine(rotateBasePos,
                                 Emgu.CV.CvEnum.INTER.CV_INTER_AREA,
                                 Emgu.CV.CvEnum.WARP.CV_WARP_FILL_OUTLIERS
                                 ,new Gray(0));
        Rectangle rectInternal= new Rectangle(rot.Width/2-(int)((d/2)-x),
                      rot.Height/2-(int)((d/2)-x),(int)(d-2*x),(int)(d-2*x));
 
            
            markers.Add(new Marker(
contourPassibleMarkers.getContourAt(possibleMarkers.getNumberInternalFrameAt(indexPM)),
contourPassibleMarkers.getContourAt(possibleMarkers.getNumberBaseAt(indexPM)),
           rot.Copy(rectInternal)));
                 markers.Last().angle=calculateAngle(center, centerTopRect, angle);
          
}
        private Image<Gray, Byte> threshold
            (Image<Gray, Byte> imgGray, int thresh, int maxVal)
        {
         return   imgGray.ThresholdBinary
                               (new Gray(thresh), new Gray(maxVal));
        }

        public void setGrayImage(Image<Gray, Byte> imgGray)
        {
            grayImage = imgGray;
        }

        public Image<Gray, Byte> getBinaryImage()
        {
            return binaryImage;
        }
        public List<Marker> getMarkers() {
            return markers;
        }
        public Marker markerAt(int index) {
            return markers.ElementAt(index);
        }
        public int getMarkerCount() { return markers.Count; }
    }
}
