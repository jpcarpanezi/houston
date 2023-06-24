import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ColumnMode } from '@swimlane/ngx-datatable';
import { UserUseCaseInterface } from 'src/app/domain/interfaces/use-cases/user-use-case.interface';
import { PageViewModel } from 'src/app/domain/view-models/page.view-model';
import { PaginatedItemsViewModel } from 'src/app/domain/view-models/paginated-items.view-model';
import { UserSessionViewModel } from 'src/app/domain/view-models/user-session.view-model';
import { UserViewModel } from 'src/app/domain/view-models/user.view-model';
import { AuthService } from 'src/app/infra/auth/auth.service';
import { Toast } from 'src/app/infra/helpers/toast';
import Swal from 'sweetalert2';

@Component({
	selector: 'app-users',
	templateUrl: './users.component.html',
	styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
	public page: PageViewModel = new PageViewModel();
	public rows: UserViewModel[] = [];
	public columns = [
		{prop: "name", name: "Name"},
		{prop: "email", name: "E-mail"},
		{prop: "userRole", name: "User role"}
	];
	public columnMode: ColumnMode = ColumnMode.force;
	public isLoading: boolean = true;

	public user: UserSessionViewModel | null = this.authService.userInfo;

	constructor(
		private userUseCase: UserUseCaseInterface,
		private authService: AuthService
	) { }

	ngOnInit(): void {
		this.page.pageSize = 10;
		this.page.pageIndex = 0;
		this.setPage({offset: 0});
	}

	setPage(pageInfo: any): void {
		this.isLoading = true;
		this.page.pageIndex = pageInfo.offset;

		this.userUseCase.getAll(this.page.pageSize, this.page.pageIndex).subscribe({
			next: (response: PaginatedItemsViewModel<UserViewModel>) => {
				this.page.pageIndex = response.pageIndex;
				this.page.pageSize = response.pageSize;
				this.page.count = response.count;
				this.rows = response.data;
			},
			error: () => Swal.fire("Error", "An error has occurred while trying to get the users.", "error")
		}).add(() => this.isLoading = false);
	}

	toggleStatus(id: string, button: HTMLButtonElement): void {
		button.disabled = true;

		this.userUseCase.toggleStatus(id).subscribe({
			next: () => {
				Toast.fire({
					icon: "success",
					title: "User status toggled"
				});

				this.setPage({ offset: this.page.pageIndex });
			},
			error: (error: HttpErrorResponse[]) => this.toggleStatusError(error[0])
		}).add(() => button.disabled = false);
	}

	private toggleStatusError(error: HttpErrorResponse) {
		if (error.status === HttpStatusCode.Forbidden) {
			Swal.fire("Error", "You don't have permission to toggle yourself.", "error");
		} else {
			Swal.fire("Error", "An error has occurred while trying to toggle the user status.", "error");
		}
	}
}
