using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Helpers;
using SmartSchool.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.Data
{
    public class Repository: IRepository
    {
        private readonly SmartContext _context;

        public Repository(SmartContext context)
        {
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
