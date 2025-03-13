import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BatchesListComponent } from './batches-list/batches-list.component';
import { BatchDetailComponent } from './batch-detail/batch-detail.component';
import { PatientRoutingModule } from "./patient-routing.module"
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    BatchesListComponent,
    BatchDetailComponent
  ],
  imports: [
    CommonModule,
    PatientRoutingModule,
    SharedModule
  ]
})
export class PatientModule { }
