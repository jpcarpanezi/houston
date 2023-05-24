import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ColumnMode, DatatableComponent } from '@swimlane/ngx-datatable';
import { ConnectorFunctionUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-function-use-case.interface';
import { ConnectorUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-use-case.interface';
import { ConnectorFunctionViewModel } from 'src/app/domain/view-models/connector-function.view-model';
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
	@ViewChild("connectorsTable") public connectorsTable?: DatatableComponent;
	public page: PageViewModel = new PageViewModel();
	public rows: ConnectorViewModel[] = [];
	public expanded: ConnectorViewModel = {} as ConnectorViewModel;
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
		private connectorUseCase: ConnectorUseCaseInterface,
		private connectorFunctionUseCase: ConnectorFunctionUseCaseInterface
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

	toggleExpandRow(row: ConnectorViewModel, shrinked: boolean, rowIndex: number, button: HTMLButtonElement): void {
		button.disabled = true;

		if (shrinked) {
			button.disabled = false;
			this.rows[rowIndex].connectorFunctions = [];
			this.connectorsTable?.rowDetail.toggleExpandRow(row);
			return;
		}

		this.connectorFunctionUseCase.getAll(row.id, 100, 0).subscribe({
			next: (response: PaginatedItemsViewModel<ConnectorFunctionViewModel>) => {
				this.rows[rowIndex].connectorFunctions = response.data;
				this.connectorsTable?.rowDetail.toggleExpandRow(row);
			},
			error: () => Swal.fire("Error", "An error has occurred while trying to get the connector functions.", "error")
		}).add(() => button.disabled = false);
	}

}
