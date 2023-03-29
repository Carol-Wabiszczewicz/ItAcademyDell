using System.Text;

namespace ItAcademyDell.Models
{
    public class MovimentacaoProdutoPorCidade
    {
        public int Id { get; set; }

        public string Cidade { get; set; }

        public ListaProduto Produtos { get; set; }

        public TipoMovimentacao TipoMovimentacao { get; set; }
    }

    public class DetalhesMovimentacaoEntreCidades : MovimentacaoProdutoPorCidade
    {


        public List<CaminhaoModel> Caminhoes { get; set; } = new List<CaminhaoModel>();

        public int DistanciaDaCidadeAnterior { get; set; }

        public double PrecoDoTrecho { get; set; }
    }

    public class ListaProduto : List<ProdutoMovimentacao>
    {
        public int QuantidadePorId(int Id)
        {
            return this.FirstOrDefault(prod => prod.Id.Equals(Id))?.Quantidade ?? 0;
        }
        public double PesoPorId(int Id)
        {
            return this.FirstOrDefault(prod => prod.Id.Equals(Id))?.PesoTotal ?? 0;
        }
        public override string ToString()
        {
            return string.Join(", ", this.Select(x => x.Nome).ToArray());
        }
    }

    public class ProdutoMovimentacao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Peso { get; set; }
        public int Quantidade { get; set; }
        public double PesoTotal
        {
            get
            {
                return Peso * Quantidade;
            }
        }
    }
    public enum TipoMovimentacao
    {
        Entrada,
        Saida
    }
}
