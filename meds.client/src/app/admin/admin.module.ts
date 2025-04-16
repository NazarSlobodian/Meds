import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LabAdminRoutingModule } from './admin-routing.module';
import { TestEditorComponent } from './test-editor/test-editor.component';
import { TestGeneralEditorComponent } from './test-general-editor/test-general-editor.component';
import { TestNormalValuesEditorComponent } from './test-normal-values-editor/test-normal-values-editor.component';
import { FormsModule } from '@angular/forms';
import { TestGeneralAddComponent } from './test-general-add/test-general-add.component';
import { TestNormalValuesEditModalComponent } from './test-normal-values-editor/test-normal-values-edit-modal/test-normal-values-edit-modal.component';
import { TestNormalValuesDeleteModalComponent } from './test-normal-values-editor/test-normal-values-delete-modal/test-normal-values-delete-modal.component';
import { TestNormalValuesAddModalComponent } from './test-normal-values-editor/test-normal-values-add-modal/test-normal-values-add-modal.component';
import { StatisticsComponent } from './statistics/statistics.component';
import { TestOrdersComponent } from './statistics/test-orders/test-orders.component';
import { RevenueComponent } from './statistics/revenue/revenue.component';
import { ClientDistributionComponent } from './statistics/client-distribution/client-distribution.component';
import { SharedModule } from '../shared/shared.module';
import { ActivityLogsComponent } from './activity-logs/activity-logs.component';



@NgModule({
  declarations: [
    DashboardComponent,
    TestEditorComponent,
    TestGeneralEditorComponent,
    TestNormalValuesEditorComponent,
    TestGeneralAddComponent,
    TestNormalValuesEditModalComponent,
    TestNormalValuesDeleteModalComponent,
    TestNormalValuesAddModalComponent,
    StatisticsComponent,
    TestOrdersComponent,
    RevenueComponent,
    ClientDistributionComponent,
    ActivityLogsComponent,
  ],
  imports: [
    CommonModule,
    LabAdminRoutingModule,
    FormsModule,
    SharedModule
  ]
})
export class AdminModule { }
