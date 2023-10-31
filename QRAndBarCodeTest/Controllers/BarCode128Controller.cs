
using BarcodeLib;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using static QRAndBarCodeTest.Controllers.QRBARCodes.QRBarCodeController;

namespace QRAndBarCodeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarCode128Controller : ControllerBase
    {
        [HttpPost]
        public IActionResult GenerarEtiqueta([FromBody] BarcodeRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Text))
                {
                    return BadRequest("The field 'text' is empty");
                }

                Barcode barcode = new Barcode();
                barcode.IncludeLabel = true;
                barcode.Alignment = AlignmentPositions.CENTER;
                barcode.LabelPosition = LabelPositions.BOTTOMCENTER;


                Image barcodeImage = barcode.Encode(TYPE.CODE128, request.Text, Color.Black, Color.White, 290, 120);

                using (MemoryStream ms = new MemoryStream())
                {
                    barcodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    return File(imageBytes, "image/png");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al generar el código de barras: " + ex.Message });
            }
        }
        public class BarcodeRequest
        {
            public string Text { get; set; }
        }
    }
}
