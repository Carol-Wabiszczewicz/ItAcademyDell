using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ItAcademyDell.Models
{
    public class TransporteModel
    {
        [BindProperty]
        public string NomeEmpresa { get; set; }

        [BindProperty]
        public string CidadeOrigem { get; set; }

        [BindProperty]
        public string CidadeDestino { get; set; }

    }
}