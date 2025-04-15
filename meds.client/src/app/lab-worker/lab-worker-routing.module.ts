import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { BatchesListLabWorkerComponent } from './batches-list-lab-worker/batches-list-lab-worker.component';
import { BatchResultsInputComponent } from './batches-list-lab-worker/batch-results-input/batch-results-input.component';
import { authGuard } from '../core/auth/auth.guard';


const routes: Routes = [
  { path: 'batches', component: BatchesListLabWorkerComponent },
  { path: "batches/:id/:mode", component: BatchResultsInputComponent, canActivate: [authGuard] },
  { path: '', redirectTo: "batches", pathMatch:"full"}
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LabWorkerRoutingModule { }
