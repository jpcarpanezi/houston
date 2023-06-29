import { Pipe, PipeTransform } from '@angular/core';
import { UtcToLocalTimeFormat } from 'src/app/domain/enums/utc-to-local-time-format';

@Pipe({
	name: 'utcToLocalTime'
})
export class UtcToLocalTimePipe implements PipeTransform {
	transform(utcDate: string | undefined, format: UtcToLocalTimeFormat | string): string | undefined {
		var browserLanguage = navigator.language;

		if (!utcDate) {
			return utcDate;
		}

		switch(format) {
			case UtcToLocalTimeFormat.SHORT:
				let date = new Date(utcDate).toLocaleDateString(browserLanguage);
				let time = new Date(utcDate).toLocaleTimeString(browserLanguage);
				return `${date} ${time}`;
			case UtcToLocalTimeFormat.SHORT_DATE:
				return new Date(utcDate).toLocaleDateString(browserLanguage);
			case UtcToLocalTimeFormat.SHORT_TIME:
				return new Date(utcDate).toLocaleTimeString(browserLanguage);
			case UtcToLocalTimeFormat.FULL:
				return new Date(utcDate).toString();
			default:
				console.error(`Do not have logitec to format UTC date, format: ${format}`);
				return new Date(utcDate).toString();
		}
	}
}
