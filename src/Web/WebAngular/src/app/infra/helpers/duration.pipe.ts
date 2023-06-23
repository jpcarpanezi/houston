import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
	name: 'duration'
})
export class DurationPipe implements PipeTransform {
	transform(value: string): string {
		const time = value.split('.')[0];
		const hours = parseInt(time.substring(0, 2));
		const minutes = parseInt(time.substring(3, 5));
		const seconds = parseInt(time.substring(6, 8));
		const totalSeconds = (hours * 3600) + (minutes * 60) + seconds;
		const totalMinutes = Math.floor(totalSeconds / 60);
		const remainingSeconds = totalSeconds % 60;
		let formattedDuration = '';

		if (hours > 0) {
			formattedDuration += `${hours}h `;
		}

		if (totalMinutes > 0) {
			formattedDuration += `${totalMinutes}m `;
		}

		formattedDuration += `${remainingSeconds}s`;

		return formattedDuration;
	}
}
