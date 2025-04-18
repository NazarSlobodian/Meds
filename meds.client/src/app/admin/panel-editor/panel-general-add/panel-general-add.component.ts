import { Component, EventEmitter, Output } from '@angular/core';
import { AdminTestService } from '../../services/admin-test.service';
type TestPanel = {
  name: string | null;
  cost: number | null;
};
@Component({
  selector: 'app-panel-general-add',
  templateUrl: './panel-general-add.component.html',
  styleUrl: './panel-general-add.component.css'
})
export class PanelGeneralAddComponent {
  testPanel: TestPanel = {
    name: null,
    cost: null,
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
    this.adminTestService.addTestPanel(this.testPanel)
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
