import { Component } from '@angular/core';
import { LabBatchesService } from '../services/lab-batches.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-batches-list-lab-worker',
  templateUrl: './batches-list-lab-worker.component.html',
  styleUrl: './batches-list-lab-worker.component.css'
})
export class BatchesListLabWorkerComponent {
  batches: any[] = [];

  batchId: number | null = null;
  orderId: number | null = null;

  errorMessage: string | null = null;

  page: number = 1;
  pageSize: number = 20;
  totalCount: number = 1;
  totalPages: number = 1;
  constructor(private labBatchesService: LabBatchesService, private router: Router) { }

  ngOnInit(): void {
    this.loadBatches();
  }
  loadBatches(): void {
    this.errorMessage = null;
    this.labBatchesService.getBatches(this.page, this.pageSize).subscribe(
      (data) => {
        this.batches = data.list;
        this.totalPages = Math.ceil(data.totalCount / this.pageSize);
        this.totalCount = data.totalCount;
      },
      (error) => {
        this.errorMessage = "Failed to load batches";
      }
    );
  }
  onBatchClick(batchId: number): void {
    this.router.navigate([`/lab-worker/batches/${batchId}/${"batch"}`]);
  }
  onPageChange(page: number): void {
    this.page = page;
    this.loadBatches();
    if (this.page > this.totalPages)
      this.page = this.totalPages;
  }
  onSubmit(): void {
    if (this.batchId === null) {
      return;
    }
    this.router.navigate([`/lab-worker/batches/${this.batchId}/${"batch"}`]);
  }
  onSubmitByOrder(): void {
    if (this.orderId === null) {
      return;
    }
    this.router.navigate([`/lab-worker/batches/${this.orderId}/${"order"}`]);
  }
}
