using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Sample2CognexBarcodeReader.Utils
{
    /// <summary>
    /// Görüntü işleme yardımcı sınıfı
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// Görüntüyü kontrol boyutuna uygun şekilde yeniden boyutlandırır
        /// </summary>
        /// <param name="originalSize">Orijinal görüntü boyutu</param>
        /// <param name="controlSize">Kontrol boyutu</param>
        /// <returns>Yeniden boyutlandırılmış boyut</returns>
        public static Size FitImageInControl(Size originalSize, Size controlSize)
        {
            if (originalSize.Width <= 0 || originalSize.Height <= 0)
                return controlSize;

            double aspectRatio = (double)originalSize.Width / originalSize.Height;
            double controlAspectRatio = (double)controlSize.Width / controlSize.Height;

            Size newSize;

            if (aspectRatio > controlAspectRatio)
            {
                // Görüntü daha geniş, genişliği kontrol genişliğine ayarla
                newSize = new Size(controlSize.Width, (int)(controlSize.Width / aspectRatio));
            }
            else
            {
                // Görüntü daha yüksek, yüksekliği kontrol yüksekliğine ayarla
                newSize = new Size((int)(controlSize.Height * aspectRatio), controlSize.Height);
            }

            return newSize;
        }

        /// <summary>
        /// Görüntüyü belirtilen boyuta yeniden boyutlandırır
        /// </summary>
        /// <param name="originalImage">Orijinal görüntü</param>
        /// <param name="newSize">Yeni boyut</param>
        /// <returns>Yeniden boyutlandırılmış görüntü</returns>
        public static Bitmap ResizeImageToBitmap(Image originalImage, Size newSize)
        {
            if (originalImage == null)
                return null;

            try
            {
                var resizedImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format24bppRgb);
                
                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    
                    graphics.DrawImage(originalImage, 0, 0, newSize.Width, newSize.Height);
                }

                return resizedImage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Görüntü yeniden boyutlandırılırken hata oluştu: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Graphics overlay'i görüntüye uygular
        /// </summary>
        /// <param name="image">Hedef görüntü</param>
        /// <param name="graphicsData">Graphics verisi</param>
        /// <param name="imageSize">Görüntü boyutu</param>
        /// <returns>Overlay uygulanmış görüntü</returns>
        public static Bitmap ApplyGraphicsOverlay(Bitmap image, string[] graphicsData, Size imageSize)
        {
            if (image == null || graphicsData == null || graphicsData.Length == 0)
                return image;

            try
            {
                using (var graphics = Graphics.FromImage(image))
                {
                    foreach (var graphicsString in graphicsData)
                    {
                        var resultGraphics = GraphicsResultParser.Parse(graphicsString, 
                            new Rectangle(0, 0, imageSize.Width, imageSize.Height));
                        ResultGraphicsRenderer.PaintResults(graphics, resultGraphics);
                    }
                }

                return image;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Graphics overlay uygulanırken hata oluştu: {ex.Message}");
                return image;
            }
        }

        /// <summary>
        /// Görüntüyü güvenli şekilde dispose eder
        /// </summary>
        /// <param name="image">Dispose edilecek görüntü</param>
        public static void SafeDisposeImage(Image image)
        {
            try
            {
                image?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Görüntü dispose edilirken hata oluştu: {ex.Message}");
            }
        }
    }
}
