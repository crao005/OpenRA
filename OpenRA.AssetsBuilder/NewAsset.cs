using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenRA.Utility;
using System.IO;
using OpenRA.FileFormats;
using OpenRA.FileFormats.Graphics;
using System.Runtime.InteropServices;

namespace OpenRA.AssetsBuilder
{
    public partial class NewAsset : Form
    {
        public NewAsset()
        {
            InitializeComponent();
        }
        private int imageWidth;
        private void button2_Click(object sender, EventArgs e)
        {

        }
        
        private void btnChoosePic_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog1 = new OpenFileDialog();
            fileDialog1.InitialDirectory = "d://";
            fileDialog1.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
            fileDialog1.FilterIndex = 1;
            fileDialog1.RestoreDirectory = true;
            if (fileDialog1.ShowDialog() == DialogResult.OK)
            {
                String fileName = fileDialog1.FileName;                                
                this.Focus();

                Image pic = Image.FromFile(fileName);
                imageWidth = pic.Width/16;

                //Resize(fileName,fileName);
                tbPicSource.Text = fileName;
                pictureBox1.Image = pic;
            }
            else
            {

            }  
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            YamlHelper ym = new YamlHelper();
            
            //String[] args = new String[] { "--shp", srcs, imageWidth.ToString() };
            //Command.ConvertPngToShp(args);

            var src = tbPicSource.Text;
            var name = Path.GetFileNameWithoutExtension(src);
            var src_shp = Path.ChangeExtension(src, ".shp");
            var filename = Path.GetFileName(src_shp);
            var dest = "mods/ra/bits/" + filename;
            var width = imageWidth;

            var srcImage = PngLoader.Load(src);

            if (srcImage.Width % width != 0)
                throw new InvalidOperationException("Bogus width; not a whole number of frames");

            using (var destStream = File.Create(dest))
                ShpWriter.Write(destStream, width, srcImage.Height,
                    ToFrames(srcImage,width));

            Console.WriteLine(dest + " saved");

            
            ym.addSequencesYaml(name);
            ym.addShips(name);

            MessageBox.Show("Success!!");
        }

        public IEnumerable<byte[]> ToFrames(Bitmap bitmap, int width)
        {
            for (var x = 0; x < bitmap.Width; x += width)
            {
                var data = bitmap.LockBits(new Rectangle(x, 0, width, bitmap.Height), ImageLockMode.ReadOnly,
                    PixelFormat.Format8bppIndexed);

                var bytes = new byte[width * bitmap.Height];
                for (var i = 0; i < bitmap.Height; i++)
                    Marshal.Copy(new IntPtr(data.Scan0.ToInt64() + i * data.Stride),
                        bytes, i * width, width);

                bitmap.UnlockBits(data);

                yield return bytes;
            }
        }
    }
}
