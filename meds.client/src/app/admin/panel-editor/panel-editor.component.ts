import { Component } from '@angular/core';
import { AdminTestService } from '../services/admin-test.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-panel-editor',
  templateUrl: './panel-editor.component.html',
  styleUrl: './panel-editor.component.css'
})
export class PanelEditorComponent {

  testPanels: any[] = [];

  errorMessage: string | null = null;

  pageSize = 10;
  currentPage = 1;
  totalPages = 1;
  totalCount = 0;
  constructor(private adminTestService: AdminTestService, private router: Router) { }

  ngOnInit() {
    this.loadTestPanels();
  }
  goBack() {
    this.router.navigate(["/admin/options"]);
  }
  onUpdateSuccess() {
    this.loadTestPanels();
  }
  onAdditionSuccess() {
    this.loadTestPanels();
  }
  loadTestPanels() {
    this.adminTestService.getAvailableTestPanels(this.currentPage, this.pageSize)
      .subscribe(
        (response) => {
          this.testPanels = response.list;
          this.totalPages = Math.ceil(response.totalCount / this.pageSize);
          this.totalCount = response.totalCount;
          this.errorMessage = null;
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
    this.loadTestPanels();
    if (this.currentPage > this.totalPages)
      this.currentPage = this.totalPages;
  }
  onPanelSelectContents(id: number, name: string) {
    localStorage.setItem('panelName', name);
    this.router.navigate([`/admin/options/editPanel/${id}/panel`]);
  }
}
