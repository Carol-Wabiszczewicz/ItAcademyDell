using ItAcademyDell.Models;
using ItAcademyDell;
using Microsoft.AspNetCore.Mvc;

namespace ItAcademyDell.Controllers
{
    public class TransporteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GravarDadosIniciais()
        {
            var entrada = RetornaMovimentacaoDaTela(0, Request.Form, TipoMovimentacao.Entrada);
            TempData.Put("NomeEmpresa", Request.Form["NomeEmpresa"].ToString());
            TempData.Put("EntradaProdutos", entrada);
            return PartialView("_movimentacoes");
        }

        [HttpPost]
        public IActionResult AdicionarItemMovimentacoes()
        {
            if (TempData["SaidaProdutos"] == null)
                TempData.Put("SaidaProdutos", new List<MovimentacaoProdutoPorCidade>());

            var lista = TempData.Peek<List<MovimentacaoProdutoPorCidade>>("SaidaProdutos");
            var id = lista.Count > 0 ? lista.Max(x => x.Id) + 1 : 1;
            lista.Add(
                RetornaMovimentacaoDaTela(id, Request.Form, TipoMovimentacao.Saida)
            );

            TempData.Put("SaidaProdutos", lista);
            return PartialView("_movimentacoes");
        }

        [HttpPost]
        public IActionResult RemoverSaida(int id)
        {
            if (TempData["SaidaProdutos"] == null)
                TempData["SaidaProdutos"] = new List<MovimentacaoProdutoPorCidade>();

            var lista = TempData.Peek<List<MovimentacaoProdutoPorCidade>>("SaidaProdutos");
            var Item = lista.FirstOrDefault(x => x.Id == id);
            if (Item != null)
                lista.Remove(Item);

            TempData.Put("SaidaProdutos", lista);
            return PartialView("_movimentacoes");
        }
        [HttpPost]
        public IActionResult Gravar()
        {
            var id = TransporteData.Orcamentos?.Count > 0 ? TransporteData.Orcamentos.Max(x => x.Id) + 1 : 1;
            List<DetalhesMovimentacaoEntreCidades> detalhesMovimentacaoEntreCidades = new List<DetalhesMovimentacaoEntreCidades>();
            foreach (var saida in TempData.Peek<List<MovimentacaoProdutoPorCidade>>("SaidaProdutos"))
            {
                detalhesMovimentacaoEntreCidades.Add(new DetalhesMovimentacaoEntreCidades()
                {
                    Id = saida.Id,
                    Cidade = saida.Cidade,
                    Produtos = saida.Produtos,
                    TipoMovimentacao = saida.TipoMovimentacao
                });
            }
            var orcamento = new OrcamentoModel(
                    id,
                    TempData.Get<string>("NomeEmpresa"),
                    TempData.Get<MovimentacaoProdutoPorCidade>("EntradaProdutos"),
                    detalhesMovimentacaoEntreCidades);
            TransporteData.Orcamentos.Add(orcamento);
            
            return View(orcamento);
        }
        private static MovimentacaoProdutoPorCidade RetornaMovimentacaoDaTela(int id, IFormCollection formData, TipoMovimentacao tipoMovimentacao)
        {
            if (formData == null)
                return null;

            var listaProdutos = new ListaProduto();
            foreach (var item in TransporteData.ProdutosDisponiveis())
            {
                string nomeCampoProduto =
                    tipoMovimentacao == TipoMovimentacao.Entrada ?
                    "txtEntProd" :
                    "txtSaidaProd";

                if (int.TryParse(formData[$"{nomeCampoProduto}{item.Id}"], out int value))
                    listaProdutos.Add(
                        new ProdutoMovimentacao() { Id = item.Id, Nome = item.Nome, Quantidade = value, Peso = item.PesoKg }
                    );
            }

            return new MovimentacaoProdutoPorCidade()
            {
                Id = id,
                Cidade = tipoMovimentacao == TipoMovimentacao.Entrada ? formData["CidadeOrigem"] : formData["CidadeDestino"],
                TipoMovimentacao = TipoMovimentacao.Saida,
                Produtos = listaProdutos
            };
        }
    }
}
