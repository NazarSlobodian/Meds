import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientAddModalComponent } from './patient-add-modal.component';

describe('PatientAddModalComponent', () => {
  let component: PatientAddModalComponent;
  let fixture: ComponentFixture<PatientAddModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PatientAddModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PatientAddModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
