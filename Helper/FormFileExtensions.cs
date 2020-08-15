using Microsoft.AspNetCore.Http;
using System.IO;

namespace QRCodeReader.Helper
{
    public static class FormFileExtensions
    {
        public static byte[] ConvertToByteArray(this IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                return fileBytes;
            }
        }
    }
}
