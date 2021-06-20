using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.Dtos
{
    public class RetornoFluxoDto
    {

        public string Descricao { get; set; }
        public string Sigla { get; set; }
        public double VolumeCorretora { get; set; }
        public decimal TaxaFluxo { get; set; }
        public double VolumeTotal { get; set; }
        public decimal VarD { get; set; }
        public string Quem { get; set; }
        public DateTime? DataPregao { get; set; }

    }
}
