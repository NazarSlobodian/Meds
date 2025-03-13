import { Component, OnInit } from '@angular/core';
import { BatchesService } from "../services/batches.service"

@Component({
  selector: 'app-batches-list',
  templateUrl: './batches-list.component.html',
  styleUrl: './batches-list.component.css'
})
export class BatchesListComponent implements OnInit {
  batches: any[] = [];
  errorMessage: string | null = null;

  constructor(private batchesService: BatchesService) { }

  ngOnInit(): void {
    this.loadBatches();
  }

  loadBatches(): void {
    this.batchesService.getBatches().subscribe(
      (data) => {
        this.batches = data;
      },
      (error) => {
        this.errorMessage = "Failed to load test batches";
        console.error(error);
      }
    );
  }

}
