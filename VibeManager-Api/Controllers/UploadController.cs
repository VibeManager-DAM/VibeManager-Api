using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using VibeManager_Api.Models;

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
                    { "audio/3gpp", "Audios" }
                };

                foreach (var file in provider.Contents)
                {
                    var fileName = file.Headers.ContentDisposition.FileName?.Trim('"') ?? "file";
                    var extension = Path.GetExtension(fileName).ToLower();
                    var mimeType = GetMimeTypeFromExtension(extension);

                    // Log para depuración
                    System.Diagnostics.Debug.WriteLine($"Nombre: {fileName}, Extensión: {extension}, MIME detectado: {mimeType}");

                    if (!allowedMimeTypes.ContainsKey(mimeType))
                    {
                        return BadRequest("Invalid file type. Only images, videos, and audios are allowed.");
                    }

                    string folder = allowedMimeTypes[mimeType];
                    string directoryPath = HttpContext.Current.Server.MapPath($"~/{folder}");

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var buffer = await file.ReadAsByteArrayAsync();
                    string uniqueFileName = Guid.NewGuid().ToString() + extension;
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


        private string GetMimeTypeFromExtension(string extension)
        {
            var mimeTypes = new Dictionary<string, string>
            {
                { ".jpeg", "image/jpeg" },
                { ".jpg", "image/jpg" },
                { ".png", "image/png" },
                { ".gif", "image/gif" },
                { ".mp4", "video/mp4" },
                { ".mov", "video/quicktime" },
                { ".mp3", "audio/mpeg" },
                { ".wav", "audio/wav" },
                { ".3gp", "audio/3gpp" } 
            };

            return mimeTypes.ContainsKey(extension) ? mimeTypes[extension] : "application/octet-stream";
        }

    }
}
