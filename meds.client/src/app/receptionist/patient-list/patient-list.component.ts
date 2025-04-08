import { Component } from '@angular/core';
import { PatientsService } from '../services/patients.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-patient-list',
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.css'
})
export class PatientListComponent {
  searchQueryName: string | null = null;
  searchQueryPhone: string | null = null;
  searchQueryEmail: string | null = null;
  searchQueryDateOfBirth: string | null = null;
  patients: any[] = [];
  columnNames: string[] = [];
  errorMessage: string | null = null;

  pageSize = 5;
  currentPage = 2;
  totalPages = 999;
  constructor(private patientsSevice: PatientsService, private router: Router) { }

  onSearch() {
    if ((!this.searchQueryName || this.searchQueryName.trim().length === 0) &&
      (!this.searchQueryPhone || this.searchQueryPhone.trim().length === 0) &&
      (!this.searchQueryEmail || this.searchQueryEmail.trim().length === 0) &&
      (!this.searchQueryDateOfBirth || this.searchQueryDateOfBirth.trim().length === 0)) {
      this.errorMessage = "Please, fill in at least one query field";
      this.patients = [];
      return;
    }
    this.patientsSevice.getPatients(this.searchQueryName, this.searchQueryPhone, this.searchQueryEmail, this.searchQueryDateOfBirth)
      .subscribe(
        (response) => {
          this.patients = response;
          this.errorMessage = null;
          this.columnNames = Object.keys(this.patients[0]);
        },
        (error) => {
          this.errorMessage = error.error.message;
          this.patients = [];
        }
      );
  }
  onPatientSelect(id: number) {
    this.router.navigate([`/receptionist/patientView/addBatch/${id}`]);
  }
  onAddSuccess(): void {

  }
  onPageChange(page: number): void {
    this.currentPage = page;
  }
}
