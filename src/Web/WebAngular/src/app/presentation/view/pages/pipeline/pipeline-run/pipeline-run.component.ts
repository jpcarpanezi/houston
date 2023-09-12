import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, HostListener, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { RunPipelineCommand } from 'src/app/domain/commands/pipeline-commands/run-pipeline.command';
import { PipelineUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-use-case.interface';
import Swal from 'sweetalert2';

@Component({
	selector: 'app-pipeline-run',
	templateUrl: './pipeline-run.component.html',
	styleUrls: ['./pipeline-run.component.css']
})
export class PipelineRunComponent implements OnInit {
	@Input("isLoading") public isLoading: boolean = false;
	@Output("run") public run: EventEmitter<void> = new EventEmitter<void>();

	public pipelineId: string | null = null;
	public runPipelineForm: FormGroup = this.initializeRunPipelineForm();
	public isRunPipelineOpen: boolean = false;

	constructor(
		private fb: FormBuilder,
		private pipelineUseCase: PipelineUseCaseInterface,
		private route: ActivatedRoute,
		private elementRef: ElementRef
	) { }

	ngOnInit(): void {
		this.pipelineId = this.route.snapshot.paramMap.get("id");
	}

	private initializeRunPipelineForm(): FormGroup {
		return this.runPipelineForm = this.fb.group({
			branch: ["", [
				Validators.required,
				Validators.maxLength(250)
			]]
		});
	}

	toggle(event: Event): void {
		event.stopPropagation();

		this.isRunPipelineOpen = !this.isRunPipelineOpen;
	}

	@HostListener('document:click', ['$event'])
	handleClick(event: Event): void {
		const clickedInside = this.elementRef.nativeElement.contains(event.target);

		if (!clickedInside && this.isRunPipelineOpen) {
			this.toggle(event);
		}
	}

	submitRunPipeline(): void {
		this.runPipelineForm.markAllAsTouched();
		if (this.runPipelineForm.invalid) return;
		this.runPipelineForm.disable();

		this.run.emit();

		const command: RunPipelineCommand = {
			id: this.pipelineId!,
			branch: this.runPipelineForm.value.branch
		};

		this.pipelineUseCase.run(command).subscribe({
			next: () => this.toggle(new Event("click")),
			error: (error: HttpErrorResponse[]) => this.pipelineStatusError(error[0])
		}).add(() => this.runPipelineForm.enable());
	}

	private pipelineStatusError(error: HttpErrorResponse): void {
		if (error.status == HttpStatusCode.Locked) {
			Swal.fire("Already running", "The pipeline was already running. Please wait!", "warning");
		} else {
			Swal.fire("Error", "An error occurred while running the pipeline.", "error");
		}
	}
}
