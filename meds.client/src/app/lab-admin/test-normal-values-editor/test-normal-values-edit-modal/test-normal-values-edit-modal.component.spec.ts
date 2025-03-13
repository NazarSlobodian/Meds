import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestNormalValuesEditModalComponent } from './test-normal-values-edit-modal.component';

describe('TestNormalValuesEditModalComponent', () => {
  let component: TestNormalValuesEditModalComponent;
  let fixture: ComponentFixture<TestNormalValuesEditModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TestNormalValuesEditModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestNormalValuesEditModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
