import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailabilityChangerComponent } from './availability-changer.component';

describe('AvailabilityChangerComponent', () => {
  let component: AvailabilityChangerComponent;
  let fixture: ComponentFixture<AvailabilityChangerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AvailabilityChangerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AvailabilityChangerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
