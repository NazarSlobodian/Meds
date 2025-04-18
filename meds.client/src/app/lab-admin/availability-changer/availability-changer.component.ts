import { Component } from '@angular/core';
import { AvailabilityService } from '../services/availability.service';

@Component({
  selector: 'app-availability-changer',
  templateUrl: './availability-changer.component.html',
  styleUrl: './availability-changer.component.css'
})
export class AvailabilityChangerComponent {
  allTestTypes: any[] = [];
  testTypes: any[] = [];


  errorMessage: string | null = null;
  pageSize = 9;
  currentPage = 1;
  totalPages = 1;
  totalCount = 0;
  constructor(private availabilityService: AvailabilityService) { }

  ngOnInit() {
    this.load();
  }
  load() {
    this.availabilityService.getTestTypes().subscribe({
      next: (response) => {
        this.allTestTypes = response;
        this.totalCount = this.allTestTypes.length;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.testTypes = this.allTestTypes.slice((this.currentPage - 1) * this.pageSize, this.currentPage * this.pageSize);
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage = error.error.message;
        this.allTestTypes = [];
        this.testTypes = [];
        this.totalPages = 1;
        this.totalCount = 0;
        this.currentPage = 1;
      }
    })
  }
  onPageChange(page: number): void {
    this.currentPage = page;
    this.testTypes = this.allTestTypes.slice((this.currentPage - 1) * this.pageSize, this.currentPage * this.pageSize);
    if (this.currentPage > this.totalPages) {
      this.currentPage = this.totalPages;
    }
  }
  save() {
    this.availabilityService.setTestTypes(this.allTestTypes).subscribe({
      next: () => { alert("Availibility updated!"); this.load() },
      error: (error) => { this.errorMessage = error.error.message }
    });
  } 
}
