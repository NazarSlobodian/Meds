import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AvailabilityChangerComponent } from './availability-changer/availability-changer.component';
import { authGuard } from '../core/auth/auth.guard';

const routes: Routes = [
  { path: 'availabilityChanger', component: AvailabilityChangerComponent, canActivate: [authGuard] },
  { path: '', redirectTo: "availabilityChanger", pathMatch: "full" },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LabAdminRoutingModule { }
