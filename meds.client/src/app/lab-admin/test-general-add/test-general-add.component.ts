import { Component, EventEmitter, Output } from '@angular/core';
import { AdminTestService } from '../services/admin-test.service';

@Component({
  selector: 'app-test-general-add',
  templateUrl: './test-general-add.component.html',
  styleUrl: './test-general-add.component.css'
})
export class TestGeneralAddComponent {
  testType = {
    name: "name",
    cost: 0,
    daysTillOverdue: 1,
    measurementsUnit: "kg/g"
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
          this.errorMessage = error.error.message;
        }
      )
    this.closeModal();
  }
}
