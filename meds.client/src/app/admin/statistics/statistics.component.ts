import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrl: './statistics.component.css'
})
export class StatisticsComponent {

  constructor(private router: Router) {}
  goToRevenue(): void {
    this.router.navigate(["/admin/options/statistics/revenue"]);
  }
  goToTestOrders(): void {
    this.router.navigate(["/admin/options/statistics/test-orders"]);
  }
  goToClientDistribution(): void {
    this.router.navigate(["/admin/options/statistics/client-distribution"]);
  }
  goBack(): void {
    this.router.navigate(["/admin/options/"]);
  }
}
