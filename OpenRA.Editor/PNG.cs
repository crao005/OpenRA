using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using OpenRA.FileFormats.Graphics;

using System.Runtime.InteropServices;
using OpenRA.Utility;

namespace OpenRA.Editor
{
    public partial class PNGform : Form
    {
        public PNGform()
        {
            InitializeComponent();
        }

        public void Resize(string imageFile, string outputFile)
        {
            using (var srcImage = Image.FromFile(imageFile))
            {
                var newWidth = (int)(srcImage.Height * 16);
                var newHeight = (int)(srcImage.Height );
                using (var newImage = new Bitmap(newWidth, newHeight))
                using (var graphics = System.Drawing.Graphics.FromImage(newImage))
                {
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

                   
                    graphics.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));


                  
                    newImage.Save("test.png", GetPngCodecInfo(), GetEncoderParameters());
                    //System.Drawing.Bitmap b0 = CopyToBpp(newImage, 8);
                    //b0.Save("test2.png");
                    MemoryStream ms = new MemoryStream();
                    newImage.Save(ms, ImageFormat.Bmp);
                    var img2 = Image.FromStream(ms);
                    img2.Save("test2.png");

                }
            }
        }
        public EncoderParameters GetEncoderParameters()
        {
            var parameters = new EncoderParameters();
            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 8);

            return parameters;
        }
        public ImageCodecInfo GetPngCodecInfo()
        {
            var encoders = ImageCodecInfo.GetImageEncoders();

            ImageCodecInfo codecInfo = null;

            foreach (var imageCodecInfo in encoders)
            {
                if (imageCodecInfo.FormatID != ImageFormat.Png.Guid)
                    continue;

                codecInfo = imageCodecInfo;
                break;
            }

            return codecInfo;
        }
        static System.Drawing.Bitmap CopyToBpp(System.Drawing.Bitmap b, int bpp)

        {

            if (bpp != 1 && bpp != 8) throw new System.ArgumentException("1 or 8", "bpp");

 

            int w = b.Width, h = b.Height;

            IntPtr hbm = b.GetHbitmap();

 

            BITMAPINFO bmi = new BITMAPINFO();

            bmi.biSize = 40;   

            bmi.biWidth = w;

            bmi.biHeight = h;

            bmi.biPlanes = 1;  

            bmi.biBitCount = (short)bpp;  

            bmi.biCompression = BI_RGB;  

            bmi.biSizeImage = (uint)(((w + 7) & 0xFFFFFFF8) * h / 8);

            bmi.biXPelsPerMeter = 1000000;  

            bmi.biYPelsPerMeter = 1000000;  

            // Now for the colour table.

            uint ncols = (uint)1 << bpp;  

            bmi.biClrUsed = ncols;

            bmi.biClrImportant = ncols;

            bmi.cols = new uint[256];

            if (bpp == 1) { bmi.cols[0] = MAKERGB(0, 0, 0); bmi.cols[1] = MAKERGB(255, 255, 255); }

            else { for (int i = 0; i < ncols; i++) bmi.cols[i] = MAKERGB(i, i, i); }

            IntPtr bits0;            IntPtr hbm0 = CreateDIBSection(IntPtr.Zero, ref bmi, DIB_RGB_COLORS, out bits0, IntPtr.Zero, 0);

            IntPtr sdc = GetDC(IntPtr.Zero);      

            IntPtr hdc = CreateCompatibleDC(sdc); SelectObject(hdc, hbm);

            IntPtr hdc0 = CreateCompatibleDC(sdc); SelectObject(hdc0, hbm0);

            BitBlt(hdc0, 0, 0, w, h, hdc, 0, 0, SRCCOPY);

            System.Drawing.Bitmap b0 = System.Drawing.Bitmap.FromHbitmap(hbm0);

 

            DeleteDC(hdc);

            DeleteDC(hdc0);

            ReleaseDC(IntPtr.Zero, sdc);

            DeleteObject(hbm);

            DeleteObject(hbm0);

            //

            return b0;

        }

 

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern int DeleteDC(IntPtr hdc);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern int BitBlt(IntPtr hdcDst, int xDst, int yDst, int w, int h, IntPtr hdcSrc, int xSrc, int ySrc, int rop);

        static int SRCCOPY = 0x00CC0020;

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO bmi, uint Usage, out IntPtr bits, IntPtr hSection, uint dwOffset);
        static uint BI_RGB = 0;
        static uint DIB_RGB_COLORS = 0;
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]

        public struct BITMAPINFO

        {

            public uint biSize;

            public int biWidth, biHeight;

            public short biPlanes, biBitCount;

            public uint biCompression, biSizeImage;

            public int biXPelsPerMeter, biYPelsPerMeter;

            public uint biClrUsed, biClrImportant;

            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 256)]

            public uint[] cols;

        }

        static uint MAKERGB(int r, int g, int b)

        {

            return ((uint)(b & 255)) | ((uint)((r & 255) << 8)) | ((uint)((g & 255) << 16));

        }


        private int imageWidth;

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog1 = new OpenFileDialog();
            fileDialog1.InitialDirectory = "d://";
            fileDialog1.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
            fileDialog1.FilterIndex = 1;
            fileDialog1.RestoreDirectory = true;
            if (fileDialog1.ShowDialog() == DialogResult.OK)
            {
                String fileName =fileDialog1.FileName;
                textBox1.Text = fileName;
                this.Focus();

                Image pic = Image.FromFile(fileName);
                imageWidth = pic.Width;
                //Resize(fileName,fileName);
                
            pictureBox2.Image = pic;
            }
            else
            {
               
            }  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select Posation";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                textBox2.Text = foldPath;               
            }
        }

        private void tanslateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var src = textBox1.Text;
            String[] arg = new String[] { "--shp", src, imageWidth.ToString() };
            Command.ConvertPngToShp(arg);
            MessageBox.Show("Success!!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var src = textBox1.Text;
            String[] arg = new String[] { "--shp", src, (imageWidth/16).ToString() };
            Command.ConvertPngToShp(arg);
            MessageBox.Show("Success!!");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

       
   
    }
}
