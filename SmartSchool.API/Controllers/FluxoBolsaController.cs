using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Analysis;
using SmartSchool.API.Data;
using SmartSchool.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartSchool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FluxoBolsaController : ControllerBase
    {
        public readonly IRepository _repo;

        public readonly IMapper _mapper;
        public FluxoBolsaController(IRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        // GET: api/<FluxoBolsaController>
        [HttpGet]
        public IActionResult Get()
        {

            var acoes = _repo.GetAllAcoesSigla();

            foreach (var item in acoes)
            {

                var pathArquivo = @"D:\CompradoreseVendedores\03052021\" + item + ".csv";
                if (System.IO.File.Exists(pathArquivo))
                {

                    StreamReader rd = new StreamReader(pathArquivo, Encoding.ASCII);

                  
                    while (!rd.EndOfStream)
                    {
                        var linha = rd.ReadLine();
                        if (!linha.Contains("Corretora") && linha.Length > 10)
                        {
                            linha = linha.Substring(1, linha.Length - 3);
                            linha = linha.Replace("\"", "X");
                            linha = linha.Replace("\"", " ").Replace("\"","").Replace(".","").Trim('"');
                            var partes = linha.Split("X,X");
                            var codNome = partes[1].Split("-");
                            FluxoBolsa fluxo = new FluxoBolsa
                            {
                                CodCorretora = codNome[0],
                                NomeCorretora = codNome[1],
                                Quantidade = Convert.ToInt32(partes[2]),
                                Volume = Convert.ToDouble(partes[5]),
                                NumNegocio = 0,
                                PrecoMedio = Convert.ToDecimal(partes[6]),
                                AcoesInfoId = _repo.GetIdAcao(item),
                                Data = Convert.ToDateTime("03/05/2021"),
                            };

                            var acoesModel = _mapper.Map<FluxoBolsa>(fluxo);
                            _repo.Add(acoesModel);
                            _repo.SaveChanges();
                        }

                       
                    }
                    rd.Dispose();
                    rd.Close();
                    System.IO.File.Delete(pathArquivo);
                }
            }

            return Ok(acoes);
        }
    }
}
