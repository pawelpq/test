using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.Util;
namespace Pikto
{
    public partial class Form1 : Form
    {
        private Capture capture;        //takes images from camera as image frames
        private Emgu.CV.UI.ImageViewer v;
        private MarkerDetector md;
        private bool captureInProgress; // checks if capture is executing
        public Form1()
        {
            InitializeComponent();
            md = new MarkerDetector();
        }
        private void ProcessFrame(object sender, EventArgs arg)
        {
          Image<Bgr, Byte> ImageFrame = capture.QueryFrame();  //line 1
          Stopwatch stopWatch = new Stopwatch();
          stopWatch.Start();
            md.findMarkers(capture.QueryGrayFrame());
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
           

            // Format and display the TimeSpan value. 
         
    
      //    EmguTools.drawR_CInternal(ImageFrame, md.markers);
          
                     TimeSpan ts = stopWatch.Elapsed;
     //     EmguTools.drawAllContour(ImageFrame, md.contours);
        //   imageBox1.Image = ImageFrame;
        //   textBox1.Text = ("RunTime " + ts.Milliseconds);
          
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            capture = new Capture();
            v = new ImageViewer();
            Application.Idle += ProcessFrame;
        }
    }
}
