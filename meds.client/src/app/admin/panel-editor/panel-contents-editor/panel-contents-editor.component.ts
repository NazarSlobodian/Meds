import { Component } from '@angular/core';
import { AdminTestService } from '../../services/admin-test.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-panel-contents-editor',
  templateUrl: './panel-contents-editor.component.html',
  styleUrl: './panel-contents-editor.component.css'
})
export class PanelContentsEditorComponent {
  allTestContents: any[] = [];
  panelContents: any[] = [];
  panelId: number | null = null;
  panelName: string | null = localStorage.getItem("testPanelName");;

  errorMessage: string | null = null;
  pageSize = 9;
  currentPage = 1;
  totalPages = 1;
  totalCount = 0;
  constructor(private adminTestService: AdminTestService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.panelId = Number(this.route.snapshot.paramMap.get('id'));
    this.load();
  }
  load() {
    if (this.panelId==null) {
      alert("No panel id");
      return;
    }
    this.adminTestService.getPanelContents(this.panelId).subscribe({
      next: (response) => {
        this.allTestContents = response;
        this.totalCount = this.allTestContents.length;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.panelContents = this.allTestContents.slice((this.currentPage - 1) * this.pageSize, this.currentPage * this.pageSize);
        this.errorMessage = null;
        if (this.currentPage > this.totalPages) {
          this.currentPage = 1;
          if (this.totalCount != 0) {
            this.load();
          }
        }
      },
      error: (error) => {
        this.errorMessage = error.error.message;
        this.allTestContents = [];
        this.panelContents = [];
        this.totalPages = 1;
        this.totalCount = 0;
        this.currentPage = 1;
      }
    })
  }
  onPageChange(page: number): void {
    this.currentPage = page;
    this.panelContents = this.allTestContents.slice((this.currentPage - 1) * this.pageSize, this.currentPage * this.pageSize);
    if (this.currentPage > this.totalPages) {
      this.currentPage = this.totalPages;
    }
  }
  save() {
    if (this.panelId == null) {
      alert("No panel id");
      return;
    }
    this.adminTestService.updatePanelContents(this.allTestContents, this.panelId).subscribe({
      next: () => { alert("Panel content updated!"); this.load() },
      error: (error) => { this.errorMessage = error.error.message }
    });
  }
  goBack() {
    this.router.navigate(["/admin/options/editPanel"]);
  }
}
