using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.Dtos
{
    public class FluxoBolsaDto
    {
        public class FluxoBolsa
        {
            public int Id { get; set; }
            public string CodCorretora { get; set; }
            public string NomeCorretora { get; set; }
            public int Quantidade { get; set; }
            public double Volume { get; set; }
            public int NumNegocio { get; set; }
            public decimal PrecoMedio { get; set; }
            public DateTime? Data { get; set; } = DateTime.Now.Date;

        }
    }
}
