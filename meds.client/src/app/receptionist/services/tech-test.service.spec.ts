import { TestBed } from '@angular/core/testing';

import { TechTestService } from './tech-test.service';

describe('TechTestService', () => {
  let service: TechTestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TechTestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
