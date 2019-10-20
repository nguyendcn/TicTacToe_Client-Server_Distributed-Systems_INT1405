using System;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace DCN.TicTacToe.UI.SkinComponents
{
    /// <summary>
    /// The class presenting the label that may be arranged only on     
    /// SkinTooltip with the help of the Add label command of the designer.
    /// </summary>
    [ToolboxItem(false)]
    public class LabelItem : System.ComponentModel.Component
    {
        #region Fields

        /// <summary>
        /// the label text
        /// </summary>
        string labelText = "";

        /// <summary>
        /// the label font.
        /// </summary>
        Font textFont = null;

        /// <summary>
        /// the label position.
        /// </summary>
        Rectangle labelRect = Rectangle.Empty;

        /// <summary>
        /// the brush that draws the text.
        /// </summary>
        Brush textBrush = null;

        /// <summary>
        /// the parent control.
        /// </summary>
        SkinCustomArea parent = null;
        /// <summary>
        /// Indicates whether a control is selected.
        /// </summary>
        private bool isSelected = false;

        /// <summary>
        /// The pen to draw the bound in the design-time.
        /// </summary>
        private Pen borderPen = null;

        /// <summary>
        /// Indicates whether we see the given label.
        /// </summary>
        private bool visible = true;


        #endregion

        #region Constructors

        /// <summary>
        /// The class constructor.
        /// </summary>
        public LabelItem()
        {
            textFont = new Font("arial", 10);
            textBrush = Brushes.Green;
            borderPen = new Pen(Color.FromArgb(255, 0, 0));
            borderPen.DashStyle = DashStyle.Dash;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The property sets the text that is displayed in the label.
        /// </summary>
        public string LabelText
        {
            get { return this.labelText; }
            set
            {
                this.labelText = value;

                if (parent != null)
                {
                    //parent.Invalidate();
                    //parent.Update();
                }
            }
        }

        /// <summary>
        /// The property sets the font of the text that will be displayed.
        /// </summary>
        public Font TextFont
        {
            get { return this.textFont; }
            set
            {
                this.textFont = value;

                if (parent != null)
                {
                    parent.Invalidate();
                    parent.Update();
                }
            }
        }

        /// <summary>
        /// The property sets the label position.
        /// </summary>
        public Rectangle LabelRect
        {
            get { return this.labelRect; }
            set
            {
                this.labelRect = value;
                if (parent != null)
                {
                    parent.Invalidate();
                    parent.Update();
                }
            }
        }

        /// <summary>
        /// The property sets the text color.
        /// </summary>
        public Color TextColor
        {
            get { return ((SolidBrush)this.textBrush).Color; }
            set
            {
                this.textBrush = new SolidBrush(value);

                if (parent != null)
                {
                    parent.Invalidate();
                    parent.Update();
                }
            }
        }

        /// <summary>
        /// The property is needed to support the design-time. 
        /// Sets the parent control.
        /// </summary>
        [Browsable(false)]
        public SkinCustomArea Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        /// <summary>
        /// The property defines whether the label is selecetd in the design-time.
        /// </summary>
        [Browsable(false)]
        public bool Selected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;

                if (parent != null)
                {
                    parent.Invalidate();
                    parent.Update();
                }
            }
        }

        /// <summary>
        /// The property defines whether we see the given label.
        /// </summary>
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The method draws the label.
        /// </summary>
        /// <param name="g">The context where the control will be drawn.</param>
        /// <param name="DesignMode">The parameter indicates whether the label is in the design-time.</param>
        public void DrawLabel(Graphics g, bool DesignMode)
        {
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Near;
            sf.Alignment = StringAlignment.Near;

            g.DrawString(labelText, textFont, textBrush, labelRect, sf);
            if (DesignMode)
            {
                if (this.isSelected)
                {
                    borderPen.DashStyle = DashStyle.Dot;
                    borderPen.Width = 2;

                }
                else
                {
                    borderPen.Width = 2;
                    borderPen.DashStyle = DashStyle.Solid;

                }

                g.DrawRectangle(borderPen, this.LabelRect);
            }


        }
        #endregion
    }
}
