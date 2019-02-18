using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Scribe
    {
        private List<Point> currentStrip = new List<Point>();
        private List<List<Point>> stripList = new List<List<Point>>();

        //sets the range when a position 'snap to' occurs.
        private int snapTolerance = 5;

        public int xRegister;
        public int yRegister;

        public Scribe()
        { }

        //check to see if the current point is close enough to complete the polygon.
        public bool CheckSnap()
        {
            bool check = false;

            Point checkPosition = currentStrip.First();

            if (((this.xRegister + snapTolerance) > checkPosition.X) && ((this.xRegister - snapTolerance) < checkPosition.X))
            {
                if (((this.yRegister + snapTolerance) > checkPosition.Y) && ((this.yRegister - snapTolerance) < checkPosition.Y))
                {
                    //snap
                    currentStrip.Add(checkPosition);
                    check = true;
                }
            }

            return check;
        }

        //place first point in the chain
        public void PlaceInitialPoint()
        {
            List<Point> newStrip = new List<Point>();

            Point firstPoint = new Point(this.xRegister, this.yRegister);
            newStrip.Add(firstPoint);
            this.currentStrip = newStrip;
        }

        //place intermediate points in chain
        public void PlacePoint()
        {
            Point addPoint = new Point(this.xRegister, this.yRegister);
            this.currentStrip.Add(addPoint);
        }

        //finalize the chain/polygon
        public void PlaceFinalPoint()
        {
             //TODO: flood fill the polygon
        }

        public void PaintActiveLine(PictureBox pbx)
        {
            Point startPoint = this.currentStrip.Last();
            Point endPoint = new Point(this.xRegister, this.yRegister);

            //place them in contaniner for drawing
            Point[] points = { startPoint, endPoint };

            Pen myPen = new Pen(Color.Red);
            myPen.Width = 1;

            var g = pbx.CreateGraphics();

            g.DrawLines(myPen, points);
        }

        public void DrawCurrentStrip(PictureBox pbx)
        {
            //with only one point, we only have a point-mouse cursor line.
            if (this.currentStrip.Count <= 1)
            {
                return;
            }

            int i = 0;
            int k = 0;
            Point startPoint;
            Point endPoint;

            Pen myPen = new Pen(Color.Green);
            myPen.Width = 1;

            var g = pbx.CreateGraphics();

            for (i = 0; i < currentStrip.Count - 1; i++)
            {
                k = i + 1;

                if (k == currentStrip.Count()) //overrun
                {
                    break;
                }

                startPoint = currentStrip[i];
                endPoint = currentStrip[i + 1];

                //place them in contaniner for drawing
                //There is probably and optimization to shove in all the
                //points at once in the array (TODO- check on this)
                Point[] points = { startPoint, endPoint };

                g.DrawLines(myPen, points);
            }
        }
    }
}
