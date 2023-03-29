using ItAcademyDell.Models;

namespace ItAcademyDell
{
    internal static class TransporteData
    {
        private const string Separator = ";";
        
        private const string C_CAMINHO_DADOS = @"./data.csv";

        internal static List<string> NomesCidades { get; set; }

        internal static int[][] DistanciasCidades { get; set; }

        internal static List<OrcamentoModel> Orcamentos { get; set; } = new List<OrcamentoModel>();

        internal static void PreencherDadosCSV()
        {

            using (StreamReader reader = new(C_CAMINHO_DADOS))
            {
                string cidades = reader.ReadLine(); // A primeira linha contem o nome das cidades
                NomesCidades = cidades.Trim().Split(Separator).ToList();
                DistanciasCidades = new int[NomesCidades.Count][];
                int contadorLinha = 0;
                while (!reader.EndOfStream)
                {

                    string[] linhaDistancias = reader.ReadLine().Trim().Split(Separator);
                    DistanciasCidades[contadorLinha] =
                        linhaDistancias
                        .ToList()
                        .ConvertAll(x => int.Parse(x))
                        .ToArray();

                    contadorLinha++;
                }
            }

        }

        internal static int BuscarDistanciaEntreCidades(string nomeOrigem, string nomeDestino)
        {
            int posOrigem = NomesCidades.FindIndex((x) => x.Equals(nomeOrigem));
            int posDestino = NomesCidades.FindIndex((x) => x.Equals(nomeDestino));

            if (posOrigem == -1)
                throw new ArgumentException("Cidade de Origem não encontrada.");
            if (posDestino == -1)
                throw new ArgumentException("Cidade de Destino não encontrada.");

            return DistanciasCidades[posOrigem][posDestino];

        }

        internal static List<CaminhaoModel> ModelosDeTransporte()
        {
            return new List<CaminhaoModel>
            {
                new CaminhaoModel { Id = 1, Nome = "Caminhão de pequeno porte", PrecoPorKm = 4.87, PesoMaximo = 1000 },
                new CaminhaoModel { Id = 2, Nome = "Caminhão de médio porte", PrecoPorKm = 11.92, PesoMaximo = 4000 },
                new CaminhaoModel { Id = 3, Nome = "Caminhão de grande porte", PrecoPorKm = 27.44, PesoMaximo = 10000 }
            };
        }

        internal static List<ProdutoModel> ProdutosDisponiveis()
        {
            return new List<ProdutoModel>()
            {
                new ProdutoModel(){Id=1, Nome= "Celular", PesoKg = 0.5 },
                new ProdutoModel(){Id=2, Nome= "Geladeira", PesoKg = 60.0 },
                new ProdutoModel(){Id=3, Nome= "Freezer", PesoKg = 100.0 },
                new ProdutoModel(){Id=4, Nome= "Cadeira", PesoKg = 5.0 },
                new ProdutoModel(){Id=5, Nome= "Luminária", PesoKg = 0.8 },
                new ProdutoModel(){Id=6, Nome= "Lavadora de roupa", PesoKg = 120 }
            };
        }

    }
}
