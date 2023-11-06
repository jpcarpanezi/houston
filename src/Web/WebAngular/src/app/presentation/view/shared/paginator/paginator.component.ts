import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';

@Component({
	selector: 'app-paginator',
	templateUrl: './paginator.component.html',
	styleUrls: ['./paginator.component.css']
})
export class PaginatorComponent implements OnInit, OnChanges {
	@Output() changePage: EventEmitter<any> = new EventEmitter<any>(true);

	@Input() items: Array<any> = new Array<any>();
	@Input() totalItems: number = 0;
	@Input() initialPage: number = 1;
	@Input() pageSize: number = 10;
	@Input() maxPages: number = 10;
	@Input() serverSide: boolean = false;

	pager: any = {};

	ngOnInit() {
		if (this.items && this.items.length) {
			this.setPage(this.initialPage);
		}
	}

	ngOnChanges(changes: SimpleChanges) {
		if (changes["items"] && changes["items"].currentValue !== changes["items"].previousValue) {
			this.setPage(this.initialPage);
		}

		if (changes["totalItems"]) {
			this.setPage(this.initialPage);
		}
	}

	setPage(page: number) {
		if (!this.serverSide && this.items.length > 0) {
			this.pager = this.paginate(this.items.length, page, this.pageSize, this.maxPages);

			var pageOfItems = this.items.slice(this.pager.startIndex, this.pager.endIndex + 1);

			this.changePage.emit(pageOfItems);
		} else {
			this.pager = this.paginate(this.totalItems, page, this.pageSize, this.maxPages);

			this.changePage.emit(this.pager);
		}
	}

	paginate(totalItems: number, currentPage: number = 1, pageSize: number = 10, maxPages: number = 10) {
		let totalPages = Math.ceil(totalItems / pageSize);

		if (currentPage < 1) {
			currentPage = 1;
		} else if (currentPage > totalPages) {
			currentPage = totalPages;
		}

		let startPage: number, endPage: number;
		if (totalPages <= maxPages) {
			startPage = 1;
			endPage = totalPages;
		} else {
			let maxPagesBeforeCurrentPage = Math.floor(maxPages / 2);
			let maxPagesAfterCurrentPage = Math.ceil(maxPages / 2) - 1;

			if (currentPage <= maxPagesBeforeCurrentPage) {
				startPage = 1;
				endPage = maxPages;
			} else if (currentPage + maxPagesAfterCurrentPage >= totalPages) {
				startPage = totalPages - maxPages + 1;
				endPage = totalPages;
			} else {
				startPage = currentPage - maxPagesBeforeCurrentPage;
				endPage = currentPage + maxPagesAfterCurrentPage;
			}
		}

		let startIndex = (currentPage - 1) * pageSize;
		let endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

		let pages = Array.from(Array((endPage + 1) - startPage).keys()).map(i => startPage + i);

		return {
			totalItems: totalItems,
			currentPage: currentPage,
			pageSize: pageSize,
			totalPages: totalPages,
			startPage: startPage,
			endPage: endPage,
			startIndex: startIndex,
			endIndex: endIndex,
			pages: pages
		};
	}
}
