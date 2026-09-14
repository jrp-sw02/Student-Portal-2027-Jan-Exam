using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
namespace EConnect
{
    public class CaptchaImage 
    {
        // Internal properties.
        String m_text;
        Int32 m_width;
        Int32 m_height;
        String familyName;
        Bitmap m_image;
        Random random = new Random();
        public String Text
        {
            get
            {
                return this.m_text;
            }
        }
        public Bitmap Image
        {
            get
            {
                return this.m_image;
            }
        }
        public Int32  Width
        {
            get
            {
                return this.m_width;
            }
        }
        public Int32 Height
        {
            get
            {
                return this.m_height;
            }
        }
        public Byte[] ImageBytes 
        {
            get
            {
                MemoryStream ms = new MemoryStream();
                this.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }
        public String ImageSource
        {
            get
            {
                return "data:image/jpg;base64," + Convert.ToBase64String(this.ImageBytes);
            }
        }
        //====================================================================
        //Initializes a new instance of the CaptchaImage class using the
        //specified text, width and height.
        //====================================================================
        public CaptchaImage(String text, Int32 width, Int32 height)
        {
            this.m_text = text;
            this.SetDimensions(width, height);
            this.GenerateImage();
        }
        //====================================================================
        //Initializes a new instance of the CaptchaImage class using the
        //specified text, width and height and font family.
        //====================================================================
        public CaptchaImage(String text, Int32 width, Int32 height, String familyName)
        {
            this.m_text = text;
            this.familyName = familyName;
            this.SetDimensions(width, height);
            this.GenerateImage();
        }
        //====================================================================
        //Releases all resources used by this object.
        //====================================================================
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true) ;
        }
        //====================================================================
        //Custom Dispose method to clean up unmanaged resources.
        //====================================================================
        public void Dispose(Boolean disposing)
        {
            if (disposing)
                this.m_image.Dispose();
        }

        // ====================================================================
        //Sets the image width and height.
        //====================================================================
        private void SetDimensions(Int32 width, Int32 height)
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException("width", width, "Argument out of range, must be greater than zero");
            if (height < 0)
                throw new ArgumentOutOfRangeException("height", height, "Argument out of range, must be greater than zero");
            this.m_height = height;
            this.m_width = width;
        }

        // ====================================================================
        //Sets the font used for the image text.
        //====================================================================
        private void SetFamilyName(String familyName)
        {
             //If the named font is not installed, default to a system font.
            try
            {
                Font  font = new Font(this.familyName, 12.0F);
                this.familyName = familyName;
            }
            catch(Exception)
            {
                this.familyName = System.Drawing.FontFamily.GenericSerif.Name;
            }
        }

        // ====================================================================
        //Creates the bitmap image.
        //====================================================================
        private void GenerateImage()
        {
            //Create a graphics object for drawing.

            Bitmap bitmap = new Bitmap(this.m_width, this.m_height, PixelFormat.Format32bppArgb);
            //' Create a graphics object for drawing.
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, this.m_width, this.m_height);
            //' Fill in the background.
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.DarkVertical, Color.Black, Color.Black);
            g.FillRectangle(hatchBrush, rect);

            //' Set up the text font.
            SizeF size;
            Single fontSize = rect.Height + 1;
            Font font;
            //' Adjust the font size until the text fits within the image.
            do
            {
                fontSize -= 1;
                font = new Font(this.familyName, fontSize, FontStyle.Bold);
                size = g.MeasureString(this.m_text, font);
            }
            while (size.Width > rect.Width);

            //' Set up the text format.
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            //' Create a path using the text and warp it randomly.
            GraphicsPath path = new GraphicsPath();
            path.AddString(this.m_text, font.FontFamily, Convert.ToInt32(font.Style), font.Size, rect, format);
            Single v = 4.0F;
            PointF[] points = { new PointF(this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v), new PointF(rect.Width - this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v), new PointF(this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v), new PointF(rect.Width - this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v) };
            Matrix matrix = new Matrix();
            matrix.Translate(0.0F, 0.0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0.0F);

            //' Draw the text.
            hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
            g.FillPath(hatchBrush, path);
            Int32 m = Math.Max(rect.Width, rect.Height);

            for (Int32 i = 0; i < Convert.ToInt32((rect.Width * rect.Height / 30.0F)) - 1; i++)
            {
                Int32 x = this.random.Next(rect.Width);
                Int32 y = this.random.Next(rect.Height);
                Int32 w = this.random.Next(m / 50);
                Int32 h = this.random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }
            //' Clean up.
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();
            this.m_image = bitmap;
        }
    } 
}
