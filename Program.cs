using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EstacionamentoApp
{
    public class Estacionamento
    {
        private readonly decimal precoInicial;
        private readonly decimal precoPorHora;
        private readonly List<string> veiculos;

        public Estacionamento(decimal precoInicial, decimal precoPorHora)
        {
            if (precoInicial < 0 || precoPorHora < 0)
                throw new ArgumentException("Preços não podem ser negativos, hein.");

            this.precoInicial = precoInicial;
            this.precoPorHora = precoPorHora;
            this.veiculos = new List<string>();
        }


        public bool AdicionarVeiculo(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                return false;

            placa = placa.ToUpperInvariant();

            if (veiculos.Contains(placa))
                return false;

            veiculos.Add(placa);
            return true;
        }


        public decimal? RemoverVeiculo(string placa, int horasEstacionado)
        {
            if (string.IsNullOrWhiteSpace(placa) || horasEstacionado < 0)
                return null;

            placa = placa.ToUpperInvariant();

            if (!veiculos.Contains(placa))
                return null;

            veiculos.Remove(placa);

            decimal valorTotal = precoInicial + precoPorHora * horasEstacionado;

            return valorTotal;
        }


        public List<string> ListarVeiculos()
        {
            return new List<string>(veiculos);
        }


        public bool VeiculoEstacionado(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                return false;

            return veiculos.Contains(placa.ToUpperInvariant());
        }
    }

    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("=== Bem-vindo ao Sistema de Estacionamento! ===");
            Console.WriteLine();

            decimal precoInicial = LerDecimal("Quanto custa a entrada? R$ ");
            decimal precoPorHora = LerDecimal("E o preço por hora? R$ ");

            var estacionamento = new Estacionamento(precoInicial, precoPorHora);

            while (true)
            {
                Console.Clear();

                Console.WriteLine("O que você quer fazer agora?");
                Console.WriteLine("1 - Estacionar um veículo");
                Console.WriteLine("2 - Retirar um veículo");
                Console.WriteLine("3 - Mostrar veículos estacionados");
                Console.WriteLine("4 - Sair do sistema");
                Console.Write("Sua opção: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        AdicionarVeiculo(estacionamento);
                        break;

                    case "2":
                        RemoverVeiculo(estacionamento);
                        break;

                    case "3":
                        ListarVeiculos(estacionamento);
                        break;

                    case "4":
                        Console.WriteLine("\nValeu! Até a próxima.");
                        return;

                    default:
                        Console.WriteLine("\nEssa opção não é válida, tenta de novo!");
                        Pausar();
                        break;
                }
            }
        }


        private static void AdicionarVeiculo(Estacionamento estacionamento)
        {
            Console.Write("\nManda aí a placa do carro que vai estacionar: ");
            string placa = Console.ReadLine()?.Trim();

            if (estacionamento.AdicionarVeiculo(placa))
                Console.WriteLine("Beleza, carro estacionado!");
            else
                Console.WriteLine("Ops, placa inválida ou esse carro já tá dentro.");

            Pausar();
        }


        private static void RemoverVeiculo(Estacionamento estacionamento)
        {
            Console.Write("\nQual a placa do carro que vai sair? ");
            string placa = Console.ReadLine()?.Trim();

            if (!estacionamento.VeiculoEstacionado(placa))
            {
                Console.WriteLine("Não achei esse carro aqui dentro, confere aí.");
                Pausar();
                return;
            }

            int horas = LerInt("Quantas horas ficou parado? ");

            decimal? valor = estacionamento.RemoverVeiculo(placa, horas);

            if (valor.HasValue)
            {
                Console.WriteLine($"\nO carro {placa.ToUpperInvariant()} saiu.");
                Console.WriteLine($"Vai pagar R$ {valor.Value:F2}, beleza?");
            }
            else
            {
                Console.WriteLine("Deu algum erro, tenta de novo.");
            }

            Pausar();
        }


        private static void ListarVeiculos(Estacionamento estacionamento)
        {
            var veiculos = estacionamento.ListarVeiculos();

            Console.WriteLine();

            if (veiculos.Any())
            {
                Console.WriteLine("Olha só quem tá estacionado:");
                foreach (var v in veiculos)
                {
                    Console.WriteLine($"- {v}");
                }
            }
            else
            {
                Console.WriteLine("Tá vazio aqui, sem carros estacionados.");
            }

            Pausar();
        }


        private static decimal LerDecimal(string mensagem)
        {
            decimal valor;

            while (true)
            {
                Console.Write(mensagem);
                string entrada = Console.ReadLine();

                if (decimal.TryParse(entrada, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out valor) && valor >= 0)
                    return valor;

                Console.WriteLine("Tá errado esse valor, coloca um número válido aí.");
            }
        }


        private static int LerInt(string mensagem)
        {
            int valor;

            while (true)
            {
                Console.Write(mensagem);
                string entrada = Console.ReadLine();

                if (int.TryParse(entrada, out valor) && valor >= 0)
                    return valor;

                Console.WriteLine("Não é um número válido, tenta de novo.");
            }
        }


        private static void Pausar()
        {
            Console.WriteLine("\nPressione qualquer tecla pra continuar...");
            Console.ReadKey();
        }
    }
}
