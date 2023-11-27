import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { ConnectorFunctionUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-function-use-case.interface';
import { ConnectorUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-use-case.interface';
import { ConnectorViewModel } from 'src/app/domain/view-models/connector.view-model';
import { PageViewModel } from 'src/app/domain/view-models/page.view-model';
import { PaginatedItemsViewModel } from 'src/app/domain/view-models/paginated-items.view-model';
import { Toast } from 'src/app/infra/helpers/toast';
import Swal from 'sweetalert2';
import { NewConnectorComponent } from './new-connector/new-connector.component';
import { ConnectorFunctionGroupedViewModel } from 'src/app/domain/view-models/connector-function-grouped.view-model';
import { ConnectorFunctionSummaryViewModel } from 'src/app/domain/view-models/connector-function-summary.view-model';
import { ConnectorFunctionHistorySummaryViewModel } from 'src/app/domain/view-models/connector-function-history-summary.view-model';

@Component({
  selector: 'app-connectors-card',
  templateUrl: './connectors-card.component.html',
  styleUrls: ['./connectors-card.component.css']
})
export class ConnectorsCardComponent {
	@Input("displayActions") public displayActions: boolean = true;
	@Input("displayHeaders") public displayHeaders: boolean = false;
	@Input("displayCursorSelect") public displayCursorSelect: boolean = false;
	@Input("displayVersions") public displayVersions: boolean = true;
	@Input("hideEmptyFunctions") public hideEmptyFunctions: boolean = false;

	@Output("selectedFunction") public selectedFunction: EventEmitter<ConnectorFunctionGroupedViewModel> = new EventEmitter<ConnectorFunctionGroupedViewModel>();
	@Output("selectedVersion") public selectedVersion: EventEmitter<ConnectorFunctionSummaryViewModel> = new EventEmitter<ConnectorFunctionSummaryViewModel>();
	@Output("selectedFunctionHistory") public selectedFunctionHistory: EventEmitter<ConnectorFunctionHistorySummaryViewModel> = new EventEmitter<ConnectorFunctionHistorySummaryViewModel>();
	@Output("connectorLoading") public connectorLoading: EventEmitter<boolean> = new EventEmitter<boolean>();

	@ViewChild("newConnector") public newConnectorModal?: NewConnectorComponent;

	public page: PageViewModel = new PageViewModel();
	public connectors: ConnectorViewModel[] = [];
	public activeConnector?: string;
	public activeConnectorName?: string;
	public connectorFunctions: ConnectorFunctionGroupedViewModel[] = [];

	public isConnectorsLoading: boolean = true;
	public isConnectorFunctionsLoading: boolean = false;

	constructor(
		private connectorUseCase: ConnectorUseCaseInterface,
		private connectorFunctionUseCase: ConnectorFunctionUseCaseInterface
	) { }

	ngOnInit(): void {	}

	setPage(pageInfo: any): void {
		this.isConnectorsLoading = true;
		this.connectorLoading.emit(true);
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
		}).add(() => {
			this.isConnectorsLoading = false;
			this.connectorLoading.emit(false);
		});
	}

	expandFunction(row: ConnectorViewModel, event: Event): void {
		event.stopPropagation();
		this.activeConnector = row.id;
		this.activeConnectorName = row.name;
		this.isConnectorFunctionsLoading = true;

		this.connectorFunctionUseCase.getAll(row.id, 100, 0).subscribe({
			next: (response: PaginatedItemsViewModel<ConnectorFunctionGroupedViewModel>) => {
				if (!this.hideEmptyFunctions) {
					this.connectorFunctions = response.data
				} else {
					this.connectorFunctions = response.data.filter(x => x.versions.length > 0);
				}
			},
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
				this.connectorLoading.emit(true);

				this.connectorUseCase.delete(row.id).subscribe({
					next: () => this.deleteConnectorNext(row),
					error: () => Swal.fire("Error", "An error has occurred while trying to delete the connector.", "error")
				}).add(() => {
					this.isConnectorFunctionsLoading = false;
					this.isConnectorsLoading = false;
					this.connectorLoading.emit(false);
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

	deleteConnectorFunctionVersion(button: HTMLButtonElement, connectorFunctionId: string, connectorFunction: string) {
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
					next: () => {
						Swal.fire("Deleted!", "The connector function version has been deleted.", "success")
						const connectorIndex = this.connectorFunctions.findIndex(x => x.name == connectorFunction);
						this.connectorFunctions[connectorIndex].versions = this.connectorFunctions[connectorIndex].versions.filter(x => x.id != connectorFunctionId);
					},
					error: () => Swal.fire("Error", "An error has occurred while trying to delete the connector function.", "error")
				}).add(() => button.disabled = false);
			}
		});
	}

	newConnector() {
		this.newConnectorModal?.open();
	}
}
