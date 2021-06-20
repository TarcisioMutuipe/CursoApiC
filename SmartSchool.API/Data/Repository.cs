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
    public class Repository: IRepository
    {
        private readonly SmartContext _context;

        public IConfiguration Configuration { get; }
        public Repository(SmartContext context, IConfiguration configuration)
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

        public async Task<PageList<Aluno>> GetAllAlunosAsync(PageParams pageParams, bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if (includeProfessor)
            {
                query = query.Include(a => a.AlunosDisciplinas)
                             .ThenInclude(ad => ad.Disciplina)
                             .ThenInclude(d => d.Professor);
            }

            query = query.AsNoTracking().OrderBy(a => a.Id);

            if (!string.IsNullOrEmpty(pageParams.Nome))
                query = query.Where(aluno => aluno.Nome
                                                  .ToUpper()
                                                  .Contains(pageParams.Nome.ToUpper()) ||
                                             aluno.Sobrenome
                                                  .ToUpper()
                                                  .Contains(pageParams.Nome.ToUpper()));

            if (pageParams.Matricula > 0)
                query = query.Where(aluno => aluno.Matricula == pageParams.Matricula);

            if (pageParams.Ativo != null)
                query = query.Where(aluno => aluno.Ativo == (pageParams.Ativo != 0));

            // return await query.ToListAsync();
            return await PageList<Aluno>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }
        public Aluno[] GetAllAlunos(bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if (includeProfessor)
            {
                query = query.Include(a => a.AlunosDisciplinas)
                    .ThenInclude(ad => ad.Disciplina)
                    .ThenInclude(d => d.Professor);
            }

            query = query.AsNoTracking().OrderBy(a => a.Id);
            return  query.ToArray();
        }

        public String[] GetAllAcoesSigla()
        {
            
            IQueryable<AcoesInfo> query = _context.AcoesInfo;

            var querys = query.AsNoTracking().OrderBy(a => a.Id).Select(x => x.Sigla).Distinct();
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
                       @"SELECT Descricao,Sigla,SUM(ff.volume), 
                            Round((SUM(ff.volume)/(select sum(volume) from acoes abc where abc.datapregao = acoes.datapregao
                            and abc.AcoesInfoId = acoes.AcoesInfoId)),2) as Taxa,
                            (select sum(volume) from acoes abc where datapregao = acoes.datapregao
                            and abc.AcoesInfoId = acoes.AcoesInfoId)as Volume
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
                       @"SELECT Descricao,Sigla,'1-Principais' as Quem,SUM(ff.volume) as volumeTotal,acoes.datapregao
                             FROM fluxobolsa ff inner join acoesInfo ac 
                            on (ff.AcoesInfoId = ac.Id )
                            inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
                            where codCorretora in('040', '016', '238', '072', '045', '013', '008') 
                            and data BETWEEN @dataIni and @dataFim
                            and sigla = @Sigla
                            group by  Descricao,acoes.datapregao                           
							Union
SELECT Descricao,Sigla,'3-Morgan' as Quem,SUM(ff.volume) as volumeTotal,acoes.datapregao
                             FROM fluxobolsa ff inner join acoesInfo ac 
                            on (ff.AcoesInfoId = ac.Id )
                            inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
                            where codCorretora in('040') 
                            and data BETWEEN @dataIni and @dataFim
                            and sigla = @Sigla
                            group by  Descricao,acoes.datapregao                         
                            Union
SELECT Descricao,Sigla,'2-JP Morgan' as Quem,SUM(ff.volume) as volumeTotal,acoes.datapregao
                             FROM fluxobolsa ff inner join acoesInfo ac 
                            on (ff.AcoesInfoId = ac.Id )
                            inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
                            where codCorretora in('016') 
                            and data BETWEEN @dataIni and @dataFim
                            and sigla = @Sigla
                            group by  Descricao,acoes.datapregao                            
                            union
SELECT Descricao,Sigla,'4-Merrill Lynch' as Quem,SUM(ff.volume) as volumeTotal,acoes.datapregao
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
        public int GetIdAcao(string acao)
        {

            IQueryable<AcoesInfo> query = _context.AcoesInfo;

            var IdAcao = query.AsNoTracking().Where(x => x.Sigla.Equals(acao)).Select(a=>a.Id).FirstOrDefault();
            return IdAcao;
        }
        public Aluno[] GetAllAlunosByDisciplinaId(int disciplinaId, bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if (includeProfessor)
            {
                query = query.Include(a => a.AlunosDisciplinas)
                    .ThenInclude(ad => ad.Disciplina)
                    .ThenInclude(d => d.Professor);
            }

            query = query.AsNoTracking().OrderBy(a => a.Id)
                .Where(aluno => aluno.AlunosDisciplinas.Any(ad => ad.DisciplinaId == disciplinaId));
            return query.ToArray();
        }

        public Aluno GetAlunoById(int alunoId, bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if (includeProfessor)
            {
                query = query.Include(a => a.AlunosDisciplinas)
                    .ThenInclude(ad => ad.Disciplina)
                    .ThenInclude(d => d.Professor);
            }

            query = query.AsNoTracking().OrderBy(a => a.Id).Where(aluno => aluno.Id == alunoId);
            return query.FirstOrDefault();
        }

        public Aluno[] ByDisciplina(int idDisciplina)
        {
            IQueryable<Aluno> query = _context.Alunos;

                query = query.Include(a => a.AlunosDisciplinas)
                    .ThenInclude(ad => ad.Disciplina)
                  ;


            query = query.AsNoTracking().OrderBy(a => a.Id).Where(aluno => aluno.AlunosDisciplinas.Any(p => p.DisciplinaId == idDisciplina));
            return query.ToArray();
        }
        public Professor[] GetAllProfessores(bool incluirAlunos = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if (incluirAlunos)
            {
                query = query.Include(x => x.Disciplinas)
                    .ThenInclude(y => y.AlunosDisciplinas)
                    .ThenInclude(u => u.Aluno);
            }
            query = query.AsNoTracking().OrderBy(x => x.Nome);

            return query.ToArray();
        }

        public Professor[] GetAllProfessoresByDisciplinaId(int disciplinaId, bool incluirAlunos = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if (incluirAlunos)
            {
                query = query.Include(x => x.Disciplinas)
                    .ThenInclude(y => y.AlunosDisciplinas)
                    .ThenInclude(u => u.Aluno);
            }
            // query.AsNoTracking().OrderBy(x => x.Nome).Where(o=>o.Disciplina.Any(p=>p.Id == disciplinaId));
            query = query.AsNoTracking().OrderBy(x => x.Nome).Where(aluno => aluno.Disciplinas
            .Any(o => o.AlunosDisciplinas.Any(p => p.DisciplinaId == disciplinaId)
            ));
            return query.ToArray();
        }

        public Professor GetProfessorById(int id, bool incluirAlunos = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if (incluirAlunos)
            {
                query = query.Include(x => x.Disciplinas)
                    .ThenInclude(y => y.AlunosDisciplinas)
                    .ThenInclude(u => u.Aluno);
            }       

            query = query.AsNoTracking().OrderBy(x => x.Id).Where(prof => prof.Id == id);

            return query.FirstOrDefault();
        }

         public Professor[] GetProfessorByAlunoId (int Alunoid, bool incluirAlunos = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if (incluirAlunos)
            {
                query = query.Include(x => x.Disciplinas)
                    .ThenInclude(y => y.AlunosDisciplinas)
                    .ThenInclude(u => u.Aluno);
            }       

            query = query.AsNoTracking().OrderBy(x => x.Id).Where(aluno => aluno.Disciplinas
            .Any(o => o.AlunosDisciplinas.Any(p => p.AlunoId == Alunoid)
            ));

            return query.ToArray();
        }


        public void GravarFluxo()
        {
            throw new NotImplementedException();
        }
    }
}
