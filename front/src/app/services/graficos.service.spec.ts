/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { GraficosService } from './graficos.service';

describe('Service: Graficos', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GraficosService]
    });
  });

  it('should ...', inject([GraficosService], (service: GraficosService) => {
    expect(service).toBeTruthy();
  }));
});
