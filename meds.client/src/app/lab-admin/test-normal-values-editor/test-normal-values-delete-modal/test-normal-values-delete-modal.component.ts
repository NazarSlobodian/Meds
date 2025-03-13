import { Component, EventEmitter, Output } from '@angular/core';
import { AdminTestService } from '../../services/admin-test.service';

@Component({
  selector: 'app-test-normal-values-delete-modal',
  templateUrl: './test-normal-values-delete-modal.component.html',
  styleUrl: './test-normal-values-delete-modal.component.css'
})
export class TestNormalValuesDeleteModalComponent {
  testNormalValuesId: number = -1;
  isOpen = false;
  errorMessage: string | null = null;
  @Output() deleteSuccessful = new EventEmitter<void>();

  constructor(private adminTestService: AdminTestService) { }

  openModal(testNormalValuesId: number) {
    this.testNormalValuesId = testNormalValuesId;
    this.isOpen = true;
  }
  closeModal() {
    this.isOpen = false;
    this.errorMessage = null;
  }
  submitDelete() {
    this.adminTestService.deleteTestNormalValue(this.testNormalValuesId)
      .subscribe(
        (response) => {
          this.deleteSuccessful.emit();
          this.closeModal();
        },
        (error) => {
          this.errorMessage = error.error.message;
        }
      )
  }
}
