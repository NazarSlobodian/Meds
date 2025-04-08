import { Component, EventEmitter, Output } from '@angular/core';
import { PatientsService } from '../../services/patients.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-patient-add-modal',
  templateUrl: './patient-add-modal.component.html',
  styleUrl: './patient-add-modal.component.css'
})
export class PatientAddModalComponent {
  newPatient = {
    fullName: "",
    gender: "m",
    dateOfBirth: "",
    email: "",
    contactNumber: ""
  };
  isOpen = false;
  errorMessage: string | null = null;
  @Output() additionSuccessful = new EventEmitter<void>();

  constructor(private patientService: PatientsService, private router: Router) { }
  openModal() {
    this.isOpen = true;
  }
  closeModal() {
    this.isOpen = false;
  }

  submitAdd() {
    this.patientService.addPatient(this.newPatient)
      .subscribe(
        (response) => {
          this.additionSuccessful.emit();
          this.closeModal();
          
          this.router.navigate([`/receptionist/patientView/addBatch/${response.id}`])
        },
        (error) => {
          this.errorMessage = error.error.message;
        }
      )
  }
}
