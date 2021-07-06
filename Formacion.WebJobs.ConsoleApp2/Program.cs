using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formacion.WebJobs.ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($">> PROCESO INICIADO {DateTime.Now}");
            try
            {
                var ficheros = Directory.GetFiles(@"C:\home\site\wwwroot\wwwroot\upload");
                Console.WriteLine($">>>> {ficheros.Length} ficheros encontrados.");
                foreach (var ruta in ficheros)
                {
                    var fichero = new FileInfo(ruta);
                    Console.WriteLine($">>>>>> {fichero.FullName}");
                    File.Move(fichero.FullName, @"C:\home\site\wwwroot\wwwroot\process\" + fichero.Name);

                    Console.WriteLine($">>>>> P R O C E S A D O");
                }
                


            }
            catch (Exception e1)
            {

                Console.WriteLine($"{e1.Message}"); ;
            }

            Console.WriteLine($">> PROCESO FINALIZADO {DateTime.Now}");
        }
    }
}
