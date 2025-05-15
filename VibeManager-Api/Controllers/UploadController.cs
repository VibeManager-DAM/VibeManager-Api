using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace VibeManager_Api.Controllers
{
    public class UploadController : ApiController
    {
        [HttpPost]
        [Route("api/upload")]
        public async Task<IHttpActionResult> UploadMedia()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("Unsupported media type");
            }

            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                var allowedMimeTypes = new Dictionary<string, string>
                {
                    { "image/jpeg", "Imgs" },
                    { "image/png", "Imgs" },
                    { "image/gif", "Imgs" },
                    { "image/jpg", "Imgs" },
                    { "video/mp4", "Videos" },
                    { "video/quicktime", "Videos" },
                    { "audio/mpeg", "Audios" },
                    { "audio/wav", "Audios" },
                    { "audio/mp3", "Audios" },
                    { "audio/3gp", "Audios" },

                };

                foreach (var file in provider.Contents)
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Trim('"');
                    var mimeType = file.Headers.ContentType.MediaType;

                    // Validar tipo
                    if (!allowedMimeTypes.ContainsKey(mimeType))
                    {
                        return BadRequest("Invalid file type. Only images, videos, and audios are allowed.");
                    }

                    string folder = allowedMimeTypes[mimeType];
                    string directoryPath = HttpContext.Current.Server.MapPath($"~/{folder}");

                    // Asegura que la carpeta existe
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var buffer = await file.ReadAsByteArrayAsync();
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                    string filePath = Path.Combine(directoryPath, uniqueFileName);
                    File.WriteAllBytes(filePath, buffer);

                    string fileUrl = $"http://10.0.3.148/api/{folder}/{uniqueFileName}";

                    return Ok(new
                    {
                        message = $"{folder} uploaded successfully",
                        fileName = uniqueFileName,
                        url = fileUrl,
                        type = mimeType
                    });
                }

                return BadRequest("No file uploaded.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
