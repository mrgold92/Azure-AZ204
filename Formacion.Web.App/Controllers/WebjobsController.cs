using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;

namespace Formacion.Web.App.Controllers
{
    public class WebjobsController : Controller
    {
        [BindProperty]
        public IFormFile UploadFileContent { get; set; }

        private IConfiguration config { get; set; }

        public WebjobsController(IConfiguration config)
        {
            this.config = config;
        }




        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Ficheros()
        {
            return View();
        }


        public IActionResult UploadFile()
        {
            try
            {
                if (UploadFileContent == null || UploadFileContent.Length == 0)
                {
                    return Content("Fichero no seleccionado.");
                }

                var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload", UploadFileContent.FileName);

                //copiar fichero a carpeta
                var stream = new FileStream(ruta, FileMode.Create);
                UploadFileContent.CopyTo(stream);
                stream.Close();

            }
            catch (Exception e)
            {

                return Content(e.Message);
            }

            return RedirectToAction("ficheros");
        }

        public IActionResult UploadFile2()
        {

            try
            {
                var cn = config.GetSection("AzureStorageConnectionString").Value;
                //Creamos el objeto para manejar blobs (cuentas de almacenamiento)
                BlobServiceClient blobClientService = new BlobServiceClient(cn);
                //Creamos un contenedor dentro con un nombre específico
                BlobContainerClient containerClient = blobClientService.GetBlobContainerClient("upload");

                if (!containerClient.Exists())
                {
                    containerClient = blobClientService.CreateBlobContainer("upload");
                }

                BlobClient blobClient = containerClient.GetBlobClient(UploadFileContent.FileName);
                blobClient.Upload(UploadFileContent.OpenReadStream());
            }
            catch (Exception e)
            {

                return Content(e.Message);
            }

            return RedirectToAction("ficheros");
        }

    }
}
