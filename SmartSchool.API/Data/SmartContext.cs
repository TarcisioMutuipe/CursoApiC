using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.Data
{
    public class SmartContext: DbContext
    {

        public SmartContext(DbContextOptions<SmartContext> options): base(options)
        {
        }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }

        public DbSet<Professor> Professores { get; set; }

        public DbSet<FluxoBolsa> FluxoBolsa { get; set; }
        public DbSet<Acoes> Acoes { get; set; }
        public DbSet<AcoesInfo> AcoesInfo { get; set; }
        public DbSet<AlunoDisciplina> AlunosDisciplinas { get; set; }

        public DbSet<AlunoCurso> AlunosCursos { get; set; }
       

        public DbSet<Curso> Curso { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AlunoDisciplina>().HasKey(AD => new { AD.AlunoId, AD.DisciplinaId });
            builder.Entity<AlunoCurso>().HasKey(AD => new { AD.AlunoId, AD.CursoId });
          
            builder.Entity<Professor>()
                 .HasData(new List<Professor>(){
                    new Professor(1, 1, "Lauro", "Oliveira"),
                    new Professor(2, 2, "Roberto", "Soares"),
                    new Professor(3, 3, "Ronaldo", "Marconi"),
                    new Professor(4, 4, "Rodrigo", "Carvalho"),
                    new Professor(5, 5, "Alexandre", "Montanha"),
                 });

            builder.Entity<Curso>()
                .HasData(new List<Curso>{
                    new Curso(1, "Tecnologia da Informação"),
                    new Curso(2, "Sistemas de Informação"),
                    new Curso(3, "Ciência da Computação")
                });

            builder.Entity<Disciplina>()
                .HasData(new List<Disciplina>{
                    new Disciplina(1, "Matemática", 1, 1),
                    new Disciplina(2, "Matemática", 1, 3),
                    new Disciplina(3, "Física", 2, 3),
                    new Disciplina(4, "Português", 3, 1),
                    new Disciplina(5, "Inglês", 4, 1),
                    new Disciplina(6, "Inglês", 4, 2),
                    new Disciplina(7, "Inglês", 4, 3),
                    new Disciplina(8, "Programação", 5, 1),
                    new Disciplina(9, "Programação", 5, 2),
                    new Disciplina(10, "Programação", 5, 3)
                });

            builder.Entity<AcoesInfo>()
         .HasData(new List<AcoesInfo>{                 
                    new AcoesInfo(1,"ABEV3","AMBEV S/A ON"),
                    new AcoesInfo(2,"AGRO3","BRASILAGRO ON NM"),
                    new AcoesInfo(3,"ALPA4","ALPARGATAS PN N1"),
                    new AcoesInfo(4,"AMBP3","AMBIPAR ON NM"),
                    new AcoesInfo(5,"ANIM3","ANIMA ON NM"),
                    new AcoesInfo(6,"ARZZ3","AREZZO CO ON NM"),
                    new AcoesInfo(7,"ASAI3","ASSAI ON NM"),
                    new AcoesInfo(8,"AZUL4","AZUL PN N2"),
                    new AcoesInfo(9,"B3SA3","B3 ON NM"),
                    new AcoesInfo(10,"BBAS3","BRASIL ON NM"),
                    new AcoesInfo(11,"BBDC4","BRADESCO PN EB N1"),
                    new AcoesInfo(12,"BBSE3","BBSEGURIDADEON NM"),
                    new AcoesInfo(13,"BEEF3","MINERVA ON ED NM"),
                    new AcoesInfo(14,"BIDI4","BANCO INTER PN N2"),
                    new AcoesInfo(15,"BKBR3","BK BRASIL ON NM"),
                    new AcoesInfo(16,"BOAS3","BOA VISTA ON NM"),
                    new AcoesInfo(17,"BPAC11","BTGP BANCO UNT N2"),
                    new AcoesInfo(18,"BPAN4","BANCO PAN PN N1"),
                    new AcoesInfo(19,"BRAP4","BRADESPAR PN N1"),
                    new AcoesInfo(20,"BRDT3","PETROBRAS BRON EDR NM"),
                    new AcoesInfo(21,"BRFS3","BRF SA ON NM"),
                    new AcoesInfo(22,"BRKM5","BRASKEM PNA N1"),
                    new AcoesInfo(23,"BRML3","BR MALLS PARON NM"),
                    new AcoesInfo(24,"BTOW3","B2W DIGITAL ON NM"),
                    new AcoesInfo(25,"CARD3","CSU CARDSYSTON ED NM"),
                    new AcoesInfo(26,"CASH3","MELIUZ ON NM"),
                    new AcoesInfo(27,"CCRO3","CCR SA ON ED NM"),
                    new AcoesInfo(28,"CEAB3","CEA MODAS ON NM"),
                    new AcoesInfo(29,"CMIG4","CEMIG PN N1"),
                    new AcoesInfo(30,"CMIN3","CSNMINERACAOON N2"),
                    new AcoesInfo(31,"COGN3","COGNA ON ON NM"),
                    new AcoesInfo(32,"CPLE6","COPEL PNB N1"),
                    new AcoesInfo(33,"CRFB3","CARREFOUR BRON ED NM"),
                    new AcoesInfo(34,"CSMG3","COPASA ON NM"),
                    new AcoesInfo(35,"CSNA3","SID NACIONALON"),
                    new AcoesInfo(36,"CVCB3","CVC BRASIL ON NM"),
                    new AcoesInfo(37,"CYRE3","CYRELA REALTON NM"),
                    new AcoesInfo(38,"ECOR3","ECORODOVIAS ON NM"),
                    new AcoesInfo(39,"ELET3","ELETROBRAS ON N1"),
                    new AcoesInfo(40,"EMBR3","EMBRAER ON NM"),
                    new AcoesInfo(41,"ENAT3","ENAUTA PART ON NM"),
                    new AcoesInfo(42,"EQTL3","EQUATORIAL ON NM"),
                    new AcoesInfo(43,"EVEN3","EVEN ON NM"),
                    new AcoesInfo(44,"GGBR4","GERDAU PN N1"),
                    new AcoesInfo(45,"GOLL4","GOL PN N2"),
                    new AcoesInfo(46,"GPCP3","GPC PART ON"),
                    new AcoesInfo(47,"GRND3","GRENDENE ON NM"),
                    new AcoesInfo(48,"GUAR3","GUARARAPES ON"),
                    new AcoesInfo(49,"HGTX3","CIA HERING ON NM"),
                    new AcoesInfo(50,"HYPE3","HYPERA ON NM"),
                    new AcoesInfo(51,"IGTA3","IGUATEMI ON NM"),
                    new AcoesInfo(52,"INTB3","INTELBRAS ON NM"),
                    new AcoesInfo(53,"ITUB4","ITAUUNIBANCOPN N1"),
                    new AcoesInfo(54,"JBSS3","JBS ON NM"),
                    new AcoesInfo(55,"JHSF3","JHSF PART ON NM"),
                    new AcoesInfo(56,"KLBN11","KLABIN S/A UNT N2"),
                    new AcoesInfo(57,"LCAM3","LOCAMERICA ON NM"),
                    new AcoesInfo(58,"LJQQ3","QUERO-QUERO ON NM"),
                    new AcoesInfo(59,"LOGN3","LOG-IN ON NM"),
                    new AcoesInfo(60,"LWSA3","LOCAWEB ON NM"),
                    new AcoesInfo(61,"MDIA3","M.DIASBRANCOON NM"),
                    new AcoesInfo(62,"MEAL3","IMC S/A ON NM"),
                    new AcoesInfo(63,"MGLU3","MAGAZ LUIZA ON NM"),
                    new AcoesInfo(64,"MOVI3","MOVIDA ON NM"),
                    new AcoesInfo(65,"MRFG3","MARFRIG ON ED NM"),
                    new AcoesInfo(66,"MRVE3","MRV ON NM"),
                    new AcoesInfo(67,"NEOE3","NEOENERGIA ON ED NM"),
                    new AcoesInfo(68,"PCAR3","P.ACUCAR-CBDON NM"),
                    new AcoesInfo(69,"PETR4","PETROBRAS PN EX N2"),
                    new AcoesInfo(70,"PGMN3","PAGUE MENOS ON NM"),
                    new AcoesInfo(71,"POSI3","POSITIVO TECON NM"),
                    new AcoesInfo(72,"PRIO3","PETRORIO ON NM"),
                    new AcoesInfo(73,"PTBL3","PORTOBELLO ON NM"),
                    new AcoesInfo(74,"RAIL3","RUMO S.A. ON NM"),
                    new AcoesInfo(75,"RANI3","IRANI ON NM"),
                    new AcoesInfo(76,"RAPT4","RANDON PART PN ED N1"),
                    new AcoesInfo(77,"RDOR3","REDE D OR ON NM"),
                    new AcoesInfo(78,"RENT3","LOCALIZA ON NM"),
                    new AcoesInfo(79,"ROMI3","INDS ROMI ON NM"),
                    new AcoesInfo(80,"RRRP3","3R PETROLEUMON NM"),
                    new AcoesInfo(81,"SBFG3","GRUPO SBF ON NM"),
                    new AcoesInfo(82,"SEQL3","SEQUOIA LOG ON NM"),
                    new AcoesInfo(83,"SIMH3","SIMPAR ON NM"),
                    new AcoesInfo(84,"SLCE3","SLC AGRICOLAON NM"),
                    new AcoesInfo(85,"SMLS3","SMILES ON NM"),
                    new AcoesInfo(86,"SMTO3","SAO MARTINHOON NM"),
                    new AcoesInfo(87,"STBP3","SANTOS BRP ON NM"),
                    new AcoesInfo(88,"SULA11","SUL AMERICA UNT N2"),
                    new AcoesInfo(89,"SUZB3","SUZANO S.A. ON NM"),
                    new AcoesInfo(90,"TASA4","TAURUS ARMASPN N2"),
                    new AcoesInfo(91,"TOTS3","TOTVS ON NM"),
                    new AcoesInfo(92,"TPIS3","TRIUNFO PARTON NM"),
                    new AcoesInfo(93,"UGPA3","ULTRAPAR ON NM"),
                    new AcoesInfo(94,"USIM5","USIMINAS PNA N1"),
                    new AcoesInfo(95,"VALE3","VALE ON NM"),
                    new AcoesInfo(96,"VVAR3","VIAVAREJO ON NM"),
                    new AcoesInfo(97,"WEGE3","WEG ON NM"),
                    new AcoesInfo(98,"WINFUT","IBOVESPA MINI:100000120240"),
                    new AcoesInfo(99,"YDUQ3","YDUQS PART ON NM")
                 });

            builder.Entity<Aluno>()
                .HasData(new List<Aluno>(){
                    new Aluno(1, 1, "Marta", "Kent", "33225555", DateTime.Parse("05/12/2005")),
                    new Aluno(2, 2, "Paula", "Isabela", "3354288", DateTime.Parse("05/12/2005")),
                    new Aluno(3, 3, "Laura", "Antonia", "55668899", DateTime.Parse("05/12/2005")),
                    new Aluno(4, 4, "Luiza", "Maria", "6565659", DateTime.Parse("05/12/2005")),
                    new Aluno(5, 5, "Lucas", "Machado", "565685415", DateTime.Parse("05/12/2005")),
                    new Aluno(6, 6, "Pedro", "Alvares", "456454545", DateTime.Parse("05/12/2005")),
                    new Aluno(7, 7, "Paulo", "José", "9874512", DateTime.Parse("05/12/2005"))
                });

            builder.Entity<AlunoDisciplina>()
                .HasData(new List<AlunoDisciplina>() {
                    new AlunoDisciplina() {AlunoId = 1, DisciplinaId = 2 },
                    new AlunoDisciplina() {AlunoId = 1, DisciplinaId = 4 },
                    new AlunoDisciplina() {AlunoId = 1, DisciplinaId = 5 },
                    new AlunoDisciplina() {AlunoId = 2, DisciplinaId = 1 },
                    new AlunoDisciplina() {AlunoId = 2, DisciplinaId = 2 },
                    new AlunoDisciplina() {AlunoId = 2, DisciplinaId = 5 },
                    new AlunoDisciplina() {AlunoId = 3, DisciplinaId = 1 },
                    new AlunoDisciplina() {AlunoId = 3, DisciplinaId = 2 },
                    new AlunoDisciplina() {AlunoId = 3, DisciplinaId = 3 },
                    new AlunoDisciplina() {AlunoId = 4, DisciplinaId = 1 },
                    new AlunoDisciplina() {AlunoId = 4, DisciplinaId = 4 },
                    new AlunoDisciplina() {AlunoId = 4, DisciplinaId = 5 },
                    new AlunoDisciplina() {AlunoId = 5, DisciplinaId = 4 },
                    new AlunoDisciplina() {AlunoId = 5, DisciplinaId = 5 },
                    new AlunoDisciplina() {AlunoId = 6, DisciplinaId = 1 },
                    new AlunoDisciplina() {AlunoId = 6, DisciplinaId = 2 },
                    new AlunoDisciplina() {AlunoId = 6, DisciplinaId = 3 },
                    new AlunoDisciplina() {AlunoId = 6, DisciplinaId = 4 },
                    new AlunoDisciplina() {AlunoId = 7, DisciplinaId = 1 },
                    new AlunoDisciplina() {AlunoId = 7, DisciplinaId = 2 },
                    new AlunoDisciplina() {AlunoId = 7, DisciplinaId = 3 },
                    new AlunoDisciplina() {AlunoId = 7, DisciplinaId = 4 },
                    new AlunoDisciplina() {AlunoId = 7, DisciplinaId = 5 }
                });
        }

    }
}
