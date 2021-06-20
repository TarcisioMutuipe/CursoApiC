using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Data;
using SmartSchool.API.Dtos;
using SmartSchool.API.Helpers;
using SmartSchool.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartSchool.API.Controllers
{
    /// <summary>
    /// Classe Aluno
    /// </summary>
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

        /// <summary>
        /// Método responsável por retornar todos alunos
        /// </summary>
        /// <returns></returns>
        // GET: api/<AlunosController>
       
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
        {
          
            var alunos = await _repo.GetAllAlunosAsync(pageParams, true);

            var alunosResult = _mapper.Map<IEnumerable<AlunoDto>>(alunos);

            Response.AddPagination(alunos.CurrentPage, alunos.PageSize, alunos.TotalCount, alunos.TotalPages);

            return Ok(alunosResult);
        }

        [HttpGet("getRegister")]
        public IActionResult GetRegister(int id)
        {
            return Ok(new AlunoRegistrarDto());
        }


        [HttpGet("ByDisciplina/{id}")]
        public IActionResult ByDisciplina(int id)
        {
            var aluno = _repo.ByDisciplina(id);
            var alunoDto = _mapper.Map<IEnumerable<AlunoDto>>(aluno);
            if (aluno == null)
            {
                return BadRequest("O Aluno não foi Encontrado");
            }
            return Ok(alunoDto);
        }
        /// <summary>
        /// Método para retornar um aluno
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<AlunosController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var aluno = _repo.GetAlunoById(id, false);
            var alunoDto = _mapper.Map<AlunoRegistrarDto>(aluno);
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
        //// Patch api/<AlunosController>/5
        //[HttpPatch("{id}")]
        //public IActionResult Patch(int id, AlunoRegistrarDto alunoD)
        //{
        //    var alu = _repo.GetAlunoById(id);
        //    if (alu == null) return BadRequest("Aluno não encontrado.");
        //    _mapper.Map(alunoD, alu);

        //    _repo.Update(alu);
        //    if (_repo.SaveChanges())
        //    {
        //        return Created($"/api/aluno/{alunoD.Id}", _mapper.Map<AlunoDto>(alu));
        //    }
        //    return BadRequest("Aluno não cadastrado.");
        //}

        // Api/Aluno/{id}/trocaestado
        [HttpPatch("{id}/trocarEstado")]
        public IActionResult trocarEstado(int id, TrocaEstadoDto trocaEstado)
        {
            var alu = _repo.GetAlunoById(id);           
            if (alu == null) return BadRequest("Aluno não encontrado.");

            alu.Ativo = trocaEstado.Estado;
            _repo.Update(alu);
            if (_repo.SaveChanges())
            {
                var msn = alu.Ativo ? "ativado" : "desativado";
                return Ok(new {message = $"Aluno {msn} com sucesso!" });
            }
            return BadRequest("Aluno não atualizado.");
        }

        // Api/Aluno/{id}
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, AlunoPatchDto aluno)
        {
            var alu = _repo.GetAlunoById(id);
            if (alu == null) return BadRequest("Aluno não encontrado.");
            _mapper.Map(aluno, alu);

            _repo.Update(alu);
            if (_repo.SaveChanges())
            {
                return Created($"/api/aluno/{aluno.Id}", _mapper.Map<AlunoPatchDto>(alu));
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
