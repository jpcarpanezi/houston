import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from 'src/app/infra/auth/auth.service';
import { UserSessionViewModel } from 'src/app/domain/view-models/user-session.view-model';
import { PageViewModel } from 'src/app/domain/view-models/page.view-model';
import { PipelineViewModel } from 'src/app/domain/view-models/pipeline.view-model';
import { ColumnMode, TableColumn } from '@swimlane/ngx-datatable';
import { PipelineUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-use-case.interface';
import { PaginatedItemsViewModel } from 'src/app/domain/view-models/paginated-items.view-model';
import Swal from 'sweetalert2';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Toast } from 'src/app/infra/helpers/toast';
import { Subscription, interval, startWith, switchMap } from 'rxjs';
import { UtcToLocalTimePipe } from 'src/app/infra/helpers/utc-to-local-time.pipe';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy {
	public userInfo: UserSessionViewModel | null = null;

	public page: PageViewModel = new PageViewModel();
	public rows: PipelineViewModel[] = [];
	public columns = [
		{ prop: "status", name: "Status" }
	];
	public columnMode: ColumnMode = ColumnMode.force;
	public isLoading: boolean = true;
	public longPooling?: Subscription;

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

	ngOnDestroy(): void {
		this.longPooling?.unsubscribe();
	}

	setPage(pageInfo: any): void {
		this.longPooling?.unsubscribe();
		this.isLoading = true;
		this.page.pageIndex = pageInfo.offset;

		this.pipelineUseCase.getAll(this.page.pageSize, this.page.pageIndex).subscribe({
			next: (response: PaginatedItemsViewModel<PipelineViewModel>) => {
				this.page.pageIndex = response.pageIndex;
				this.page.pageSize = response.pageSize;
				this.page.count = response.count;
				this.rows = response.data;
				this.triggerLongPooling();
			},
			error: () => Swal.fire("Error", "An error has occurred while trying to get the pipelines", "error")
		}).add(() => this.isLoading = false);
	}

	private triggerLongPooling(): void {
		this.longPooling = interval(5000).pipe(
			startWith(0),
			switchMap(() => this.pipelineUseCase.getAll(this.page.pageSize, this.page.pageIndex))
		).subscribe({
			next: (response: PaginatedItemsViewModel<PipelineViewModel>) => {
				this.page.pageIndex = response.pageIndex;
				this.page.pageSize = response.pageSize;
				this.page.count = response.count;
				this.rows = response.data;
			},
			error: () => {
				Swal.fire("Error", "An error has occurred while trying to get the pipelines", "error");
				this.longPooling?.unsubscribe();
			}
		});
	}

	deletePipeline(button: HTMLButtonElement, pipelineId: string): void {
		Swal.fire({
			icon: "question",
			title: "Are you sure?",
			text: "You won't be able to revert this!",
			showDenyButton: true,
			showConfirmButton: true,
			confirmButtonText: "Yes, delete it",
			denyButtonText: "No, cancel",
		}).then((result) => {
			if (result.isConfirmed) {
				button.disabled = true;

				this.pipelineUseCase.delete(pipelineId).subscribe({
					next: () => this.deletePipelineNext(),
					error: (error: HttpErrorResponse[]) => this.handlePipelineError(error[0], "delete")
				}).add(() => button.disabled = false);
			}
		});
	}

	private deletePipelineNext(): void {
		Toast.fire({
			icon: "success",
			title: "Pipeline deleted"
		});

		this.setPage({ offset: this.page.pageIndex });
	}

	private handlePipelineError(error: HttpErrorResponse, action: string): void {
		switch (error.status) {
			case HttpStatusCode.Locked:
				Swal.fire("Error", "The pipeline is running, please wait until it finishes.", "error");
				break;
			default:
				Swal.fire("Error", `An error has occurred while trying to ${action} the pipeline.`, "error");
				break;
		}
	}

	togglePipeline(button: HTMLButtonElement, pipelineId: string): void {
		button.disabled = true;

		this.pipelineUseCase.toggle(pipelineId).subscribe({
			next: () => {
				Toast.fire({
					icon: "success",
					title: "Pipeline toggled"
				});

				this.setPage({ offset: this.page.pageIndex });
			},
			error: (error: HttpErrorResponse[]) => this.handlePipelineError(error[0], "toggle")
		}).add(() => button.disabled = false);
	}
}
