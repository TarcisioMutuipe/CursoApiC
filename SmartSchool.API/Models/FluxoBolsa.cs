using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.Models
{
    public class FluxoBolsa
    {
        public FluxoBolsa() { }
        public FluxoBolsa(int id, string codCorretora, string nomeCorretora, int quantidade, double volume, int numNegocio, 
            decimal precoMedio, int acoesInfoId)
        {
            this.Id = id;
            this.CodCorretora = codCorretora;
            this.NomeCorretora = nomeCorretora;
            this.Quantidade = quantidade;
            this.Volume = volume;
            this.NumNegocio = numNegocio;
            this.AcoesInfoId = acoesInfoId;
        }
        public int Id { get; set; }
        public string CodCorretora { get; set; }
        public string NomeCorretora { get; set; }
        public int Quantidade { get; set; }
        public double Volume { get; set; }
        public int NumNegocio { get; set; }
        public int AcoesInfoId { get; set; }
        public AcoesInfo AcoesInfo { get; set; }
        public decimal PrecoMedio { get; set; }
        public DateTime? Data { get; set; } = DateTime.Now.Date;

    }

}
