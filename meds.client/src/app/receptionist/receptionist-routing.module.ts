import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PatientListComponent } from "./patient-list/patient-list.component";
import { AddOrderComponent } from "./add-order/add-order.component"
import { authGuard } from '../core/auth/auth.guard';

const routes: Routes = [
  { path: 'patientView', component: PatientListComponent, canActivate: [authGuard] },
  { path: "patientView/addBatch/:id", component: AddOrderComponent, canActivate: [authGuard] },
  { path: '', redirectTo: "patientView", pathMatch: "full" },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReceptionistRoutingModule { }
