import { trigger, transition, style, animate, state } from '@angular/animations';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ColumnMode, DatatableComponent } from '@swimlane/ngx-datatable';
import { lastValueFrom } from 'rxjs';
import { PipelineInstructionCommand } from 'src/app/domain/commands/pipeline-instruction-commands/pipeline-instruction.command';
import { SavePipelineInstructionCommand } from 'src/app/domain/commands/pipeline-instruction-commands/save-pipeline-instruction.command';
import { ConnectorFunctionUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-function-use-case.interface';
import { ConnectorUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-use-case.interface';
import { PipelineInstructionUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-instruction-use-case.interface';
import { ConnectorFunctionViewModel } from 'src/app/domain/view-models/connector-function.view-model';
import { ConnectorViewModel } from 'src/app/domain/view-models/connector.view-model';
import { PageViewModel } from 'src/app/domain/view-models/page.view-model';
import { PaginatedItemsViewModel } from 'src/app/domain/view-models/paginated-items.view-model';
import { PipelineInstructionViewModel } from 'src/app/domain/view-models/pipeline-instruction.view-model';
import { Toast } from 'src/app/infra/helpers/toast';
import Swal from 'sweetalert2';


@Component({
	selector: 'app-pipeline-instructions',
	templateUrl: './pipeline-instructions.component.html',
	styleUrls: ['./pipeline-instructions.component.css']
})
export class PipelineInstructionsComponent implements OnInit {
	@ViewChild("connectorsTable") public connectorsTable?: DatatableComponent;

	public page: PageViewModel = new PageViewModel();
	public rows: ConnectorViewModel[] = [];
	public expanded: ConnectorViewModel = {} as ConnectorViewModel;
	public columns = [{prop: "name", name: "Name"}];
	public columnMode: ColumnMode = ColumnMode.force;

	public pipelineId: string | null = null;
	public isConnectorPanelVisible: boolean = false;
	public isLoading: boolean = false;

	public pipelineInstructions: PipelineInstructionCommand[] = [];
	public connectorFunctions: ConnectorFunctionViewModel[] = [];

	constructor (
		private connectorUseCase: ConnectorUseCaseInterface,
		private connectorFunctionUseCase: ConnectorFunctionUseCaseInterface,
		private pipelineInstructionUseCase: PipelineInstructionUseCaseInterface,
		private route: ActivatedRoute
	) { }

	ngOnInit(): void {
		this.pipelineId = this.route.snapshot.paramMap.get("id");

		if (this.pipelineId)
			this.getPipelineInstructions(this.pipelineId);

		this.page.pageSize = 10;
		this.page.pageIndex = 0;
		this.setPage({offset: 0});
	}

	toggleConnectorPanel(): void {
		this.isConnectorPanelVisible = !this.isConnectorPanelVisible;
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
		});
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

	selectInstruction(connectorFunction: ConnectorFunctionViewModel, event?: Event): void {
		event?.preventDefault();

		if (!this.connectorFunctions.includes(connectorFunction)) {
			this.connectorFunctions.push(connectorFunction);
		}

		const connectedTo: number | null = this.pipelineInstructions.length == 0 ? null : this.pipelineInstructions.length - 1;

		const instruction: PipelineInstructionCommand = {
			connectorFunctionId: connectorFunction.id,
			connectedToArrayIndex: connectedTo,
			script: connectorFunction.script,
			inputs: {}
		};

		connectorFunction.inputs?.forEach(element => {
			instruction.inputs[element.id] = element.defaultValue;
		});

		this.pipelineInstructions.push(instruction);
	}

	getConnectorFunctionById(connectorFunctionId: string): ConnectorFunctionViewModel {
		return this.connectorFunctions.find(x => x.id == connectorFunctionId)!;
	}

	changeInputValue(index: number, inputId: string, value: string): void {
		let instruction = this.pipelineInstructions[index];

		if (value != "") {
			instruction.inputs[inputId] = value;
		} else {
			delete instruction.inputs[inputId];
		}
	}

	savePipelineInstructions(): void {
		this.isLoading = true;

		const command: SavePipelineInstructionCommand = {
			pipelineId: this.pipelineId!,
			pipelineInstructions: this.pipelineInstructions
		};

		if (command.pipelineInstructions.length == 0) {
			this.isLoading = false;
			return;
		}

		this.pipelineInstructionUseCase.save(command).subscribe({
			next: () => Toast.fire({ icon: "success", title: "Saved successfully" }),
			error: () => Swal.fire("Error", "An error has occurred while trying to update the pipeline instructions. Please try again later.", "error")
		}).add(() => this.isLoading = false);
	}

	private getPipelineInstructions(pipelineId: string): void {
		this.isLoading = true;

		this.pipelineInstructionUseCase.get(pipelineId).subscribe({
			next: (response: PipelineInstructionViewModel[]) => this.getPipelineInstructionsNext(response),
			error: () => {
				Swal.fire("Error", "An error has occurred while trying to get the pipeline instructions. Please try again later.", "error")
				this.isLoading = false;
			}
		});
	}

	private async getPipelineInstructionsNext(response: PipelineInstructionViewModel[]): Promise<void> {
		for (let i = 0; i < response.length; i++) {
			const instruction = response[i];

			let command: PipelineInstructionCommand = {
				connectorFunctionId: instruction.connectorFunctionId,
				connectedToArrayIndex: i == 0 ? null : i - 1,
				script: instruction.script,
				inputs: {}
			};

			let connectorFunction = this.connectorFunctions.find(x => x.id == command.connectorFunctionId);
			if (!connectorFunction) {
				try {
					const connectorFunctionResponse = await lastValueFrom(this.connectorFunctionUseCase.get(command.connectorFunctionId));
					connectorFunction = connectorFunctionResponse;
					this.connectorFunctions.push(connectorFunctionResponse)
				} catch (error) {
					Swal.fire("Error", "An error has occurred while trying to get the pipeline instructions. Please try again later.", "error")
				}
			}

			connectorFunction?.inputs?.forEach(element => {
				command.inputs[element.id] = null;
			});

			for (let j = 0; j < instruction.pipelineInstructionInputs.length; j++) {
				const input = instruction.pipelineInstructionInputs[j];
				command.inputs[input.inputId] = input.replaceValue;
			}

			this.pipelineInstructions.push(command);
		}

		this.isLoading = false;
	}

	removeInstruction(index: number): void {
		this.pipelineInstructions.splice(index, 1);

		this.pipelineInstructions.forEach((element, i) => {
			element.connectedToArrayIndex = (i === 0) ? null : i - 1;
		});
	}

	movePipelineInstruction(index: number, direction: "up" | "down"): void {
		const newIndex = direction === 'up' ? index - 1 : index + 1;

		if (newIndex < 0 || newIndex >= this.pipelineInstructions.length) {
			return;
		}

		[this.pipelineInstructions[index], this.pipelineInstructions[newIndex]] = [this.pipelineInstructions[newIndex], this.pipelineInstructions[index]];

		this.pipelineInstructions.forEach((element, i) => {
			if (i === 0) {
				element.connectedToArrayIndex = null;
			} else {
				element.connectedToArrayIndex = i - 1;
			}
		});
	}
}
