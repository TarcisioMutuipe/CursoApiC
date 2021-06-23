/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { CorretorasService } from './corretoras.service';

describe('Service: corretoras', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CorretorasService]
    });
  });

  it('should ...', inject([CorretorasService], (service: CorretorasService) => {
    expect(service).toBeTruthy();
  }));
});
