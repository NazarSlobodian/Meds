import { TestBed } from '@angular/core/testing';

import { LabBatchesService } from './lab-batches.service';

describe('LabBatchesService', () => {
  let service: LabBatchesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LabBatchesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
