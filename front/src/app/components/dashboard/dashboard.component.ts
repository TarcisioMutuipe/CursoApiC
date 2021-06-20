import { BuscaDados } from './../../models/BuscaDados';
import { DashboardService } from './../../services/dashboard.service';
import { Dashboard } from '../../models/Dashboard';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Chart } from 'chart.js';
import { map } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { takeUntil } from 'rxjs/operators';
import { Subject, Observable } from 'rxjs';
import { FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Data } from '@angular/router';
import { DatePipe } from '@angular/common';
import * as moment from 'moment';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  providers:[DatePipe]
})

export class DashboardComponent implements OnInit {

  public acoess!: String[];
  public today = Date.now();
  public DashForm!: FormGroup;
  private unsubscriber = new Subject();
  private dashboardx!: Dashboard[];
  queryField = new FormControl();
  public buscaDados!: BuscaDados;
  constructor(
    private DashboardService: DashboardService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private datePipe: DatePipe
  ) {
    this.criarForm();
    this.BuscaListaAcoes();
  }

  criarForm() : void{
    this.DashForm = this.fb.group({
      dataini: ['', Validators.required],
      datafim: ['', Validators.required],
      sigla: ['', Validators.required]
    });
  }
  @ViewChild("meuCanvasDias", { static: true }) elementoDias!: ElementRef;
  @ViewChild("meuCanvasCorretoras", { static: true }) elementoCorretoras!: ElementRef;

  ngOnInit(){
    this.DashBusca();
  }
  DataAtual():String {
    let d = new Date();
     return moment(d).format('DD/MM/YYYY');
  }
  BuscaListaAcoes():void {
    this.DashboardService.BuscaListaAcoes().pipe(takeUntil(this.unsubscriber))
    .subscribe((returnAcoes: String[]) => {
      this.acoess = returnAcoes;
    })
  }

  DashBusca():void {
    let value = this.queryField.value;
    if(value && value.trim() !='')
    {
      value = value.trim();
    }

    if (this.DashForm.valid) {
      this.DashboardService.fluxoBolsaCorretoras(this.DashForm.value.dataini,this.DashForm.value.datafim,this.DashForm.value.sigla).pipe(takeUntil(this.unsubscriber))
      .subscribe((dashboard: Dashboard[]) => {
        this.dashboardx = dashboard;

        var ListaCorreoras: string[] =[];
        var ListaVolumeMorgan: number[] = [];
        var ListaVolumeJP: number[] = [];
        var ListaVolumeMerrill: number[] = [];
        var ListaVolumePrincipal: number[] = [];
        var ListaDatasX: string[] = [];
        for (let index = 0; index < this.dashboardx.length; index++) {
          ListaCorreoras.push(this.dashboardx[index].quem);

          if(this.dashboardx[index].quem == "1-Principais"){
          ListaVolumePrincipal.push(this.dashboardx[index].volumeCorretora);}
          if(this.dashboardx[index].quem == "3-Morgan"){
          ListaVolumeMorgan.push(this.dashboardx[index].volumeCorretora);}
          if(this.dashboardx[index].quem == "2-JP Morgan"){
          ListaVolumeJP.push(this.dashboardx[index].volumeCorretora);}
          if(this.dashboardx[index].quem == "4-Merrill Lynch"){
          ListaVolumeMerrill.push(this.dashboardx[index].volumeCorretora);}

          if(this.dashboardx[index].quem == "1-Principais"){
          ListaDatasX.push(this.datePipe.transform(this.dashboardx[index].dataPregao,"dd-MM-yyyy") as any);
          }
        }
        //console.log(this.dashboardx);
        //cconsole.log(ListaVolumeMerrill);
        var chart = new Chart(this.elementoCorretoras.nativeElement, {
          type:'line',
          data:{
            labels: ListaDatasX,
            datasets: [
              {
                  label: ListaCorreoras[0],
                  data: ListaVolumePrincipal,
                  borderColor: '#00008B',
                  backgroundColor:['green','blue','yellow','red','blue','green','blue','yellow','red','blue','green','blue','yellow','red','blue','green','blue','yellow','red','blue'],
                  fill: false,
              },
              {
                label: ListaCorreoras[1],
                data: ListaVolumeJP,
                borderColor: '#00FF00',
                backgroundColor:['green','blue','yellow','red','blue','green','blue','yellow','red','blue','green','blue','yellow','red','blue','green','blue','yellow','red','blue'],
                fill: false,
            },
            {
              label: ListaCorreoras[2],
              data: ListaVolumeMorgan,
              borderColor: '#FF0000',
              backgroundColor:['green','blue','yellow','red','blue','green','blue','yellow','red','blue','green','blue','yellow','red','blue','green','blue','yellow','red','blue'],
              fill: false,
            },
            {
              label: ListaCorreoras[3],
              data: ListaVolumeMerrill,
              borderColor: '#00FFFF',
              backgroundColor:['green','blue','yellow','red','blue','green','blue','yellow','red','blue','green','blue','yellow','red','blue','green','blue','yellow','red','blue'],
              fill: false,
           },
            ]
          },
          options:{
            events: ['click']
          }
        })
      })

      this.DashboardService.fluxoBolsaDias(this.DashForm.value.dataini,this.DashForm.value.datafim,this.DashForm.value.sigla).pipe(takeUntil(this.unsubscriber))
      .subscribe((dashboard: Dashboard[]) => {
        this.dashboardx = dashboard;

        var ListaAcoes: string[] =[];
        var ListaTaxa: number[] = [];
        var ListaVolume: number[] = [];
        var ListaDatas: string[] = [];
        for (let index = 0; index < this.dashboardx.length; index++) {
          ListaAcoes.push(this.dashboardx[index].descricao);
          ListaTaxa.push(this.dashboardx[index].taxaFluxo);
          ListaVolume.push(this.dashboardx[index].volumeTotal);
          ListaDatas.push(this.datePipe.transform(this.dashboardx[index].dataPregao,"dd-MM-yyyy") as any);

        }

        var chart = new Chart(this.elementoDias.nativeElement, {
          type:'line',
          data:{
            labels: ListaDatas,
            datasets: [
              {
                  label: ListaAcoes[0],
                  data: ListaTaxa,
                  borderColor: '#3cba9f',
                  backgroundColor:['green','blue','yellow','red','blue','green','blue','yellow','red','blue','green','blue','yellow','red','blue','green','blue','yellow','red','blue'],
                  fill: false
              }
            ]
          },
          options:{
            events: ['click']
          }
        })
      })

    }
    else
    {
      this.toastr.error(`Existe campos não preenchidos!`);
    }

}
}
