namespace QRAndBarCodeTest.Models
{
    public class PrintCode
    {
        public string Title { get; set; }
        public string CodeArea { get; set; }
        public string Brand { get; set; }
        public string Divert { get; set; }
        public byte[] BarcodeBytes { get; set; }
        public byte[] QRCodeBytes { get; set; }
    }
}
