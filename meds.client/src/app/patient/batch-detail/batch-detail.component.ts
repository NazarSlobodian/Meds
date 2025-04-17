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

  groupedResults: { panelName: string; tests: any[] }[] = [];

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

        for (const test of this.batchResults.testResults) {
          const panelName = test.panelName ?? 'Other';  // use 'Other' if null/undefined
          let group = this.groupedResults.find(g => g.panelName === panelName);
          if (!group) {
            group = { panelName, tests: [] };
            this.groupedResults.push(group);
          }
          group.tests.push(test);
        }

        this.groupedResults.sort((a, b) => {
          if (a.panelName === 'Other') return 1;
          if (b.panelName === 'Other') return -1;
          return a.panelName.localeCompare(b.panelName);
        });

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
  download() {
    this.batchService.getBatchResultsPdf(this.batchResults.batchID).subscribe({
      next: (response: Blob) => {
        const url = window.URL.createObjectURL(response);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Medlab${this.batchResults.batchID}.pdf`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: () => { }
    })
  }
}
