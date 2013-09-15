using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Drawing;
namespace Pikto
{
   class PossibleMarkers
    {
       public class PossibleMarker {
           public List<int> numIncludeContourList;
           public int numberBaseContour {get; set;}
           public int numberInternalFrame {get; set;}
           public int numberTopRect { get; set; }
           
           public PossibleMarker (int numBase){
            numberBaseContour= numBase;
            numberInternalFrame = -1;
            numberTopRect = -1;
            numIncludeContourList = new List<int>(); 
            
           }
         //komentarz
       }
     public List<PossibleMarker> listMarkers;
      
     public PossibleMarkers()
     {
         listMarkers = new List<PossibleMarker>();
     }
     public void addPossibleMarker(int numberBaseContour){
         listMarkers.Add(new PossibleMarker(numberBaseContour));
     }
     public void addIncludeContourAt(int indexPossibleMarker,int numberContour) {
         listMarkers.ElementAt(indexPossibleMarker).
                                     numIncludeContourList.Add(numberContour);
     }
     public void addIncludeContourAtLast(int numberContour){
         listMarkers.Last().numIncludeContourList.Add(numberContour);
     }
     public void setInternalFrameAt(int index, int number) {
         listMarkers.ElementAt(index).numberInternalFrame = number;
         listMarkers.ElementAt(index).numIncludeContourList.Remove(number);
     }
     public void setTopRectAt(int indexPossibleMarker, int number) {
         listMarkers.ElementAt(indexPossibleMarker).numberTopRect = number;
     }
     
     public void Clear() {
         listMarkers.Clear();
     }
     public bool isMarkerAt(int indexPossibleMarker) {
         if (getNumberTopRectAt(indexPossibleMarker) == -1 
             || getNumberInternalFrameAt(indexPossibleMarker) == -1)
             return false;
         return true;
     }
     public int getCount() { 
         return listMarkers.Count; 
     }
     public int getCountIncludeContourAt(int indexPossibleMarker) { 
         return listMarkers.ElementAt(indexPossibleMarker).numIncludeContourList.Count; 
     }
     public int getNumberBaseAt(int indexPossibleMarker) {
         return listMarkers.ElementAt(indexPossibleMarker).numberBaseContour; 
     }
     public int getNumberTopRectAt(int indexPossibleMarker) { 
         return listMarkers.ElementAt(indexPossibleMarker).numberTopRect; 
     }
     public int getNumberIncludeContourAt(int indexPossibleMarker, int numberContour) {
         return listMarkers.ElementAt(indexPossibleMarker).
             numIncludeContourList.ElementAt(numberContour);
     }
     public int getNumberInternalFrameAt(int indexPossibleMarker) {
         return listMarkers.ElementAt(indexPossibleMarker).numberInternalFrame; 
     }
     public bool isPossibleMarker()
     {
        // if (numIncludeMarker.Count >= 2) return true;
         return false;
     }
  }
}
