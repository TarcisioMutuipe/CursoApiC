import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import {Chart, registerables} from 'chart.js'
Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})

export class DashboardComponent implements OnInit {

  constructor() { }

  @ViewChild("meuCanvas", { static: true }) elemento!: ElementRef;

  ngOnInit(){
    new Chart(this.elemento.nativeElement, {
      type: 'line',
      data: {
        labels: ["Janeiro","Fevereiro","Março","Abril","Maio","Junho","Julho","Agosto","Setembro","Outubro","Novembro","Dezembro"],
        datasets: [
          {
            data: [85,72,86,81,84,86,94,60,62,65,41,58],
            borderColor: '#00AEFF',
            fill: false
          },
          {
            data: [33,38,10,93,68,50,35,29,34,2,62,4],
            borderColor: "#FFCC00",
            fill: false
          }
        ]
      },
      options: {
       
      }
    });
  }
}