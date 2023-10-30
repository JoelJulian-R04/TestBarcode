using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRAndBarCodeTest.ApplicationServices;
using QRAndBarCodeTest.Models;

namespace QRAndBarCodeTest.Controllers.QRBARCodes
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRBarCodeController : ControllerBase
    {
        private readonly IQRBarCodeAppService _codeDataService;
        public QRBarCodeController(IQRBarCodeAppService codeAppService)
        {
            _codeDataService = codeAppService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerarEtiqueta([FromBody] CodeData codeData)
        {
            var etiqueta = await _codeDataService.CreatePrintCode(codeData);
            /*var result = new
            {
                QRCodeBytes = Convert.ToBase64String(etiqueta.QRCodeBytes),
                BarcodeBytes = Convert.ToBase64String(etiqueta.BarcodeBytes)
            };
            return Ok(result);*/
            var contentType = "image/png";
            return File(etiqueta.QRCodeBytes, contentType);
        }
    }
}
