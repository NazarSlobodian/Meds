import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from '../core/auth/auth.guard';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TestEditorComponent } from './test-editor/test-editor.component';
import { TestNormalValuesEditorComponent } from './test-normal-values-editor/test-normal-values-editor.component';

const routes: Routes = [
  { path: 'options', component: DashboardComponent, canActivate: [authGuard] },
  { path: 'options/editTest', component: TestEditorComponent, canActivate: [authGuard] },
  { path: 'options/editTest/:id/normal-values', component: TestNormalValuesEditorComponent, canActivate: [authGuard] },
  { path: '', redirectTo: "options", pathMatch: "full" },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LabAdminRoutingModule { }
