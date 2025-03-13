import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestNormalValuesEditorComponent } from './test-normal-values-editor.component';

describe('TestNormalValuesEditorComponent', () => {
  let component: TestNormalValuesEditorComponent;
  let fixture: ComponentFixture<TestNormalValuesEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TestNormalValuesEditorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestNormalValuesEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
