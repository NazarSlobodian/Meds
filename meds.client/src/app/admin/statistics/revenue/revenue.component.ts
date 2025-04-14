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
  selector: 'app-revenue',
  templateUrl: './revenue.component.html',
  styleUrl: './revenue.component.css'
})
export class RevenueComponent {

  allStats: any[] = [];
  availableYears: number[] = [];
  availableMonths: number[] = [];

  selectedYear: number | null = null;
  selectedMonth: number | null = null;

  constructor(private router: Router, private statisticsService: StatisticsService) { }
  goBack(): void {
    this.router.navigate(["/admin/options/statistics"]);
  }
  ngOnInit(): void {
    this.load();


    const ctx = document.getElementById('myChart') as HTMLCanvasElement | null;
    if (!ctx)
      return;
    new Chart(ctx, {
      type: 'bar',
      data: {
        labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
        datasets: [{
          label: '# of Votes',
          data: [12, 19, 3, 5, 2, 3],
          borderWidth: 1
        }]
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
  load(): void {
    this.statisticsService.getYearlyRevenue().subscribe({
      next: (response) => {
        this.allStats = response;
        this.availableYears = this.allStats.map(s => s.year);
        this.selectedYear = this.availableYears[this.availableYears.length - 1];
        this.updateMonths();
      },
      error: (error) => {
        alert(error.message);
      }
    });
  }
  updateMonths(): void {
    const yearStats = this.allStats.find(s => s.year === this.selectedYear);
    this.availableMonths = yearStats?.stats.map((m: any) => m.month) || [];
    this.selectedMonth = null;
  }
  updateChart(): void {
    const yearData = this.allStats.find(y => y.year == this.selectedYear);
    if (!yearData) {
      return;
    }

    let statsToAggregate: any[] = [];

    if (this.selectedMonth == null) {
      // Aggregate all months
      statsToAggregate = yearData.stats.flatMap((monthStat: any) => monthStat.stats);
    } else {
      const monthData = yearData.stats.find((stat: any) => stat.month == this.selectedMonth);
      statsToAggregate = monthData ? monthData.stats : [];
    }

    const revenueMap = new Map<string, number>();
    statsToAggregate.forEach(stat => {
      revenueMap.set(stat.address, (revenueMap.get(stat.address) || 0) + stat.revenue);
    });

    console.log(revenueMap);
    //this.chartData = Array.from(revenueMap.entries()).map(([name, value]) => ({ name, value }));
  }
}
