import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ConnectorFunctionUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-function-use-case.interface';
import { ConnectorUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-use-case.interface';
import { ConnectorFunctionViewModel } from 'src/app/domain/view-models/connector-function.view-model';
import { ConnectorViewModel } from 'src/app/domain/view-models/connector.view-model';
import { PageViewModel } from 'src/app/domain/view-models/page.view-model';
import { PaginatedItemsViewModel } from 'src/app/domain/view-models/paginated-items.view-model';
import { Toast } from 'src/app/infra/helpers/toast';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-connectors',
  templateUrl: './connectors.component.html',
  styleUrls: ['./connectors.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ConnectorsComponent implements OnInit {
	public page: PageViewModel = new PageViewModel();
	public connectors: ConnectorViewModel[] = [];
	public activeConnector?: string;
	public connectorFunctions: ConnectorFunctionViewModel[] = [];

	public isConnectorsLoading: boolean = true;
	public isConnectorFunctionsLoading: boolean = false;

	constructor(
		private connectorUseCase: ConnectorUseCaseInterface,
		private connectorFunctionUseCase: ConnectorFunctionUseCaseInterface
	) { }

	ngOnInit(): void {	}

	setPage(pageInfo: any): void {
		this.isConnectorsLoading = true;
		this.page.pageIndex = pageInfo.currentPage == 0 ? pageInfo.currentPage : pageInfo.currentPage - 1;
		this.page.pageSize = pageInfo.pageSize ?? 5;

		this.connectorUseCase.getAll(this.page.pageSize, this.page.pageIndex).subscribe({
			next: (response: PaginatedItemsViewModel<ConnectorViewModel>) => {
				this.page.pageIndex = response.pageIndex;
				this.page.pageSize = response.pageSize;
				this.page.count = response.count;
				this.connectors = response.data;
			},
			error: () => Swal.fire("Error", "An error has occurred while trying to get the connectors.", "error")
		}).add(() => this.isConnectorsLoading = false);
	}

	expandFunction(row: ConnectorViewModel, event: Event): void {
		event.stopPropagation();
		this.activeConnector = row.id;
		this.isConnectorFunctionsLoading = true;

		this.connectorFunctionUseCase.getAll(row.id, 100, 0).subscribe({
			next: (response: PaginatedItemsViewModel<ConnectorFunctionViewModel>) => this.connectorFunctions = response.data,
			error: () => Swal.fire("Error", "An error has occurred while trying to get the connector functions.", "error")
		}).add(() => this.isConnectorFunctionsLoading = false);
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
				this.isConnectorFunctionsLoading = true;
				this.isConnectorsLoading = true;

				this.connectorUseCase.delete(row.id).subscribe({
					next: () => this.deleteConnectorNext(row),
					error: () => Swal.fire("Error", "An error has occurred while trying to delete the connector.", "error")
				}).add(() => {
					this.isConnectorFunctionsLoading = false;
					this.isConnectorsLoading = false;
					button.disabled = false;
				});
			}
		});
	}

	private deleteConnectorNext(row: ConnectorViewModel): void {
		Toast.fire({
			icon: "success",
			title: "Connector deleted"
		});

		if (this.activeConnector == row.id) {
			this.activeConnector = undefined;
			this.connectorFunctions = [];
		}

		this.setPage({ currentPage: this.page.pageIndex });
	}

	updateConnectorFunctionSubmit(row: ConnectorFunctionViewModel): void {
		const index = this.connectorFunctions.findIndex(x => x.id == row.id);
		this.connectorFunctions[index] = row;
	}

	deleteConnectorFunction(button: HTMLButtonElement, connectorFunctionId: string): void {
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

				this.connectorFunctionUseCase.delete(connectorFunctionId).subscribe({
					next: () => Swal.fire("Deleted!", "The connector functions has been deleted.", "success").then(() => {
						const index = this.connectorFunctions.findIndex(x => x.id == connectorFunctionId);
						this.connectorFunctions.splice(index, 1);
					}),
					error: () => Swal.fire("Error", "An error has occurred while trying to delete the connector function.", "error")
				}).add(() => button.disabled = false);
			}
		});
	}
}
