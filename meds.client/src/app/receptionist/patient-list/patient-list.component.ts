import { Component } from '@angular/core';
import { PatientsService } from '../services/patients.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-patient-list',
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.css'
})
export class PatientListComponent {
  searchQuery: string | null = null;
  patients: any[] = [];
  columnNames: string[] = [];
  searchType: string = 'name';
  errorMessage: string | null = null;

  constructor(private patientsSevice: PatientsService, private router: Router) { }

  onSearch() {
    if (!this.searchQuery || this.searchQuery.trim().length === 0) {
      this.errorMessage = "Please, enter a query";
      this.patients = [];
      return;
    }
    if (this.searchType === 'name') {
      this.patientsSevice.getPatientsByName(this.searchQuery)
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
    } else if (this.searchType === 'phone') {
      this.patientsSevice.getPatientsByNumber(this.searchQuery)
        .subscribe(
          (response) => {
            this.patients = response;
            this.errorMessage = null;
            this.columnNames = Object.keys(this.patients[0]);
          },
          (error) => {
            this.errorMessage = error.message;
            this.patients = [];
          }
        );
    } else {
      this.errorMessage = "unsupported search type";
    }

  }
  onPatientSelect(id: number) {
    this.router.navigate([`/receptionist/patientView/addBatch/${id}`]);
  }
  onAddSuccess() {

  }
}
