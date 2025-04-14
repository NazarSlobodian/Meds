import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestNormalValuesDeleteModalComponent } from './test-normal-values-delete-modal.component';

describe('TestNormalValuesDeleteModalComponent', () => {
  let component: TestNormalValuesDeleteModalComponent;
  let fixture: ComponentFixture<TestNormalValuesDeleteModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TestNormalValuesDeleteModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestNormalValuesDeleteModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
