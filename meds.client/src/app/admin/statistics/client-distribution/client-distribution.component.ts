import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { StatisticsService } from '../services/statistics.service';
import {
  Chart,
  BarElement,
  CategoryScale,
  LinearScale,
  Title,
  Tooltip,
  Legend,
  BarController
} from 'chart.js';

// Register the required components
Chart.register(
  BarElement,
  CategoryScale,
  LinearScale,
  Title,
  Tooltip,
  Legend,
  BarController
);
@Component({
  selector: 'app-client-distribution',
  templateUrl: './client-distribution.component.html',
  styleUrl: './client-distribution.component.css'
})
export class ClientDistributionComponent {
  allStats: any[] = [];

  myChart: Chart | null = null;

  constructor(private router: Router, private statisticsService: StatisticsService) { }
  goBack(): void {
    this.router.navigate(["/admin/options/statistics"]);
  }
  ngOnInit(): void {
    this.load();
  }
  load(): void {
    this.statisticsService.getClientDistribution().subscribe({
      next: (response) => {
        this.allStats = response;
        this.updateChart();
      },
      error: (error) => {
        alert(error.message);
        this.goBack()
      }
    });
  }
  updateChart(): void {
    if (!this.allStats) {
      return;
    }


    const ctx = document.getElementById('myChart') as HTMLCanvasElement | null;
    if (!ctx)
      return;
    if (this.myChart)
      this.myChart.destroy();
    this.myChart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: this.allStats.filter(x => x.gender == "m").map(x => x.ageGroup),
        datasets: [{
          label: '# of male clients',
          data: this.allStats.filter(x => x.gender == "m").map(x => x.count),
          borderWidth: 1,
          backgroundColor: 'rgba(54, 162, 235, 0.6)',
          borderColor: 'rgba(0, 94, 141, 0.8)'
        },
        {
          label: '# of female clients',
          data: this.allStats.filter(x => x.gender == "f").map(x => x.count),
          borderWidth: 1,
          backgroundColor: 'rgba(245, 157, 203, 0.8)',
          borderColor: 'rgba(122, 0, 63, 0.8)'
        }
        ]
      },
      options: {
        indexAxis: 'x',
        scales: {
          y: {
            beginAtZero: true
          }
        }
      }
    });
  }
}
