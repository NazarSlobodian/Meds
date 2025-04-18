import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PanelContentsEditorComponent } from './panel-contents-editor.component';

describe('PanelContentsEditorComponent', () => {
  let component: PanelContentsEditorComponent;
  let fixture: ComponentFixture<PanelContentsEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PanelContentsEditorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PanelContentsEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
