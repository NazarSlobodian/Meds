import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { StatisticsService } from '../services/statistics.service';

@Component({
  selector: 'app-test-orders',
  templateUrl: './test-orders.component.html',
  styleUrl: './test-orders.component.css'
})
export class TestOrdersComponent {
  constructor(private router: Router, private statisticsService: StatisticsService) { }

  goBack(): void {
    this.router.navigate(["/admin/options/statistics"]);
  }
  ngOnInit(): void {
    this.load();
  }
  load(): void {
    this.statisticsService.getYearlyRevenue().subscribe({
      next: (response) => {

      },
      error: (error) => {
        alert(error.message);
      }
    });
  }
}
