import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminTestService } from '../services/admin-test.service';

@Component({
  selector: 'app-test-normal-values-editor',
  templateUrl: './test-normal-values-editor.component.html',
  styleUrl: './test-normal-values-editor.component.css'
})
export class TestNormalValuesEditorComponent {
  testNormalValues: any[] = [];
  columnNames: string[] = [];
  errorMessage: string | null = null;
  testTypeId: number;
  constructor(private adminTestService: AdminTestService, private router: Router, private route: ActivatedRoute)
  {
    this.testTypeId = Number(this.route.snapshot.paramMap.get('id'));
  }
  ngOnInit() {
    this.loadNormalValues(this.testTypeId);
  }
  loadNormalValues(testTypeID: number) {
    this.adminTestService.getNormalValues(testTypeID).subscribe(
      (response) => {
        this.testNormalValues = response;
        this.columnNames = Object.keys(this.testNormalValues[0]);
      },
      (error) => {
        this.errorMessage = error.message;
      }
    )
  }
  goBack() {
    this.router.navigate(["admin/options/editTest"]);
  }
  onUpdateSuccess() {
    this.ngOnInit();
  }
  onDeleteSuccess() {
    this.ngOnInit();
  }
  onAddSuccess() {
    this.ngOnInit();
  }
}
