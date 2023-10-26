import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { PipelineInstructionCommand } from 'src/app/domain/commands/pipeline-instruction-commands/pipeline-instruction.command';
import { SavePipelineInstructionCommand } from 'src/app/domain/commands/pipeline-instruction-commands/save-pipeline-instruction.command';
import { ConnectorFunctionHistoryUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-function-history-use-case.interface';
import { ConnectorFunctionUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-function-use-case.interface';
import { PipelineInstructionUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-instruction-use-case.interface';
import { ConnectorFunctionHistoryDetailViewModel } from 'src/app/domain/view-models/connector-function-history-detail.view-model';
import { ConnectorFunctionInputViewModel } from 'src/app/domain/view-models/connector-function-input.view-model';
import { ConnectorFunctionViewModel } from 'src/app/domain/view-models/connector-function.view-model';
import { PipelineInstructionViewModel } from 'src/app/domain/view-models/pipeline-instruction.view-model';
import { Toast } from 'src/app/infra/helpers/toast';
import Swal from 'sweetalert2';

@Component({
	selector: 'app-pipeline-instructions',
	templateUrl: './pipeline-instructions.component.html',
	styleUrls: ['./pipeline-instructions.component.css']
})
export class PipelineInstructionsComponent implements OnInit {
	public pipelineInstructions: PipelineInstructionCommand[] = [];
	public connectorFunctions: ConnectorFunctionViewModel[] = [];

	public pipelineId?: string | null;
	public selectedInstructionIndex?: number;
	public selectedConnectorFunction?: ConnectorFunctionViewModel;
	public selectedConnectorFunctionHistory?: ConnectorFunctionHistoryDetailViewModel;

	public isLoading: boolean = false;
	public isConnectorFunctionHistoryLoading: boolean = false;
	public isConnectorsOpen: boolean = false;
	public isInstructionExpanded: boolean = false;

	constructor(
		private connectorFunctionHistoryUseCase: ConnectorFunctionHistoryUseCaseInterface,
		private pipelineInstructionUseCase: PipelineInstructionUseCaseInterface,
		private connectorFunctionUseCase: ConnectorFunctionUseCaseInterface,
		private route: ActivatedRoute
	) { }

	ngOnInit(): void {
		this.pipelineId = this.route.snapshot.paramMap.get("id");

		if (this.pipelineId)
			this.getPipelineInstructions(this.pipelineId);
	}

	getPipelineInstructions(pipelineId: string): void {
		this.isLoading = true;

		this.pipelineInstructionUseCase.get(pipelineId).subscribe({
			next: (response: PipelineInstructionViewModel[]) => this.getPipelineInstructionsNext(response),
			error: () => {
				Swal.fire("Error", "An error has occurred while trying to get the pipeline instructions. Please try again later.", "error")
				this.isLoading = false;
			}
		}).add(() => this.isLoading = false);
	}

	private async getPipelineInstructionsNext(response: PipelineInstructionViewModel[]): Promise<void> {
		for (let i = 0; i < response.length; i++) {
			const element = response[i];

			let command: PipelineInstructionCommand = {
				connectorFunctionId: element.connectorFunctionId,
				connectorFunctionHistoryId: element.connectorFunctionHistoryId,
				connectedToArrayIndex: element.connectedToArrayIndex,
				inputs: {}
			}

			if (command.inputs) {
				for (let j = 0; j < element.pipelineInstructionInputs.length; j++) {
					const input = element.pipelineInstructionInputs[j];
					command["inputs"][input.inputId] = input.replaceValue;
				}
			}

			let connectorFunction = this.connectorFunctions.find(x => x.id === element.connectorFunctionId);
			if (!connectorFunction) {
				try {
					const connectorFunctionResponse = await lastValueFrom(this.connectorFunctionUseCase.get(command.connectorFunctionId));
					this.connectorFunctions.push(connectorFunctionResponse)
				} catch {
					Swal.fire("Error", "An error has occurred while trying to get the pipeline instructions. Please try again later.", "error")
				}
			}

			this.pipelineInstructions.push(command);
		}

		this.isLoading = false;
	}

	drop(event: CdkDragDrop<string[]>): void {
		if (event.currentIndex === event.previousIndex) {
			return;
		}

		if (event.previousIndex === this.selectedInstructionIndex) {
			this.selectedInstructionIndex = event.currentIndex;
		} else if (event.previousIndex < this.selectedInstructionIndex! && event.currentIndex >= this.selectedInstructionIndex!) {
			this.selectedInstructionIndex!--;
		} else if (event.previousIndex > this.selectedInstructionIndex! && event.currentIndex <= this.selectedInstructionIndex!) {
			this.selectedInstructionIndex!++;
		}

		moveItemInArray(this.pipelineInstructions, event.previousIndex, event.currentIndex);

		this.pipelineInstructions.forEach((item, index) => {
			item.connectedToArrayIndex = index == 0 ? null : index - 1;
		});
	}

	addFunction(connectorFunction: ConnectorFunctionViewModel) {
		this.isConnectorsOpen = false;

		const connectedToArrayIndex = this.pipelineInstructions.length > 0 ? this.pipelineInstructions.length - 1 : null;
		var command: PipelineInstructionCommand = {
			connectedToArrayIndex: connectedToArrayIndex,
			connectorFunctionId: connectorFunction.id,
			connectorFunctionHistoryId: "",
			inputs: {}
		}

		command['connectorFunctionHistoryId'] = connectorFunction.versions[0].id;
		if (!this.connectorFunctions.some(x => x.id === connectorFunction.id)) {
			this.connectorFunctions.push(connectorFunction);
		}

		this.pipelineInstructions.push(command);
		this.expandInstruction(command, this.pipelineInstructions.length - 1);
	}

	expandInstruction(instruction: PipelineInstructionCommand, instructionIndex: number): void {
		this.isConnectorsOpen = false;
		this.isInstructionExpanded = true;
		this.selectedConnectorFunction = this.getConnectorFunctionById(instruction.connectorFunctionId);
		this.selectedInstructionIndex = instructionIndex;

		const connectorFunctionHistoryId = this.pipelineInstructions[instructionIndex].connectorFunctionHistoryId ?? this.selectedConnectorFunction?.versions[0].id;
		this.changeInstructionVersion(connectorFunctionHistoryId, false);
	}

	changeInstructionVersion(connectorFunctionHistoryId: string, resetInputs: boolean): void {
		this.isConnectorFunctionHistoryLoading = true;

		this.connectorFunctionHistoryUseCase.get(connectorFunctionHistoryId).subscribe({
			next: (response: ConnectorFunctionHistoryDetailViewModel) => {
				this.selectedConnectorFunctionHistory = response;

				if (resetInputs) {
					this.pipelineInstructions[this.selectedInstructionIndex!].inputs = {};

					response.inputs.forEach(input => {
						this.pipelineInstructions[this.selectedInstructionIndex!]["inputs"][input.id] = input.defaultValue;
					});
				}
			},
			error: () => Swal.fire("Error", "An error has occurred while trying to get the connector function history", "error")
		}).add(() => this.isConnectorFunctionHistoryLoading = false);
	}

	getConnectorFunctionById(connectorFunctionId: string): ConnectorFunctionViewModel | undefined {
		return this.connectorFunctions.find(x => x.id === connectorFunctionId);
	}

	getInputValue(connectorFunctionInput: ConnectorFunctionInputViewModel): string | null {
		if (this.selectedInstructionIndex != undefined && this.pipelineInstructions[this.selectedInstructionIndex].inputs[connectorFunctionInput.id]) {
			return this.pipelineInstructions[this.selectedInstructionIndex].inputs[connectorFunctionInput.id] ?? null;
		}

		return connectorFunctionInput.defaultValue;
	}

	changeInputValue(connectorFunctionInput: ConnectorFunctionInputViewModel, value: string): void {
		let instruction = this.pipelineInstructions[this.selectedInstructionIndex!];

		if (value != "") {
			instruction.inputs[connectorFunctionInput.id] = value;
		} else {
			delete instruction.inputs[connectorFunctionInput.id];
		}
	}

	hasAdvancedInputs(): boolean | undefined {
		return this.selectedConnectorFunctionHistory?.inputs.some(x => x.advancedOption);
	}

	removeInstruction(instructionIndex: number) {
		if (this.selectedInstructionIndex == instructionIndex) {
			this.isInstructionExpanded = false;
			this.selectedInstructionIndex = undefined;
		}

		this.pipelineInstructions.splice(instructionIndex, 1);
	}

	savePipelineInstructions(): void {
		this.isLoading = true;

		const command: SavePipelineInstructionCommand = {
			pipelineId: this.pipelineId!,
			pipelineInstructions: this.pipelineInstructions
		}

		if (command.pipelineInstructions.length == 0) {
			this.isLoading = false;
			return;
		}

		this.pipelineInstructionUseCase.save(command).subscribe({
			next: () => Toast.fire({ icon: "success", title: "Saved successfully" }),
			error: () => Swal.fire("Error", "An error has occurred while trying to update the pipeline instructions. Please try again later.", "error")
		}).add(() => this.isLoading = false);
	}
}
