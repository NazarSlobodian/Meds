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
  panels: any[] = [];
  selectedTestType: any;
  selectedPanel: any;
  errorMessage: string | null = null;

  patientId: number = -1;
  patientName: string | null = null;

  testsToAdd: any[] = [];
  panelsToAdd: any[] = [];
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
        this.testTypes = data.testTypes;
        this.panels = data.panelInfo;
        this.selectedTestType = this.testTypes[0];
        this.selectedPanel = this.panels[0];
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
  addPanel(): void {
    if (this.selectedPanel === null) {
      this.errorMessage = "No panel selected";
      return;
    }
    this.panelsToAdd.push(this.selectedPanel);
    this.selectedPanel = null;
  }
  calculateTestsCost(): number {
    return this.testsToAdd.reduce((total, test) => total + test.cost, 0);
  }
  calculatePanelsCost(): number {
    return this.panelsToAdd.reduce((total, panel) => total + panel.cost, 0);
  }
  removeTest(index: number): void {
    this.testsToAdd.splice(index, 1);
  }
  removePanel(index: number): void {
    this.panelsToAdd.splice(index, 1);
  }
  submitOrder() {
    if (this.testsToAdd.length === 0) {
      this.errorMessage = "No tests added";
      return;
    }
    this.techTestService.submitBatch(this.patientId, this.testsToAdd.map(x=> x.testTypeId), this.panelsToAdd.map(x=>x.testPanelId)).subscribe(
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
