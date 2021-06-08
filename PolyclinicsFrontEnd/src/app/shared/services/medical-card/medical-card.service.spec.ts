import {TestBed} from '@angular/core/testing';

import {MedicalCardService} from './medical-card.service';

describe('MedicalCardService', () => {
  let service: MedicalCardService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MedicalCardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
