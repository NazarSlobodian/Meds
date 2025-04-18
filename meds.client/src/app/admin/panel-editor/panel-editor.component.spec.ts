import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PanelEditorComponent } from './panel-editor.component';

describe('PanelEditorComponent', () => {
  let component: PanelEditorComponent;
  let fixture: ComponentFixture<PanelEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PanelEditorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PanelEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
