import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { ModalComponent } from '../../../shared/modal/modal.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PipelineUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-use-case.interface';
import { PipelineViewModel } from 'src/app/domain/view-models/pipeline.view-model';
import Swal from 'sweetalert2';
import { Toast } from 'src/app/infra/helpers/toast';

@Component({
	selector: 'app-new-pipeline',
	templateUrl: './new-pipeline.component.html',
	styleUrls: ['./new-pipeline.component.css']
})
export class NewPipelineComponent {
	@ViewChild("newPipeline") public newPipelineModal?: ModalComponent;
	@Output("submitResponse") public submitResponse: EventEmitter<PipelineViewModel> = new EventEmitter<PipelineViewModel>();

	public createPipelineForm: FormGroup = this.initializePipelineForm();

	constructor(
		private fb: FormBuilder,
		private pipelineUseCase: PipelineUseCaseInterface
	) { }

	public initializePipelineForm(): FormGroup {
		return this.createPipelineForm = this.fb.group({
			name: ["", [
				Validators.required,
				Validators.maxLength(50),
			]],
			description: ["", [
				Validators.maxLength(5000)
			]]
		});
	}

	createPipeline(): void {
		if (this.createPipelineForm.invalid) return;
		this.createPipelineForm.disable();

		this.pipelineUseCase.create(this.createPipelineForm.value).subscribe({
			next: (response: PipelineViewModel) => this.createPipelineNext(response),
			error: () => Swal.fire("Error", "An error has occurred while trying to create the pipeline", "error")
		}).add(() => {
			this.createPipelineForm.enable();
			this.newPipelineModal?.close();
		});
	}

	private createPipelineNext(response: PipelineViewModel): void {
		Toast.fire({
			icon: "success",
			title: "Pipeline created."
		});

		this.submitResponse.emit(response);
	}

	open(): void {
		this.newPipelineModal?.open();
	}
}
