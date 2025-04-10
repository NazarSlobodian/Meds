import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LabWorkerRoutingModule } from './lab-worker-routing.module';
import { BatchesListLabWorkerComponent } from './batches-list-lab-worker/batches-list-lab-worker.component';



@NgModule({
  declarations: [
    BatchesListLabWorkerComponent
  ],
  imports: [
    CommonModule,
    LabWorkerRoutingModule
  ]
})
export class LabWorkerModule { }
