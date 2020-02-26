using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RGMail.Utils
{
    public static class QRCodeHelp
    {
        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap Create(string text)
        {
            QRCoder.QRCodeGenerator generator = new QRCoder.QRCodeGenerator();
            QRCoder.QRCodeData qrdata = generator.CreateQrCode(text, QRCoder.QRCodeGenerator.ECCLevel.Q);

            QRCoder.QRCode code = new QRCoder.QRCode(qrdata);
            System.Drawing.Bitmap bmp = code.GetGraphic(15);
            return bmp;
        }
        public static BitmapSource Bitmap2BitmapImage(this System.Drawing.Bitmap bmp)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }
        public static void SaveImageToFile(this BitmapSource @this, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(@this));
            using (Stream stream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(stream);
            }
        }
    }
}
