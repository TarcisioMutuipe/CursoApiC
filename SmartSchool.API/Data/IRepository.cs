using SmartSchool.API.Helpers;
using SmartSchool.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchool.API.Data
{
    public interface IRepository
    {

        void Add<T>(T entity) where T : class;

        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        bool SaveChanges();

        Task<PageList<Aluno>> GetAllAlunosAsync(PageParams pageParams, bool includeProfessor = false);
        Aluno[] GetAllAlunos(bool includeProfessor = false);

        Aluno[] GetAllAlunosByDisciplinaId(int alunoId, bool includeProfessor=false);
        Aluno GetAlunoById(int alunoId, bool includeProfessor = false);

        Professor[] GetAllProfessores(bool incluirAlunos = false);
        Professor[] GetAllProfessoresByDisciplinaId(int id, bool incluirAlunos = false);
        Professor GetProfessorById(int professorid, bool incluirAlunos = false);

        void GravarFluxo();
        String[] GetAllAcoesSigla();

         int GetIdAcao(string acao);

         Professor[] GetProfessorByAlunoId (int Alunoid, bool incluirAlunos = false);

    }
}
