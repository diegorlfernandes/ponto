using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ponto
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter log = new StreamWriter("c:\\ponto_publicacao\\log.txt", true, Encoding.ASCII);
            log.WriteLine("inicio");
            try
            {

                DirectoryInfo dir;
                dir = new DirectoryInfo(@"\\srvarq\\publico\\");

                foreach (FileInfo file in dir.GetFiles())
                {
                    if (file.Name.ToUpper().Contains("PONTO_")
                    && !file.Name.ToUpper().Contains("RESULTADO")
                    && !file.Name.ToUpper().Contains("PROCESSADO"))
                    {
                        log.WriteLine("processando "+file.Name);
                        new Ponto(dir.FullName, file.Name).calculo();
                    }
                }
            }
            catch (Exception e)
            {
                log.WriteLine(e.Message+e.StackTrace);

            }
            finally
            {
                log.WriteLine("Fim");
                log.Close();
                log.Dispose();

            }



        }

    }
}
