import { Component, OnInit } from '@angular/core';
import { TechTestService } from '../services/tech-test.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-add-order',
  templateUrl: './add-order.component.html',
  styleUrl: './add-order.component.css'
})
export class AddOrderComponent implements OnInit {
  testTypes: any[] = [];
  selectedTestType: any;
  errorMessage: string | null = null;
  patientId: number = -1;
  patientName: string | null = null;

  testsToAdd: any[] = [];
  constructor(private route: ActivatedRoute, private techTestService: TechTestService, private router: Router) { }

  ngOnInit(): void {
    this.loadTestTypes();
    this.route.params.subscribe(params => {
      this.patientId = params['id'];
    })
    this.route.queryParams.subscribe(params => {
      this.patientName = params['name'];
    })
  }
  loadTestTypes(): void {
    this.techTestService.getAvailableTestTypes().subscribe(
      (data) => {
        this.testTypes = data;
        this.selectedTestType = this.testTypes[0];
      },
      (error) => {
        this.errorMessage = "Failed to load test types";
        console.error(error);
      }
    )
  }
  addTest(): void {
    if (this.selectedTestType === null) {
      this.errorMessage = "No test type selected";
      return;
    }
    this.testsToAdd.push(this.selectedTestType);
    this.selectedTestType = null;
  }
  calculateTotalCost(): number {
    return this.testsToAdd.reduce((total, test) => total + test.cost, 0);
  }
  removeTest(index: number): void {
    this.testsToAdd.splice(index, 1);
  }
  submitOrder() {
    if (this.testsToAdd.length === 0) {
      this.errorMessage = "No tests added";
      return;
    }
    this.techTestService.submitBatch(this.patientId, this.testsToAdd).subscribe(
      (reponse) => {
        this.router.navigate(["/receptionist/patientView/"]);
      },
      (error) => {
        alert(error.error.message);
      }
    )
  }
  goBack() {
    this.router.navigate(["/receptionist/patientView"]);
  }
}
