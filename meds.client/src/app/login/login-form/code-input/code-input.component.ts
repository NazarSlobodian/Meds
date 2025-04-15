import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../../../core/auth/auth.service';

@Component({
  selector: 'app-code-input',
  templateUrl: './code-input.component.html',
  styleUrl: './code-input.component.css'
})
export class CodeInputComponent {
  email: string = "";
  code: string = "";
  isOpen = false;
  errorMessage: string = "";
  @Output() codeEntered = new EventEmitter<string>();

  constructor(private authService: AuthService) { }

  openModal(email: string) {
    this.email = email;
    this.isOpen = true;
  }
  closeModal() {
    this.isOpen = false;
  }
  submitCode() {
    this.authService.submitCode(this.email, this.code)
      .subscribe(
        (response) => {
          this.codeEntered.emit(this.email);
          this.closeModal();
        },
        (error) => {
          this.errorMessage = "Invalid code. Check it and try again";
        }
      )
  }
}
