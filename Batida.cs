using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


namespace ponto
{
    public abstract class Batida
    {
        public DateTime Entrada1 { get; protected set; }
        public DateTime Saida1 { get; protected set;}
        public DateTime Entrada2 { get; protected set;}
        public DateTime Saida2 { get; protected set;}
        public int tolerancia { get; protected set;}
        protected string diretorio;
        protected string arquivo;



        public abstract void lerBatida(DateTime data);
        public abstract void gerarBatida(DateTime data);
    }
}