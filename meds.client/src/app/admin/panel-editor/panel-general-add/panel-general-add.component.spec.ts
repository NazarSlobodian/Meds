import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PanelGeneralAddComponent } from './panel-general-add.component';

describe('PanelGeneralAddComponent', () => {
  let component: PanelGeneralAddComponent;
  let fixture: ComponentFixture<PanelGeneralAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PanelGeneralAddComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PanelGeneralAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
