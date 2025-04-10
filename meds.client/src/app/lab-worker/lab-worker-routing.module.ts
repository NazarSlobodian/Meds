import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { BatchesListLabWorkerComponent } from './batches-list-lab-worker/batches-list-lab-worker.component';


const routes: Routes = [
  { path: 'batches', component: BatchesListLabWorkerComponent },
  { path: '', redirectTo: "batches", pathMatch:"full"}
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LabWorkerRoutingModule { }
