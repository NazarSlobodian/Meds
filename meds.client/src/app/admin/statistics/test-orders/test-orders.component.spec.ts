import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestOrdersComponent } from './test-orders.component';

describe('TestOrdersComponent', () => {
  let component: TestOrdersComponent;
  let fixture: ComponentFixture<TestOrdersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TestOrdersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestOrdersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
