import { Pagination, PaginatedResult } from './../../models/Pagination';
import { BuscaDados } from '../../models/BuscaDados';
import { GraficosService } from '../../services/graficos.service';
import { GraficoCorretoras } from '../../models/GraficoCorretoras';
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
import { CorretorasService } from '../../services/corretoras.service';

@Component({
  selector: 'app-graficos',
  templateUrl: './graficos.component.html',
  styleUrls: ['./graficos.component.css'],
  providers: [DatePipe],
})
export class GraficosComponent implements OnInit {
  public corretoras!: String[];
  public today = Date.now();
  public GrafForm!: FormGroup;
  private unsubscriber = new Subject();
  public pagination!: Pagination;
  public graficox!: GraficoCorretoras[];
  queryField = new FormControl();
  public buscaDados!: BuscaDados;
  constructor(
    private corretorasService: CorretorasService,
    private graficosService: GraficosService,
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
  DataAtual(): String {
    let d = new Date();
    return moment(d).format('DD/MM/YYYY');
  }
  BuscaListaCorretoras():void {
    this.corretorasService.BuscaListaCorretoras().pipe(takeUntil(this.unsubscriber))
    .subscribe((returnCorretoras: String[]) => {
      this.corretoras = returnCorretoras;
    })
  }
 pageChanged(event: any): void {
  this.pagination.currentPage = event.page;
  this.DashBusca();
  }
  DashBusca(): void {
      this.spinner.show();
      console.log(this.GrafForm.value)
      this.graficosService.GetFluxoAcertivas(this.GrafForm.value.dataini,this.GrafForm.value.datafim,this.GrafForm.value.sigla,this.pagination.currentPage, this.pagination.itemsPerPage)
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
