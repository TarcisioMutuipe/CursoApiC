import { Pagination, PaginatedResult } from './../../models/Pagination';
import { Component, OnInit, TemplateRef, OnDestroy } from '@angular/core';
import { Aluno } from '../../models/Aluno';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AlunoService } from '../../services/aluno.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { ProfessorService } from '../../services/professor.service';
import { Professor } from '../../models/Professor';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-alunos',
  templateUrl: './alunos.component.html',
  styleUrls: ['./alunos.component.css']
})
export class AlunosComponent implements OnInit, OnDestroy {

  public modalRef!: BsModalRef;
  public alunoForm!: FormGroup;
  public titulo = 'Alunos';
  public alunoSelecionado!: Aluno;
  public textSimple!: string;
  public profsAlunos!: Professor[];

  private unsubscriber = new Subject();

  public alunos!: Aluno[];
  public aluno!: Aluno;
  public msnDeleteAluno!: string;
  public modeSave: string = 'post';
  public pagination!: Pagination;

  constructor(
    private alunoService: AlunoService,
    private route: ActivatedRoute,
    private professorService: ProfessorService,
    private fb: FormBuilder,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    this.criarForm();
  }



  professoresAlunos(template: TemplateRef<any>, id: number) {
    this.spinner.show();
    this.professorService.getByAlunoId(id)
      .pipe(takeUntil(this.unsubscriber))
      .subscribe((professores: Professor[]) => {
        this.profsAlunos = professores;
        this.modalRef = this.modalService.show(template);
      }, (error: any) => {
        this.toastr.error(`erro: ${error}`);
        console.log(error);
        this.spinner.hide();
      }, () => this.spinner.hide()
    );
  }

  ngOnInit(): void {
    this.pagination = { currentPage: 1, itemsPerPage: 4} as Pagination;
    this.carregarAlunos();
  }

  criarForm() : void{
    this.alunoForm = this.fb.group({
      id: [0],
      nome: ['', Validators.required],
      sobrenome: ['', Validators.required],
      telefone: ['', Validators.required],
      ativo:[]
    });
  }

  saveAluno(): void {
    if (this.alunoForm.valid) {
      this.spinner.show();

      if (this.modeSave === 'post') {
        this.aluno = {...this.alunoForm.value};
      } else {
        this.aluno = {id: this.alunoSelecionado.id, ...this.alunoForm.value};
      }

      this.alunoService[this.modeSave](this.aluno)
        .pipe(takeUntil(this.unsubscriber))
        .subscribe(
          () => {
            this.carregarAlunos();
            this.toastr.success('Aluno salvo com sucesso!');
          }, (error: any) => {
            this.toastr.error(`Erro: Aluno não pode ser salvo!`);
            console.error(error);
            this.spinner.hide();
          }, () => this.spinner.hide()
        );

    }
  }

    trocaEstado(aluno): void {

      this.alunoService.trocarEstado(aluno.id, !aluno.ativo)
      .pipe(takeUntil(this.unsubscriber))
      .subscribe(
        (resp) => {
          console.log(resp);
          this.carregarAlunos();
          this.toastr.success('Aluno salvo com sucesso!');
        }, (error: any) => {
          this.toastr.error(`Erro: Aluno não pode ser salvo!`);
          console.error(error);
          this.spinner.hide();
        }, () => this.spinner.hide()
      );
    }

  carregarAlunos(): void {
    const alunoId = Number(this.route.snapshot.paramMap.get('id'));

    this.spinner.show();
    this.alunoService.getAll(this.pagination.currentPage, this.pagination.itemsPerPage)
      .pipe(takeUntil(this.unsubscriber))
      .subscribe((alunos: PaginatedResult<Aluno[]>) => {
        this.alunos = alunos.result;
        this.pagination = alunos.pagination;
        console.log(this.alunos);

        if (alunoId > 0) {
          this.alunoSelect(alunoId);
        }

        this.toastr.success('Alunos foram carregado com Sucesso!');
      }, (error: any) => {
        this.toastr.error('Alunos não carregados!');
        console.error(error);
        this.spinner.hide();
      }, () => this.spinner.hide()
    );
  }

  pageChanged(event: any): void {
  this.pagination.currentPage = event.page;
  this.carregarAlunos();
  }



  alunoSelect(alunoId: number):void {
    this.modeSave = 'patch';
    this.alunoService.getById(alunoId).subscribe(
      (alunoReturn)=>{
        this.alunoSelecionado = alunoReturn;
        this.alunoForm.patchValue(this.alunoSelecionado);
      },
      (error) => {
        this.toastr.error('Falha ao Carregar Alunos!');
        console.error(error);
        this.spinner.hide();
      },
      ()=> this.spinner.hide()
    );

  }

  voltar(): void {
    this.alunoSelecionado = null as any;
  }
  openModal(template: TemplateRef<any>, alunoId: number) {
    this.professoresAlunos(template, alunoId);
  }

  closeModal(): void {
    this.modalRef.hide();
  }

  ngOnDestroy(): void {
    this.unsubscriber.next();
    this.unsubscriber.complete();
  }
}
