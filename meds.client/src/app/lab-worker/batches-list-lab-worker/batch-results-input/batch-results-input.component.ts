import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BatchResultsService } from '../../services/batch-results.service';

@Component({
  selector: 'app-batch-results-input',
  templateUrl: './batch-results-input.component.html',
  styleUrl: './batch-results-input.component.css'
})
export class BatchResultsInputComponent {
  searchType: string | null = null;
  batchId: number = -1;
  orders: any[] = [];
  errorMessage: string | null = null;
  constructor(private route: ActivatedRoute, private router: Router, private batchResultsService: BatchResultsService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.searchType = params['mode'];
      this.batchId = params['id'];
    })
    if (this.searchType == "batch") {
      this.batchResultsService.getResults(this.batchId).subscribe(
        (data) => {
          this.orders = data;
        },
        (error) => {
          this.errorMessage = error.message;
          alert(error.error.message || "An error occurred");
          this.router.navigate(['lab-worker/batches'])
        }
      );
    }
    else if (this.searchType == "order") {
      this.batchResultsService.getResultsByOrder(this.batchId).subscribe(
        (data) => {
          this.orders = data.data;
          this.batchId = data.id;
        },
        (error) => {
          this.errorMessage = error.error.message;
          alert(error.error.message || "An error occurred");
          this.router.navigate(['lab-worker/batches'])
        }
      );
    }

  }
  goBack(): void {
    this.router.navigate(["/lab-worker/batches"]);
  }
  onSubmitClick(): void {
    for (const res of this.orders) {
      const confirmed = confirm("Are you sure you want to submit the results? They are final and cannot be changed if all fields are filled");
      if (!confirmed) {
        return;
      }
      if (res.result === "") {
        res.result = null;
      }
    }
    this.batchResultsService.submitResults(this.orders).subscribe({
      next: (res) => {
        this.goBack();
      },
      error: (error) => this.errorMessage = error.message
    }
    );
  }
  hasInvalidResults(): boolean {
    return this.orders.some(order =>
      order.result !== null &&
      order.result !== undefined &&
      (isNaN(order.result))
    );
  }


}
