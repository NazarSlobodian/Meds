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
  loginError = false;

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
            else if (response.role === "admin") {
              this.router.navigate(["/lab-admin"]);
            }
            else {
              console.error("Role not implemented");
            }
          }
        },
        (error) => {
          this.loginError = true;
          console.error(error);
        }
      );
  }
}
