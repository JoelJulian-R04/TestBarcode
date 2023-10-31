
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRAndBarCodeTest.ApplicationServices;
using QRAndBarCodeTest.Models;
using System.Net;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;

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
            var resultPrint = await _codeDataService.SendPrintCode(etiqueta);
            if (resultPrint != false)
            {
                return Ok("Successful");
            }
            return BadRequest("Error in the process!.");
        }




    }
}
