import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AdminTestService } from '../services/admin-test.service';

@Component({
  selector: 'app-test-editor',
  templateUrl: './test-editor.component.html',
  styleUrl: './test-editor.component.css'
})
export class TestEditorComponent {

  testTypes: any[] = [];
  errorMessage: string | null = null;

  pageSize = 10;
  currentPage = 1;
  totalPages = 1;
  totalCount = 0;
  constructor(private adminTestService: AdminTestService, private router: Router) { }

  ngOnInit() {
    this.loadTestTypes();
  }
  goBack() {
    this.router.navigate(["/admin/options"]);
  }
  onUpdateSuccess() {
    this.loadTestTypes();
  }
  onAdditionSuccess() {
    this.loadTestTypes();
  }
  loadTestTypes() {
    this.adminTestService.getAvailableTestTypes(this.currentPage, this.pageSize)
      .subscribe(
        (response) => {
          this.testTypes = response.list;
          this.totalPages = Math.ceil(response.totalCount / this.pageSize);
          this.totalCount = response.totalCount;
          this.errorMessage = null;
          if (this.currentPage > this.totalPages) {
            this.currentPage = 1;
            if (this.totalCount != 0) {
              this.loadTestTypes();
            }
          }
        },
        (error) => {
          this.errorMessage = error.error.message;
          this.totalPages = 1;
          this.totalCount = 0;
          this.currentPage = 1;
          this.errorMessage = error.error.message;
        }
      )
  }
  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadTestTypes();
    if (this.currentPage > this.totalPages)
      this.currentPage = this.totalPages;
  }
  onTestSelectNormalValues(id: number, name: string) {
    localStorage.setItem('testName', name);
    this.router.navigate([`/admin/options/editTest/${id}/normal-values`]);
  }
  deleteTestType(testTypeId: number) {
    this.adminTestService.deleteTestType(testTypeId).subscribe({
      next: () => { this.loadTestTypes(); },
      error: (error) => { alert(error.error.message) }
    })
  }
  toggleTestType(testTypeId: number) {
    this.adminTestService.toggleTestType(testTypeId).subscribe({
      next: () => { this.loadTestTypes(); },
      error: (error) => { alert(error.error.message) }
    })
  }
}
