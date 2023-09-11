import { Component, ViewChild } from '@angular/core';
import { ModalComponent } from '../../../shared/modal/modal.component';
import { CodemirrorComponent } from '@ctrl/ngx-codemirror';

@Component({
	selector: 'app-connector-function-stderr',
	templateUrl: './connector-function-stderr.component.html',
	styleUrls: ['./connector-function-stderr.component.css']
})
export class ConnectorFunctionStderrComponent {
	@ViewChild("connectorFunctionStderrModal") public connectorFunctionStderrModal?: ModalComponent;
	@ViewChild("codeEditor") private codeEditor?: CodemirrorComponent;

	open(stderr: string): void {
		this.connectorFunctionStderrModal?.open();

		this.codeEditor?.codeMirror?.getDoc().setValue(window.atob(stderr));
		setTimeout(() => this.codeEditor?.codeMirror?.refresh(), 1); // Prevent not loading until the user clicks on the editor
	}
}
