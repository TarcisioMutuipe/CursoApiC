using SmartSchool.API.Models;
using System;
using System.Collections.Generic;

namespace SmartSchool.API.Dtos
{
    public class ProfessorDto
    {
        public int Id { get; set; }
        public int Registro { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
        public DateTime DataIni { get; set; } = DateTime.Now;
        public DateTime? DataFim { get; set; } = null;
        public bool Ativo { get; set; } = true;

        public IEnumerable<DisciplinaDto> Disciplinas { get; set; }

    }
}