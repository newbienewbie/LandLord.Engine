import { TestBed, inject } from '@angular/core/testing';

import { CardConverterService } from './card-converter.service';

describe('CardConverterService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CardConverterService]
    });
  });

  it('should be created', inject([CardConverterService], (service: CardConverterService) => {
    expect(service).toBeTruthy();
  }));
});
