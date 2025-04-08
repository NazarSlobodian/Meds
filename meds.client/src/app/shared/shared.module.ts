import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatusTransformPipe } from './pipes/status-transform.pipe';
import { HeaderComponent } from './header/header.component';
import { ThemeTogglerComponent } from './theme-toggler/theme-toggler.component';
import { PaginationComponent } from './pagination/pagination.component';



@NgModule({
  declarations: [
    StatusTransformPipe,
    HeaderComponent,
    ThemeTogglerComponent,
    PaginationComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    StatusTransformPipe,
    HeaderComponent,
    ThemeTogglerComponent,
    PaginationComponent
  ]
})
export class SharedModule { }
