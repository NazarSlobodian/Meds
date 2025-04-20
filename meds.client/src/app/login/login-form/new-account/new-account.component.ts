import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../../../core/auth/auth.service';

@Component({
  selector: 'app-new-account',
  templateUrl: './new-account.component.html',
  styleUrl: './new-account.component.css'
})
export class NewAccountComponent {
  email: string = "";
  password: string = "";
  isOpen = false;
  errorMessage: string = "";
  @Output() codeEntered = new EventEmitter<string>();

  constructor(private authService: AuthService) { }

  openModal(email: string) {
    this.email = email;
    this.isOpen = true;
    this.errorMessage = "";
  }
  closeModal() {
    this.isOpen = false;
  }
  submitNewAccount() {
    this.authService.submitPassword(this.email, this.password)
      .subscribe(
        (response) => {
          alert("Account created. Log in to view your results.");
          this.closeModal();
        },
        (error) => {
          this.errorMessage = "Invalid code. Check it and try again";
        }
      )
  }
}
