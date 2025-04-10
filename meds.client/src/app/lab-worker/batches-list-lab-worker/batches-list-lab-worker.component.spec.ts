import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BatchesListLabWorkerComponent } from './batches-list-lab-worker.component';

describe('BatchesListLabWorkerComponent', () => {
  let component: BatchesListLabWorkerComponent;
  let fixture: ComponentFixture<BatchesListLabWorkerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BatchesListLabWorkerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BatchesListLabWorkerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
