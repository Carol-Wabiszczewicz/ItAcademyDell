namespace ItAcademyDell.Models
{
    internal class OrcamentoModel
    {
        public OrcamentoModel(int id, string nomeEmpresa, MovimentacaoProdutoPorCidade entrada, List<DetalhesMovimentacaoEntreCidades> saidas)
        {
            Id = id;
            NomeEmpresa = nomeEmpresa;
            Entrada = entrada;
            Saida = saidas;
            ValorTotal = CalcularDistanciaTotalPercorrida();
            CalcularValorTotal();

        }
        public int Id { get; set; }
        public string NomeEmpresa { get; set; }

        public Dictionary<int, CaminhaoModel> Caminhoes { get; set; }

        public MovimentacaoProdutoPorCidade Entrada { get; set; }
        public List<DetalhesMovimentacaoEntreCidades> Saida { get; set; }
        public long ValorTotal { get; set; }

        public double PesoTotalEntrada
        {
            get
            {
                return Entrada.Produtos.Sum(x => x.PesoTotal);
            }
        }

        public long CalcularDistanciaTotalPercorrida()
        {
            Saida[0].DistanciaDaCidadeAnterior =
                TransporteData.BuscarDistanciaEntreCidades(Entrada.Cidade, Saida[0].Cidade);
            for (int i = 1; i < Saida.Count; i++)
            {
                Saida[i].DistanciaDaCidadeAnterior = TransporteData.BuscarDistanciaEntreCidades(Saida[i - 1].Cidade, Saida[i].Cidade);
            }
            return Saida.Sum(x => x.DistanciaDaCidadeAnterior);
        }

        public double CalcularValorTotal()
        {
            double totalReducaoDeCarga = 0;
            for (int i = 0; i < Saida.Count; i++)
            {
                totalReducaoDeCarga += (i > 0) ? Saida[i - 1].Produtos.Sum(p => p.PesoTotal) : 0;
                var pesoRestante = PesoTotalEntrada - totalReducaoDeCarga;
                foreach (var caminhao in TransporteData.ModelosDeTransporte().OrderByDescending(x => x.PesoMaximo))
                {
                    Saida[i].Caminhoes.Add(new CaminhaoModel()
                    {
                        Id = caminhao.Id,
                        PesoMaximo = caminhao.PesoMaximo,
                        Nome = caminhao.Nome,
                        Quantidade = (int)(pesoRestante / caminhao.PesoMaximo)
                    });
                    pesoRestante %= caminhao.PesoMaximo;
                }
                if (pesoRestante > 0)
                    Saida[i].Caminhoes.First(c => c.Id == 1).Quantidade++;

                long distanciaTotalPercorrida = CalcularDistanciaTotalPercorrida();
                foreach (var caminhao in Saida[i].Caminhoes)
                {
                    Saida[i].PrecoDoTrecho += caminhao.PrecoPorKm * distanciaTotalPercorrida;
                }
            }
            return Saida.Sum(x => x.PrecoDoTrecho);
        }
    }
}