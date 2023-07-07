import { AfterViewInit, Component, Host, Input, OnInit, Optional } from '@angular/core';
import { FormControl, FormGroupDirective } from '@angular/forms';

@Component({
	selector: 'app-form-errors',
	template: `
		<p class="text-red-500 text-xs" *ngIf="control.touched && control.invalid && (error ? control.errors[error] : true)">
			<ng-content></ng-content>
		</p>
	`,
	styles: []
})
export class FormErrorsComponent implements OnInit, AfterViewInit {
	@Input("controlName") controlName?: string;
	@Input("error") error?: string;
	@Input("control") control: any;

	private isPageLoaded: boolean = false;

	constructor(
		@Optional() @Host() public form: FormGroupDirective,
	) { }

	ngOnInit(): void {
		if (this.form && this.controlName) {
			this.control = this.control ? this.control.get(this.controlName) : this.form.form.get(this.controlName) as FormControl;
		}
	}

	ngAfterViewInit() {
		this.isPageLoaded = true;
	}

	get hasError(): boolean {
		if (this.isPageLoaded) {
			return this.control?.touched && this.control?.invalid && (this.error ? this.control?.errors![this.error] : true);
		}

		return false;
	}
}
