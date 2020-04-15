using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Watermark
{
    public class WaterMark
    {
        //https://stackoverflow.com/questions/12725643/watermarking-on-image-in-c

        public string SourceImage { get; set; }
        public string Text { get; set; }
        public string ShortText { get; set; }

        public string TargetRootPath { get; set; }

        public WaterMark(string sourceImage, string text, string outPath) 
        {
            this.SourceImage = sourceImage;
            this.Text = text;
            this.TargetRootPath = outPath;

            if (!TargetRootPath.EndsWith("\\"))
            {
                TargetRootPath += "\\";
            }
        }

        public string AddTextWalterMark()
        {
            FileInfo sourceFileInfo = new FileInfo(SourceImage);

            if (!sourceFileInfo.Exists)
            {
                throw new FileNotFoundException("Souce image cannot found", SourceImage);
            }

            var fileName = sourceFileInfo.Name;
            string outFileName = fileName.Replace(sourceFileInfo.Extension, $".waltermark{sourceFileInfo.Extension}");
            string targetImageFileName = TargetRootPath + outFileName;

            ImageFormat imgFormat = GetImageFormat(sourceFileInfo.Extension);

            try
            {
                // Read source image: open source image as stream 
                using (FileStream sourceStream = new FileStream(SourceImage, FileMode.Open))
                {
                    Image imgSource = Image.FromStream(sourceStream);

                    var waterMarkText = Text;
                    if (imgSource.Width < 2048)
                    {
                        waterMarkText = string.IsNullOrEmpty(ShortText) ? "CACA" : ShortText;
                    }

                    int fontEmSize = (int)(imgSource.Width * 0.03);
                    if (fontEmSize < 30)
                    {
                        fontEmSize = 30;
                    }

                    Font font = new Font("Arial", fontEmSize, FontStyle.Bold, GraphicsUnit.Pixel); // choose font for text
                    Color color = Color.FromArgb(100, 255, 255, 255); // color and transparency
                    SolidBrush brush = new SolidBrush(color);

                    using (Graphics graphics = Graphics.FromImage(imgSource))
                    {
                        SizeF imgSize = graphics.MeasureString(waterMarkText, font);
                        Point point = new Point(imgSource.Width - ((int)imgSize.Width + 10), imgSource.Height - ((int)imgSize.Height + 10));

                        // Draw text on image
                        graphics.DrawString(waterMarkText, font, brush, point);

                        // Update image memory stream
                        using (Stream outputStream = new MemoryStream())
                        {
                            imgSource.Save(outputStream, imgFormat);  // Image ( with text ) save to new output stream
                            Image imgFinal = Image.FromStream(outputStream); // Create new Image instance for target image
                            imgFinal.Save(targetImageFileName, imgFormat);

                            // THIS is anoter way to: write modified image to file , but do NOT work as expected.
                            //Bitmap bmpNewImage = new Bitmap(imgSource.Width, imgSource.Height, imgSource.PixelFormat);
                            //Graphics graphics2 = Graphics.FromImage(bmpNewImage);
                            //graphics2.DrawImage(imgFinal, new Point(0, 0)); //DrawImageUnscaled no diffrance
                            //bmpNewImage.Save(targetImageFileName, imgFormat);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }

            return targetImageFileName;
        }

        public string AddTextWalterMarkResize()
        {
            FileInfo sourceFileInfo = new FileInfo(SourceImage);

            if (!sourceFileInfo.Exists)
            {
                throw new FileNotFoundException("Souce image cannot found", SourceImage);
            }

            var fileName = sourceFileInfo.Name;
            string wmFileName = fileName.Replace(sourceFileInfo.Extension, $".waltermark{sourceFileInfo.Extension}");
            string waterMarkedImageFileName = TargetRootPath + wmFileName;

            ImageFormat imgFormat = GetImageFormat(sourceFileInfo.Extension);

            try
            {
                Bitmap yourImage = new Bitmap(SourceImage);
                Bitmap yourWatermark = new Bitmap(waterMarkedImageFileName);

                int newWaterWidth = (int)((float)yourImage.Width * .3);
                int newWaterHeight = (int)((float)yourImage.Height * .3);

                using (Bitmap resizedWaterm = new Bitmap(newWaterWidth, newWaterHeight)) 
                {
                    using (Graphics g = Graphics.FromImage((Image)resizedWaterm))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(yourWatermark, 0, 0, newWaterWidth, newWaterHeight);
                    }

                    //int x = ..., y = ...;
                    //SizeF imgSize = graphics.MeasureString(Text, font);
                    //Point point = new Point(imgSource.Width - ((int)imgSize.Width + 10), imgSource.Height - ((int)imgSize.Height + 10));
                    //using (Graphics g2 = Graphics.FromImage((Image)resizedWaterm))
                    //{
                    //    SolidBrush brush = new SolidBrush(Color.FromArgb(100, 255, 255, 255)); // color and transparency
                    //    g2.FillRectangle(brush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width + 1, watermarkImage.Height)));
                    //}
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }

            return waterMarkedImageFileName;
        }

        private ImageFormat GetImageFormat(string extension)
        {
            if (extension.StartsWith("."))
            {
                extension = extension.Substring(1);
            }

            if (extension.Equals("jpg", StringComparison.InvariantCultureIgnoreCase))
            {
                extension = "jpeg";
            }

            ImageFormat result = null;
            PropertyInfo prop = typeof(ImageFormat).GetProperties().Where(p => p.Name.Equals(extension, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (prop != null)
            {
                result = prop.GetValue(prop) as ImageFormat;
            }

            return result;
        }
    }
}
