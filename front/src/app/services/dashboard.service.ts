import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Dashboard } from '../models/Dashboard';
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
}
