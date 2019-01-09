using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ponto
{
    public class Atualizacao
    {
        private DirectoryInfo dirApp;
        private DirectoryInfo dirAtu;
        private FileInfo FileApp;
        public Atualizacao()
        {
            dirApp = new DirectoryInfo("c:\\ponto\\bin\\");
            dirAtu = new DirectoryInfo("p:\\ponto\\bin\\");

        }
        public void Atualizar()
        {

            if (!System.IO.Directory.Exists(dirAtu.FullName))
            {
                Console.WriteLine("Não existe atualização disponível");
                return;
            }

            bool atualizou = false;
            
            foreach (FileInfo file in dirAtu.GetFiles())
            {
                if(System.IO.File.Exists(dirApp.FullName + file.Name))
                {
                    FileApp = new FileInfo(dirApp.FullName + file.Name);
                }

                if (file.LastWriteTime > FileApp.LastWriteTime)
                {
                    copiar(file.Name);
                    atualizou = true;
                }

            }
            if(atualizou)
                Console.WriteLine("Aplicação foi atualizada.");        
        }
        private void copiar(string fileName)
        {
            string sourcePath = dirAtu.FullName;
            string targetPath = dirApp.FullName;

            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }

            System.IO.File.Copy(sourceFile, destFile, true);

        }
    }
}