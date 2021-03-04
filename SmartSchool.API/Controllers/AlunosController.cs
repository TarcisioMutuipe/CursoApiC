using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Data;
using SmartSchool.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartSchool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunosController : ControllerBase
    {
        private readonly SmartContext _context;
        public AlunosController(SmartContext context)
        {
            _context = context;
        }

        // GET: api/<AlunosController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Alunos);
        }

        // GET api/<AlunosController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var aluno = _context.Alunos.FirstOrDefault(x => x.Id == id);
            if (aluno == null)
            {
                return BadRequest("O Aluno não foi Encontrado");
            }
            return Ok(aluno);
        }

        // GET api/<AlunosController>/5
        [HttpGet("byName")]
        public IActionResult Get(string name)
        {
            var aluno = _context.Alunos.FirstOrDefault(x => x.Nome == name);
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
            _context.Add(aluno);
            _context.SaveChanges();
            return Ok(aluno);
        }

        // PUT api/<AlunosController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Aluno aluno)
        {
            var alu = _context.Alunos.AsNoTracking().FirstOrDefault(a => a.Id == id);
            if (alu == null) return BadRequest("Aluno não encontrado.");
            _context.Update(aluno);
            _context.SaveChanges();
            return Ok(aluno);
        }
        // Patch api/<AlunosController>/5
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Aluno aluno)
        {
            var alu = _context.Alunos.AsNoTracking().FirstOrDefault(a => a.Id == id);
            if (alu == null) return BadRequest("Aluno não encontrado.");
            _context.Update(aluno);
            _context.SaveChanges();
            return Ok(aluno);
        }
        // DELETE api/<AlunosController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var aluno = _context.Alunos.FirstOrDefault(a => a.Id == id);
            if(aluno == null) return BadRequest("Aluno não encontrado.");
            _context.Remove(aluno);
            _context.SaveChanges();
            return Ok();
        }
    }
}
