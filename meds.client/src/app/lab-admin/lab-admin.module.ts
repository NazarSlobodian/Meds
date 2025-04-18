import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LabAdminRoutingModule } from './lab-admin-routing.module';
import { AvailabilityChangerComponent } from './availability-changer/availability-changer.component';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    AvailabilityChangerComponent
  ],
  imports: [
    CommonModule,
    LabAdminRoutingModule,
    FormsModule,
    SharedModule
  ]
})
export class LabAdminModule { }
