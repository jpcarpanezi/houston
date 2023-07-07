import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ColumnMode } from '@swimlane/ngx-datatable';
import { PipelineLogUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-log-use-case.interface';
import { PageViewModel } from 'src/app/domain/view-models/page.view-model';
import { PaginatedItemsViewModel } from 'src/app/domain/view-models/paginated-items.view-model';
import { PipelineLogViewModel } from 'src/app/domain/view-models/pipeline-log.view-model';
import Swal from 'sweetalert2';

@Component({
	selector: 'app-runs',
	templateUrl: './runs.component.html',
	styleUrls: ['./runs.component.css']
})
export class RunsComponent implements OnInit {
	public page: PageViewModel = new PageViewModel();
	public rows: PipelineLogViewModel[] = [];
	public columnMode: ColumnMode = ColumnMode.force;
	public isLoading: boolean = true;

	public pipelineId: string | null = null;

	constructor(
		private pipelineLogUseCase: PipelineLogUseCaseInterface,
		private route: ActivatedRoute
	) { }

	ngOnInit(): void {
		this.pipelineId = this.route.snapshot.paramMap.get("id");
		this.page.pageSize = 10;
		this.page.pageIndex = 0;
		this.setPage({offset: 0});
	}

	setPage(pageInfo: any): void {
		this.isLoading = true;
		this.page.pageIndex = pageInfo.offset;

		this.pipelineLogUseCase.getAll(this.pipelineId!, this.page.pageSize, this.page.pageIndex).subscribe({
			next: (response: PaginatedItemsViewModel<PipelineLogViewModel>) => {
					this.page.pageIndex = response.pageIndex;
					this.page.pageSize = response.pageSize;
					this.page.count = response.count;
					this.rows = response.data;
			},
			error: () => Swal.fire("Error", "An error has occurred while trying to get the runs.", "error")
		}).add(() => this.isLoading = false);
	}
}
