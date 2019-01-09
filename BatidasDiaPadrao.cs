using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


namespace ponto
{
    public class BatidasDiaPadrao
    {
        public DateTime Entrada1 { get; private set; }
        public DateTime Saida1 { get; private set;}
        public DateTime Entrada2 { get; private set;}
        public DateTime Saida2 { get; private set;}
        public int tolerancia { get; private set;}
        private string diretorio;
        private string arquivo;


        public BatidasDiaPadrao(string data,string _diretorio,string _arquivo)
        {
            diretorio = _diretorio;
            arquivo = _arquivo;
            tolerancia = 10;
            lerHorarioExtrato(data);

        }

        private void lerHorarioIni(string data)
        {
            var iniFile = new IniFile(@diretorio+arquivo);

            Entrada1 = DateTime.Parse(data + " " + iniFile.GetValue("Horarios", "Entrada1", "08:00"));
            Saida1 = DateTime.Parse(data + " " + iniFile.GetValue("Horarios", "Saida1", "12:00"));
            Entrada2 = DateTime.Parse(data + " " + iniFile.GetValue("Horarios", "Entrada2", "14:00"));
            Saida2 = DateTime.Parse(data + " " + iniFile.GetValue("Horarios", "Saida2", "18:00"));
        }
        private void lerHorarioExtrato(string data)
        {
            string[] lines = System.IO.File.ReadAllLines(@diretorio+arquivo);

            string[] line = lines[10].Split(" ");
        
            Entrada1 = DateTime.Parse(data + " " + line[3]);
            Saida1 = DateTime.Parse(data + " " + line[4]);
            Entrada2 = DateTime.Parse(data + " " + line[5]);
            Saida2 = DateTime.Parse(data + " " + line[6]);
        }
    }
}