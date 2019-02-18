using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


namespace WindowsFormsApp1
{
    public enum DrawMode
    {
        NONE,
        DRAW
    }

    public partial class Form1 : Form
    {
        public DrawMode drawMode = DrawMode.NONE;

        public bool fileLoaded = false;
        private Scribe scribe = new Scribe();

        public Form1()
        {
            InitializeComponent();
            Application.Idle += HandleApplicationIdle;
        }

        //updates the painting on every "tick", if needed.
        protected void HandleApplicationIdle(object sender, EventArgs e)
        {
            //update debug display
            this.lbl_AbsMousePosition.Text = "X: " + MousePosition.X.ToString() + ", Y:" + MousePosition.Y.ToString();

            if (this.drawMode == DrawMode.DRAW)
            {
                //paint all established lines
                scribe.DrawCurrentStrip(this.pbx_Main);
                
                //paint the temp. point-mouse cursor line
                scribe.PaintActiveLine(this.pbx_Main);
            }
        }

        //protected override void OnPaint(EventArgs e)
        //{
        //    //base.OnPaint(e);

        //    //paint overlay?

        //    //possibly paint active line
        //    if (this.drawMode == DrawMode.DRAW)
        //    {
        //        PaintActiveLine(e);
        //    }
        //}

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;
            string filePath = string.Empty;

            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.InitialDirectory = Application.StartupPath + "\\Projects";
            fileDialog.Filter = "jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|All files (*.*)|*.*";
            fileDialog.FilterIndex = 3;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = fileDialog.FileName;

                try
                {
                    pbx_Main.ImageLocation = filePath;
                    pbx_Main.Refresh();
                    this.fileLoaded = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image. ERROR:" + ex.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Debug.WriteLine("Error: " + ex.InnerException + " STACK:" + ex.StackTrace);
                }
            }
        }

        private void pbx_Main_Click(object sender, EventArgs e)
        {
            //TODO: disabled for testing only
            //Enable this to force the user to load an image first
            //if(this.fileLoaded == false)
            //{
            //    return;
            //}

            //main activity switch
            if(this.drawMode == DrawMode.NONE)
            {
                this.drawMode = DrawMode.DRAW;

                scribe.PlaceInitialPoint();
            }
            else
            {
                //if strip is done, finalize it.
                //check to see if the current and the first are near:
                if(scribe.CheckSnap())
                {
                    scribe.PlaceFinalPoint();

                    //stop drawing
                    this.drawMode = DrawMode.NONE;
                }
                else
                {
                    //otherwise, add a new point
                    scribe.PlacePoint();
                }
            }
        }
               
        //update display and the scribe's registers
        private void pbx_Main_MouseMove(object sender, MouseEventArgs e)
        {
            this.scribe.xRegister = e.X;
            this.scribe.yRegister = e.Y;
            this.lbl_MousePos.Text = "X: " + e.X.ToString() + ", Y:" + e.Y.ToString();
        }
    }
}
