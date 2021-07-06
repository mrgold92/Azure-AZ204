using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Formacion.WebJobs.WebJob1
{
    public class Functions
    {
        public static void Func1(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Func1 ejecutada correctamente");
        }

        public static void Func2([TimerTrigger("*/10 * * * * *")] TimerInfo timer, TextWriter log)
        {
            log.WriteLine($">> PROCESO INICIADO {DateTime.Now}");
            try
            {
                var ficheros = Directory.GetFiles(@"C:\home\site\wwwroot\wwwroot\upload");
                log.WriteLine($">>>> {ficheros.Length} ficheros encontrados.");
                foreach (var ruta in ficheros)
                {
                    var fichero = new FileInfo(ruta);
                    log.WriteLine($">>>>>> {fichero.FullName}");
                    File.Move(fichero.FullName, @"C:\home\site\wwwroot\wwwroot\process\" + fichero.Name);

                    log.WriteLine($">>>>> P R O C E S A D O");


                }


            }
            catch (Exception e1)
            {

                log.WriteLine($"{e1.Message}"); ;
            }

            log.WriteLine($">> PROCESO FINALIZADO {DateTime.Now}");
        }

        public static void Func3(
            [BlobTrigger("upload/{name}")] TextReader upload,
            [Blob("process/{name}")] out string process,
            string name,
            TextWriter log)
        {
            log.WriteLine($">> PROCESO INICIADO {DateTime.Now}");
            log.WriteLine($">> {name}");
            process = upload.ReadToEnd();
            log.WriteLine($">> PROCESO FINALIZADO {DateTime.Now}");
        }
    }
}
