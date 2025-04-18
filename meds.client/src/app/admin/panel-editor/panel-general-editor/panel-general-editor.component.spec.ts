import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PanelGeneralEditorComponent } from './panel-general-editor.component';

describe('PanelGeneralEditorComponent', () => {
  let component: PanelGeneralEditorComponent;
  let fixture: ComponentFixture<PanelGeneralEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PanelGeneralEditorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PanelGeneralEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
