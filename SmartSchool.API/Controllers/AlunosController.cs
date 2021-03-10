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
    public class AlunosController : ControllerBase
    {


        public readonly IRepository _repo;

        public readonly IMapper _mapper;
        public AlunosController( IRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }


        // GET: api/<AlunosController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = _repo.GetAllAlunos(true );
                      
            return Ok(_mapper.Map<IEnumerable<AlunoDto>>(result));
        }

        [HttpGet("getRegister")]
        public IActionResult GetRegister(int id)
        {
            return Ok(new AlunoRegistrarDto());
        }

        // GET api/<AlunosController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var aluno = _repo.GetAlunoById(id, false);
            var alunoDto = _mapper.Map<AlunoDto>(aluno);
            if (aluno == null)
            {
                return BadRequest("O Aluno não foi Encontrado");
            }
            return Ok(alunoDto);
        }

        // POST api/<AlunosController>
        [HttpPost]
        public IActionResult Post(AlunoRegistrarDto aluno)
        {
            var alunoModel = _mapper.Map<Aluno>(aluno);
            _repo.Add(alunoModel);
            if (_repo.SaveChanges())
            {
                return Created($"/api/aluno/{aluno.Id}", _mapper.Map<AlunoDto>(alunoModel));
            }
            return BadRequest("Aluno não cadastrado.");
        }

        // PUT api/<AlunosController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, AlunoRegistrarDto alunoD)
        {
            var alu = _repo.GetAlunoById(id);
            if (alu == null) return BadRequest("Aluno não encontrado.");

            _mapper.Map(alunoD, alu);
            _repo.Update(alu);
            if (_repo.SaveChanges())
            {
                return Created($"/api/aluno/{alunoD.Id}", _mapper.Map<AlunoDto>(alu));
            }
            return BadRequest("Aluno não cadastrado.");
        }
        // Patch api/<AlunosController>/5
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, AlunoRegistrarDto alunoD)
        {
            var alu = _repo.GetAlunoById(id);
            if (alu == null) return BadRequest("Aluno não encontrado.");
            _mapper.Map(alunoD, alu);

            _repo.Update(alu);
            if (_repo.SaveChanges())
            {
                return Created($"/api/aluno/{alunoD.Id}", _mapper.Map<AlunoDto>(alu));
            }
            return BadRequest("Aluno não cadastrado.");
        }
        // DELETE api/<AlunosController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var aluno = _repo.GetAlunoById(id);
            if (aluno == null) return BadRequest("Aluno não encontrado.");
            _repo.Delete(aluno);
            if (_repo.SaveChanges())
            {
                return Ok("Aluno Deletado");
            }
            return BadRequest("Aluno não Deletado.");
        }
    }
}
