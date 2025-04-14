import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';

const routes: Routes = [
  { path: 'login', loadChildren: () => import('./login/login.module').then(m => m.LoginModule) },
  { path: 'patient', loadChildren: () => import('./patient/patient.module').then(m => m.PatientModule), canActivate: [authGuard] },
  { path: 'receptionist', loadChildren: () => import('./receptionist/receptionist.module').then(m => m.ReceptionistModule), canActivate: [authGuard] },
  { path: 'lab-worker', loadChildren: () => import('./lab-worker/lab-worker.module').then(m => m.LabWorkerModule), canActivate: [authGuard] },
  { path: 'admin', loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule), canActivate: [authGuard] },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
