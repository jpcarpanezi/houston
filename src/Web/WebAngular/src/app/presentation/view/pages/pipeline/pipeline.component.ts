import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { PipelineDetailsComponent } from './pipeline-details/pipeline-details.component';
import { PipelineTriggerComponent } from './pipeline-trigger/pipeline-trigger.component';
import { PipelineInstructionsComponent } from './pipeline-instructions/pipeline-instructions.component';
import { PipelineUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-use-case.interface';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { Subscription, interval, startWith, switchMap, takeWhile, timer } from 'rxjs';

@Component({
	selector: 'app-pipeline',
	templateUrl: './pipeline.component.html',
	styleUrls: ['./pipeline.component.css']
})
export class PipelineComponent implements OnInit, OnDestroy {
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

	ngOnDestroy(): void {
		this.runInterval?.unsubscribe();
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

		this.checkPipelineStatus();
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
