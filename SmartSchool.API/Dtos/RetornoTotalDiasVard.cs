using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.Dtos
{
    public class RetornoTotalDiasVard
    {

        public string Sigla { get; set; }
        public string CodCorretora { get; set; }
        public string NomeCorretora { get; set; }
        public int ContadorAcerto { get; set; }      
        public decimal PercentualAcerto { get; set; }
        public double VolumeCorretora { get; set; }

    }
}
