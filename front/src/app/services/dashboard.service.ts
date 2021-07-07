import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Dashboard } from '../models/Dashboard';
import { GraficoCorretoras } from '../models/GraficoCorretoras';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  baseURL = `${environment.mainUrlAPI}`;
  constructor(private http: HttpClient) { }


  fluxoBolsaDias(DataIni:Date, DataFim:Date, Sigla:string): Observable<Dashboard[]>{
    const params =
    {
        dataini: DataIni,
        DataFim: DataFim
    }
    return this.http.get<Dashboard[]>(`${this.baseURL}FluxoBolsa/BuscaFluxoDias?Dataini=${DataIni}&DataFim=${DataFim}&Sigla=${Sigla}`);

  }
  fluxoBolsaCorretoras(DataIni:Date, DataFim:Date, Sigla:string): Observable<Dashboard[]>{
    return this.http.get<Dashboard[]>(`${this.baseURL}FluxoBolsa/BuscaFluxoCorretoras?Dataini=${DataIni}&DataFim=${DataFim}&Sigla=${Sigla}`);
  }
  BuscaListaAcoes(): Observable<String[]>{
    return this.http.get<String[]>(`${this.baseURL}FluxoBolsa/BuscaListaAcoes`);
  }
  GetFluxoVolumexVard(DataIni:Date, DataFim:Date, Sigla:string): Observable<GraficoCorretoras[]>{
    return this.http.get<GraficoCorretoras[]>(`${this.baseURL}FluxoBolsa/GetFluxoVolumexVard?Dataini=${DataIni}&DataFim=${DataFim}&Sigla=${Sigla}`);
  }
  GetComparatodas(DataIni:Date, DataFim:Date): Observable<Dashboard[]>{
    return this.http.get<Dashboard[]>(`${this.baseURL}FluxoBolsa/GetComparatodas?Dataini=${DataIni}&DataFim=${DataFim}`);
  }
}
