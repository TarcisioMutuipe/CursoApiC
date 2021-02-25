using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartSchool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunosController : ControllerBase
    {
        public List<Aluno> Alunos = new List<Aluno>()
        {
            new Aluno()
            {
                Id = 1,
                Nome ="Marcos",
                SobreNome="Silva",
                Telefone = "71988612407"
            },
        new Aluno()
        {
            Id = 2,
                Nome = "Lana",
                 SobreNome="Moura",
                Telefone = "71988612407"
            },
        new Aluno()
        {
            Id = 3,
                Nome = "João",
                 SobreNome="Cerol",
                Telefone = "71988612407"

            },
        };
        // GET: api/<AlunosController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Alunos);
        }

        // GET api/<AlunosController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var aluno = Alunos.FirstOrDefault(x => x.Id == id);
            if (aluno == null)
            {
                return BadRequest("O Aluno não foi Encontrado");
            }
            return Ok(aluno);
        }

        // GET api/<AlunosController>/5
        [HttpGet("byName")]
        public IActionResult Get(string name,string sobrenome)
        {
            var aluno = Alunos.FirstOrDefault(x => x.Nome == name);
            if (aluno == null)
            {
                return BadRequest("O Aluno não foi Encontrado");
            }
            return Ok(aluno);
        }

        // POST api/<AlunosController>
        [HttpPost]
        public IActionResult Post(Aluno aluno)
        {
            return Ok(aluno);
        }

        // PUT api/<AlunosController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Aluno aluno)
        {
            return Ok(aluno);
        }
        // Patch api/<AlunosController>/5
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Aluno aluno)
        {
            return Ok(aluno);
        }
        // DELETE api/<AlunosController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
