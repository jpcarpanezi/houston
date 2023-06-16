import { Component } from '@angular/core';

@Component({
	selector: 'app-pipeline-instructions',
	templateUrl: './pipeline-instructions.component.html',
	styleUrls: ['./pipeline-instructions.component.css']
})
export class PipelineInstructionsComponent {
	public isConnectorPanelVisible: boolean = false;
	public isLoading: boolean = false;

	toggleConnectorPanel(): void {
		this.isConnectorPanelVisible = !this.isConnectorPanelVisible;
	}
}
