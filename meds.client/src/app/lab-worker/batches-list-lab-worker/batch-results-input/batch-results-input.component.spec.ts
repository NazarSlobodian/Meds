import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BatchResultsInputComponent } from './batch-results-input.component';

describe('BatchResultsInputComponent', () => {
  let component: BatchResultsInputComponent;
  let fixture: ComponentFixture<BatchResultsInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BatchResultsInputComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BatchResultsInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
