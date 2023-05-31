import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { PipelineViewModel } from 'src/app/domain/view-models/pipeline.view-model';
import { ModalComponent } from '../../../shared/modal/modal.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PipelineUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-use-case.interface';
import { Toast } from 'src/app/infra/helpers/toast';
import Swal from 'sweetalert2';

@Component({
	selector: 'app-update-pipeline',
	templateUrl: './update-pipeline.component.html',
	styleUrls: ['./update-pipeline.component.css']
})
export class UpdatePipelineComponent {
	@ViewChild("updatePipelineModal") public updatePipelineModal?: ModalComponent;
	@Output("onUpdate") public onUpdate: EventEmitter<PipelineViewModel> = new EventEmitter<PipelineViewModel>();

	public updatePipelineForm: FormGroup = this.initializePipelineForm();

	public pipeline?: PipelineViewModel;

	constructor(
		private fb: FormBuilder,
		private pipelineUseCase: PipelineUseCaseInterface
	) { }

	private initializePipelineForm(): FormGroup {
		return this.updatePipelineForm = this.fb.group({
			name: ["", [
				Validators.required,
				Validators.maxLength(50)
			]],
			description: ["", [
				Validators.maxLength(5000)
			]]
		});
	}

	updatePipeline(): void {
		if (this.updatePipelineForm.invalid) return;
		this.updatePipelineForm.disable();

		this.updatePipelineForm.value["id"] = this.pipeline?.id;
		this.pipelineUseCase.update(this.updatePipelineForm.value).subscribe({
			next: (response: PipelineViewModel) => this.updatePipelineNext(response),
			error: () => Swal.fire("Error", "An error has occurred while trying to update the pipeline.", "error")
		}).add(() => {
			this.updatePipelineForm.enable();
			this.updatePipelineModal?.close();
		});
	}

	private updatePipelineNext(response: PipelineViewModel): void {
		Toast.fire({
			icon: "success",
			title: "Pipeline updated."
		});

		this.onUpdate.emit(response);
	}

	open(pipeline: PipelineViewModel): void {
		this.pipeline = pipeline;
		this.updatePipelineForm.patchValue(pipeline);
		this.updatePipelineModal?.open();
	}
}
