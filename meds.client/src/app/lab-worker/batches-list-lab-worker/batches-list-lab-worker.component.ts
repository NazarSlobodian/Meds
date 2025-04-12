import { Component } from '@angular/core';
import { LabBatchesService } from '../services/lab-batches.service';

@Component({
  selector: 'app-batches-list-lab-worker',
  templateUrl: './batches-list-lab-worker.component.html',
  styleUrl: './batches-list-lab-worker.component.css'
})
export class BatchesListLabWorkerComponent {
  batches: any[] = [];
  columnNames: string[] = []; 

  errorMessage: string | null = null;
  constructor(private labBatchesService: LabBatchesService) { }

  ngOnInit(): void {

  }
  loadBatches(): void {

  }
  onBatchClick(batchId: number): void {

  }
}
