﻿using System;
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
        public async Task<IHttpActionResult> UploadImage()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("Unsupported media type");
            }

            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                // Directorio donde se almacenarán las imágenes
                string directoryPath = HttpContext.Current.Server.MapPath("~/Imgs");

                // Asegura que la carpeta existe
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                foreach (var file in provider.Contents)
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Trim('"');

                    // Validación de tipo MIME (solo imágenes)
                    var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/jpg" };
                    if (!allowedMimeTypes.Contains(file.Headers.ContentType.MediaType))
                    {
                        return BadRequest("Invalid file type. Only images are allowed.");
                    }

                    var buffer = await file.ReadAsByteArrayAsync();

                    // Genera un nombre único para evitar sobrescribir archivos
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                    string filePath = Path.Combine(directoryPath, uniqueFileName);

                    // Guarda el archivo en el servidor
                    File.WriteAllBytes(filePath, buffer);

                    // 🔥 Retornar la URL de la imagen subida
                    return Ok(new
                    {
                        message = "Image uploaded successfully",
                        fileName = uniqueFileName,
                        url = $"http://10.0.3.148/api/Imgs/{uniqueFileName}"
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
