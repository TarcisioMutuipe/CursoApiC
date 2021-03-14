using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Data;
using SmartSchool.API.Dtos;
using SmartSchool.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartSchool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        public readonly IMapper _mapper;

        private readonly IRepository _repo;

        public ProfessorController( IRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        /// <summary>
        /// Retorna todos Professores.
        /// </summary>
        /// <returns></returns>
        // GET: api/<ProfessorController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = _repo.GetAllProfessores(true);
           
            return Ok(_mapper.Map<IEnumerable<ProfessorDto>>(result));
        }

        /// <summary>
        /// Retorna um Professor específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<ProfessorController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var prof = _repo.GetProfessorById(id);
            if (prof == null) return BadRequest("Professor não existe.");

            var ProfDto = _mapper.Map<ProfessorDto>(prof);
           
            return Ok(ProfDto);
        }      

        // POST api/<ProfessorController>
        [HttpPost]
        public IActionResult Post(ProfessorRegistrarDto professor)
        {
            var profNovo = _mapper.Map<Professor>(professor);

            _repo.Add(profNovo);

            if (_repo.SaveChanges())
            {
                return Created($"/api/professor/{professor.Id}", _mapper.Map<ProfessorDto>(profNovo));
            }
            return BadRequest("Professor não cadastrado.");
        }

        // PUT api/<ProfessorController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id,ProfessorRegistrarDto professor)
        {
            var prof = _repo.GetProfessorById(id);

            if (prof == null) return BadRequest("Professor não existe.");

            _mapper.Map(professor, prof);

            _repo.Update(prof);
            if (_repo.SaveChanges())
            {
                return Created($"/api/professor/{professor.Id}", _mapper.Map<ProfessorDto>(prof));
            }
            return BadRequest("Professor não Atualizado.");
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, ProfessorRegistrarDto professor)
        {
            var prof = _repo.GetProfessorById(id);
            if (prof == null) return BadRequest("Professor não existe.");

            _mapper.Map(professor, prof);

            _repo.Update(prof);
            if (_repo.SaveChanges())
            {
                return Created($"/api/professor/{professor.Id}", _mapper.Map<ProfessorDto>(prof));
            }
            return BadRequest("Professor não Atualizado.");
        }


        // DELETE api/<ProfessorController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var professor = _repo.GetProfessorById(id);
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
