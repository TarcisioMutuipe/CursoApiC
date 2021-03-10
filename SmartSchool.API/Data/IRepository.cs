using SmartSchool.API.Models;

namespace SmartSchool.API.Data
{
    public interface IRepository
    {

        void Add<T>(T entity) where T : class;

        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        bool SaveChanges();

        Aluno[] GetAllAlunos(bool includeProfessor = false);
        Aluno[] GetAllAlunosByDisciplinaId(int alunoId, bool includeProfessor=false);
        Aluno GetAlunoById(int alunoId, bool includeProfessor = false);

        Professor[] GetAllProfessores(bool incluirAlunos = false);
        Professor[] GetAllProfessoresByDisciplinaId(int id, bool incluirAlunos = false);
        Professor GetProfessorById(int professorid, bool incluirAlunos = false);
    }
}
