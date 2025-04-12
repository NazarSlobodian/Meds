import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BatchResultsService } from '../../services/batch-results.service';

@Component({
  selector: 'app-batch-results-input',
  templateUrl: './batch-results-input.component.html',
  styleUrl: './batch-results-input.component.css'
})
export class BatchResultsInputComponent {
  batchId: number = -1;
  orders: any[] = [];
  errorMessage: string | null = null;
  constructor(private route: ActivatedRoute, private router: Router, private batchResultsSevice: BatchResultsService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.batchId = params['id'];
    })
    this.batchResultsSevice.getResults(this.batchId).subscribe(
      (data) => {
        this.orders = data;
      },
      (error) => {
        this.errorMessage = "Failed to load batches";
      }
    );

  }
  goBack(): void {
    this.router.navigate(["/lab-worker/batches"]);
  }
  onSaveClick(order: any): void {

  }
}
