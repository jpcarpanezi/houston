import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-connectors',
  templateUrl: './connectors.component.html',
  styleUrls: ['./connectors.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ConnectorsComponent implements OnInit {
	public isConnectorsLoading: boolean = true;

	constructor() { }

	ngOnInit(): void {	}
}
