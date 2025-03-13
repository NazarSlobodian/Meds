import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PatientListComponent } from './patient-list/patient-list.component';
import { AddOrderComponent } from './add-order/add-order.component';
import { TechnicianRoutingModule } from './technician-routing.module';
import { FormsModule } from '@angular/forms';
import { PatientAddModalComponent } from './patient-list/patient-add-modal/patient-add-modal.component';


@NgModule({
  declarations: [
    PatientListComponent,
    AddOrderComponent,
    PatientAddModalComponent
  ],
  imports: [

    CommonModule,
    FormsModule,
    TechnicianRoutingModule
  ]
})
export class TechnicianModule { }
