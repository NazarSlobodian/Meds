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
  selector: 'app-test-orders',
  templateUrl: './test-orders.component.html',
  styleUrl: './test-orders.component.css'
})
export class TestOrdersComponent {
  separateTests: any[] = [];
  panels: any[] = [];

  choices: string[] = ["Tests", "Panels"];
  choice: string | null = null;

  myChart: Chart | null = null;

  constructor(private router: Router, private statisticsService: StatisticsService) { }
  goBack(): void {
    this.router.navigate(["/admin/options/statistics"]);
  }
  ngOnInit(): void {
    this.load();
  }
  load(): void {
    this.statisticsService.getTestOrdersNumbers().subscribe({
      next: (response) => {
        this.separateTests = response.separate;
        this.panels = response.panels;
        this.choice = this.choices[0];
        this.updateChart();
      },
      error: (error) => {
        alert(error.message);
        this.goBack()
      }
    });
  }
  updateChart(): void {
    const chartData = (this.choice == "Tests" ? this.separateTests : this.panels);

    if (!chartData) {
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
        labels: chartData.map(x => x.name),
        datasets: [{
          label: '# of Votes',
          data: chartData.map(x => x.value),
          borderWidth: 1,
          backgroundColor: 'rgba(54, 162, 235, 0.6)',
          borderColor: 'rgba(0, 94, 141, 0.8)'
        }
        ]
      },
      options: {
        indexAxis: 'y',
        scales: {
          y: {
            beginAtZero: true
          }
        }
      }
    });
  }
}
