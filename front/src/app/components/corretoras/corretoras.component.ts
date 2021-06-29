import { GraficoCorretoras } from '../../models/GraficoCorretoras';
import { Pagination, PaginatedResult } from '../../models/Pagination';
import { BuscaDados } from '../../models/BuscaDados';
import { CorretorasService } from '../../services/corretoras.service';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Chart } from 'chart.js';
import { takeUntil } from 'rxjs/operators';
import { Subject, Observable } from 'rxjs';
import { FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Data } from '@angular/router';
import { DatePipe } from '@angular/common';
import * as moment from 'moment';
import { rendererTypeName } from '@angular/compiler';
import { readJsonConfigFile } from 'typescript';

@Component({
  selector: 'app-corretoras',
  templateUrl: './corretoras.component.html',
  styleUrls: ['./corretoras.component.css'],
  providers: [DatePipe],
})
export class CorretorasComponent implements OnInit {
  public acoess!: String[];
  public today = Date.now();
  public GrafForm!: FormGroup;
  private unsubscriber = new Subject();
  public pagination!: Pagination;
  public graficox!: GraficoCorretoras[];
  queryField = new FormControl();
  public buscaDados!: BuscaDados;
  constructor(
    private CorretorasService: CorretorasService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    this.BuscaListaCorretoras();
    this.criarForm();

  }

  criarForm(): void {
    this.GrafForm = this.fb.group({
      dataini: ['', Validators.required],
      datafim: ['', Validators.required],
      sigla: ['', Validators.required],
    });
  }
  @ViewChild('meuCanvasTodasCorretoras', { static: true })
  elementoDiasContador!: ElementRef;

  ngOnInit() {
    this.pagination = { currentPage: 1, itemsPerPage: 4} as Pagination;
    this.DashBusca();
  }
  BuscaListaCorretoras():void {
    this.CorretorasService.BuscaListaCorretoras().pipe(takeUntil(this.unsubscriber))
    .subscribe((returnAcoes: String[]) => {
      this.acoess = returnAcoes;
    })
  }

  DataAtual(): String {
    let d = new Date();
    return moment(d).format('DD/MM/YYYY');
  }
 pageChanged(event: any): void {
  this.pagination.currentPage = event.page;
  this.DashBusca();
  }
  DashBusca(): void {
      this.spinner.show();
      this.CorretorasService.GetFluxoAcertivas(this.GrafForm.value.dataini,this.GrafForm.value.datafim,this.GrafForm.value.corretora,this.pagination.currentPage, this.pagination.itemsPerPage)
        .pipe(takeUntil(this.unsubscriber))
        .subscribe((grafic: PaginatedResult<GraficoCorretoras[]>) => {
          this.graficox = grafic.result;
          this.pagination = grafic.pagination;
          console.log(this.graficox);

          this.toastr.success('Lista Foi carregada com Sucesso!');
        }, (error: any) => {
          this.toastr.error('Lista não carregada!');
          console.error(error);
          this.spinner.hide();
        }, () => this.spinner.hide()
      );
    }

}
