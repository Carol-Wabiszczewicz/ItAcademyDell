using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ItAcademyDell.Models
{
    public class HomeModel
    {
        [BindProperty]
        public string CidadeOrigem { get; set; }

        [BindProperty]
        public string CidadeDestino { get; set; }

        [BindProperty]
        public int ModeloDeTransporte { get; set; }

        public string DescricaoTransporte { get; internal set; }
        
        internal int Distancia { get; set; }

        internal bool ValoresCalculados { get; set; }
        
        internal double CustoTotal { get; set; }

    }
}