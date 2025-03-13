import { Component, OnInit } from '@angular/core';
import { BatchesService } from "../services/batches.service"
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  selector: 'app-batch-detail',
  templateUrl: './batch-detail.component.html',
  styleUrl: './batch-detail.component.css'
})
export class BatchDetailComponent implements OnInit {
  batchResults: any;
  errorMessage: string | null = null;

  constructor(private batchService: BatchesService,
    private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    const batchId = Number(this.route.snapshot.paramMap.get('id'));
    this.getBatchResults(batchId);
  }
  getBatchResults(batchId: number): void {
    this.batchService.getBatchResults(batchId).subscribe(
      (data) => {
        this.batchResults = data;
      },
      (err) => {
        this.errorMessage = "Error fethcing batch results";
        console.error(err);
      }
    );
  }
  goBack() {
    this.router.navigate(["/patient/batches"]);
  }
  
}
