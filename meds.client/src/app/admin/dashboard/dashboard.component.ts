import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

  constructor(private router: Router) { }
  goToTestEdit() {
    this.router.navigate(["/admin/options/editTest"]);
  }
  goToStats() {
    this.router.navigate(["/admin/options/statistics"]);
  }

}
