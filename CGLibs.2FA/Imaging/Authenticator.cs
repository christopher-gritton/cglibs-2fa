using System;
using System.Drawing;
using System.IO;
using ZXing.QrCode;

namespace CGLibs._2FA.Imaging
{
    public class Authenticator
    {
        public Authenticator()
        {
        }

        public AuthenticatorType Type { get; set; } = AuthenticatorType.TOTP;
        public AuthenticatorDigits Digits = AuthenticatorDigits.SIXDIGIT;
        public AuthenticatorAlgorithm Algorithm { get; set; } = AuthenticatorAlgorithm.SHA1;
        public int Period { get; set; } = 30;
        public int Counter { get; set; } = 0;

        public int Height { get; set; } = 300;
        public int Width { get; set; } = 300;

        public Image GetQRCodeImage(string secret, string issuer, string account)
        {
            //WebUtility.UrlEncode("")
            string encsecret = Encoding.Base32.ToBase32String(secret).TrimEnd('=');
            issuer = Uri.EscapeDataString(issuer);
            account = Uri.EscapeDataString(account);

            string uri = $"otpauth://{(string)Type}/{issuer}{(string.IsNullOrWhiteSpace(issuer) ? "" : ":")}{account}?secret={encsecret}&issuer={issuer}&algorithm={(string)Algorithm}&digits={(int)Digits}{(Type.ToString() == "totp" ? "&period=" + Period : "")}{(Type.ToString() == "hotp" ? "&counter=" + Counter : "")}";

            var qrWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions { Height = Height, Width = Width, Margin = 2 }
            };


            var pixelData = qrWriter.Write(uri);

            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB
            // the System.Drawing.Bitmap class is provided by the CoreCompat.System.Drawing package
            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            using (var ms = new MemoryStream())
            {
                // lock the data area for fast access
                var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                   System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                       pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                // save to stream as PNG
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                return (Image)new Bitmap(ms);
            }


        }

        public Stream GetQRCodeStream(string secret, string issuer, string account)
        {
            string encsecret = Encoding.Base32.ToBase32String(secret).TrimEnd('=');
            issuer = Uri.EscapeDataString(issuer);
            account = Uri.EscapeDataString(account);

            string uri = $"otpauth://{(string)Type}/{issuer}{(string.IsNullOrWhiteSpace(issuer) ? "" : ":")}{account}?secret={encsecret}&issuer={issuer}&algorithm={(string)Algorithm}&digits={(int)Digits}{(Type.ToString() == "totp" ? "&period=" + Period : "")}{(Type.ToString() == "hotp" ? "&counter=" + Counter : "")}";

            var qrWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions { Height = Height, Width = Width, Margin = 2 }
            };


            var pixelData = qrWriter.Write(uri);

            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB
            // the System.Drawing.Bitmap class is provided by the CoreCompat.System.Drawing package
            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                var ms = new MemoryStream();

                // lock the data area for fast access
                var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                   System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                       pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                // save to stream as PNG
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                return ms;
            }
        }
    }
}
