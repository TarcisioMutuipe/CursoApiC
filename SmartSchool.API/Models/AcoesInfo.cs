using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.Models
{
    public class AcoesInfo
    {
        public AcoesInfo() { }
        public AcoesInfo(int id, string sigla, string descricao)
        {
            this.Id = id;
            this.Sigla = sigla;
            this.Descricao = descricao;

        }
        public int Id { get; set; }
        public string Sigla { get; set; }
        public string Descricao { get; set; }

    }


}
