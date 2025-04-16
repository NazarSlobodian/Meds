import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ActivityLogsService } from './services/activity-logs.service';

@Component({
  selector: 'app-activity-logs',
  templateUrl: './activity-logs.component.html',
  styleUrl: './activity-logs.component.css'
})
export class ActivityLogsComponent {
  begin: Date = new Date(2025, 3, 1, 0, 0, 0, 0);
  end: Date = new Date(2025, 4, 1, 0, 0, 0, 0);

  logs: any[] = [];

  pageSize = 20;
  currentPage = 1;
  totalPages = 1;
  totalCount = 0;

  errorMessage: string | null = null;

  columnNames: string[] = [];
  constructor(private router: Router, private activityLogsService: ActivityLogsService) { }

  ngOnInit() {

  }

  loadLogs() {
    this.activityLogsService.getActivityLogs(this.begin, this.end, this.currentPage, this.pageSize).subscribe({
      next: (response) => {
        this.logs = response.list;
        this.totalPages = Math.ceil(response.totalCount / this.pageSize);
        this.totalCount = response.totalCount;
        this.errorMessage = null;
        this.columnNames = Object.keys(this.logs[0]);
      },
      error: (error) => {
        this.logs = [];
        this.totalPages = 1;
        this.totalCount = 0;
        this.currentPage = 1;
        this.errorMessage = error.error.message;
      }
    });
  }
  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadLogs();
    if (this.currentPage > this.totalPages)
      this.currentPage = this.totalPages;
  }

  goBack(): void {
    this.router.navigate(["/admin/options/"]);
  }
}
