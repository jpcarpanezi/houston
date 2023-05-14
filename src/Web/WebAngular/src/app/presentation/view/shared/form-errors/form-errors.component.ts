import { Component, Host, Input, OnInit, Optional } from '@angular/core';
import { FormControl, FormGroupDirective } from '@angular/forms';

@Component({
	selector: 'app-form-errors',
	template: `
		<p class="text-red-500 text-xs" *ngIf="control.touched && control.invalid && (error?control.errors[error]:true)">
			<ng-content></ng-content>
		</p>
	`,
	styles: []
})
export class FormErrorsComponent implements OnInit {
	@Input('controlName') controlName?: string;
	@Input('error') error?: string;
	@Input('control') control: any;

	constructor(@Optional() @Host() public form: FormGroupDirective) { }

	ngOnInit(): void {
		if (this.form) {
			this.control = this.form.form.get(this.controlName!) as FormControl;
		}
	}
}
