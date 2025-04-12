import { TestBed } from '@angular/core/testing';

import { BatchResultsService } from './batch-results.service';

describe('BatchResultsService', () => {
  let service: BatchResultsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BatchResultsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
