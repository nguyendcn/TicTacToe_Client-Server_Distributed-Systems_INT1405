using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using DCN.TicTacToe.UI.Common;
using DCN.TicTacToe.UI.Design;
using DCN.TicTacToe.UI.SkinComponents;

namespace DCN.TicTacToe.UI
{

    /// <summary>huhu
    /// the enumeration setting the animation method by changing the location
    /// and the physical size of the control.
    /// </summary>
    public enum ExpandAnimationType
    {
        FromLeftSide,
        FromTopSide,
        FromRightSide,
        FromBottomSide,
        FromLeftTopCorner,
        FromRightTopCorner,
        FromRightBottomCorner,
        FromLeftBottomCorner,
        FromCenter,
        ScrollFromLeft,
        ScrollFromTop,
        ScrollFromRight,
        ScrollFromBottom
    }

    /// <summary>
    /// The class of the control - the pop-up tooltip.
    /// Allows the child elements to be arranged in the tooltip.
    /// Allows the shape to be specified with the help of a bitmap.
    /// </summary>
    [
    ToolboxItem(true),
    Designer(typeof(SkinCustomAreaDesign))
    ]
    public class SkinCustomArea : DCN.TicTacToe.UI.SkinComponents.SkinControl
    {
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// the bitmap containing the background that is taken from the parent control before 
        /// displaying the pop-up window. It allows us to imitate 
        /// the semi-transparent window with non-transparent controls and text.
        /// </summary>
        private Bitmap backgroundBitmap = null;

        /// <summary>
        /// The parameter that is passed to the DrawImage function to perform additional
        /// operations on the displayed image.
        /// </summary>
        private ImageAttributes ia = new ImageAttributes();

        /// <summary>
        /// The matrix of the color conversion applied to each pixel.
        /// </summary>
        private ColorMatrix cm = new ColorMatrix();

        /// <summary>
        /// The timer that is responsible fro animation.
        /// </summary>
        private Timer alphaTimer = new Timer();

        /// <summary>
        /// the coordinates of the part of the background that should be displayed by the control.
        /// The point of origin is the top left corner of the parent window of the control.
        /// </summary>
        private Point globalPos;

        /// <summary>
        /// the image defining the appearcance of the pop-up control.
        /// </summary>
        private Bitmap frontImage = null;

        /// <summary>
        /// the field defining the number os animation steps.
        /// increasing the number of animation steps together with decreasing 
        /// the timer interval leads to more smooth animation.
        /// </summary>
        private int expandAnimationSteps = 10;

        /// <summary>
        /// the current width of the control during the animation.
        /// </summary>
        private float currentWidth;
        /// <summary>
        /// the current height of the control during the animation.
        /// </summary>

        private float currentHeight;
        /// <summary>
        /// the current position of the control during the animation.
        /// </summary>
        private PointF currentPos;

        /// <summary>
        /// sets the expand method of animation.
        /// </summary>
        private bool expandAnimation = false;
        /// <summary>
        /// sets the method of animation with the variable transparency.
        /// </summary>
        private bool alphaAnimation = true;

        /// <summary>
        /// the given field defines the changes of the control's sizes during 
        /// the expand animation.		
        /// </summary>
        private ExpandAnimationType expandType = ExpandAnimationType.FromLeftTopCorner;

        /// <summary>
        /// the array of labels
        /// </summary>
        private LabelCollection items = new LabelCollection();

        /// <summary>
        /// the position, where the control will be displayed during the animation.
        /// </summary>
        private Point animationPosition = new Point(100, 100);

        /// <summary>
        /// Whether to draw the controls on the background only when loading the control or
        /// before every displaying (is needed if the control's background will change).
        /// </summary>
        private bool updateBitmapsOnce = false;

        /// <summary>
        /// Whether to take the background with controls or the user specifies it.
        /// </summary>
        private bool obtainBackground = true;

        #endregion

        #region Constructors

        /// <summary>
        /// the class constructor.
        /// Setts the animation timers and turns on the message handlers.		
        /// </summary>
        public SkinCustomArea()
        {
            InitializeComponent();

            alphaTimer.Interval = 20;
            alphaTimer.Tick += new EventHandler(timer_Tick);

            cm.Matrix33 = 1.0f;

            Paint += new PaintEventHandler(SkinTooltip_Paint);
            Load += new EventHandler(SkinTooltip_Load);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The property defining whether the control should get its background or the user
        /// will specify it.
        /// Using this functionality allows us to shorten the time needed to initialize  
        /// the control. Is used together with the BackgroundBitmap property.
        /// </summary>
        public bool ObtainBackground
        {
            get { return this.obtainBackground; }
            set { this.obtainBackground = value; }
        }


        /// <summary>
        /// Allows the control's background to be determined (as a bitmap)
        /// Is used together with the ObtainBackground property.
        /// </summary>
        public Bitmap BackgroundBitmap
        {
            get { return this.backgroundBitmap; }
            set { this.backgroundBitmap = value; }
        }

        /// <summary>
        /// The propert that defines whether to update the control's background only once 
        /// when loading or to do it every time the animation starts.		
        /// </summary>
        public bool UpdateBitmapsOnce
        {
            get { return this.updateBitmapsOnce; }
            set { this.updateBitmapsOnce = value; }
        }

        /// <summary>
        /// The start position of the control's animation.
        /// </summary>
        public Point AnimationPosition
        {
            get { return this.animationPosition; }
            set { this.animationPosition = value; }
        }

        /// <summary>
        /// The propetry sets the type of pop-up with the turned on flag.
        /// ExpandAnimation.
        /// </summary>
        public ExpandAnimationType ExpandType
        {
            get { return this.expandType; }
            set { this.expandType = value; }
        }

        /// <summary>
        /// The property sets the image that is displayed as a control's background.
        /// </summary>
        public Bitmap FrontImage
        {
            get { return this.frontImage; }
            set
            {
                this.frontImage = value;

                this.Invalidate();
            }
        }

        /// <summary>
        /// The property sets the interval of the animation timer.
        /// To change the animation quality one may change the property together 
        /// with the value of the ExpandAnimationSteps property.
        /// The system determines the low bound of the interval decrease.		
        /// When the value is less than 30, the machine most likely
        /// fails to process every animation step.
        /// </summary>
        public int AnimationInterval
        {
            get { return this.alphaTimer.Interval; }
            set { this.alphaTimer.Interval = value; }
        }

        /// <summary>
        /// Sets the number of animation steps.
        /// Larger number of the animation steps and less interval of 
        /// the ainmation timer lead to smoother animation. 
        /// Decreasing the value lead to rather rough but also fast animation
        /// of the control displaying. 		
        /// </summary>
        public int AnimationSteps
        {
            get { return this.expandAnimationSteps; }
            set { this.expandAnimationSteps = value; }
        }

        /// <summary>
        /// Sets whether to display the control with the help of the expand animation.
        /// </summary>
        public bool ExpandAnimation
        {
            get { return this.expandAnimation; }
            set { this.expandAnimation = value; }
        }

        /// <summary>
        /// Sets whether to display the control with the help of the animation with 
        /// changing the transparency.
        /// </summary>
        public bool AlphaAnimation
        {
            get { return this.alphaAnimation; }
            set { this.alphaAnimation = value; }
        }

        /// <summary>
        /// The property sets or returns the collection of label elements.
        /// </summary>
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Browsable(false)
        ]
        public LabelCollection Labels
        {
            get { return this.items; }
            set { this.items = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (this.obtainBackground && backgroundBitmap != null)
                    this.backgroundBitmap.Dispose();

                if (frontImage != null)
                    this.frontImage.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        /// <summary>
        /// Drawing the outline.
        /// </summary>
        /// <param name="sender">The object invoked the event.</param>
        /// <param name="e">the parameters of the event of drawing the control.</param>

        private void SkinTooltip_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            if ((this.DesignMode) && this.frontImage != null)
            {
                e.Graphics.DrawImage(frontImage, 0, 0);

            }

            if (!this.DesignMode && this.frontImage != null)
            {

                Rectangle currentRc = new Rectangle((int)currentPos.X, (int)currentPos.Y, (int)this.currentWidth, (int)this.currentHeight);

                if (obtainBackground)
                    e.Graphics.DrawImage(this.backgroundBitmap,
                        0, 0, new Rectangle(0, 0, this.Width, this.Height), GraphicsUnit.Pixel);
                else
                    e.Graphics.DrawImage(this.backgroundBitmap,
                        0, 0, new Rectangle(globalPos.X, globalPos.Y, this.Width, this.Height), GraphicsUnit.Pixel);


                ia.SetColorMatrix(cm);


                e.Graphics.DrawImage(frontImage,
                    currentRc, 0, 0, frontImage.Width, frontImage.Height,
                    GraphicsUnit.Pixel, ia);

            }

            if (DesignMode)
            {
                foreach (Object item in this.items)
                {
                    if (item is LabelItem)
                        ((LabelItem)item).DrawLabel(e.Graphics, true);
                }
            }

        }

        /// <summary>
        /// the initialization when loading the background by drawing all child controls on it.		
        /// </summary>
        /// <param name="sender">The object invoked the event.</param>
        /// <param name="e">the event's parameters.</param>
        public void SkinTooltip_Load(object sender, EventArgs e)
        {
            this.BringToFront();

            if (!this.DesignMode && this.obtainBackground)
                Utils.GetControlScreenshot(this.Parent, ref this.backgroundBitmap);

            int x = Parent.PointToScreen(this.AnimationPosition).X - ((Form)Parent).DesktopLocation.X;
            int y = Parent.PointToScreen(this.AnimationPosition).Y - ((Form)Parent).DesktopLocation.Y;

            this.globalPos = new Point(x, y);

            if (!DesignMode)
            {
                foreach (Control ctrl in this.Controls)
                    ctrl.Visible = true;

                if (this.frontImage != null)
                    UpdateBitmap();

                foreach (Control ctrl in this.Controls)
                    ctrl.Visible = false;

                this.Visible = false;
            }

            if (!this.DesignMode && this.obtainBackground)
                GetBackScreenshot();

        }

        /// <summary>
        /// The funciton adding the image disaplyed during the animation.
        /// Is useful when the labels in text fileds are changed or when
        /// the state of child controls is changed.
        /// </summary>
        /// <param name="controlsHidden">The parameter is needed to pass true in the case when
        /// the control is in the hidden state.</param>
        public void UpdateFrontImage(bool controlsHidden)
        {

            if (this.frontImage != null)
            {
                if (controlsHidden)
                    foreach (Control ctrl in this.Controls)
                        ctrl.Visible = true;

                UpdateBitmap();

                if (controlsHidden)
                    foreach (Control ctrl in this.Controls)
                        ctrl.Visible = false;
            }
        }

        /// <summary>
        /// The function of handling the timer's event.
        /// modifies the control's parameters for animation.
        /// </summary>
        /// <param name="sender">The object invoked the event.</param>
        /// <param name="e">the events parameters.</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            bool stopped = false;

            if (this.ExpandAnimation)
            {

                switch (this.expandType)
                {
                    case ExpandAnimationType.FromLeftTopCorner:

                        this.currentHeight += (float)this.Height / (float)this.expandAnimationSteps;
                        this.currentWidth += (float)this.Width / (float)this.expandAnimationSteps;

                        break;

                    case ExpandAnimationType.FromRightSide:

                        this.currentWidth += (float)this.Width / (float)this.expandAnimationSteps;
                        this.currentPos.X -= (float)this.Width / (float)this.expandAnimationSteps;

                        break;

                    case ExpandAnimationType.FromTopSide:
                        this.currentHeight += (float)this.Height / (float)this.expandAnimationSteps;
                        break;

                    case ExpandAnimationType.FromCenter:

                        this.currentHeight += (float)this.Height / (float)this.expandAnimationSteps;
                        this.currentWidth += (float)this.Width / (float)this.expandAnimationSteps;
                        this.currentPos.X -= (float)this.Width / (float)(2 * this.expandAnimationSteps);
                        this.currentPos.Y -= (float)this.Height / (float)(2 * this.expandAnimationSteps);

                        break;

                    case ExpandAnimationType.FromBottomSide:

                        this.currentHeight += (float)this.Height / (float)this.expandAnimationSteps;
                        this.currentPos.Y -= (float)this.Height / (float)this.expandAnimationSteps;


                        break;

                    case ExpandAnimationType.FromLeftBottomCorner:

                        this.currentHeight += (float)this.Height / (float)this.expandAnimationSteps;
                        this.currentWidth += (float)this.Width / (float)this.expandAnimationSteps;
                        this.currentPos.Y -= (float)this.Height / (float)this.expandAnimationSteps;

                        break;

                    case ExpandAnimationType.FromLeftSide:

                        this.currentWidth += (float)this.Width / (float)this.expandAnimationSteps;

                        break;

                    case ExpandAnimationType.FromRightBottomCorner:

                        this.currentHeight += (float)this.Height / (float)this.expandAnimationSteps;
                        this.currentWidth += (float)this.Width / (float)this.expandAnimationSteps;
                        this.currentPos.Y -= (float)this.Height / (float)this.expandAnimationSteps;
                        this.currentPos.X -= (float)this.Width / (float)this.expandAnimationSteps;

                        break;

                    case ExpandAnimationType.FromRightTopCorner:

                        this.currentHeight += (float)this.Height / (float)this.expandAnimationSteps;
                        this.currentWidth += (float)this.Width / (float)this.expandAnimationSteps;
                        this.currentPos.X -= (float)this.Width / (float)this.expandAnimationSteps;

                        break;

                    case ExpandAnimationType.ScrollFromLeft:

                        this.currentPos.X += (float)this.Width / (float)this.expandAnimationSteps;

                        break;

                    case ExpandAnimationType.ScrollFromTop:

                        this.currentPos.Y += (float)this.Height / (float)this.expandAnimationSteps;

                        break;


                    case ExpandAnimationType.ScrollFromRight:

                        this.currentPos.X -= (float)this.Width / (float)this.expandAnimationSteps;

                        break;


                    case ExpandAnimationType.ScrollFromBottom:

                        this.currentPos.Y -= (float)this.Height / (float)this.expandAnimationSteps;

                        break;


                }

                bool needStop = false;

                if (
                    expandType != ExpandAnimationType.ScrollFromLeft &&
                    expandType != ExpandAnimationType.ScrollFromTop &&
                    expandType != ExpandAnimationType.ScrollFromRight &&
                    expandType != ExpandAnimationType.ScrollFromBottom)
                {

                    if (this.currentWidth >= this.Width && this.currentHeight >= this.Height)
                    {
                        needStop = true;
                    }
                }
                else
                {
                    if (expandType == ExpandAnimationType.ScrollFromLeft || expandType == ExpandAnimationType.ScrollFromTop)
                    {
                        if (this.currentPos.X >= 0 && this.currentPos.Y >= 0)
                        {
                            needStop = true;
                        }
                    }
                    else
                        if (this.currentPos.X <= 0 && this.currentPos.Y <= 0)
                        needStop = true;
                }

                if (needStop)
                {
                    foreach (Control ctrl in this.Controls)
                        ctrl.Visible = true;

                    this.currentHeight = this.Height;
                    this.currentWidth = this.Width;

                    this.currentPos = new Point(0, 0);

                    if (this.AlphaAnimation)
                        cm.Matrix33 = 1.0f;

                    alphaTimer.Stop();

                    if (AnimationOver != null && !stopped)
                    {
                        AnimationOver(this, new EventArgs());
                    }

                    stopped = true;

                }

            }

            if (this.AlphaAnimation)
            {

                if (cm.Matrix33 < 1.0f)
                    cm.Matrix33 += 1.0f / (float)this.expandAnimationSteps;

                if (cm.Matrix33 >= 1.0f)
                {

                    if (!this.ExpandAnimation)
                    {
                        foreach (Control ctrl in this.Controls)
                            ctrl.Visible = true;

                        alphaTimer.Stop();
                    }

                    cm.Matrix33 = 1.0f;

                    if (AnimationOver != null && !stopped)
                    {
                        AnimationOver(this, new EventArgs());
                    }

                }

            }

            Invalidate();
            Update();
        }


        /// <summary>
        /// The function should be called to start the animation.
        /// </summary>
        /// <param name="location">ïîçèöèÿ âåðõíåãî ëåâîãî óãëà âñïëûâøåãî ïîñëå àíèìàöèè êîíòðîëà.</param>
        public void Animate()
        {
            this.BringToFront();
            this.Location = this.animationPosition;

            int x = Parent.PointToScreen(this.Location).X - ((Form)Parent).DesktopLocation.X;
            int y = Parent.PointToScreen(this.Location).Y - ((Form)Parent).DesktopLocation.Y;

            this.globalPos = new Point(x, y);

            this.Visible = false;

            if (!this.updateBitmapsOnce && obtainBackground)
            {
                Utils.GetControlScreenshot(this.Parent, ref this.backgroundBitmap);
            }

            foreach (Control ctrl in this.Controls)
                ctrl.Visible = true;

            if (!this.updateBitmapsOnce && obtainBackground)
            {
                UpdateBackground();
                GetBackScreenshot();
            }

            foreach (Control ctrl in this.Controls)
                ctrl.Visible = false;

            this.Visible = true;



            if (this.AlphaAnimation)
            {
                cm.Matrix33 = 0.0f;
            }

            if (this.ExpandAnimation)
            {
                RectangleF rectBound = this.Region.GetBounds(Graphics.FromHwnd(this.Handle));

                switch (this.expandType)
                {
                    case ExpandAnimationType.FromLeftTopCorner:

                        this.currentHeight = 0;
                        this.currentWidth = 0;
                        this.currentPos = new Point(0, 0);

                        break;

                    case ExpandAnimationType.FromRightSide:

                        this.currentHeight = this.Height;
                        this.currentPos.X = this.Width;
                        this.currentPos.Y = 0;
                        this.currentWidth = 0;

                        break;

                    case ExpandAnimationType.FromTopSide:

                        this.currentHeight = 0;
                        this.currentWidth = this.Width;
                        this.currentPos.X = 0;
                        this.currentPos.Y = 0;

                        break;

                    case ExpandAnimationType.FromCenter:

                        this.currentHeight = 0;
                        this.currentWidth = 0;
                        this.currentPos.X = this.Width / 2.0f;
                        this.currentPos.Y = this.Height / 2.0f;

                        break;

                    case ExpandAnimationType.FromBottomSide:

                        this.currentHeight = 0;
                        this.currentWidth = this.Width;
                        this.currentPos.X = 0.0f;
                        this.currentPos.Y = this.Height;

                        break;

                    case ExpandAnimationType.FromLeftBottomCorner:

                        this.currentHeight = 0;
                        this.currentWidth = 0;
                        this.currentPos = new Point(0, this.Height);

                        break;

                    case ExpandAnimationType.FromLeftSide:

                        this.currentHeight = this.Height;
                        this.currentPos.X = 0;
                        this.currentPos.Y = 0;
                        this.currentWidth = 0;

                        break;

                    case ExpandAnimationType.FromRightBottomCorner:

                        this.currentHeight = 0;
                        this.currentWidth = 0;
                        this.currentPos = new Point(this.Width, this.Height);

                        break;

                    case ExpandAnimationType.FromRightTopCorner:

                        this.currentHeight = 0;
                        this.currentWidth = 0;
                        this.currentPos = new Point(this.Width, 0);

                        break;

                    case ExpandAnimationType.ScrollFromLeft:

                        this.currentWidth = this.Width;
                        this.currentHeight = this.Height;
                        this.currentPos = new Point(-this.Width, 0);

                        break;


                    case ExpandAnimationType.ScrollFromTop:

                        this.currentWidth = this.Width;
                        this.currentHeight = this.Height;
                        this.currentPos = new Point(0, -this.Height);

                        break;

                    case ExpandAnimationType.ScrollFromRight:

                        this.currentWidth = this.Width;
                        this.currentHeight = this.Height;
                        this.currentPos = new Point(this.Width, 0);

                        break;

                    case ExpandAnimationType.ScrollFromBottom:

                        this.currentWidth = this.Width;
                        this.currentHeight = this.Height;
                        this.currentPos = new Point(0, this.Height);

                        break;

                }

            }
            else
            {
                this.currentHeight = this.Height;
                this.currentWidth = this.Width;
                this.currentPos = new Point(0, 0);
            }

            if (this.AlphaAnimation || this.ExpandAnimation)
                alphaTimer.Start();
            else
            {
                foreach (Control ctrl in this.Controls)
                    ctrl.Visible = true;
            }


        }


        /// <summary>
        /// the function draws the child elements on the control's backgroud bitmap.
        /// </summary>
        private void UpdateBitmap()
        {
            Graphics g = null;
            Bitmap bmp = null;
            Region old = null;

            if (frontImage != null)
            {
                g = Graphics.FromImage(frontImage);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;

                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is SkinControl)
                    {
                        Region r = ctrl.Region.Clone();
                        r.Translate(ctrl.Location.X, ctrl.Location.Y);
                        old = g.Clip;

                        g.Clip = r;
                    }

                    Utils.GetControlScreenshot(ctrl, ref bmp);
                    g.DrawImage(bmp, ctrl.Location.X, ctrl.Location.Y);

                    if (ctrl is SkinControl)
                        g.Clip = old;

                }

                foreach (Object item in this.items)
                {
                    if (item is LabelItem)
                        ((LabelItem)item).DrawLabel(g, false);
                }

                g.Dispose();
            }

            if (obtainBackground)
                UpdateBackground();

        }


        /// <summary>
        /// draws the controls of the parent window on the bitmap that is used to 
        /// hide the control.
        /// </summary>
        private void UpdateBackground()
        {
            Graphics g = null;
            Bitmap bmp = null;
            Region old = null;

            if (backgroundBitmap != null)
            {

                g = Graphics.FromImage(this.backgroundBitmap);
                bmp = null;

                foreach (Control ctrl in Parent.Controls)
                {
                    if (ctrl != this)
                    {

                        int x = ((Form)Parent).PointToScreen(ctrl.Location).X - ((Form)Parent).DesktopLocation.X;
                        int y = ((Form)Parent).PointToScreen(ctrl.Location).Y - ((Form)Parent).DesktopLocation.Y;

                        if (ctrl is SkinControl)
                        {
                            Region r = ctrl.Region.Clone();
                            r.Translate(x, y);
                            old = g.Clip;

                            g.Clip = r;
                        }

                        Utils.GetControlScreenshot(ctrl, ref bmp);
                        g.DrawImage(bmp, x, y);

                        if (ctrl is SkinControl)
                            g.Clip = old;


                    }
                }

                g.Dispose();
            }


        }

        /// <summary>
        /// The function gets the part from the screenshot of the parent window, which corresponds
        /// to the area that is occupied by the control.
        /// </summary>
        public void GetBackScreenshot()
        {
            int right = globalPos.X + this.Size.Width;
            int bottom = globalPos.Y + this.Size.Height;

            if (backgroundBitmap != null && right < backgroundBitmap.Width && bottom < backgroundBitmap.Height)
                this.backgroundBitmap = backgroundBitmap.Clone(new Rectangle(globalPos, this.Size),
                    backgroundBitmap.PixelFormat);

            GC.Collect();
        }

        #endregion

        #region Events

        /// <summary>
        /// the event that is invoked in the moment when the animation is finished
        /// (i.e. after the last animation step).
        /// </summary>
        public event EventHandler AnimationOver;

        #endregion

    }
}

