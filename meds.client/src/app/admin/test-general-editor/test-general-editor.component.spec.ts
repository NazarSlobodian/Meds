import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestGeneralEditorComponent } from './test-general-editor.component';

describe('TestGeneralEditorComponent', () => {
  let component: TestGeneralEditorComponent;
  let fixture: ComponentFixture<TestGeneralEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TestGeneralEditorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestGeneralEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
