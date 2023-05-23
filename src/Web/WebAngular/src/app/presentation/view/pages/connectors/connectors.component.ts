import { Component, OnInit, TemplateRef } from '@angular/core';
import { ColumnMode } from '@swimlane/ngx-datatable';
import { ConnectorUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-use-case.interface';
import { ConnectorViewModel } from 'src/app/domain/view-models/connector.view-model';
import { PageViewModel } from 'src/app/domain/view-models/page.view-model';
import { PaginatedItemsViewModel } from 'src/app/domain/view-models/paginated-items.view-model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-connectors',
  templateUrl: './connectors.component.html',
  styleUrls: ['./connectors.component.css']
})
export class ConnectorsComponent implements OnInit {
	public page: PageViewModel = new PageViewModel();
	public rows: ConnectorViewModel[] = [];
	public columns = [
		{prop: "name", name: "Name"},
		{prop: "createdBy", name: "Created by"},
		{prop: "creationDate", name: "Created at"},
		{prop: "updatedBy", name: "Updated by"},
		{prop: "lastUpdate", name: "Last update"}
	];
	public columnMode: ColumnMode = ColumnMode.force;
	public isLoading: boolean = true;

	constructor(
		private connectorUseCase: ConnectorUseCaseInterface
	) { }

	ngOnInit(): void {
		this.page.pageSize = 10;
		this.page.pageIndex = 0;
		this.setPage({offset: 0});
	}

	setPage(pageInfo: any): void {
		this.isLoading = true;
		this.page.pageIndex = pageInfo.offset;

		this.connectorUseCase.getAll(this.page.pageSize, this.page.pageIndex).subscribe({
			next: (response: PaginatedItemsViewModel<ConnectorViewModel>) => {
				this.page.pageIndex = response.pageIndex;
				this.page.pageSize = response.pageSize;
				this.page.count = response.count;
				this.rows = response.data;
			},
			error: () => Swal.fire("Error", "An error has occurred while trying to get the connectors.", "error")
		}).add(() => this.isLoading = false);
	}

	deleteConnector(button: HTMLButtonElement, row: ConnectorViewModel): void {
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

				this.connectorUseCase.delete(row.id).subscribe({
					next: () => Swal.fire("Deleted!", "The connector has been deleted.", "success").then(() => this.setPage({offset: this.page.pageIndex})),
					error: () => Swal.fire("Error", "An error has occurred while trying to delete the connector.", "error")
				}).add(() => button.disabled = false);
			}
		})
	}
}
