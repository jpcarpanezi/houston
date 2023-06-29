import { Component, OnInit, ViewChild } from '@angular/core';
import { PipelineDetailsComponent } from './pipeline-details/pipeline-details.component';
import { PipelineTriggerComponent } from './pipeline-trigger/pipeline-trigger.component';
import { PipelineInstructionsComponent } from './pipeline-instructions/pipeline-instructions.component';
import { PipelineUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-use-case.interface';
import { ActivatedRoute } from '@angular/router';
import { RunPipelineCommand } from 'src/app/domain/commands/pipeline-commands/run-pipeline.command';
import Swal from 'sweetalert2';
import { Subscription, exhaustMap, filter, find, interval, repeat, retry, share, skipWhile, startWith, switchMap, take, takeUntil, takeWhile, timer } from 'rxjs';
import { PipelineStatus } from 'src/app/domain/enums/pipeline-status.enum';
import { PipelineViewModel } from 'src/app/domain/view-models/pipeline.view-model';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';

@Component({
	selector: 'app-pipeline',
	templateUrl: './pipeline.component.html',
	styleUrls: ['./pipeline.component.css']
})
export class PipelineComponent implements OnInit {
	@ViewChild("pipelineDetails") private pipelineDetails?: PipelineDetailsComponent;
	@ViewChild("pipelineTrigger") private pipelineTrigger?: PipelineTriggerComponent;
	@ViewChild("pipelineInstructions") private pipelineInstructions?: PipelineInstructionsComponent

	public pipelineId: string | null = null;
	public isRunning: boolean = false;
	public runInterval?: Subscription;

	constructor(
		private pipelineUseCase: PipelineUseCaseInterface,
		private route: ActivatedRoute
	) { }

	ngOnInit(): void {
		this.pipelineId = this.route.snapshot.paramMap.get("id");
	}

	savePipeline(): void {
		this.pipelineDetails?.pipelineDetailsForm.markAllAsTouched();
		this.pipelineTrigger?.pipelineTriggerForm.markAllAsTouched();

		if (this.pipelineDetails?.pipelineDetailsForm.invalid) return;
		if (this.pipelineTrigger?.pipelineTriggerForm.invalid) return;

		this.pipelineDetails?.savePipelineDetails();
		this.pipelineTrigger?.savePipelineTrigger();
		this.pipelineInstructions?.savePipelineInstructions();
	}

	runPipeline(): void {
		this.isRunning = true;
		this.pipelineDetails!.isLoading = true;
		this.pipelineTrigger!.isLoading = true;
		this.pipelineInstructions!.isLoading = true;

		this.pipelineUseCase.run({ id: this.pipelineId! }).subscribe({
			next: () => this.checkPipelineStatus(),
			error: (error: HttpErrorResponse[]) => this.pipelineStatusError(error[0])
		});
	}

	private pipelineStatusError(error: HttpErrorResponse): void {
		if (error.status == HttpStatusCode.Locked) {
			Swal.fire("Already running", "The pipeline was already running. Please wait!", "warning").then(() => {
				this.checkPipelineStatus();
			});
		} else {
			Swal.fire("Error", "An error occurred while running the pipeline.", "error");
			this.isRunning = false;
			this.pipelineDetails!.isLoading = false;
			this.pipelineTrigger!.isLoading = false;
			this.pipelineInstructions!.isLoading = false;
		}
	}

	private checkPipelineStatus(): void {
		timer(10000).pipe(
			switchMap(() => interval(5000).pipe(
				startWith(0),
				switchMap(() => this.pipelineUseCase.get(this.pipelineId!)),
				takeWhile(x => x.status === "Running")
			))
		).subscribe({
			error: () => Swal.fire("Error", "An error occurred while running the pipeline.", "error")
		}).add(() => {
			this.isRunning = false;
			this.pipelineDetails!.isLoading = false;
			this.pipelineTrigger!.isLoading = false;
			this.pipelineInstructions!.isLoading = false;
		});
	}
}
