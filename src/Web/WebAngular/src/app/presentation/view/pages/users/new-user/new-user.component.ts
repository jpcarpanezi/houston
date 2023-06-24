import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { ModalComponent } from '../../../shared/modal/modal.component';
import { UserViewModel } from 'src/app/domain/view-models/user.view-model';

@Component({
	selector: 'app-new-user',
	templateUrl: './new-user.component.html',
	styleUrls: ['./new-user.component.css']
})
export class NewUserComponent {
	@ViewChild("newUser") public newPipelineModal?: ModalComponent;
	@Output("onCreate") public submitResponse: EventEmitter<UserViewModel> = new EventEmitter<UserViewModel>();

	open(): void {
		this.newPipelineModal?.open();
	}
}
