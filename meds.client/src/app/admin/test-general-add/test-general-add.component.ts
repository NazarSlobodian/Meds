import { Component, EventEmitter, Output } from '@angular/core';
import { AdminTestService } from '../services/admin-test.service';
type TestType = {
  name: string | null;
  cost: number | null;
  measurementsUnit: string | null;
};

@Component({
  selector: 'app-test-general-add',
  templateUrl: './test-general-add.component.html',
  styleUrl: './test-general-add.component.css'
})
export class TestGeneralAddComponent {

  testType: TestType = {
    name: null,
    cost: null,
    measurementsUnit: null
  };

  isOpen = false;
  errorMessage: string | null = null;
  @Output() additionSuccessful = new EventEmitter<void>();

  constructor(private adminTestService: AdminTestService) { }
  openModal() {
    this.isOpen = true;
  }
  closeModal() {
    this.isOpen = false;
  }

  submitAdd() {
    this.adminTestService.addTestType(this.testType)
      .subscribe(
        (response) => {
          this.additionSuccessful.emit();
          this.closeModal();
        },
        (error) => {
          alert("Couldn't add the test");
          this.errorMessage = error.error.message;
        }
      )
    this.closeModal();
  }
}
