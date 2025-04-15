import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../../../core/auth/auth.service';

@Component({
  selector: 'app-email-input',
  templateUrl: './email-input.component.html',
  styleUrl: './email-input.component.css'
})
export class EmailInputComponent {
  email: string ="";
  isOpen = false;

  @Output() emailEntered = new EventEmitter<string>();

  constructor(private authService: AuthService) { }

  openModal() {
    this.email = "";
    this.isOpen = true;
  }
  closeModal() {
    this.isOpen = false;
  }
  submitEmail() {
    this.authService.initRegistration(this.email)
      .subscribe(
        (response) => {
          this.emailEntered.emit(this.email);
          this.closeModal();
        },
        (error) => {
          this.closeModal();
        }
      )
  }
}
