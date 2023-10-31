using QRAndBarCodeTest.Models;
using ZXing.Common;
using ZXing;
using QRCoder;
using ZXing.QrCode.Internal;
using System.Drawing;
using BarcodeLib;
using System.Drawing.Printing;

namespace QRAndBarCodeTest.ApplicationServices
{
    public class QRBarCodeAppService : IQRBarCodeAppService
    {
        public async Task<PrintCode> CreatePrintCode(CodeData codeData)
        {
            Barcode barcode = new Barcode();
            barcode.IncludeLabel = true;
            barcode.Alignment = AlignmentPositions.CENTER;
            barcode.LabelPosition = LabelPositions.BOTTOMCENTER;

            Image barcodeImage = barcode.Encode(TYPE.CODE128, codeData.Title, Color.Black, Color.White, 400, 170);

            using var ms2 = new MemoryStream();
            barcodeImage.Save(ms2, System.Drawing.Imaging.ImageFormat.Png);
            byte[] imageBytes = ms2.ToArray();

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(codeData.Title, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            var scale = 5;
            var qrImage = qrCode.GetGraphic(scale);

            using var ms = new MemoryStream();
            ms.Write(qrImage);
            byte[] byteImage = ms.ToArray();


            var printCode = new PrintCode
            {
                Title = codeData.Title,
                CodeArea = codeData.CodeArea,
                Brand = codeData.Brand,
                Divert = codeData.Divert,
                BarcodeBytes = imageBytes,
                QRCodeBytes = byteImage
            };

            return printCode;
        }

        public async Task<bool> SendPrintCode(PrintCode printCode)
        {
            if (printCode != null)
            {
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += (sender, e) =>
                {
                    // Dibuja el código QR
                    using (MemoryStream stream = new MemoryStream(printCode.QRCodeBytes))
                    using (Image img = Image.FromStream(stream, false, true))
                    {
                        e.Graphics.DrawImage(img, 150, 50);
                    }
                    // Dibuja la información en el documento a imprimir
                    int infoX = 350; // Ajusta esta coordenada para cambiar la posición de los datos
                    e.Graphics.DrawString($"{printCode.Title}", new Font("Arial", 23), Brushes.Black, infoX, 70);
                    e.Graphics.DrawString($"{printCode.CodeArea}", new Font("Arial", 23), Brushes.Black, infoX, 130);
                    e.Graphics.DrawString($"{printCode.Brand}", new Font("Arial", 23), Brushes.Black, infoX, 190);
                    e.Graphics.DrawString($"{printCode.Divert}", new Font("Arial", 23), Brushes.Black, infoX, 250);

                    // Dibuja el código de barras
                    using (MemoryStream stream = new MemoryStream(printCode.BarcodeBytes))
                    using (Image img = Image.FromStream(stream, false, true))
                    {
                        e.Graphics.DrawImage(img, 150, 310);
                    }


                };
                pd.Print();
                return true;
            }
            return false;
        }
    }
}
