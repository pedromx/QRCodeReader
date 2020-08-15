using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCodeReader.Helper;

namespace QRCodeReader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QRCodeController : ControllerBase
    {
        private const string QR_READER_URL = "http://api.qrserver.com/v1/read-qr-code/";

        /// <summary>
        /// INFO
        /// </summary>
        /// <returns>Info about API</returns>
        [HttpGet]
        public string Start()
        {
            return $"INFO: The API to test is https://localhost:(portNumber)/QRCode { Environment.NewLine} it is a POST and just admit .jpg extensions in the form-data.";
        }

        /// <summary>
        /// Get QR code content
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Specified data from QR Code</returns>
        [HttpPost]
        public async Task<ActionResult<string>> GetQRCodeContent(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return Problem("File is empty");
            }

            if (!Path.GetExtension(file.FileName).Equals(".jpg", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity("Not Support file extension");
            }
            string serealizedJson = string.Empty;
            try
            {
                serealizedJson = await Upload(file.ConvertToByteArray(), file.FileName);
                return GetDesearelizedResult(serealizedJson);
            }
            catch (Exception e)
            {
                return UnprocessableEntity($"{serealizedJson} {Environment.NewLine} {e.Message} ");
            }
        }

        private string GetDesearelizedResult(string serealizedJson)
        {
            var deserealizedQRData = JsonSerializer.Deserialize<QRCodeData[]>(serealizedJson);
            return deserealizedQRData[0].symbol[0].data;
        }

        private static async Task<string> Upload(byte[] qrCodeImage, string fileName)
        {
            var content = new ByteArrayContent(qrCodeImage);
            using (var client = new HttpClient())
            {
                var multPartFormData = new MultipartFormDataContent {
                          { content, "file", fileName }
                    };

                var postResponse = await client.PostAsync(QR_READER_URL, multPartFormData);

                var result = await postResponse.Content.ReadAsStringAsync();
                return result;
            }
        }
    }
}
