import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestGeneralAddComponent } from './test-general-add.component';

describe('TestGeneralAddComponent', () => {
  let component: TestGeneralAddComponent;
  let fixture: ComponentFixture<TestGeneralAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TestGeneralAddComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestGeneralAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
