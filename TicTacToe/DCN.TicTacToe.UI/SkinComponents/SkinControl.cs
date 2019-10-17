///////////////////////////////////////////////////////////////////////////////
//
//  File:           SkinControl.cs
//
//  Facility:		The unit contains the SkinControl class.
//
//  Abstract:       The base class for all controls that support defining the 
//					shape with the help of an image.
//
//  Environment:    VC 7.1
//
//  Author:         DCN Ltd.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DCN.TicTacToe.UI.SkinControl
{
    /// <summary>
    /// The base class providing an arbitrary shape of the control basing 
    /// on the bitmap.
    /// </summary>
    [
        ToolboxItem(false)
    ]

    public class SkinControl : System.Windows.Forms.UserControl, ISupportInitialize
    {
        #region Fields

        /// <summary>
        /// The outline of the control.
        /// </summary>
        protected GraphicsPath controlArea = null;

        /// <summary>
        /// The bitmap defining the control shape.
        /// </summary>
        protected Bitmap patternBitmap = null;

        /// <summary>
        /// The color that is transparent in the bitmap.
        /// </summary>
        private Color transparentColor = Color.FromArgb(255, 255, 255);

        /// <summary>
        /// The collection supporting the regions caching while controls are repeatedly created. 
        /// Allows the performance to be raised when creating a control. 
        /// </summary>
        private static RegionCollection regions = new RegionCollection();

        /// <summary>
        /// The field defines whether to use the regions caching or to calculate them
        /// again every time when creating a control.
        /// </summary>
        private bool useCashing = false;

        #endregion

        #region Properties

        /// <summary>
        /// the property setting the bitmap that defines the control's shape.
        /// </summary>
        public Bitmap PatternBitmap
        {
            get { return patternBitmap; }
            set
            {
                if (value == null)
                    return;

                patternBitmap = value;

                this.Height = value.Height;
                this.Width = value.Width;

            }
        }

        /// <summary>
        /// The property defining whether to use the regions caching or to calculate them again
        /// every time when creating a control.
        /// </summary>
        public bool UseCashing
        {
            get { return this.useCashing; }
            set { this.useCashing = value; }
        }

        /// <summary>
        /// the property setting the color that will be considered as transparent
        /// while analysing the bitmap.
        /// </summary>
        public Color TransparentColor
        {
            get { return this.transparentColor; }
            set
            {
                this.transparentColor = value;

                Invalidate();
                UpdateRegion();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// the constructor setting the window's styles.
        /// </summary>
        public SkinControl()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.DoubleBuffer
                | ControlStyles.UserPaint,
                true);

            this.UpdateStyles();
        }

        /// <summary>
        /// the method setting a new shape for the control basing on
        /// the specified bitmap.
        /// </summary>
        public void UpdateRegion()
        {
            if (patternBitmap == null)
                return;

            ArrayList pts = new ArrayList();

            Color pixelColor;

            //Getting the rectangular region of the control
            Region region = new Region(this.ClientRectangle);

            //Scanning the patternBitmap bitmap from the zero string from left to right.
            //If we find a pixel that is not transparent and does not coincide with the 
            //transparent color, we add it to the list of the points of the future outline
            //that bounds the control.
            for (int i = 0; i < patternBitmap.Height; i++)
            {
                for (int j = 0; j < patternBitmap.Width; j++)
                {

                    pixelColor = patternBitmap.GetPixel(j, i);
                    if (pixelColor != transparentColor && pixelColor.A != 0)
                    {
                        pts.Add(new Point(j, i));
                        break;
                    }

                }
            }

            Point last = (Point)pts[pts.Count - 1];
            Point dop = new Point(last.X, last.Y + 1);

            pts.Add(dop);
            bool addDopPoint = true;

            // Do the same from right to left beginning from the last (lower)
            // string of pixels for obtaining the right bound of the outline.
            for (int i = patternBitmap.Height - 1; i >= 0; i--)
            {
                for (int j = patternBitmap.Width - 1; j >= 0; j--)
                {
                    pixelColor = patternBitmap.GetPixel(j, i);
                    if (pixelColor != transparentColor && pixelColor.A != 0)
                    {
                        if (addDopPoint == true)
                        {
                            addDopPoint = false;
                            pts.Add(new Point(j + 1, i + 1));
                        }

                        pts.Add(new Point(j + 1, i));
                        break;
                    }

                }
            }

            //closing the outline.
            pts.Add(new Point(((Point)pts[0]).X, ((Point)pts[0]).Y));


            //putting the resulting points to the array of points.
            Point[] pp = new Point[pts.Count];
            for (int i = 0; i < pts.Count; i++)
                pp[i] = (Point)pts[i];

            //creating the GraphicsPath object basing on the array of points.
            controlArea = new GraphicsPath();
            controlArea.AddLines(pp);

            //Search the inersection of the initial region of the control with 
            //the found closed outline and set the resulting region as the 
            //control's region.
            region.Intersect(controlArea);
            this.Region = region;


            this.Invalidate();
            this.Update();

        }

        #endregion

        #region ISupportInitialize Members

        /// <summary>
        /// Is called immediately after the component creation.
        /// </summary>
        public void BeginInit()
        {
        }

        /// <summary>
        /// After setting the UseCashing and PatternBitmap properties the method
        /// defines whether the region bounding the control should be calculated.
        /// </summary>
        public void EndInit()
        {
            if (patternBitmap == null)
                return;

            if (regions.Contains(this.Name) && this.useCashing && !DesignMode)
            {
                this.Region = regions[this.Name];
            }
            else
            {
                UpdateRegion();

                if (this.useCashing && !DesignMode)
                    regions[this.Name] = this.Region;

            }

            Invalidate();
            Update();

            if (!DesignMode && useCashing)
                patternBitmap = null;

            GC.Collect();

        }

        #endregion
    }
}
