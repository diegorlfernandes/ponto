using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


namespace ponto
{
    public class BatidaAleatoria:Batida
    {


        public BatidaAleatoria(string _diretorio,string _arquivo)
        {
            
            diretorio = _diretorio;
            arquivo = _arquivo;
            tolerancia = 10;

        }
        public override void lerBatida(DateTime data)
        {

        }

        public override void gerarBatida(DateTime data)
        {
           string strData = data.Day.ToString()+"/"+data.Month.ToString()+"/"+data.Year.ToString();

            Random rd = new Random();
            
            string minuto = rd.Next(0,10).ToString();        
            Entrada1 = DateTime.Parse(strData + " " + "08:"+minuto);
            
             minuto = rd.Next(0,10).ToString();
            Saida1 = DateTime.Parse(strData + " " + "12:"+minuto);
            
            minuto = rd.Next(0,10).ToString();
            Entrada2 = DateTime.Parse(strData + " " + "13:"+minuto);
            
            minuto = rd.Next(0,10).ToString();
            Saida2 = DateTime.Parse(strData + " " + "17:"+minuto);
        }
    }
}