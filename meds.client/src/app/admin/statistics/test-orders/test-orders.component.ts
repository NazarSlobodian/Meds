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
  allStats: any[] = [];

  selectedCategoryStats: any[] = [];

  availableCategories: string[] = [];
  availableYears: number[] = [];
  availableMonths: number[] = [];


  selectedCategory: string | null = null;
  selectedYear: number | null = null;
  selectedMonth: number | null = null;

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
        this.allStats = response;
        this.availableCategories = this.allStats.map(x => x.name);
        this.selectedCategory = this.availableCategories[this.availableCategories.length - 1];
        this.updateYears();
      },
      error: (error) => {
        alert(error.message);
        this.goBack()
      }
    });
  }
  updateYears(): void {
    this.selectedCategoryStats = this.allStats.find(s => s.name == this.selectedCategory).list;

    this.availableYears = this.selectedCategoryStats?.map((m: any) => m.year) || [];
    this.selectedYear = null;
    this.updateMonths();

  }
  updateMonths(): void {
    const yearStats = this.selectedCategoryStats.find(s => s.year == this.selectedYear);
    this.availableMonths = yearStats?.stats.map((m: any) => m.month) || [];
    this.selectedMonth = null;
    this.updateChart();
  }
  updateChart(): void {
    if (!this.selectedCategoryStats) {
      return;
    }
    let statsToAggregate: any[] = [];
    let yearData: any[] = [];
    if (this.selectedYear != null) {
      yearData = this.selectedCategoryStats.find(y => y.year == this.selectedYear).stats;
    }
    else {
      yearData = this.selectedCategoryStats.flatMap((x => x.stats));
    }

    if (this.selectedMonth == null) {
      // Aggregate all months
      statsToAggregate = yearData.flatMap((monthStat: any) => monthStat.stats);
    } else {
      const monthData = yearData.find((stat: any) => stat.month == this.selectedMonth);
      statsToAggregate = monthData ? monthData.stats : [];
    }


    const countMap = new Map<string, number>();
    statsToAggregate.forEach(stat => {
      countMap.set(stat.name, (countMap.get(stat.name) || 0) + stat.count);
    });

    console.log(countMap);
    let chartData = Array.from(countMap.entries()).map(([name, value]) => ({ name, value }));



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
          label: '# of orders',
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
