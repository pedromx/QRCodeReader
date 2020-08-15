namespace QRCodeReader
{
    public sealed class Symbol
    {
        public int seq { get; set; }
        public string data { get; set; }
        public string error { get; set; }        
    }

    public sealed class QRCodeData
    {       
        public string type { get; set; }
        public  Symbol[] symbol { get; set; }   
    }
}
