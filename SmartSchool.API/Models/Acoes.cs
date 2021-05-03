using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.Models
{
    public class Acoes
    {
        public Acoes() { }
        public Acoes(int id,  decimal varD, decimal ultima, string descricao, int numNegocio, int volume, decimal AgressorCompra, decimal agressorVenda, int notaForca, int acoesInfoId)
        {
            this.Id = id;           
            this.VarD = varD;
            this.Ultima = ultima;           
            this.NumNegocio = numNegocio;
            this.Volume = volume;
            this.AgressorCompra = AgressorCompra;
            this.AgressorVenda = agressorVenda;
            this.NotaForca = notaForca;
            this.AcoesInfoId = acoesInfoId;

        }
        public int Id { get; set; }
        public decimal VarD { get; set; }
        public decimal Ultima { get; set; }
        public int NumNegocio { get; set; }
        public double Volume { get; set; }
        public Decimal AgressorCompra { get; set; }
        public Decimal AgressorVenda { get; set; }
        public int NotaForca { get; set; }
        public int AcoesInfoId { get; set; }
        public DateTime? DataPregao { get; set; } = DateTime.Now.Date;
        public AcoesInfo AcoesInfo { get; set; }
        public IEnumerable<FluxoBolsa> FluxoBolsas { get; set; }

    }


}
