using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ponto
{
    public class Ponto
    {
        private static StringBuilder compensacao = new StringBuilder();
        private static StringBuilder horasExtras = new StringBuilder();
        private static StringBuilder avisos = new StringBuilder();
        private static StringBuilder almoco = new StringBuilder();
        private static StringBuilder batidas = new StringBuilder();
        private static StringBuilder erros = new StringBuilder();
        private string diretorio;
        private string arquivo;


        public Ponto(string _diretorio, string _arquivo)
        {
            diretorio = _diretorio;
            arquivo = _arquivo;
            CabecalhoBatidasNaoRegistradas();

        }

        public void calculo()
        {
            var diasDaSemana = new List<string>();
            diasDaSemana.Add("SEG");
            diasDaSemana.Add("TER");
            diasDaSemana.Add("QUA");
            diasDaSemana.Add("QUI");
            diasDaSemana.Add("SEX");

            var observacoes = new List<string>();
            observacoes.Add("Faltas");
            observacoes.Add("Feriado");

            string[] lines = System.IO.File.ReadAllLines(@diretorio + arquivo);
            foreach (string line in lines)
            {
                String data = BatidasDia.lerData(line);
                try
                {
                    if (data == String.Empty)
                        continue;
                    if (!diasDaSemana.Any(word => line.Contains(word)))
                        continue;

                    if (observacoes.Any(word => line.Contains(word)))
                        continue;

                    BatidasDia batidasDia = new BatidasDia(line, diretorio, arquivo);

                    gerarCompensacao(batidasDia);
                    gerarHoraExtra(batidasDia);
                    ConferirHorarioAlmoco(batidasDia);
                    ConferirBatidasNaoRegistradas(batidasDia);

                }
                catch (Exception e)
                {
                    erros.AppendLine(data + " - " + e.Message);
                    continue;
                }

            }
            StreamWriter resultado = new StreamWriter(@diretorio + "resultado_" + arquivo, false, Encoding.ASCII);

            File.Move(@diretorio + arquivo, @diretorio + "processado_" + arquivo);

            if (compensacao.Length > 0)
            {
                resultado.WriteLine("          Compensacoes");
                resultado.WriteLine(compensacao);
            }

            if (horasExtras.Length > 0)
            {
                resultado.WriteLine("          Horas extras");
                resultado.WriteLine(horasExtras);

            }

            if (almoco.Length > 0)
            {
                resultado.WriteLine("          Almoco com menos de uma hora");
                resultado.WriteLine(almoco);

            }
            if (batidas.Length > 0)
            {
                resultado.WriteLine("          Batidas nao realizadas");
                resultado.WriteLine(batidas);

            }

            if (erros.Length > 0)
            {
                resultado.WriteLine("          Erros");
                resultado.WriteLine(erros);

            }

            resultado.Close();

        }
        static void gerarCompensacao(BatidasDia batidasDia)
        {
            double dif = 0;
            string dia = batidasDia.Data.Day.ToString().PadLeft(2, '0');
            string mes = batidasDia.Data.Month.ToString().PadLeft(2, '0');

            if (batidasDia.Entrada1 != null && batidasDia.Entrada1 > batidasDia.batidasDiaPadrao.Entrada1.AddMinutes(10))
            {
                dif = Math.Abs(batidasDia.obterDiferencaHorarioPadraoBatida("E1"));
                compensacao.AppendLine($"data: {dia}/{mes} horario: {batidasDia.batidasDiaPadrao.Entrada1.TimeOfDay} - {((DateTime)batidasDia.Entrada1).TimeOfDay} - {dif} min.");
            }
            if (batidasDia.Saida1 != null && batidasDia.Saida1 < batidasDia.batidasDiaPadrao.Saida1)
            {
                dif = Math.Abs(batidasDia.obterDiferencaHorarioPadraoBatida("S1"));
                compensacao.AppendLine($"data: {dia}/{mes} horario: {((DateTime)batidasDia.Saida1).TimeOfDay} - {batidasDia.batidasDiaPadrao.Saida1.TimeOfDay} - {dif} min.");
            }
            if (batidasDia.Entrada2 != null && batidasDia.Entrada2 > batidasDia.batidasDiaPadrao.Entrada2.AddMinutes(10))
            {
                dif = Math.Abs(batidasDia.obterDiferencaHorarioPadraoBatida("E2"));
                compensacao.AppendLine($"data: {dia}/{mes} horario: {batidasDia.batidasDiaPadrao.Entrada2.TimeOfDay} - {((DateTime)batidasDia.Entrada2).TimeOfDay} - {dif} min.");
            }
            if (batidasDia.Saida2 != null && batidasDia.Saida2 < batidasDia.batidasDiaPadrao.Saida2)
            {
                dif = Math.Abs(batidasDia.obterDiferencaHorarioPadraoBatida("S2"));
                compensacao.AppendLine($"data: {dia}/{mes} horario: {((DateTime)batidasDia.Saida2).TimeOfDay} - {batidasDia.batidasDiaPadrao.Saida2.TimeOfDay} - {dif} min.");
            }

        }
        static void gerarHoraExtra(BatidasDia batidasDia)
        {
            double dif = 0;
            double totalHorasHextra = 0;
            string dia = batidasDia.Data.Day.ToString().PadLeft(2, '0');
            string mes = batidasDia.Data.Month.ToString().PadLeft(2, '0');

            if (batidasDia.Entrada1 != null && batidasDia.Entrada1 < batidasDia.batidasDiaPadrao.Entrada1.AddMinutes(-10))
            {
                dif = Math.Abs(batidasDia.obterDiferencaHorarioPadraoBatida("E1"));
                totalHorasHextra += dif;
                horasExtras.AppendLine($"data: {dia}/{mes} horario: {((DateTime)batidasDia.Entrada1).TimeOfDay} - {batidasDia.batidasDiaPadrao.Entrada1.TimeOfDay} - {dif} min.");
            }
            if (batidasDia.Saida1 != null && batidasDia.Saida1 > batidasDia.batidasDiaPadrao.Saida1.AddMinutes(10))
            {
                dif = Math.Abs(batidasDia.obterDiferencaHorarioPadraoBatida("S1"));
                totalHorasHextra += dif;
                horasExtras.AppendLine($"data: {dia}/{mes} horario: {batidasDia.batidasDiaPadrao.Saida1.TimeOfDay} - {((DateTime)batidasDia.Saida1).TimeOfDay} - {dif} min.");
            }
            if (batidasDia.Entrada2 != null && batidasDia.Entrada2 < batidasDia.batidasDiaPadrao.Entrada2.AddMinutes(-10))
            {
                dif = Math.Abs(batidasDia.obterDiferencaHorarioPadraoBatida("E2"));
                totalHorasHextra += dif;
                horasExtras.AppendLine($"data: {dia}/{mes} horario: {((DateTime)batidasDia.Entrada2).TimeOfDay} - {batidasDia.batidasDiaPadrao.Entrada2.TimeOfDay} - {dif} min.");
            }
            if (batidasDia.Saida2 != null && batidasDia.Saida2 > batidasDia.batidasDiaPadrao.Saida2.AddMinutes(10))
            {
                dif = Math.Abs(batidasDia.obterDiferencaHorarioPadraoBatida("S2"));
                totalHorasHextra += dif;
                horasExtras.AppendLine($"data: {dia}/{mes} horario: {batidasDia.batidasDiaPadrao.Saida2.TimeOfDay} - {((DateTime)batidasDia.Saida2).TimeOfDay} - {dif} min.");
            }

            if (totalHorasHextra > 120)
                avisos.AppendLine($"data: {batidasDia.Data.Day}/{mes} quantidade de horas extras excedida nesta data.");

        }

        static void ConferirHorarioAlmoco(BatidasDia batidasDia)
        {
            if (!batidasDia.Entrada2.HasValue || !batidasDia.Saida1.HasValue)
                return;


            TimeSpan dif = batidasDia.Entrada2.Value - batidasDia.Saida1.Value;

            if (dif.TotalMinutes < 60)
            {
                almoco.AppendLine($"data: {batidasDia.Data.Day}/{batidasDia.Data.Month} Intervalo de almoco foi menor que uma hora.");
            }
        }

        static void CabecalhoBatidasNaoRegistradas(BatidasDia batidasDia)
        {
            batidas.AppendLine();
            batidas.AppendLine(@"               Entrada 1      Saida 1        Entrada 2      Saida 2        ");

        }
        static void ConferirBatidasNaoRegistradas(BatidasDia batidasDia)
        {

            batidas.Append($"data: {batidasDia.Data.Day}/{batidasDia.Data.Month}");
            if (!batidasDia.Entrada1.HasValue)
                batidas.Append($"       X       ");
            else
                batidas.Append($"               ");

            if (!batidasDia.Saida1.HasValue)
                batidas.Append($"       X       ");
            else
                batidas.Append($"               ");

            if (!batidasDia.Entrada2.HasValue)
                batidas.Append($"       X       ");
            else
                batidas.Append($"               ");

            if (!batidasDia.Saida2.HasValue)
                batidas.Append($"       X       ");
            else
                batidas.Append($"               ");

            batidas.AppendLine();
        }

    }
}