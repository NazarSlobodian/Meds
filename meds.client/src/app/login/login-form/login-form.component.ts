import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';
@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.css'
})
export class LoginFormComponent {
  loginModel = {
    login: "",
    password: ""
  };
  errorMessage: string | null = null;
  constructor(private authService: AuthService, private router: Router) { }
  onSubmit() {
    this.authService.login(this.loginModel.login, this.loginModel.password)
      .subscribe(
        (response) => {
          if (response) {
            if (response.role === "patient") {
              this.router.navigate(['/patient']);
            }
            else if (response.role === 'receptionist') {
              this.router.navigate(['/receptionist']);
            }
            else if (response.role === "lab_worker") {
              this.router.navigate(['/lab-worker']);
            }
            else if (response.role === "admin") {
              this.router.navigate(["/admin"]);
            }
            else if (response.role === "lab_admin") {
              this.router.navigate(["/lab-admin"]);
            }
            else {
              console.error("Role not implemented");
            }
          }
        },
        (error) => {
          this.errorMessage = error.error.message;
        }
      );
  }
}
