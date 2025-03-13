import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatusTransformPipe } from './pipes/status-transform.pipe';
import { HeaderComponent } from './header/header.component';
import { ThemeTogglerComponent } from './theme-toggler/theme-toggler.component';



@NgModule({
  declarations: [
    StatusTransformPipe,
    HeaderComponent,
    ThemeTogglerComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    StatusTransformPipe,
    HeaderComponent,
    ThemeTogglerComponent
  ]
})
export class SharedModule { }
