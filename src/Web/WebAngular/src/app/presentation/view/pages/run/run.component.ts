import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CodemirrorComponent } from '@ctrl/ngx-codemirror';
import { PipelineLogUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-log-use-case.interface';
import { PipelineLogViewModel } from 'src/app/domain/view-models/pipeline-log.view-model';
import Swal from 'sweetalert2';

@Component({
	selector: 'app-run',
	templateUrl: './run.component.html',
	styleUrls: ['./run.component.css']
})
export class RunComponent implements OnInit {
	@ViewChild("codeEditor") private codeEditor?: CodemirrorComponent;

	public pipelineLogId: string | null = null;
	public pipelineRun?: PipelineLogViewModel;
	public isLoading: boolean = false;

	constructor(
		private router: Router,
		private route: ActivatedRoute,
		private pipelineLogUseCase: PipelineLogUseCaseInterface
	) {}

	ngOnInit(): void {
		this.pipelineLogId = this.route.snapshot.paramMap.get("id");

		if (this.pipelineLogId)
			this.getPipelineLog(this.pipelineLogId);
	}

	private getPipelineLog(pipelineLogId: string): void {
		this.isLoading = true;

		this.pipelineLogUseCase.get(pipelineLogId).subscribe({
			next: (response: PipelineLogViewModel) => {
				this.pipelineRun = response;

				if (response.stdout) {
					this.codeEditor?.codeMirror?.getDoc().setValue(response.stdout);
				}
			},
			error: () => Swal.fire("Error", "An error has occurred while getting the pipeline run.", "error").then(() => this.router.navigate(["/home"]))
		}).add(() => this.isLoading = false);
	}

	public back(): void {
		this.router.navigate(["/runs", this.pipelineRun?.pipelineId]);
	}
}
