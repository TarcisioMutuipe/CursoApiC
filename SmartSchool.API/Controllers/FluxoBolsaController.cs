using System;
using System.Collections.Generic;
using System.Data;
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
using SmartSchool.API.Dtos;
using SmartSchool.API.Helpers;
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

        [HttpGet("BuscaFluxoDias")]
        public IActionResult BuscaFluxoDias(DateTime DataIni, DateTime DataFim,string Sigla)
        {
            var Fluxo = _repo.GetFluxoDias(DataIni, DataFim, Sigla);
            if (Fluxo == null) return BadRequest("Professor não existe.");

            IList<RetornoFluxoDto> FluxoRetorno = new List<RetornoFluxoDto>();


                foreach (DataRow item in Fluxo.Rows)
                 {
                    RetornoFluxoDto itemFluxo = new RetornoFluxoDto();
                    itemFluxo.Descricao = item[0].ToString();
                    itemFluxo.Sigla = item[1].ToString();
                    itemFluxo.VolumeCorretora = Convert.ToDouble(item[2]);
                itemFluxo.DataPregao = Convert.ToDateTime(item["datapregao"]);
                decimal number;
                    if(decimal.TryParse((item[3].ToString()), out number))
                    itemFluxo.TaxaFluxo = Convert.ToDecimal(item[3]);
                 if (decimal.TryParse((item[4].ToString()), out number))
                    itemFluxo.VolumeTotal = Convert.ToDouble(item[4]);
                    FluxoRetorno.Add(itemFluxo);
                }
            return Ok(_mapper.Map<IEnumerable<RetornoFluxoDto>>(FluxoRetorno));
        }

        [HttpGet("BuscaFluxoCorretoras")]
        public IActionResult BuscaFluxoCorretoras(DateTime DataIni, DateTime DataFim, string Sigla)
        {
            var Fluxo = _repo.GetFluxoCorretoras(DataIni, DataFim, Sigla);
            if (Fluxo == null) return BadRequest("Não foi encontrado.");

            IList<RetornoFluxoDto> FluxoRetorno = new List<RetornoFluxoDto>();


            foreach (DataRow item in Fluxo.Rows)
            {
                RetornoFluxoDto itemFluxo = new RetornoFluxoDto();
                itemFluxo.Descricao = item[0].ToString();
                itemFluxo.Sigla = item[1].ToString();
                itemFluxo.Quem = item[2].ToString();
                itemFluxo.VolumeCorretora = Convert.ToDouble(item[3]);
                itemFluxo.DataPregao = Convert.ToDateTime(item["datapregao"]);               
                FluxoRetorno.Add(itemFluxo);
            }
            return Ok(_mapper.Map<IEnumerable<RetornoFluxoDto>>(FluxoRetorno));
        }

        [HttpGet("GetFluxoVolumexVard")]
        public IActionResult GetFluxoVolumexVard(DateTime DataIni, DateTime DataFim, string Sigla)
        {
            var Fluxo = _repo.GetFluxoVolumexVard(DataIni, DataFim, Sigla);
            if (Fluxo == null) return BadRequest("não existe.");

            IList<RetornoTotalDiasVard> FluxoRetorno = new List<RetornoTotalDiasVard>();

            foreach (DataRow item in Fluxo.Rows)
            {
                RetornoTotalDiasVard itemFluxo = new RetornoTotalDiasVard();
                itemFluxo.Sigla = item[0].ToString();
                itemFluxo.CodCorretora = item[1].ToString();                
                itemFluxo.NomeCorretora = item[2].ToString();
                itemFluxo.ContadorAcerto = Convert.ToInt32(item[3]);
                itemFluxo.PercentualAcerto = Convert.ToDecimal(item[4]);
                itemFluxo.VolumeCorretora = Convert.ToDouble(item[5]);
                FluxoRetorno.Add(itemFluxo);
            }
            return Ok(_mapper.Map<IEnumerable<RetornoTotalDiasVard>>(FluxoRetorno));

        }

        [HttpGet("GetFluxoAcertivas")]
        public IActionResult GetFluxoAcertivas(DateTime DataIni, DateTime DataFim,string corretora, [FromQuery] PageParams pageParams)
        {
            var Fluxo = _repo.GetFluxoAcertivas(DataIni, DataFim, corretora);
            if (Fluxo == null) return BadRequest("não existe.");           

            var items = Fluxo.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                    .Take(pageParams.PageSize);                                    
                
            Response.AddPagination(pageParams.PageNumber, pageParams.PageSize, Fluxo.Count, (int)Math.Ceiling(Fluxo.Count / (double)pageParams.PageSize));

            return Ok(_mapper.Map<IEnumerable<RetornoVariasInfoCorretoras>>(Fluxo));

        }


        [HttpGet("BuscaListaAcoes")]
        public IActionResult BuscaListaAcoes()
        {
            var acoes = _repo.GetAllAcoesSigla();
            return Ok(acoes);
        }

        [HttpGet("BuscaListaCorretoras")]
        public IActionResult BuscaListaCorretoras()
        {
            var corretoras = _repo.GetAllCorretoras();
            return Ok(corretoras);
        }

        // GET: api/<FluxoBolsaController>
        [HttpGet]
        public IActionResult Get()
        {

            var acoes = _repo.GetAllAcoesSigla();

            foreach (var item in acoes)
            {

                var pathArquivo = @"C:\Projetos\CompradoreseVendedores\05052021\" + item + ".csv";
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
                            linha = linha.Replace("\"", " ").Replace("\"", "").Replace(".", "").Trim('"');
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
                               // Data = Convert.ToDateTime("21/05/2021"),
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
