import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginFormComponent } from './login-form/login-form.component';
import { FormsModule } from '@angular/forms';
import { LoginRoutingModule } from './login-routing.module';
import { EmailInputComponent } from './login-form/email-input/email-input.component';
import { CodeInputComponent } from './login-form/code-input/code-input.component'; 

@NgModule({
  declarations: [
    LoginFormComponent,
    EmailInputComponent,
    CodeInputComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    LoginRoutingModule
  ]
})
export class LoginModule { }
