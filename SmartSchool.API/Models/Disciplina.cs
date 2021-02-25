using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.Models
{
    public class Disciplina
    {

        public Disciplina() { }

        public Disciplina(int id, string nome, int professorId)
        {
            this.ProfessorId = professorId;
            this.Nome = nome;
            this.Id = id;
        }
     

        public int Id { get; set; }

        public string Nome { get; set; }

        public int ProfessorId { get; set; }

        public Professor Professor { get; set; }

        public IEnumerable<AlunoDisciplina> AlunosDisciplina { get; set; }

    }
}
