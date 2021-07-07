import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpParams,
  JsonpClientBackend,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Dashboard } from '../models/Dashboard';
import { GraficoCorretoras } from '../models/GraficoCorretoras';
import { environment } from 'src/environments/environment';
import { DatepickerServiceInputs } from '@ng-bootstrap/ng-bootstrap/datepicker/datepicker-service';
import { PaginatedResult } from '../models/Pagination';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class CorretorasService {
  baseURL = `${environment.mainUrlAPI}`;
  constructor(private http: HttpClient) {}

  BuscaListaCorretoras(): Observable<String[]>{
    return this.http.get<String[]>(`${this.baseURL}FluxoBolsa/BuscaListaCorretoras`);
  }
  BuscaListaAcoes(): Observable<String[]>{
    return this.http.get<String[]>(`${this.baseURL}FluxoBolsa/BuscaListaAcoes`);
  }

  GetFluxoAcertivas(
    DataIni: Date,
    DataFim: DatepickerServiceInputs,
    Corretora: String,
    page?: number,
    itemsPerPage?: number
  ): Observable<PaginatedResult<GraficoCorretoras[]>> {
    const paginatedResult: PaginatedResult<GraficoCorretoras[]> =
      new PaginatedResult<GraficoCorretoras[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }
    // return this.http.get<GraficoCorretoras[]>(`${this.baseURL}FluxoBolsa/GetFluxoAcertivas?Dataini=${DataIni}&DataFim=${DataFim}`);

    return this.http
      .get<GraficoCorretoras[]>(
        `${this.baseURL}FluxoBolsa/GetFluxoAcertivas?Dataini=${DataIni}&DataFim=${DataFim}&Corretora=${Corretora}`,
        { observe: 'response', params }
      )
      .pipe(
        map((response) => {
          paginatedResult.result = response.body as any;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination') as any
            );
          }
          return paginatedResult;
        })
      );
  }


  GetDiasComprasdos(
    DataIni: Date,
    DataFim: DatepickerServiceInputs,
    sigla: String,
    page?: number,
    itemsPerPage?: number
  ): Observable<PaginatedResult<GraficoCorretoras[]>> {
    const paginatedResult: PaginatedResult<GraficoCorretoras[]> =
      new PaginatedResult<GraficoCorretoras[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }
    // return this.http.get<GraficoCorretoras[]>(`${this.baseURL}FluxoBolsa/GetFluxoAcertivas?Dataini=${DataIni}&DataFim=${DataFim}`);

    return this.http
      .get<GraficoCorretoras[]>(
        `${this.baseURL}FluxoBolsa/GetDiasComprasdos?Dataini=${DataIni}&DataFim=${DataFim}&Sigla=${sigla}`,
        { observe: 'response', params }
      )
      .pipe(
        map((response) => {
          paginatedResult.result = response.body as any;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination') as any
            );
          }
          return paginatedResult;
        })
      );
  }

}
