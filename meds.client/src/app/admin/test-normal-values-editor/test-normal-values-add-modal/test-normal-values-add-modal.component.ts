import { Component, EventEmitter, Output } from '@angular/core';
import { AdminTestService } from '../../services/admin-test.service';

@Component({
  selector: 'app-test-normal-values-add-modal',
  templateUrl: './test-normal-values-add-modal.component.html',
  styleUrl: './test-normal-values-add-modal.component.css'
})
export class TestNormalValuesAddModalComponent {
  testNormalValue = {
    testTypeId: -1,
    minAge: 0,
    maxAge: 18,
    gender: 'f',
    minResValue: 0.0,
    maxResValue: 10.0
  };
  isOpen = false;
  errorMessage: string | null = null;
  @Output() addSuccessful = new EventEmitter<void>();

  constructor(private adminTestService: AdminTestService) { }
  openModal(testTypeId: number) {
    this.testNormalValue.testTypeId = testTypeId;
    this.isOpen = true;
  }
  closeModal() {
    this.testNormalValue.testTypeId = -1;
    this.isOpen = false;
    this.errorMessage = null
  }

  submitAdd() {
    this.adminTestService.addTestNormalValue(this.testNormalValue)
      .subscribe(
        (response) => {
          this.addSuccessful.emit();
          this.closeModal();
        },
        (error) => {
          this.errorMessage = error.error.message;
        }
      )
  }
}
