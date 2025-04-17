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
  columnNames: string[] = [];
  errorMessage: string | null = null;
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
    this.adminTestService.getAvailableTestTypes()
      .subscribe(
        (response) => {
          this.testTypes = response;
        },
        (error) => {
          this.errorMessage = error.error.message;
        }
      )
  }
  onTestSelectNormalValues(id: number) {
    this.router.navigate([`/admin/options/editTest/${id}/normal-values`]);
  }
}
