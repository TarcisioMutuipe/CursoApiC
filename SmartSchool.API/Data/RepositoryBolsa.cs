using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SmartSchool.API.Dtos;
using SmartSchool.API.Helpers;
using SmartSchool.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SmartSchool.API.Data
{
    public class RepositoryBolsa: IRepositoryBolsa
    {

        private readonly SmartContext _context;

        public IConfiguration Configuration { get; }
        public RepositoryBolsa(SmartContext context, IConfiguration configuration)
        {
            Configuration = configuration;
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }
        public String[] GetAllAcoesSigla()
        {

            IQueryable<AcoesInfo> query = _context.AcoesInfo;

            var querys = query.AsNoTracking().OrderBy(a => a.Id).Select(x => x.Sigla).Distinct();
            return querys.ToArray();
        }

        public String[] GetAllCorretoras()
        {

            IQueryable<FluxoBolsa> query = _context.FluxoBolsa;

            var querys = query.AsNoTracking().OrderBy(a => a.NomeCorretora).Select(x => x.NomeCorretora).Distinct();
            return querys.ToArray();
        }

        public DataTable GetFluxoDias(DateTime dataInicio, DateTime dataFim, string Sigla)
        {
            var mConn = new MySql.Data.MySqlClient.MySqlConnection(Configuration.GetConnectionString("MySqlConnection"));
            try
            {
                //abre a conexao
                mConn.Open();
            }
            catch (System.Exception e)
            {
                return null;
            }
            if (mConn.State == ConnectionState.Open)
            {
                var mAdapter = new MySqlDataAdapter(
                 String.Format("set @dataIni = '{0} ' ;", dataInicio.ToString("yyyy-MM-dd")) +
                  String.Format("set @dataFim = '{0}' ;", dataFim.ToString("yyyy-MM-dd")) +
                   String.Format("set @Sigla = '{0}' ;", Sigla) +
                       @"SELECT Descricao,Sigla,SUM(ff.volume)*-1, 
                            Round((SUM(ff.volume)/(select sum(volume) from acoes abc where abc.datapregao = acoes.datapregao
                            and abc.AcoesInfoId = acoes.AcoesInfoId)),2)*-1 as Taxa,
                            (select sum(volume) from acoes abc where datapregao = acoes.datapregao
                            and abc.AcoesInfoId = acoes.AcoesInfoId) as Volume
                            ,acoes.datapregao
                             FROM fluxobolsa ff inner join acoesInfo ac 
                            on (ff.AcoesInfoId = ac.Id )
                            inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
                            where codCorretora in('040', '016', '238', '072', '045', '013', '008') 
                            and data BETWEEN @dataIni and @dataFim
                            and sigla = @Sigla
                            group by  Descricao,acoes.datapregao
                            
                            order by SIGLA,DataPregao ASC,Taxa desc", mConn);

                var datax = new DataSet();
                RetornoFluxoDto retornof = new RetornoFluxoDto();
                mAdapter.Fill(datax, "Fluxo");
                return datax.Tables[0];
            }
            else return null;
        }

        public DataTable GetFluxoCorretoras(DateTime dataInicio, DateTime dataFim, string Sigla)
        {
            var mConn = new MySql.Data.MySqlClient.MySqlConnection(Configuration.GetConnectionString("MySqlConnection"));
            try
            {
                //abre a conexao
                mConn.Open();
            }
            catch (System.Exception e)
            {
                return null;
            }
            if (mConn.State == ConnectionState.Open)
            {
                var mAdapter = new MySqlDataAdapter(
                 String.Format("set @dataIni = '{0} ' ;", dataInicio.ToString("yyyy-MM-dd")) +
                  String.Format("set @dataFim = '{0}' ;", dataFim.ToString("yyyy-MM-dd")) +
                   String.Format("set @Sigla = '{0}' ;", Sigla) +
                       @"SELECT Descricao,Sigla,'1-Principais' as Quem,SUM(ff.volume)*-1 as volumeTotal,acoes.datapregao
                             FROM fluxobolsa ff inner join acoesInfo ac 
                            on (ff.AcoesInfoId = ac.Id )
                            inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
                            where codCorretora in('040', '016', '238', '072', '045', '013', '008') 
                            and data BETWEEN @dataIni and @dataFim
                            and sigla = @Sigla
                            group by  Descricao,acoes.datapregao                           
							Union
SELECT Descricao,Sigla,'3-Morgan' as Quem,SUM(ff.volume)*-1 as volumeTotal,acoes.datapregao
                             FROM fluxobolsa ff inner join acoesInfo ac 
                            on (ff.AcoesInfoId = ac.Id )
                            inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
                            where codCorretora in('040') 
                            and data BETWEEN @dataIni and @dataFim
                            and sigla = @Sigla
                            group by  Descricao,acoes.datapregao                         
                            Union
SELECT Descricao,Sigla,'2-JP Morgan' as Quem,SUM(ff.volume)*-1 as volumeTotal,acoes.datapregao
                             FROM fluxobolsa ff inner join acoesInfo ac 
                            on (ff.AcoesInfoId = ac.Id )
                            inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
                            where codCorretora in('016') 
                            and data BETWEEN @dataIni and @dataFim
                            and sigla = @Sigla
                            group by  Descricao,acoes.datapregao                            
                            union
SELECT Descricao,Sigla,'4-Merrill Lynch' as Quem,SUM(ff.volume)*-1 as volumeTotal,acoes.datapregao
                             FROM fluxobolsa ff inner join acoesInfo ac 
                            on (ff.AcoesInfoId = ac.Id )
                            inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
                            where codCorretora in('013') 
                            and data BETWEEN @dataIni and @dataFim
                            and sigla = @Sigla
                            group by  Descricao,acoes.datapregao                            
                            order by DataPregao ASC, Quem", mConn);

                var datax = new DataSet();
                RetornoFluxoDto retornof = new RetornoFluxoDto();
                mAdapter.Fill(datax, "Fluxo");
                return datax.Tables[0];
            }
            else return null;
        }

        public DataTable GetFluxoVolumexVard(DateTime dataInicio, DateTime dataFim, string Sigla)
        {
            var mConn = new MySql.Data.MySqlClient.MySqlConnection(Configuration.GetConnectionString("MySqlConnection"));
            try
            {
                //abre a conexao
                mConn.Open();
            }
            catch (System.Exception e)
            {
                return null;
            }
            if (mConn.State == ConnectionState.Open)
            {
                var mAdapter = new MySqlDataAdapter(
                 String.Format("set @dataIni = '{0} ' ;", dataInicio.ToString("yyyy-MM-dd")) +
                  String.Format("set @dataFim = '{0}' ;", dataFim.ToString("yyyy-MM-dd")) +
                   String.Format("set @Sigla = '{0}' ;", Sigla) +
                       @"SELECT Sigla,codCorretora,NomeCorretora , SUM(case when (ff.volume < 0 and vard > 0) or (ff.volume > 0 and vard < 0) then 1 else 0 end) as ContadorAcerto, Count(data) as TOtalDias,
ROUND(( SUM(case when (ff.volume < 0 and vard > 0) or (ff.volume > 0 and vard < 0) then 1 else 0 end) / Count(data)) * 100,2) as Porcental
, ROUND(SUM(ABS(ff.VOLUME))/ Count(data),2)*-1 as VolMedioCorretora
                             FROM fluxobolsa ff inner join acoesInfo ac 
                            on (ff.AcoesInfoId = ac.Id )
                            inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
                            where  data BETWEEN @dataIni and @dataFim
                            and sigla = @Sigla
                            group by codCorretora
                            order by Porcental asc, ContadorAcerto asc", mConn);

                var datax = new DataSet();
                RetornoFluxoDto retornof = new RetornoFluxoDto();
                mAdapter.Fill(datax, "Fluxo");
                return datax.Tables[0];
            }
            else return null;
        }

        public IList<RetornoVariasInfoCorretoras> GetFluxoAcertivas(DateTime dataInicio, DateTime dataFim, string corretora)
        {
            var mConn = new MySql.Data.MySqlClient.MySqlConnection(Configuration.GetConnectionString("MySqlConnection"));
            try
            {
                //abre a conexao
                mConn.Open();
            }
            catch (System.Exception e)
            {
                return null;
            }
            if (mConn.State == ConnectionState.Open)
            {
                string msSql =
                   String.Format("set @Sigla = '{0}' ;", "VVAR3") +
                        @" SELECT Sigla,codCorretora,NomeCorretora,
ROUND(( SUM(case when (ff.volume < 0 and vard > 0) or (ff.volume > 0 and vard < 0) then 1 else 0 end) / Count(data)) * 100,2) as Porcental,
SUM(case when (ff.volume < 0 and vard > 0) or (ff.volume > 0 and vard < 0) then 1 else 0 end) as AcertoGeral ,
 ROUND(SUM(ABS(ff.VOLUME))/ Count(data),2) * -1 as VolMedioCorretora,
ROUND(SUM(ABS(ff.VOLUME))/ Count(data)/(sum(acoes.volume)/Count(data)),3) as Porcentagem,
SUM(case when (ff.volume < 0 and vard > 0) then 1 else 0 end) as AcertoAlta ,
SUM(case when (ff.volume > 0 and vard < 0) then 1 else 0 end) as AcertoBaixa
                             FROM fluxobolsa ff inner join acoesInfo ac 
                            on (ff.AcoesInfoId = ac.Id )
                            inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
                            where  data BETWEEN @dataIni and @dataFim ";

                if (corretora != "")
                {
                    msSql += " and NomeCorretora = @corretora ";
                }
                msSql += @" group by codCorretora,Sigla
							having ((SUM(case when (ff.volume < 0 and vard > 0) or (ff.volume > 0 and vard < 0) then 1 else 0 end) / Count(data)) * 100) > 50
                            and (Count(data)/(Select count(datapregao) from acoes aco inner join acoesInfo ac 
                            on (aco.AcoesInfoId = ac.Id ) where datapregao BETWEEN @dataIni and @dataFim and @Sigla = Sigla)) = 1
                          and ROUND(SUM(ABS(ff.VOLUME))/ Count(data)/(sum(acoes.volume)/Count(data)),3) > 0.01
                            order by Porcental desc ";

                var mAdapter = new MySqlCommand(msSql, mConn);
                mAdapter.Parameters.AddWithValue("@dataIni", dataInicio.ToString("yyyy-MM-dd"));
                mAdapter.Parameters.AddWithValue("@dataFim", dataFim.ToString("yyyy-MM-dd"));
                mAdapter.Parameters.AddWithValue("@corretora", corretora);
                var datax = new DataSet();
                RetornoFluxoDto retornof = new RetornoFluxoDto();
                mAdapter.CommandTimeout = 200;
                MySqlDataReader dr = mAdapter.ExecuteReader(CommandBehavior.CloseConnection);

                IList<RetornoVariasInfoCorretoras> FluxoRetorno = new List<RetornoVariasInfoCorretoras>();

                while (dr.Read())
                {
                    RetornoVariasInfoCorretoras itemFluxo = new RetornoVariasInfoCorretoras();
                    itemFluxo.Sigla = dr[0].ToString();
                    itemFluxo.CodCorretora = dr[1].ToString();
                    itemFluxo.NomeCorretora = dr[2].ToString();
                    itemFluxo.ContadorAcerto = Convert.ToInt32(dr[3]);
                    itemFluxo.PercentualAcerto = Convert.ToDecimal(dr[4]);
                    itemFluxo.VolumeCorretora = Convert.ToDouble(dr[5]);
                    itemFluxo.PorcentagemVolume = Convert.ToDouble(dr[6]);
                    itemFluxo.ContadorAcertoPositivo = Convert.ToInt32(dr[7]);
                    itemFluxo.ContadorAcertoNegativo = Convert.ToInt32(dr[8]);
                    FluxoRetorno.Add(itemFluxo);
                }

                return FluxoRetorno;
            }
            else return null;
        }


        public IList<RetornoVariasInfoCorretoras> GetDiasComprasdos(DateTime dataInicio, DateTime dataFim, string sigla)
        {
            var mConn = new MySql.Data.MySqlClient.MySqlConnection(Configuration.GetConnectionString("MySqlConnection"));
            try
            {
                //abre a conexao
                mConn.Open();
            }
            catch (System.Exception e)
            {
                return null;
            }
            if (mConn.State == ConnectionState.Open)
            {
                string msSql =                 
                        @" SELECT Sigla,codCorretora,NomeCorretora,
                        SUM(case when (ff.volume < 0 ) then 1 else 0 end) as DiasdeCompras ,
                        SUM(case when (ff.volume > 0 ) then 1 else 0 end) as DiasdeVenda,
                        SUM(ff.volume) * -1 as Volume
                             FROM fluxobolsa ff inner join acoesInfo ac 
                            on (ff.AcoesInfoId = ac.Id )
                            inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
                            where  data BETWEEN @dataIni and @dataFim ";
                    if (!string.IsNullOrEmpty(sigla)) 
                    {
                        msSql = msSql + " and sigla = @Sigla";
                    }
                msSql = msSql + @" and codCorretora in('016','040','013','008','045','238','003','114','1618','127','085')  
                                   group by codCorretora,Sigla,NomeCorretora             
                                   order by DiasdeCompras desc, Volume desc";

                var mAdapter = new MySqlCommand(msSql, mConn);
                mAdapter.Parameters.AddWithValue("@dataIni", dataInicio.ToString("yyyy-MM-dd"));
                mAdapter.Parameters.AddWithValue("@dataFim", dataFim.ToString("yyyy-MM-dd"));
                mAdapter.Parameters.AddWithValue("@Sigla", sigla);
                var datax = new DataSet();
                RetornoFluxoDto retornof = new RetornoFluxoDto();
                mAdapter.CommandTimeout = 200;
                MySqlDataReader dr = mAdapter.ExecuteReader(CommandBehavior.CloseConnection);

                IList<RetornoVariasInfoCorretoras> FluxoRetorno = new List<RetornoVariasInfoCorretoras>();

                while (dr.Read())
                {
                    RetornoVariasInfoCorretoras itemFluxo = new RetornoVariasInfoCorretoras();
                    itemFluxo.Sigla = dr[0].ToString();
                    itemFluxo.CodCorretora = dr[1].ToString();
                    itemFluxo.NomeCorretora = dr[2].ToString();                   
                    itemFluxo.ContadorAcertoPositivo = Convert.ToInt32(dr[3]);
                    itemFluxo.ContadorAcertoNegativo = Convert.ToInt32(dr[4]);
                    itemFluxo.VolumeCorretora = Convert.ToDouble(dr[5]);
                    FluxoRetorno.Add(itemFluxo);
                }
                IQueryable<Acoes> query = _context.Acoes;
                var querys = query.AsNoTracking().Where(y=>y.DataPregao >= dataInicio && y.DataPregao <= dataFim).Select(x => x.DataPregao).Distinct();
                var contadorDias = querys.Count();

                var DiasPregao =
                FluxoRetorno = FluxoRetorno.Where(x => (x.ContadorAcertoPositivo + x.ContadorAcertoNegativo) >= contadorDias-2).ToList();
                return FluxoRetorno;
            }
            else return null;
        }

        public int GetIdAcao(string acao)
        {

            IQueryable<AcoesInfo> query = _context.AcoesInfo;

            var IdAcao = query.AsNoTracking().Where(x => x.Sigla.Equals(acao)).Select(a => a.Id).FirstOrDefault();
            return IdAcao;
        }


        public void GravarFluxo()
        {
            throw new NotImplementedException();
        }

        public DataTable GetComparatodas(DateTime dataInicio, DateTime dataFim)
        {
            var mConn = new MySql.Data.MySqlClient.MySqlConnection(Configuration.GetConnectionString("MySqlConnection"));
            try
            {
                //abre a conexao
                mConn.Open();
            }
            catch (System.Exception e)
            {
                return null;
            }
            if (mConn.State == ConnectionState.Open)
            {
                var mAdapter = new MySqlDataAdapter(
                 String.Format("set @dataIni = '{0} ' ;", dataInicio.ToString("yyyy-MM-dd")) +
                  String.Format("set @dataFim = '{0}' ;", dataFim.ToString("yyyy-MM-dd")) +
                       @"
                        set @dataIni = '2021-07-02';
                        set @dataFim = '2021-07-02';
                        SELECT Descricao,Sigla,'1-Principais' as Quem, SUM(ff.volume)*-1
                             FROM fluxobolsa ff inner join acoesInfo ac 
                            on (ff.AcoesInfoId = ac.Id )
                            inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
                            where codCorretora in('040', '016', '238', '072', '045', '013', '008') 
                            and data BETWEEN @dataIni and @dataFim                         
                            group by  Descricao              
							Union
                        SELECT Descricao,Sigla,'2-Morgan&JPMorgan' as Quem,SUM(ff.volume)*-1
                             FROM fluxobolsa ff inner join acoesInfo ac 
                            on (ff.AcoesInfoId = ac.Id )
                            inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
                            where codCorretora in('040','016') 
                            and data BETWEEN @dataIni and @dataFim                  
                            group by  Descricao       
                            order by sigla,Quem", mConn);

                var datax = new DataSet();
                RetornoFluxoDto retornof = new RetornoFluxoDto();
                mAdapter.Fill(datax, "Fluxo");
                return datax.Tables[0];
            }
            else return null;
        }
    }
}
