import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ExportService } from '../services/export.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

  constructor(private router: Router, private exportService: ExportService) { }
  goToTestEdit() {
    this.router.navigate(["/admin/options/editTest"]);
  }
  goToPanelEdit() {
    this.router.navigate(["/admin/options/editPanel"]);
  }
  goToStats() {
    this.router.navigate(["/admin/options/statistics"]);
  }
  goToActivityLogs() {
    this.router.navigate(["/admin/options/activityLogs"]);
  }
  getBatchesJson() {
    this.exportService.getBatchesJson().subscribe({
      next: (response: Blob) => {
        const link = document.createElement('a');
        const url = window.URL.createObjectURL(response);

        link.href = url;
        link.download = 'data.json';

        link.click();

        window.URL.revokeObjectURL(url);
      },
      error: () => { alert("Couldn't get json")}
    })
  }
}
