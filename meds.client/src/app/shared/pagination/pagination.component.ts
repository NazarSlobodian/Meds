import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent {
  @Input() pageSize: number = 999;
  @Input() currentPage: number = 9;
  @Input() totalPages: number = 9999;
  @Input() totalCount: number = 9999;
  @Output() pageChanged: EventEmitter<number> = new EventEmitter();

  onPageChange(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.pageChanged.emit(page);
    }
  }

}
