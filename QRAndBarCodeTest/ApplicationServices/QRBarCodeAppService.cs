using QRAndBarCodeTest.Models;
using ZXing.Common;
using ZXing;
using QRCoder;
using ZXing.QrCode.Internal;
using System.Drawing;
namespace QRAndBarCodeTest.ApplicationServices
{
    public class QRBarCodeAppService : IQRBarCodeAppService
    {
        public async Task<PrintCode> CreatePrintCode(CodeData codeData)
        {
            // Ejemplo de generación de código de barras
            var barcodeWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 100,
                    Width = 300,
                    Margin = 10
                }
            };

            var barcodeData = barcodeWriter.Write(codeData.Title);

            // Ejemplo de generación de código QR
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(codeData.Title, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            var qrImage = qrCode.GetGraphic(10);

            using var ms = new MemoryStream();
            ms.Write(qrImage);
            byte[] byteImage = ms.ToArray();


            var printCode = new PrintCode
            {
                Title = codeData.Title,
                CodeArea = codeData.CodeArea,
                Brand = codeData.Brand,
                Divert = codeData.Divert,
                BarcodeBytes = barcodeData.Pixels,
                QRCodeBytes = byteImage
            };

            return printCode;
        }
    }
}
