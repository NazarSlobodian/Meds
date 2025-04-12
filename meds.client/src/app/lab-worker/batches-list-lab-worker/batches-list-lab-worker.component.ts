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

  errorMessage: string | null = null;
  constructor(private labBatchesService: LabBatchesService, private router: Router) { }

  ngOnInit(): void {
    this.labBatchesService.getBatches().subscribe(
      (data) => {
        this.batches = data;
      },
      (error) => {
        this.errorMessage = "Failed to load batches";
      }
    );
  }
  loadBatches(): void {
    this.errorMessage = null;
    this.labBatchesService.getBatches().subscribe(
      (data) => {
        this.batches = data;
      },
      (error) => {
        this.errorMessage = "Failed to load batches";
      }
    );
  }
  onBatchClick(batchId: number): void {
    this.router.navigate([`/lab-worker/batches/${batchId}`]);
  }
}
