import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';

const routes: Routes = [
  { path: 'login', loadChildren: () => import('./login/login.module').then(m => m.LoginModule) },
  { path: 'patient', loadChildren: () => import('./patient/patient.module').then(m => m.PatientModule), canActivate: [authGuard] },
  { path: 'technician', loadChildren: () => import('./technician/technician.module').then(m => m.TechnicianModule), canActivate: [authGuard] },
  { path: 'lab-admin', loadChildren: () => import('./lab-admin/lab-admin.module').then(m => m.LabAdminModule), canActivate: [authGuard] },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
