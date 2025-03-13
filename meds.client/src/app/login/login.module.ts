import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginFormComponent } from './login-form/login-form.component';
import { FormsModule } from '@angular/forms';
import { LoginRoutingModule } from './login-routing.module'; 

@NgModule({
  declarations: [
    LoginFormComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    LoginRoutingModule
  ]
})
export class LoginModule { }
