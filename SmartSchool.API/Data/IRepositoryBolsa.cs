using SmartSchool.API.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.Data
{
    public interface IRepositoryBolsa: IRepositoryBase
    {
        void GravarFluxo();
        String[] GetAllAcoesSigla();
        int GetIdAcao(string acao);

        DataTable GetFluxoDias(DateTime dataInicio, DateTime dataFim, string Sigla);
        DataTable GetFluxoCorretoras(DateTime dataInicio, DateTime dataFim, string Sigla);
        DataTable GetFluxoVolumexVard(DateTime dataInicio, DateTime dataFim, string Sigla);

        String[] GetAllCorretoras();
        IList<RetornoVariasInfoCorretoras> GetFluxoAcertivas(DateTime dataInicio, DateTime dataFim, string corretora);

        DataTable GetComparatodas(DateTime dataInicio, DateTime dataFim);

        IList<RetornoVariasInfoCorretoras> GetDiasComprasdos(DateTime dataInicio, DateTime dataFim, string sigla);
    }
}
