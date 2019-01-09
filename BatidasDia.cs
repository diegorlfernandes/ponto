using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ponto
{
    public class BatidasDia
    {
        public DateTime Data { get; private set; }
        public string strData { get; private set; }
        public DateTime? Entrada1 { get; private set; }
        public DateTime? Saida1 { get; private set; }
        public DateTime? Entrada2 { get; private set; }
        public DateTime? Saida2 { get; private set; }
        public int diferenca { get; private set; }
        public BatidasDiaPadrao batidasDiaPadrao { get; private set; }

        public BatidasDia(string linha, string _diretorio, string _arquivo)
        {
            strData = lerData(linha);
            diferenca = 50;

            batidasDiaPadrao = new BatidasDiaPadrao(strData, _diretorio, _arquivo);
            lerBatida(linha);
        }

        private void lerBatida(string line)
        {
            try
            {

                this.Data = DateTime.Parse(strData);

                Regex regex = new Regex(@"(?<hora>[\d][\d]:[\d][\d])");
                MatchCollection matches = regex.Matches(line.Substring(0, 50));

                if (matches.Count == 4)
                {
                    Entrada1 = DateTime.Parse(strData + " " + matches[0].Value.ToString());
                    Saida1 = DateTime.Parse(strData + " " + matches[1].Value.ToString());
                    Entrada2 = DateTime.Parse(strData + " " + matches[2].Value.ToString());
                    Saida2 = DateTime.Parse(strData + " " + matches[3].Value.ToString());
                }
                else
                {
                    foreach (var item in matches)
                        definirBatida(item.ToString());
                }



            }
            catch (Exception)
            {
                throw new Exception("Erro na leitura deste dia.");
            }
        }

        public static String lerData(string line)
        {
            if (line.Length < 5)
                return "";

            string data = line.Substring(0, 5);
            if (string.IsNullOrWhiteSpace(data))
                return "";

            return data + "/" + DateTime.Today.Year.ToString();
        }
        public double obterDiferencaHorarioPadraoBatida(string tipoBatida)
        {
            TimeSpan? dif;
            switch (tipoBatida)
            {
                case "E1":
                    dif = Entrada1 != null ? Entrada1 - batidasDiaPadrao.Entrada1 : null;
                    return dif != null ? ((TimeSpan)dif).TotalMinutes : 0;
                case "S1":
                    dif = Saida1 - batidasDiaPadrao.Saida1;
                    return dif != null ? ((TimeSpan)dif).TotalMinutes : 0;
                case "E2":
                    dif = Entrada2 - batidasDiaPadrao.Entrada2;
                    return dif != null ? ((TimeSpan)dif).TotalMinutes : 0;
                case "S2":
                    dif = Saida2 - batidasDiaPadrao.Saida2;
                    return dif != null ? ((TimeSpan)dif).TotalMinutes : 0;
                default:
                    return 0;
            }
        }

        public void definirBatida(string Batida)
        {


            DateTime dateBatida = DateTime.Parse(strData + " " + Batida);

            if (diferencaMinutos(dateBatida, batidasDiaPadrao.Entrada1) < diferenca)
                Entrada1 = dateBatida;
            else if (diferencaMinutos(dateBatida, batidasDiaPadrao.Entrada2) < diferenca)
                Entrada2 = dateBatida;
            else if (diferencaMinutos(dateBatida, batidasDiaPadrao.Saida1) < diferenca)
                Saida1 = dateBatida;
            else if (diferencaMinutos(dateBatida, batidasDiaPadrao.Saida2) < diferenca)
                Saida2 = dateBatida;
        }
        public void definirBatidaPelaDiferenca(string Batida)
        {


            DateTime dateBatida = DateTime.Parse(strData + " " + Batida);

            if (diferencaMinutos(dateBatida, batidasDiaPadrao.Entrada1) < diferenca)
                Entrada1 = dateBatida;
            else if (diferencaMinutos(dateBatida, batidasDiaPadrao.Entrada2) < diferenca)
                Entrada2 = dateBatida;
            else if (diferencaMinutos(dateBatida, batidasDiaPadrao.Saida1) < diferenca)
                Saida1 = dateBatida;
            else if (diferencaMinutos(dateBatida, batidasDiaPadrao.Saida2) < diferenca)
                Saida2 = dateBatida;
        }

        private double diferencaMinutos(DateTime batida, DateTime batidaPadrao)
        {
            double ret = Math.Abs((batida - batidaPadrao).TotalMinutes);
            return ret;
        }

    }
}