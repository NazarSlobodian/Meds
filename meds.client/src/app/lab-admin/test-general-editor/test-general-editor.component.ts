import { Component, EventEmitter, Output } from '@angular/core';
import { AdminTestService } from '../services/admin-test.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-test-general-editor',
  templateUrl: './test-general-editor.component.html',
  styleUrl: './test-general-editor.component.css'
})
export class TestGeneralEditorComponent {
  testType: any;
  isOpen = false;
  errorMessage: string | null = null;
  @Output() updateSuccessful = new EventEmitter<void>();

  constructor(private adminTestService: AdminTestService) { }

  openModal(testType: any) {
    this.testType = { ...testType };
    this.isOpen = true;
  }
  closeModal() {
    this.isOpen = false;
    this.errorMessage = null;
  }
  submitEdit() {
    this.adminTestService.updateTestInfo(this.testType)
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
