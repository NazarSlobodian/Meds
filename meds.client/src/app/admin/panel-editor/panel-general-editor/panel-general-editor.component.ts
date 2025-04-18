import { Component, EventEmitter, Output } from '@angular/core';
import { AdminTestService } from '../../services/admin-test.service';

@Component({
  selector: 'app-panel-general-editor',
  templateUrl: './panel-general-editor.component.html',
  styleUrl: './panel-general-editor.component.css'
})
export class PanelGeneralEditorComponent {
  testPanel: any;
  isOpen = false;
  errorMessage: string | null = null;
  @Output() updateSuccessful = new EventEmitter<void>();

  constructor(private adminTestService: AdminTestService) { }

  openModal(testType: any) {
    this.testPanel = { ...testType };
    this.isOpen = true;
  }
  closeModal() {
    this.isOpen = false;
    this.errorMessage = null;
  }
  submitEdit() {
    this.adminTestService.updatePanelInfo(this.testPanel)
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
