using QRAndBarCodeTest.Models;

namespace QRAndBarCodeTest.ApplicationServices
{
    public interface IQRBarCodeAppService
    {
        Task<PrintCode> CreatePrintCode(CodeData codeData);
        Task<bool> SendPrintCode(PrintCode printData);
    }
}
