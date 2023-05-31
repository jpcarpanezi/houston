import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/infra/auth/auth.service';
import { UserSessionViewModel } from 'src/app/domain/view-models/user-session.view-model';
import { PageViewModel } from 'src/app/domain/view-models/page.view-model';
import { PipelineViewModel } from 'src/app/domain/view-models/pipeline.view-model';
import { ColumnMode } from '@swimlane/ngx-datatable';
import { PipelineUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-use-case.interface';
import { PaginatedItemsViewModel } from 'src/app/domain/view-models/paginated-items.view-model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
	public userInfo: UserSessionViewModel | null = null;

	public page: PageViewModel = new PageViewModel();
	public rows: PipelineViewModel[] = [];
	public columns = [
		{ prop: "name", name: "Name" },
		{ prop: "status", name: "Status" },
		{ prop: "createdBy", name: "Created by" },
		{ prop: "creationDate", name: "Created at" },
		{ prop: "updatedBy", name: "Updated by" },
		{ prop: "lastUpdate", name: "Last update" },

	];
	public columnMode: ColumnMode = ColumnMode.force;
	public isLoading: boolean = true;

	constructor(
		private authService: AuthService,
		private pipelineUseCase: PipelineUseCaseInterface
	) { }

	ngOnInit(): void {
		this.userInfo = this.authService.userInfo;
		this.page.pageSize = 10;
		this.page.pageIndex = 0;
		this.setPage({ offset: 0 });
	}

	setPage(pageInfo: any): void {
		this.isLoading = true;
		this.page.pageIndex = pageInfo.offset;

		this.pipelineUseCase.getAll(this.page.pageSize, this.page.pageIndex).subscribe({
			next: (response: PaginatedItemsViewModel<PipelineViewModel>) => {
				this.page.pageIndex = response.pageIndex;
				this.page.pageSize = response.pageSize;
				this.page.count = response.count;
				this.rows = response.data;
			},
			error: () => Swal.fire("Error", "An error has occurred while trying to get the pipelines", "error")
		}).add(() => this.isLoading = false);
	}
}
