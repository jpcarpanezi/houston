import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { PipelineUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-use-case.interface';
import { PipelineViewModel } from 'src/app/domain/view-models/pipeline.view-model';
import { Toast } from 'src/app/infra/helpers/toast';
import Swal from 'sweetalert2';

@Component({
	selector: 'app-pipeline-details',
	templateUrl: './pipeline-details.component.html',
	styleUrls: ['./pipeline-details.component.css']
})
export class PipelineDetailsComponent implements OnInit {
	public isLoading: boolean = true;
	public pipelineDetailsForm: FormGroup = this.initializePipelineDetailsForm();
	public pipelineId: string | null = null;

	constructor(
		private fb: FormBuilder,
		private route: ActivatedRoute,
		private pipelineUseCase: PipelineUseCaseInterface
	) { }

	ngOnInit(): void {
		this.pipelineId = this.route.snapshot.paramMap.get("id");

		if (this.pipelineId)
			this.getPipelineDetails(this.pipelineId);
	}

	private initializePipelineDetailsForm(): FormGroup {
		return this.pipelineDetailsForm = this.fb.group({
			name: ["", [
				Validators.required,
				Validators.maxLength(50)
			]],
			description: ["", [
				Validators.maxLength(5000)
			]]
		});
	}

	getPipelineDetails(id: string): void {
		this.pipelineUseCase.get(id).subscribe({
			next: (response: PipelineViewModel) => this.pipelineDetailsForm.patchValue(response),
			error: () => Swal.fire("Error", "An error has occurred while getting the pipeline details", "error")
		}).add(() => this.isLoading = false);
	}

	savePipelineDetails(): void {
		if (this.pipelineDetailsForm.invalid) return;
		this.pipelineDetailsForm.disable();
		this.isLoading = true;

		this.pipelineDetailsForm.value["id"] = this.pipelineId;
		this.pipelineUseCase.update(this.pipelineDetailsForm.value).subscribe({
			next: () => {
				Toast.fire({
					icon: "success",
					title: "Saved successfully"
				});
			},
			error: () => Swal.fire("Error", "An error has occurred while saving the pipeline details", "error")
		}).add(() => {
			this.pipelineDetailsForm.enable();
			this.isLoading = false;
		});
	}
}
