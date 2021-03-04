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
    public class ProfessorController : ControllerBase
    {
        private readonly SmartContext _context;

        public ProfessorController(SmartContext context)
        {
            _context = context;
        }

        // GET: api/<ProfessorController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Professores);
        }

        // GET api/<ProfessorController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var prof = _context.Professores.FirstOrDefault(x => x.Id == id);
            if (prof == null) return BadRequest("Professor não existe.");
            return Ok(prof);
        }

        [HttpGet("ByName")]
        public IActionResult GetByName(string nome)
        {
            var prof = _context.Professores.FirstOrDefault(x => x.Nome.Contains(nome));

            if (prof == null) return BadRequest("Professor não Cadastrado.");
            return Ok(prof);
        }

        // POST api/<ProfessorController>
        [HttpPost]
        public IActionResult Post(Professor professor)
        {
            var prof = _context.Professores.AsNoTracking().FirstOrDefault(x => x.Nome == professor.Nome);
            if (prof != null) return BadRequest("Professor já existe.");
            _context.Add(professor);
            _context.SaveChanges();
            return Ok(professor);
        }

        // PUT api/<ProfessorController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id,Professor professor)
        {
            var prof = _context.Professores.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (prof == null) return BadRequest("Professor não existe.");
            _context.Update(professor);
            _context.SaveChanges();
            return Ok(professor);
        }

        [HttpPut("{id}")]
        public IActionResult Patch(int id, Professor professor)
        {
            var prof = _context.Professores.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (prof == null) return BadRequest("Professor não existe.");
            _context.Update(professor);
            _context.SaveChanges();
            return Ok(professor);
        }


        // DELETE api/<ProfessorController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _context.Remove(id);
            _context.SaveChanges();
            return Ok();
        }
    }
}
