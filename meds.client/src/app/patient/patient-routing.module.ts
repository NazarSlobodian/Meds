import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BatchesListComponent } from "./batches-list/batches-list.component";
import { BatchDetailComponent } from "./batch-detail/batch-detail.component"
import { authGuard } from '../core/auth/auth.guard';

const routes: Routes = [
  { path: 'batches', component: BatchesListComponent, canActivate: [authGuard] },
  { path: "batches/:id", component: BatchDetailComponent, canActivate: [authGuard] },
  { path: '', redirectTo: "batches", pathMatch:"full" },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PatientRoutingModule { }
