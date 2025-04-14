import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from '../core/auth/auth.guard';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TestEditorComponent } from './test-editor/test-editor.component';
import { TestNormalValuesEditorComponent } from './test-normal-values-editor/test-normal-values-editor.component';
import { StatisticsComponent } from './statistics/statistics.component';
import { RevenueComponent } from './statistics/revenue/revenue.component';
import { TestOrdersComponent } from './statistics/test-orders/test-orders.component';
import { ClientDistributionComponent } from './statistics/client-distribution/client-distribution.component';

const routes: Routes = [
  { path: 'options', component: DashboardComponent, canActivate: [authGuard] },
  { path: 'options/editTest', component: TestEditorComponent, canActivate: [authGuard] },
  { path: 'options/editTest/:id/normal-values', component: TestNormalValuesEditorComponent, canActivate: [authGuard] },
  { path: 'options/statistics', component: StatisticsComponent, canActivate: [authGuard] },
  { path: 'options/statistics/revenue', component: RevenueComponent, canActivate: [authGuard] },
  { path: 'options/statistics/test-orders', component: TestOrdersComponent, canActivate: [authGuard] },
  { path: 'options/statistics/client-distribution', component: ClientDistributionComponent, canActivate: [authGuard] },
  { path: '', redirectTo: "options", pathMatch: "full" },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LabAdminRoutingModule { }
