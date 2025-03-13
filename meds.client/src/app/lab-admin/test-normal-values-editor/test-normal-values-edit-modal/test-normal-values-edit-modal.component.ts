import { Component, EventEmitter, Output } from '@angular/core';
import { AdminTestService } from '../../services/admin-test.service';

@Component({
  selector: 'app-test-normal-values-edit-modal',
  templateUrl: './test-normal-values-edit-modal.component.html',
  styleUrl: './test-normal-values-edit-modal.component.css'
})
export class TestNormalValuesEditModalComponent {
  testNormalValue: any;
  isOpen = false;
  errorMessage: string | null = null;
  @Output() updateSuccessful = new EventEmitter<void>();

  constructor(private adminTestService: AdminTestService) { }

  openModal(testNormalValue: any) {
    this.testNormalValue = { ...testNormalValue };
    this.isOpen = true;
  }
  closeModal() {
    this.isOpen = false;
    this.errorMessage = null;
  }
  submitEdit() {
    this.adminTestService.updateTestNormalValue(this.testNormalValue)
      .subscribe(
        (response) => {
          this.updateSuccessful.emit();
          this.closeModal();
        },
        (error) => {
          this.errorMessage = error.error.message;
        }
      )
  }
}
