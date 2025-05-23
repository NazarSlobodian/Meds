import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LabWorkerRoutingModule } from './lab-worker-routing.module';
import { BatchesListLabWorkerComponent } from './batches-list-lab-worker/batches-list-lab-worker.component';
import { BatchResultsInputComponent } from './batches-list-lab-worker/batch-results-input/batch-results-input.component';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [
    BatchesListLabWorkerComponent,
    BatchResultsInputComponent
  ],
  imports: [
    CommonModule,
    LabWorkerRoutingModule,
    FormsModule,
    SharedModule
  ]
})
export class LabWorkerModule { }
