import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientDistributionComponent } from './client-distribution.component';

describe('ClientDistributionComponent', () => {
  let component: ClientDistributionComponent;
  let fixture: ComponentFixture<ClientDistributionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ClientDistributionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClientDistributionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
