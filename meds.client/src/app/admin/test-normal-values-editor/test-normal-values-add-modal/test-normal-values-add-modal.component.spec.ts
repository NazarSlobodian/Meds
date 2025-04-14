import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestNormalValuesAddModalComponent } from './test-normal-values-add-modal.component';

describe('TestNormalValuesAddModalComponent', () => {
  let component: TestNormalValuesAddModalComponent;
  let fixture: ComponentFixture<TestNormalValuesAddModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TestNormalValuesAddModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestNormalValuesAddModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
