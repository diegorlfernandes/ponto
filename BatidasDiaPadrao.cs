using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


namespace ponto
{
    public class BatidasDiaPadrao
    {
        public DateTime Entrada1 { get; protected set; }
        public DateTime Saida1 { get; protected set;}
        public DateTime Entrada2 { get; protected set;}
        public DateTime Saida2 { get; protected set;}
        public int tolerancia { get; protected set;}
        private 

        public BatidasDiaPadrao(string data,string[] lines)
        {
            tolerancia = 10;
            lerHorarioExtrato(data);

        }

        private void lerHorarioExtrato(string data)
        {

            string[] line = lines[10].Split(" ");
        
            Entrada1 = DateTime.Parse(data + " " + line[3]);
            Saida1 = DateTime.Parse(data + " " + line[4]);
            Entrada2 = DateTime.Parse(data + " " + line[5]);
            Saida2 = DateTime.Parse(data + " " + line[6]);
        }
    }
}