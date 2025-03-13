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
    this.router.navigate(["/lab-admin/options/editTest"]);
  }
  goToAnotherEdit() {
    this.router.navigate(["/lab-admin/options/editTest"]);
  }

}
