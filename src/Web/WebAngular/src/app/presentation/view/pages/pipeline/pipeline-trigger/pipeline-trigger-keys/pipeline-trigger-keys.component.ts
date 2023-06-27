import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PipelineTriggerUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-trigger-use-case.interface';
import { PipelineTriggerKeysViewModel } from 'src/app/domain/view-models/pipeline-trigger-keys.view-model';
import { ModalComponent } from 'src/app/presentation/view/shared/modal/modal.component';
import Swal from 'sweetalert2';

@Component({
	selector: 'app-pipeline-trigger-keys',
	templateUrl: './pipeline-trigger-keys.component.html',
	styleUrls: ['./pipeline-trigger-keys.component.css']
})
export class PipelineTriggerKeysComponent implements OnInit {
	@ViewChild("privateKey") privateKey?: ElementRef<HTMLTextAreaElement>;
	@ViewChild("publicKey") publicKey?: ElementRef<HTMLTextAreaElement>;
	@ViewChild("revealDeployKeysModal") revealDeployKeysModal?: ModalComponent;

	@Output("onHide") ohHide: EventEmitter<void> = new EventEmitter<void>();

	public isLoading: boolean = true;
	public pipelineId: string | null = null;

	constructor(
		private route: ActivatedRoute,
		private pipelineTriggerUseCase: PipelineTriggerUseCaseInterface
	) { }

	ngOnInit(): void {
		this.pipelineId = this.route.snapshot.paramMap.get("id");
	}

	open(): void {
		this.pipelineTriggerUseCase.revealKeys(this.pipelineId as string).subscribe({
			next: (response: PipelineTriggerKeysViewModel) => this.revealKeysNext(response),
			error: (errors: HttpErrorResponse[]) => this.revealKeysError(errors[0])
		}).add(() => this.isLoading = false);

		this.revealDeployKeysModal?.open();
	}

	emitOnHide(): void {
		this.ohHide.emit();
	}

	private revealKeysNext(response: PipelineTriggerKeysViewModel): void {
		this.privateKey!.nativeElement.value = atob(response.privateKey);
		this.publicKey!.nativeElement.value = atob(response.publicKey);
	}

	private revealKeysError(error: HttpErrorResponse): void {
		if (error.status == HttpStatusCode.Forbidden) {
			Swal.fire("Error", "Deploy keys already revealed.", "error");
		} else {
			Swal.fire("Error", "An error occurred while trying to reveal the deploy keys.", "error");
		}

		this.revealDeployKeysModal?.close()
	}
}
