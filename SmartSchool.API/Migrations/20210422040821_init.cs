using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartSchool.API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcoesInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Sigla = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcoesInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Alunos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Matricula = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    Sobrenome = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    DataNasc = table.Column<DateTime>(nullable: false),
                    DataIni = table.Column<DateTime>(nullable: false),
                    DataFim = table.Column<DateTime>(nullable: true),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alunos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Curso",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curso", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Professores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Registro = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    Sobrenome = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    DataIni = table.Column<DateTime>(nullable: false),
                    DataFim = table.Column<DateTime>(nullable: true),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Acoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VarD = table.Column<decimal>(nullable: false),
                    Ultima = table.Column<decimal>(nullable: false),
                    NumNegocio = table.Column<int>(nullable: false),
                    Volume = table.Column<int>(nullable: false),
                    AgressorCompra = table.Column<decimal>(nullable: false),
                    AgressorVenda = table.Column<decimal>(nullable: false),
                    NotaForca = table.Column<int>(nullable: false),
                    AcoesInfoId = table.Column<int>(nullable: false),
                    DataPregao = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Acoes_AcoesInfo_AcoesInfoId",
                        column: x => x.AcoesInfoId,
                        principalTable: "AcoesInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlunosCursos",
                columns: table => new
                {
                    AlunoId = table.Column<int>(nullable: false),
                    CursoId = table.Column<int>(nullable: false),
                    DataIni = table.Column<DateTime>(nullable: false),
                    DataFim = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlunosCursos", x => new { x.AlunoId, x.CursoId });
                    table.ForeignKey(
                        name: "FK_AlunosCursos_Alunos_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlunosCursos_Curso_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Curso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Disciplinas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    CargaHoraria = table.Column<int>(nullable: false),
                    PrerequisitoId = table.Column<int>(nullable: true),
                    ProfessorId = table.Column<int>(nullable: false),
                    CursoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplinas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disciplinas_Curso_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Curso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Disciplinas_Disciplinas_PrerequisitoId",
                        column: x => x.PrerequisitoId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Disciplinas_Professores_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Professores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FluxoBolsa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodCorretora = table.Column<string>(nullable: true),
                    NomeCorretora = table.Column<string>(nullable: true),
                    Quantidade = table.Column<int>(nullable: false),
                    Volume = table.Column<double>(nullable: false),
                    NumNegocio = table.Column<int>(nullable: false),
                    AcoesInfoId = table.Column<int>(nullable: false),
                    PrecoMedio = table.Column<decimal>(nullable: false),
                    Data = table.Column<DateTime>(nullable: true),
                    AcoesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluxoBolsa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FluxoBolsa_Acoes_AcoesId",
                        column: x => x.AcoesId,
                        principalTable: "Acoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FluxoBolsa_AcoesInfo_AcoesInfoId",
                        column: x => x.AcoesInfoId,
                        principalTable: "AcoesInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlunosDisciplinas",
                columns: table => new
                {
                    AlunoId = table.Column<int>(nullable: false),
                    DisciplinaId = table.Column<int>(nullable: false),
                    DataIni = table.Column<DateTime>(nullable: false),
                    DataFim = table.Column<DateTime>(nullable: true),
                    Nota = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlunosDisciplinas", x => new { x.AlunoId, x.DisciplinaId });
                    table.ForeignKey(
                        name: "FK_AlunosDisciplinas_Alunos_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlunosDisciplinas_Disciplinas_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AcoesInfo",
                columns: new[] { "Id", "Descricao", "Sigla" },
                values: new object[,]
                {
                    { 1, "AMBEV S/A ON", "ABEV3" },
                    { 74, "RUMO S.A. ON NM", "RAIL3" },
                    { 73, "PORTOBELLO ON NM", "PTBL3" },
                    { 72, "PETRORIO ON NM", "PRIO3" },
                    { 71, "POSITIVO TECON NM", "POSI3" },
                    { 70, "PAGUE MENOS ON NM", "PGMN3" },
                    { 69, "PETROBRAS PN EX N2", "PETR4" },
                    { 68, "P.ACUCAR-CBDON NM", "PCAR3" },
                    { 67, "NEOENERGIA ON ED NM", "NEOE3" },
                    { 66, "MRV ON NM", "MRVE3" },
                    { 65, "MARFRIG ON ED NM", "MRFG3" },
                    { 64, "MOVIDA ON NM", "MOVI3" },
                    { 63, "MAGAZ LUIZA ON NM", "MGLU3" },
                    { 62, "IMC S/A ON NM", "MEAL3" },
                    { 61, "M.DIASBRANCOON NM", "MDIA3" },
                    { 60, "LOCAWEB ON NM", "LWSA3" },
                    { 59, "LOG-IN ON NM", "LOGN3" },
                    { 58, "QUERO-QUERO ON NM", "LJQQ3" },
                    { 56, "KLABIN S/A UNT N2", "KLBN11" },
                    { 55, "JHSF PART ON NM", "JHSF3" },
                    { 54, "JBS ON NM", "JBSS3" },
                    { 53, "ITAUUNIBANCOPN N1", "ITUB4" },
                    { 75, "IRANI ON NM", "RANI3" },
                    { 52, "INTELBRAS ON NM", "INTB3" },
                    { 76, "RANDON PART PN ED N1", "RAPT4" },
                    { 78, "LOCALIZA ON NM", "RENT3" },
                    { 99, "YDUQS PART ON NM", "YDUQ3" },
                    { 98, "IBOVESPA MINI:100000120240", "WINFUT" },
                    { 97, "WEG ON NM", "WEGE3" },
                    { 96, "VIAVAREJO ON NM", "VVAR3" },
                    { 95, "VALE ON NM", "VALE3" },
                    { 94, "USIMINAS PNA N1", "USIM5" },
                    { 93, "ULTRAPAR ON NM", "UGPA3" },
                    { 92, "TRIUNFO PARTON NM", "TPIS3" },
                    { 91, "TOTVS ON NM", "TOTS3" },
                    { 90, "TAURUS ARMASPN N2", "TASA4" },
                    { 89, "SUZANO S.A. ON NM", "SUZB3" },
                    { 88, "SUL AMERICA UNT N2", "SULA11" },
                    { 87, "SANTOS BRP ON NM", "STBP3" },
                    { 86, "SAO MARTINHOON NM", "SMTO3" },
                    { 85, "SMILES ON NM", "SMLS3" },
                    { 84, "SLC AGRICOLAON NM", "SLCE3" },
                    { 83, "SIMPAR ON NM", "SIMH3" },
                    { 82, "SEQUOIA LOG ON NM", "SEQL3" },
                    { 81, "GRUPO SBF ON NM", "SBFG3" },
                    { 80, "3R PETROLEUMON NM", "RRRP3" },
                    { 79, "INDS ROMI ON NM", "ROMI3" },
                    { 77, "REDE D OR ON NM", "RDOR3" },
                    { 51, "IGUATEMI ON NM", "IGTA3" },
                    { 57, "LOCAMERICA ON NM", "LCAM3" },
                    { 49, "CIA HERING ON NM", "HGTX3" },
                    { 23, "BR MALLS PARON NM", "BRML3" },
                    { 22, "BRASKEM PNA N1", "BRKM5" },
                    { 21, "BRF SA ON NM", "BRFS3" },
                    { 20, "PETROBRAS BRON EDR NM", "BRDT3" },
                    { 50, "HYPERA ON NM", "HYPE3" },
                    { 18, "BANCO PAN PN N1", "BPAN4" },
                    { 17, "BTGP BANCO UNT N2", "BPAC11" },
                    { 16, "BOA VISTA ON NM", "BOAS3" },
                    { 15, "BK BRASIL ON NM", "BKBR3" },
                    { 14, "BANCO INTER PN N2", "BIDI4" },
                    { 24, "B2W DIGITAL ON NM", "BTOW3" },
                    { 13, "MINERVA ON ED NM", "BEEF3" },
                    { 11, "BRADESCO PN EB N1", "BBDC4" },
                    { 10, "BRASIL ON NM", "BBAS3" },
                    { 9, "B3 ON NM", "B3SA3" },
                    { 8, "AZUL PN N2", "AZUL4" },
                    { 7, "ASSAI ON NM", "ASAI3" },
                    { 6, "AREZZO CO ON NM", "ARZZ3" },
                    { 5, "ANIMA ON NM", "ANIM3" },
                    { 4, "AMBIPAR ON NM", "AMBP3" },
                    { 3, "ALPARGATAS PN N1", "ALPA4" },
                    { 2, "BRASILAGRO ON NM", "AGRO3" },
                    { 12, "BBSEGURIDADEON NM", "BBSE3" },
                    { 25, "CSU CARDSYSTON ED NM", "CARD3" },
                    { 19, "BRADESPAR PN N1", "BRAP4" },
                    { 27, "CCR SA ON ED NM", "CCRO3" },
                    { 48, "GUARARAPES ON", "GUAR3" },
                    { 47, "GRENDENE ON NM", "GRND3" },
                    { 46, "GPC PART ON", "GPCP3" },
                    { 45, "GOL PN N2", "GOLL4" },
                    { 26, "MELIUZ ON NM", "CASH3" },
                    { 43, "EVEN ON NM", "EVEN3" },
                    { 42, "EQUATORIAL ON NM", "EQTL3" },
                    { 41, "ENAUTA PART ON NM", "ENAT3" },
                    { 40, "EMBRAER ON NM", "EMBR3" },
                    { 39, "ELETROBRAS ON N1", "ELET3" },
                    { 44, "GERDAU PN N1", "GGBR4" },
                    { 37, "CYRELA REALTON NM", "CYRE3" },
                    { 36, "CVC BRASIL ON NM", "CVCB3" },
                    { 35, "SID NACIONALON", "CSNA3" },
                    { 34, "COPASA ON NM", "CSMG3" },
                    { 33, "CARREFOUR BRON ED NM", "CRFB3" },
                    { 32, "COPEL PNB N1", "CPLE6" },
                    { 31, "COGNA ON ON NM", "COGN3" },
                    { 30, "CSNMINERACAOON N2", "CMIN3" },
                    { 29, "CEMIG PN N1", "CMIG4" },
                    { 28, "CEA MODAS ON NM", "CEAB3" },
                    { 38, "ECORODOVIAS ON NM", "ECOR3" }
                });

            migrationBuilder.InsertData(
                table: "Alunos",
                columns: new[] { "Id", "Ativo", "DataFim", "DataIni", "DataNasc", "Matricula", "Nome", "Sobrenome", "Telefone" },
                values: new object[,]
                {
                    { 7, true, null, new DateTime(2021, 4, 22, 1, 8, 20, 477, DateTimeKind.Local).AddTicks(4555), new DateTime(2005, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Paulo", "José", "9874512" },
                    { 6, true, null, new DateTime(2021, 4, 22, 1, 8, 20, 477, DateTimeKind.Local).AddTicks(4546), new DateTime(2005, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "Pedro", "Alvares", "456454545" },
                    { 5, true, null, new DateTime(2021, 4, 22, 1, 8, 20, 477, DateTimeKind.Local).AddTicks(4530), new DateTime(2005, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Lucas", "Machado", "565685415" },
                    { 2, true, null, new DateTime(2021, 4, 22, 1, 8, 20, 477, DateTimeKind.Local).AddTicks(4371), new DateTime(2005, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Paula", "Isabela", "3354288" },
                    { 3, true, null, new DateTime(2021, 4, 22, 1, 8, 20, 477, DateTimeKind.Local).AddTicks(4509), new DateTime(2005, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Laura", "Antonia", "55668899" },
                    { 1, true, null, new DateTime(2021, 4, 22, 1, 8, 20, 476, DateTimeKind.Local).AddTicks(7078), new DateTime(2005, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Marta", "Kent", "33225555" },
                    { 4, true, null, new DateTime(2021, 4, 22, 1, 8, 20, 477, DateTimeKind.Local).AddTicks(4522), new DateTime(2005, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Luiza", "Maria", "6565659" }
                });

            migrationBuilder.InsertData(
                table: "Curso",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Tecnologia da Informação" },
                    { 2, "Sistemas de Informação" },
                    { 3, "Ciência da Computação" }
                });

            migrationBuilder.InsertData(
                table: "Professores",
                columns: new[] { "Id", "Ativo", "DataFim", "DataIni", "Nome", "Registro", "Sobrenome", "Telefone" },
                values: new object[,]
                {
                    { 4, true, null, new DateTime(2021, 4, 22, 1, 8, 20, 466, DateTimeKind.Local).AddTicks(5070), "Rodrigo", 4, "Carvalho", null },
                    { 1, true, null, new DateTime(2021, 4, 22, 1, 8, 20, 463, DateTimeKind.Local).AddTicks(6079), "Lauro", 1, "Oliveira", null },
                    { 2, true, null, new DateTime(2021, 4, 22, 1, 8, 20, 466, DateTimeKind.Local).AddTicks(4954), "Roberto", 2, "Soares", null },
                    { 3, true, null, new DateTime(2021, 4, 22, 1, 8, 20, 466, DateTimeKind.Local).AddTicks(5066), "Ronaldo", 3, "Marconi", null },
                    { 5, true, null, new DateTime(2021, 4, 22, 1, 8, 20, 466, DateTimeKind.Local).AddTicks(5073), "Alexandre", 5, "Montanha", null }
                });

            migrationBuilder.InsertData(
                table: "Disciplinas",
                columns: new[] { "Id", "CargaHoraria", "CursoId", "Nome", "PrerequisitoId", "ProfessorId" },
                values: new object[,]
                {
                    { 1, 0, 1, "Matemática", null, 1 },
                    { 2, 0, 3, "Matemática", null, 1 },
                    { 3, 0, 3, "Física", null, 2 },
                    { 4, 0, 1, "Português", null, 3 },
                    { 5, 0, 1, "Inglês", null, 4 },
                    { 6, 0, 2, "Inglês", null, 4 },
                    { 7, 0, 3, "Inglês", null, 4 },
                    { 8, 0, 1, "Programação", null, 5 },
                    { 9, 0, 2, "Programação", null, 5 },
                    { 10, 0, 3, "Programação", null, 5 }
                });

            migrationBuilder.InsertData(
                table: "AlunosDisciplinas",
                columns: new[] { "AlunoId", "DisciplinaId", "DataFim", "DataIni", "Nota" },
                values: new object[,]
                {
                    { 2, 1, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(76), null },
                    { 4, 5, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(101), null },
                    { 2, 5, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(86), null },
                    { 1, 5, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(73), null },
                    { 7, 4, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(126), null },
                    { 6, 4, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(117), null },
                    { 5, 4, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(103), null },
                    { 4, 4, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(99), null },
                    { 1, 4, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(29), null },
                    { 7, 3, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(124), null },
                    { 5, 5, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(106), null },
                    { 6, 3, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(113), null },
                    { 7, 2, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(122), null },
                    { 6, 2, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(110), null },
                    { 3, 2, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(90), null },
                    { 2, 2, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(79), null },
                    { 1, 2, null, new DateTime(2021, 4, 22, 1, 8, 20, 477, DateTimeKind.Local).AddTicks(7628), null },
                    { 7, 1, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(120), null },
                    { 6, 1, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(108), null },
                    { 4, 1, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(97), null },
                    { 3, 1, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(88), null },
                    { 3, 3, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(93), null },
                    { 7, 5, null, new DateTime(2021, 4, 22, 1, 8, 20, 478, DateTimeKind.Local).AddTicks(128), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Acoes_AcoesInfoId",
                table: "Acoes",
                column: "AcoesInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AlunosCursos_CursoId",
                table: "AlunosCursos",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_AlunosDisciplinas_DisciplinaId",
                table: "AlunosDisciplinas",
                column: "DisciplinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Disciplinas_CursoId",
                table: "Disciplinas",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_Disciplinas_PrerequisitoId",
                table: "Disciplinas",
                column: "PrerequisitoId");

            migrationBuilder.CreateIndex(
                name: "IX_Disciplinas_ProfessorId",
                table: "Disciplinas",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_FluxoBolsa_AcoesId",
                table: "FluxoBolsa",
                column: "AcoesId");

            migrationBuilder.CreateIndex(
                name: "IX_FluxoBolsa_AcoesInfoId",
                table: "FluxoBolsa",
                column: "AcoesInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlunosCursos");

            migrationBuilder.DropTable(
                name: "AlunosDisciplinas");

            migrationBuilder.DropTable(
                name: "FluxoBolsa");

            migrationBuilder.DropTable(
                name: "Alunos");

            migrationBuilder.DropTable(
                name: "Disciplinas");

            migrationBuilder.DropTable(
                name: "Acoes");

            migrationBuilder.DropTable(
                name: "Curso");

            migrationBuilder.DropTable(
                name: "Professores");

            migrationBuilder.DropTable(
                name: "AcoesInfo");
        }
    }
}
