///////////////////////////////////////////////////////////////////////////////
//
//  File:           Utils.cs
//
//  Facility:       The unit contains the Margins structure, the Utils and Win32API classes
//
//  Abstract:       Additional functions for work with windows and graphics, and with the 
//                  unmanaged code of the win32 function.
//
//  Environment:    VC 7.1
//
//  Author:         DCN Ltd.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace DCN.TicTacToe.UI.Common
{
    #region Margins class

    /// <summary>
    /// The structure that presents the indents from the bounds of the rectangular area.
    /// Allows different operations with the indents to be performed.
    /// </summary>
    public struct Margins
    {

        #region Structure fields

        /// <summary>
        /// The left margin.
        /// </summary>
        private int left;

        /// <summary>
        /// The top margin.
        /// </summary>
        private int top;

        /// <summary>
        /// The right margin.
        /// </summary>
        private int right;

        /// <summary>
        /// The bottom margin.
        /// </summary>
        private int bottom;

        /// <summary>
        /// The flag indicating that the structure fields hold the values
        /// different from the default ones.
        /// </summary>
        private bool changed;

        #endregion // Structure fields

        #region Structure constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="leftMargin">The left margin.</param>
        /// <param name="topMargin">The top margin.</param>
        /// <param name="rightMargin">The right margin.</param>
        /// <param name="bottomMargin">The bottom margin.</param>
        public Margins(int leftMargin, int topMargin, int rightMargin,
            int bottomMargin)
        {
            this.left = leftMargin;
            this.top = topMargin;
            this.right = rightMargin;
            this.bottom = bottomMargin;
            this.changed = true;
        }

        #endregion // Structure constructors

        #region Structure properties

        /// <summary>
        /// Getting/setting the left margin.
        /// </summary>
        public int LeftMargin
        {
            get
            {
                return this.left;
            }
            set
            {
                this.left = value;
            }
        }

        /// <summary>
        /// Getting/setting the top margin.
        /// </summary>
        public int TopMargin
        {
            get
            {
                return this.top;
            }
            set
            {
                this.top = value;
            }
        }

        /// <summary>
        /// Getting/setting the right margin.
        /// </summary>
        public int RightMargin
        {
            get
            {
                return this.right;
            }
            set
            {
                this.right = value;
            }
        }

        /// <summary>
        /// Getting/setting the bottom margin.
        /// </summary>
        public int BottomMargin
        {
            get
            {
                return this.bottom;
            }
            set
            {
                this.bottom = value;
            }
        }

        #endregion // Structure properties

        #region Structure methods

        /// <summary>
        /// Initialisation of the class members.
        /// </summary>
        /// <param name="leftMargin">The left margin.</param>
        /// <param name="topMargin">The top margin.</param>
        /// <param name="rightMargin">The right margin.</param>
        /// <param name="bottomMargin">The bottom margin.</param>
        public void SetMargins(int leftMargin, int topMargin, int rightMargin,
            int bottomMargin)
        {
            this.left = leftMargin;
            this.top = topMargin;
            this.right = rightMargin;
            this.bottom = bottomMargin;
            this.changed = true;
        }

        /// <summary>
        /// Checks if the margins match the default ones.
        /// </summary>
        /// <returns>true if the margins match the default ones. 
        /// </returns>
        public bool IsDefault()
        {
            return this.changed == false;
        }

        /// <summary>
        /// Expands the margins to the specified number of items.
        /// </summary>
        /// <param name="l">Increment for the left margin.</param>
        /// <param name="t">Increment for the top margin.</param>
        /// <param name="r">Increment for the right margin.</param>
        /// <param name="b">Increment for the bottom margin.</param>
        public void Inflate(int l, int t, int r, int b)
        {
            this.left -= l;
            this.top -= t;
            this.right += r;
            this.bottom += b;
            this.changed = true;
        }

        /// <summary>
        /// Expands the margins by the values specified in the m parameter.
        /// </summary>
        /// <param name="m">
        /// The structure that contains increments for each margin.
        /// </param>
        public void Inflate(Margins m)
        {
            this.Inflate(m.LeftMargin, m.TopMargin, m.RightMargin,
                m.BottomMargin);
            this.changed = true;
        }

        /// <summary>
        /// This method returns a human-readable string that represents the
        /// current Margins instance.
        /// </summary>
        /// <returns>
        /// Returns a String that represents the current Margins object.
        /// </returns>
        public override string ToString()
        {
            StringBuilder description = new StringBuilder(base.ToString());
            description.Append(": ");
            String content = String.Format(
                "Left: {0}, Top: {1}, Right: {2}, Bottom: {3}", this.left,
                this.top, this.right, this.bottom);
            description.Append(content);
            return description.ToString();
        }


        #endregion // Structure methods
    }

    #endregion

    #region Utils Class
    /// <summary>
    /// The Utils class contains the set of static methods
    /// used in different components.
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// The constructor of the class.
        /// </summary>
        public Utils() { }
        #region Class Methods
        /// <summary>
        /// This method inflates the Rectangle structure by moving its sides
        /// away from its center. To do this, InflateRect subtracts margins
        /// units to the left and top and adds margins units from the right and
        /// bottom.
        /// </summary>
        /// <param name="rec">The rectangle to inflate.</param>
        /// <param name="margins">
        /// The margins at which the rectangle is moved.
        /// </param>
        /// <returns>The inflated rectangle.</returns>
        public static Rectangle InflateRect(Rectangle rec, Margins margins)
        {
            return new Rectangle(rec.Left - margins.LeftMargin, rec.Top -
                margins.TopMargin, rec.Width + margins.RightMargin +
                margins.LeftMargin, rec.Height + margins.BottomMargin +
                margins.TopMargin);
        }

        /// <summary>
        /// This method deflates the Rectangle structure by moving its sides
        /// toward its center. To do this, DeflateRect adds margins units to the
        /// left and top and subtracts margins units from the right and bottom.
        /// </summary>
        /// <param name="rect">The rectangle to be deflected.</param>
        /// <param name="margins">
        /// The margins at which the rectangle is deflected.
        /// </param>
        public static void DeflateRect(ref Rectangle rect, Margins margins)
        {
            rect = new Rectangle(rect.Left + margins.LeftMargin, rect.Top +
                margins.TopMargin, rect.Width - margins.LeftMargin -
                margins.RightMargin, rect.Height - margins.TopMargin -
                margins.BottomMargin);
        }

        public static void DeflateRect(ref Rectangle rect, int l, int t, int r, int b)
        {
            rect = new Rectangle(rect.Left + l, rect.Top + t, rect.Width - l - r,
                rect.Height - t - b);

        }

        /// <summary>
        /// This method calculates the coordinates of the rectangle center.
        /// </summary>
        /// <param name="rect">The rectangle for which it is necessary to
        /// determine the central point.</param>
        /// <returns>
        /// The point with the coordinates of the rectangle center.
        /// </returns>
        public static Point CenterPoint(Rectangle rect)
        {
            return new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height
                / 2);
        }

        /// <summary>
        /// Getting the icon title font
        /// </summary>
        /// <param name="font">The font reference. The output parameter.</param>
        public static Font GetItemIconTitleFont()
        {
            Win32API.LOGFONT lf = new Win32API.LOGFONT();
            lf.lfFaceName = new char[32];

            Font font = null;
            if (Utils.GetItemIconTitleLogFont(ref lf))
            {
                try
                {
                    font = Font.FromLogFont(lf);
                    return font;
                }
                catch (ArgumentException)
                {
                }
            }
            font = new Font("Tahoma", 11.0f, FontStyle.Regular);
            return font;
        }

        /// <summary>
        /// Getting the icon title Log Font
        /// </summary>
        /// <param name="lf">
        /// The LogFont reference. The output parameter.
        /// </param>
        /// <returns><b>false</b> If failed to get the font.</returns>
        public static bool GetItemIconTitleLogFont(ref Win32API.LOGFONT lf)
        {
            Version osVersion = Environment.OSVersion.Version;
            Version frameworkVersion = Environment.Version;
            if (osVersion.Major == 5 && osVersion.Minor == 2 &&
                frameworkVersion.Major == 1 && frameworkVersion.Minor <= 1)
            {
                //When working on Windows 2003 (Framework 1.0 and 1.1),
                //the SystemParametersInfo function invocation causes
                //ExecutionEngineException.
                //In this case, set the font parameters by default.

                lf.lfFaceName[0] = 'T';
                lf.lfFaceName[2] = 'a';
                lf.lfFaceName[4] = 'h';
                lf.lfFaceName[6] = 'o';
                lf.lfFaceName[8] = 'm';
                lf.lfFaceName[10] = 'a';

                const byte DEFAULT_CHARSET = 1;
                const int FW_NORMAL = 400;
                lf.lfCharSet = DEFAULT_CHARSET;
                lf.lfClipPrecision = 0;
                lf.lfEscapement = 0;
                lf.lfHeight = -11;
                lf.lfItalic = 0;
                lf.lfOrientation = 0;
                lf.lfOutPrecision = 0;
                lf.lfPitchAndFamily = 0;
                lf.lfQuality = 0;
                lf.lfUnderline = 0;
                lf.lfStrikeOut = 0;
                lf.lfWeight = FW_NORMAL;
                lf.lfWidth = 0;
            }
            else
            {
                if (Win32API.SystemParametersInfo
                    (Win32API.SPI_GETICONTITLELOGFONT, (uint)
                    System.Runtime.InteropServices.Marshal.SizeOf(lf), ref lf,
                    0) == false)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Method modifies passed in bitmap by inverting color of each pixel.
        /// Bitmap must be Format32bppArgb formatted.
        /// </summary>
        /// <param name="bmp">Bitmap being modified.</param>
        public static void InvertBitmapColor(Bitmap bmp)
        {
            if (bmp == null) return;
            if (bmp.PixelFormat != PixelFormat.Format32bppArgb) return;

            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color srcClr = bmp.GetPixel(i, j);
                    Color dstClr = Color.FromArgb(srcClr.A,
                        255 - srcClr.R,
                        255 - srcClr.G,
                        255 - srcClr.B);
                    bmp.SetPixel(i, j, dstClr);
                }

        }

        /// <summary>
        /// The function gets the control screenshotto the object of the Bitmap class.
        /// </summary>
        /// <param name="control">The control, which  screenshot should be made.</param>
        /// <param name="bitmap">The reference to the Bitmap, where the control screenshot should be set.</param>
        public static void GetControlScreenshot(Control control, ref Bitmap bitmap)
        {
            if (control == null || control.Handle == IntPtr.Zero)
                return;

            if (control.Width < 1 || control.Height < 1) return;

            if (bitmap != null)
                bitmap.Dispose();

            // preparing the bitmap.
            bitmap = new Bitmap(control.Width, control.Height);
            Graphics destGraphics = Graphics.FromImage(bitmap);

            // setting the flag indicating that we need to print both the client's and
            // the non-client's window rectangles.
            int printFlags = (int)(
                    Win32API.PRF_NONCLIENT | Win32API.PRF_CLIENT);

            System.IntPtr param = new System.IntPtr(printFlags);
            System.IntPtr hdc = destGraphics.GetHdc();

            // sending the WM_PRINT message to the control window.
            Win32API.SendMessage(control.Handle, Win32API.WM_PRINT, hdc, param);
            destGraphics.ReleaseHdc(hdc);

            destGraphics.Dispose();
        }

        #endregion // Class Methods
    }
    #endregion // Utils Class

    #region Win32API Class
    /// <summary>
    /// The Win32 API imported routines, struts and constants..
    /// </summary>
    public class Win32API
    {
        #region Constants

        public const int S_OK = 0;

        public const int VK_SHIFT = 0x10;

        public const int SPI_GETICONTITLELOGFONT = 31;
        public const int SPI_GETWORKAREA = 48;
        public const int SPI_GETMOUSEHOVERTIME = 102;
        public const int SPI_GETMENUANIMATION = 0x1002;

        /// <summary>
        /// Definition of constants for the WM_PRINT message
        /// </summary>        
        public const long PRF_NONCLIENT = 0x00000002L;
        public const long PRF_CLIENT = 0x00000004L;
        public const long PRF_ERASEBKGND = 0x00000008L;
        public const long PRF_CHILDREN = 0x00000010L;
        public const long PRF_OWNED = 0x00000020L;

        public const uint WS_POPUP = 0x80000000;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WS_EX_TOPMOST = 0x00000008;
        public const int WS_EX_LAYERED = 0x00080000;
        public const int WM_PAINT = 0x000F;
        public const int WM_NCPAINT = 0x0085;
        public const int WM_ACTIVATE = 0x0006;
        public const int WM_NCHITTEST = 0x0084;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_NCMOUSEMOVE = 0x00A0;
        public const int HTCAPTION = 2;

        public const int WA_INACTIVE = 0;
        public const int WA_ACTIVE = 1;
        public const int WA_CLICKACTIVE = 2;

        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOREDRAW = 0x0008;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_NOOWNERZORDER = 0x0200;   /* Don't do owner Z ordering */
        public const int SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */

        public const int RDW_INVALIDATE = 0x0001;
        public const int RDW_ALLCHILDREN = 0x0080;
        public const int RDW_UPDATENOW = 0x0100;

        public const int LWA_ALPHA = 0x00000002;

        public const int MA_NOACTIVATE = 3;

        public const int TME_LEAVE = 0x00000002;
        public const int TME_NONCLIENT = 0x00000010;
        public const int TME_CANCEL = unchecked((int)0x80000000);

        /// <summary>
        /// A flag of copying image without changes for the BitBlt function.
        /// </summary>
        public const int SRCCOPY = 0x00CC0020;

        public enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        public enum HookCodes : int
        {
            HC_ACTION = 0,
            HC_GETNEXT = 1,
            HC_SKIP = 2,
            HC_NOREMOVE = 3,
            HC_SYSMODALON = 4,
            HC_SYSMODALOFF = 5
        };

        #endregion // Constants

        #region Messages
        /// <summary>
        /// The constant that specifies the identifier of the window's
        /// horizontal scroll message.
        /// </summary>
        public const int WM_HSCROLL = 0x0114;

        /// <summary>
        /// The constant that specifies the identifier of the window's
        /// vertical scroll message.
        /// </summary>
        public const int WM_VSCROLL = 0x0115;

        /// <summary>
        /// The constant that specifies the identifier of the message
        /// about the window lost the focus.
        /// </summary>
        public const int WM_CAPTURE_CHANGED = 0x0215;

        /// <summary>
        /// The constant that specifies the identifier of the mouse wheel
        /// scroll message.
        /// </summary>
        public const int WM_MOUSEWHEEL = 0x020a;

        public const int WM_PRINT = 0x0317;
        public const int WM_PRINTCLIENT = 0x0318;
        public const int WM_ERASEBKGND = 0x0014;
        public const int WM_MOVE = 0x0003;
        public const int WM_MOVING = 0x0216;
        public const int WM_WINDOWPOSCHANGING = 0x0046;
        public const int WM_WINDOWPOSCHANGED = 0x0047;
        public const int WM_MOUSEACTIVATE = 0x0021;
        public const int WM_NCMOUSELEAVE = 0x02A2;
        public const int WM_MOUSELEAVE = 0x02A3;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCLBUTTONUP = 0x00A2;
        public const int WM_ENTERSIZEMOVE = 0x0231;
        public const int WM_EXITSIZEMOVE = 0x0232;

        public const int WM_SETTINGCHANGE = 0x001A;
        public const int WM_DISPLAYCHANGE = 0x007E;
        public const int WM_CAPTURECHANGED = 0x0215;

        public const int WM_STYLECHANGING = 0x007C;
        public const int WM_STYLECHANGED = 0x007D;

        #endregion // Messages

        #region Structures
        /// <summary>
        /// The Point structure defines the x- and y-coordinates of a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            #region Fields
            /// <summary>
            /// The x value of the point's coordinates.
            /// </summary>
            public int X;

            /// <summary>
            /// The y value of the point's coordinates.
            /// </summary>
            public int Y;
            #endregion

            #region Lifecycle
            /// <summary>
            /// Initializes a new instance of the <c>POINT</c> structure.
            /// </summary>
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
            #endregion

            #region Operator overloads
            /// <summary>
            /// Implicitly casts a <see cref="Point"/> to a <c>POINT</c>.
            /// </summary>
            /// <param name="p">
            /// The <c>POINT</c> instance to cast to a <c>Point</c> instance.
            /// </param>
            /// <returns>The casted <c>Point</c> structure.</returns>
            public static implicit operator Point(POINT p)
            {
                return new Point(p.X, p.Y);
            }

            /// <summary>
            /// Implicitly casts a <see cref="Point"/> to a <c>POINT</c>.
            /// </summary>
            /// <param name="p">
            /// The <c>Point</c> instance to cast to a <c>POINT</c> instance.
            /// </param>
            /// <returns>The casted <c>POINT</c> structure.</returns>
            public static implicit operator POINT(Point p)
            {
                return new POINT(p.X, p.Y);
            }
            #endregion
        } // struct POINT

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            /// <summary>
            /// Implicitly casts a <see cref="RECT"/> to a <c>Rectangle</c>.
            /// </summary>
            /// <param name="rc">
            /// The <c>RECT</c> instance to cast to a <c>Rectangle</c> instance.
            /// </param>
            /// <returns>The casted <c>Rectangle</c> structure.</returns>
            public static implicit operator Rectangle(RECT rc)
            {
                return new Rectangle(rc.left, rc.top, rc.right - rc.left, rc.bottom - rc.top);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LOGFONT
        {
            public int lfHeight;
            public int lfWidth;
            public int lfEscapement;
            public int lfOrientation;
            public int lfWeight;
            public byte lfItalic;
            public byte lfUnderline;
            public byte lfStrikeOut;
            public byte lfCharSet;
            public byte lfOutPrecision;
            public byte lfClipPrecision;
            public byte lfQuality;
            public byte lfPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public /*string*/char[] lfFaceName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hWnd;
            public IntPtr hWndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TRACKMOUSEEVENT
        {
            public int cbSize;
            public int dwFlags;
            public IntPtr hwndTrack;
            public int dwHoverTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public RECT rcPaint;
            public bool fRestore;
            public bool fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public byte[] rgbReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STYLESTRUCT
        {
            public uint oldStyleFlags;
            public uint newStyleFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEHOOKSTRUCT
        {
            public POINT pt;
            public IntPtr hwnd;
            public uint wHitTestCode;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEHOOKSTRUCTEX
        {
            public MOUSEHOOKSTRUCT mouseHookStruct;
            public uint mouseData;
        }

        #endregion // Structures

        #region Functions

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT Point);

        /// <summary>
        /// This function sends the specified message to a window or windows.
        /// </summary>
        /// <param name="hwnd">
        /// Handle to the window whose window procedure will receive the
        /// message.
        /// </param>
        /// <param name="msg">Specifies the message to be sent.</param>
        /// <param name="wParam">
        /// Specifies additional message-specific information.
        /// </param>
        /// <param name="lParam">
        /// Specifies additional message-specific information.
        /// </param>
        /// <returns>
        /// The return value specifies the result of the message processing
        /// and depends on the message sent.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr
            wParam, IntPtr lParam);

        //Overload for string lParam (e.g. WM_GETTEXT)
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr
            wParam, [Out]StringBuilder lParam);

        /// <summary>
        /// The function performs a bit-block transfer of the color data
        /// corresponding to a rectangle of pixels from the specified
        /// source device context into a destination device context.
        /// </summary>
        /// <param name="hdcDest">
        /// Handle to the destination device context.
        /// </param>
        /// <param name="xDest">
        /// Specifies the x-coordinate, in logical units, of the upper-left
        /// corner of the destination rectangle.
        /// </param>
        /// <param name="yDest">
        /// Specifies the y-coordinate, in logical units, of the upper-left
        /// corner of the destination rectangle.
        /// </param>
        /// <param name="wDest">
        /// Specifies the width, in logical units, of the source and
        /// destination rectangles.
        /// </param>
        /// <param name="hDest">
        /// Specifies the height, in logical units, of the source and
        /// the destination rectangles.
        /// </param>
        /// <param name="hdcSource">
        /// Handle to the source device context.
        /// </param>
        /// <param name="xSrc">
        /// Specifies the x-coordinate, in logical units, of the upper-left
        /// corner of the source rectangle.
        /// </param>
        /// <param name="ySrc">
        /// Specifies the y-coordinate, in logical units, of the upper-left
        /// corner of the source rectangle.
        /// </param>
        /// <param name="RasterOp">
        /// Specifies a raster-operation code. These codes define how the
        /// color data for the source rectangle is to be combined with the
        /// color data for the destination rectangle to achieve the final color.
        /// </param>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        public static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest,
            int wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, int
            RasterOp);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SystemParametersInfo(uint uiAction, uint
            uiParam, ref LOGFONT lf, uint fWinIni);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SystemParametersInfo(uint uiAction, uint
            uiParam, out RECT rect, uint fWinIni);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SystemParametersInfo(uint uiAction, uint
            uiParam, out int nValue, uint fWinIni);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Win32API.POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Win32API.POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
            int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey,
            byte bAlpha, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetCapture(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern IntPtr GetCapture();

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, ref RECT lprcUpdate,
            IntPtr hrgnUpdate, uint flags);
        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate,
            IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll")]
        public static extern bool ValidateRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ValidateRect(IntPtr hWnd, IntPtr rect);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("COMCTL32.DLL")]
        public static extern bool _TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

        [DllImport("user32.DLL")]
        public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, [In] ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(HookType hook, HookProc callback,
            IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        public static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam,
            IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool GetUpdateRect(IntPtr hWnd, out RECT rect, bool bErase);

        /// <summary>
        /// Open the theme data for the specified HWND and semi-colon separated 
        /// list of class names. OpenThemeData() will try each class name, one at 
        /// a time, and use the first matching theme info found. If a match is found, 
        /// a theme handle to the data is returned. If no match is found, a "NULL" 
        /// handle is returned. When the window is destroyed or a WM_THEMECHANGED
        /// msg is received, "CloseThemeData()" should be called to close the theme handle.        
        /// </summary>
        /// <param name="hWnd">Window handle of the control/window to be themed.</param>
        /// <param name="classList">
        /// Class name (or list of names) to match to theme data
        /// section. if the list contains more than one name, the names are tested one at 
        /// a time for a match. If a match is found, OpenThemeData() returns a theme handle 
        /// associated with the matching class. This param is a list (instead of just a 
        /// single class name) to provide the class an opportunity to get the "best" match 
        /// between the class and the current theme.  For example, a button might pass 
        /// L"OkButton, Button" if its ID=ID_OK. If the current theme has an entry for 
        /// OkButton, that will be used. Otherwise, we fall back on the normal Button entry.
        /// </param>
        /// <returns>The opened theme handle or NULL in case of the error.</returns>
        [DllImport("uxtheme.dll", EntryPoint = "OpenThemeData",
             CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenThemeData(IntPtr hWnd, string classList);

        /// <summary>
        /// Closes the theme data handle. This should be done when the window being themed is 
        /// destroyed or whenever a WM_THEMECHANGED msg is received (followed by an attempt 
        /// to create a new Theme data handle).
        /// </summary>
        /// <param name="hTheme">Open theme data handle (returned from prior call
        /// to OpenThemeData()).
        /// </param>
        [DllImport("uxtheme.dll", EntryPoint = "CloseThemeData",
             CharSet = CharSet.Unicode, SetLastError = true)]
        public extern static void CloseThemeData(IntPtr hTheme);

        /// <summary>
        /// Checks if the app is themed.
        /// </summary>
        /// <returns>
        /// Returns TRUE if a theme is active and available to the current process.
        /// </returns>
        [DllImport("uxtheme.dll", EntryPoint = "IsAppThemed",
             CharSet = CharSet.Unicode, SetLastError = true)]
        public extern static bool IsAppThemedAPI();

        /// <summary>
        /// Draws the background image defined by the visual style for the 
        /// specified control part.
        /// </summary>
        /// <param name="hTheme">Handle to a window's specified theme data. 
        /// Use OpenThemeData to create an HTHEME.</param>
        /// <param name="hdc">Handle to a device context (HDC) used for 
        /// drawing the theme-defined background image.</param>
        /// <param name="iPartId">Value of type int that specifies the part 
        /// to draw.</param>
        /// <param name="iStateId">Value of type int that specifies the state 
        /// of the part to draw.</param>
        /// <param name="pRect">Pointer to a RECT structure that contains the 
        /// rectangle, in logical coordinates, in which the background image 
        /// is drawn. </param>
        /// <param name="pClipRect">Pointer to a RECT structure that contains 
        /// a clipping rectangle. This parameter may be set to NULL.</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise.</returns>
        [DllImport("uxtheme.dll", EntryPoint = "DrawThemeBackground",
             CharSet = CharSet.Unicode, SetLastError = true)]
        public extern static int DrawThemeBackground(IntPtr hTheme, IntPtr hdc,
            int iPartId, int iStateId, ref RECT pRect, IntPtr clipRect);

        /// <summary>
        /// Retrieves the value of a color property.
        /// </summary>
        /// <param name="hTheme">Handle to a window's specified theme data. 
        /// Use OpenThemeData to create an IntPtr.</param>
        /// <param name="iPartId">Value of type int that specifies the 
        /// part that contains the color property.</param>
        /// <param name="iStateId">Value of type int that specifies the 
        /// state of the part.</param>
        /// <param name="iPropId">Value of type int that specifies the 
        /// property to retrieve.</param>
        /// <param name="pColor">Retrived color value.</param>
        /// <returns>Returns S_OK if successful, or an error 
        /// value otherwise.</returns>
        [DllImport("uxtheme.dll", EntryPoint = "GetThemeColor",
             CharSet = CharSet.Unicode, SetLastError = true,
             ExactSpelling = true)]
        public extern static int GetThemeColor(IntPtr hTheme,
            int iPartId, int iStateId, int iPropId, out Int32 pColor);

        /// <summary>
        /// The function retrieves a handle to the specified window's 
        /// parent or owner. 
        /// </summary>
        /// <param name="hWnd">Handle to the window whose parent window 
        /// handle is to be retrieved. </param>
        /// <returns>If the window is a child window, the return value is 
        /// a handle to the parent window. If the window is a top-level 
        /// window, the return value is a handle to the owner window. 
        /// If the window is a top-level unowned window or if the function 
        /// fails, the return value is IntPtr.Zero.</returns>
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        /// <summary>
        /// The function retrieves a handle to the first control that has 
        /// the WS_TABSTOP style that precedes (or follows) the specified 
        /// control. 
        /// </summary>
        /// <param name="hDlg">Handle to the dialog box to be searched.</param>
        /// <param name="hCtl">Handle to the control to be used as the 
        /// starting point for the search. If this parameter is IntPtr.Zero, 
        /// the function uses the last (or first) control in the dialog box 
        /// as the starting point for the search.</param>
        /// <param name="bPrevious">Specifies how the function is to search 
        /// the dialog box. If this parameter is true, the function searches 
        /// for the previous control in the dialog box. If this parameter is 
        /// false, the function searches for the next control 
        /// in the dialog box.</param>
        /// <returns>If the function succeeds, the return value is the window 
        /// handle of the previous (or next) control that has the WS_TABSTOP 
        /// style set. If the function fails, the return value 
        /// is IntPtr.Zero.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetNextDlgTabItem(IntPtr hDlg, IntPtr hCtl,
            bool bPrevious);

        /// <summary>
        /// The function sets the keyboard focus to the specified window. 
        /// The window must be attached to the calling thread's message queue. 
        /// </summary>
        /// <param name="hWnd">Handle to the window that will receive the 
        /// keyboard input. If this parameter is IntPtr.Zero, keystrokes are 
        /// ignored.</param>
        /// <returns>If the function succeeds, the return value is the handle 
        /// to the window that previously had the keyboard focus. If the hWnd 
        /// parameter is invalid or the window is not attached to the calling 
        /// thread's message queue, the return value is IntPtr.Zero. </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        /// <summary>
        /// The function retrieves the handle to the window that has the 
        /// keyboard focus, if the window is attached to the calling 
        /// thread's message queue. 
        /// </summary>
        /// <returns>The return value is the handle to the window with 
        /// the keyboard focus. If the calling thread's message queue 
        /// does not have an associated window with the keyboard focus, 
        /// the return value is IntPtr.Zero.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        /// <summary>
        /// The function retrieves the status of the specified virtual key. 
        /// The status specifies whether the key is up, down, or toggled 
        /// (on, off—alternating each time the key is pressed). 
        /// </summary>
        /// <param name="virtKey">Specifies a virtual key. If the desired 
        /// virtual key is a letter or digit (A through Z, a through z, 
        /// or 0 through 9), nVirtKey must be set to the ASCII value of 
        /// that character. For other keys, 
        /// it must be a virtual-key code.</param>
        /// <returns>The return value specifies the status of the 
        /// specified virtual key. If the high-order bit is 1, the key 
        /// is down; otherwise, it is up. If the low-order bit is 1, the key 
        /// is toggled. A key, such as the CAPS LOCK key, is toggled if it 
        /// is turned on. The key is off and untoggled if the low-order bit 
        /// is 0. A toggle key's indicator light (if any) on the keyboard 
        /// will be on when the key is toggled, and off when the key is 
        /// untoggled.</returns>
        [DllImport("user32.dll")]
        public static extern short GetKeyState(int virtKey);

        #endregion // Functions

        #region Delegates

        public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

        #endregion // Delegates
    }
    #endregion // Win32API Class
}