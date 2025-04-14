import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { StatisticsService } from '../services/statistics.service';

@Component({
  selector: 'app-client-distribution',
  templateUrl: './client-distribution.component.html',
  styleUrl: './client-distribution.component.css'
})
export class ClientDistributionComponent {
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
