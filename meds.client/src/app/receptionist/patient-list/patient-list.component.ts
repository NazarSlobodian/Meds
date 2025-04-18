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

  frozenName: string | null = null;
  frozenPhone: string | null = null;
  frozenEmail: string | null = null;
  frozenDateOfBirth: string | null = null;

  patients: any[] = [];
  columnNames: string[] = [];
  errorMessage: string | null = null;

  pageSize = 20;
  currentPage = 1;
  totalPages = 1;
  totalCount = 0;
  constructor(private patientsService: PatientsService, private router: Router) { }

  onSearch() {
    if ((!this.searchQueryName || this.searchQueryName.trim().length === 0) &&
      (!this.searchQueryPhone || this.searchQueryPhone.trim().length === 0) &&
      (!this.searchQueryEmail || this.searchQueryEmail.trim().length === 0) &&
      (!this.searchQueryDateOfBirth || this.searchQueryDateOfBirth.trim().length === 0)) {
      this.errorMessage = "Please, fill in at least one query field";
      this.patients = [];
      return;
    }

    this.frozenName = this.searchQueryName;
    this.frozenPhone = this.searchQueryPhone;
    this.frozenEmail = this.searchQueryEmail;
    this.frozenDateOfBirth = this.searchQueryDateOfBirth;

    this.currentPage = 1;
    this.search();
  }
  onPatientSelect(id: number, name: string) {
    this.router.navigate([`/receptionist/patientView/addBatch/${id}`], {
      queryParams: {name:name}
    });
  }
  onPatientDelete(id: number) {
    const confirmed = confirm("Are you sure?");
    if (!confirmed) {
      return;
    }
    this.patientsService.deletePatient(id).subscribe({
      next: () => { this.search() },
      error: (error) => { alert(error.error.message); }
    });
  }
  onAddSuccess(): void {

  }
  onPageChange(page: number): void {
    this.currentPage = page;
    this.search();
    if (this.currentPage > this.totalPages)
      this.currentPage = this.totalPages;
  }
  private search() {
    this.patientsService.getPatients(this.frozenName, this.frozenPhone, this.frozenEmail, this.frozenDateOfBirth, this.currentPage, this.pageSize)
      .subscribe(
        (response) => {
          this.patients = response.list;
          this.totalPages = Math.ceil(response.totalCount / this.pageSize);
          this.totalCount = response.totalCount;
          this.errorMessage = null;
          if (this.currentPage > this.totalPages) {
            this.currentPage = 1;
            if (this.totalCount != 0) {
              this.search();
            }
          }
        },
        (error) => {
          this.patients = [];
          this.totalPages = 1;
          this.totalCount = 0;
          this.currentPage = 1;
          this.errorMessage = error.error.message;
        }
      );
  }
}
