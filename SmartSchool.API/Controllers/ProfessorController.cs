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
       
        private readonly IRepository _repo;

        public ProfessorController( IRepository repo)
        {           
            _repo = repo;
        }

        // GET: api/<ProfessorController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = _repo.GetAllProfessores(true);
            return Ok(result);
        }

        // GET api/<ProfessorController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var prof = _repo.GetProfessoresById(id);
            if (prof == null) return BadRequest("Professor não existe.");
            return Ok(prof);
        }      

        // POST api/<ProfessorController>
        [HttpPost]
        public IActionResult Post(Professor professor)
        {
            _repo.Add(professor);
            if (_repo.SaveChanges())
            {
                return Ok(professor);
            }
            return BadRequest("Professor não cadastrado.");
        }

        // PUT api/<ProfessorController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id,Professor professor)
        {
            var prof = _repo.GetProfessoresById(id);
            if (prof == null) return BadRequest("Professor não existe.");
            _repo.Update(professor);
            if (_repo.SaveChanges())
            {
                return Ok(professor);
            }
            return BadRequest("Professor não Atualizado.");
        }

        [HttpPut("{id}")]
        public IActionResult Patch(int id, Professor professor)
        {
            var prof = _repo.GetProfessoresById(id);
            if (prof == null) return BadRequest("Professor não existe.");
            _repo.Update(professor);
            if (_repo.SaveChanges())
            {
                return Ok(professor);
            }
            return BadRequest("Professor não Atualizado.");
        }


        // DELETE api/<ProfessorController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var professor = _repo.GetProfessoresById(id);
            if (professor == null) BadRequest("Professor Não encontrado.");
            _repo.Delete(professor);

            if (_repo.SaveChanges())
            {
                return Ok();
            }
            return BadRequest("Professor não Deletado.");
        }
    }
}
