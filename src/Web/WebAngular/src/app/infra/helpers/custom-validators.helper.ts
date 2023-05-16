import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export class CustomValidators {
	public static fieldMatch(matchTo: string, reverse?: boolean): ValidatorFn {
		return (control: AbstractControl): ValidationErrors | null => {
			if (control.pristine || (control.parent?.controls as any)[matchTo].pristine) return null;

			if (control.parent && reverse) {
				const c = (control.parent?.controls as any)[matchTo] as AbstractControl;

				if (c) {
					c.updateValueAndValidity();
				}

				return null;
			}

			return !!control.parent && !!control.parent.value &&
					control.value === (control.parent?.controls as any)[matchTo].value ? null : { matching: true };
		}
	}
}
