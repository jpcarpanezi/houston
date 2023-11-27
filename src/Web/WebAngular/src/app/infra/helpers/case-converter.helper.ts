export class CaseConverterHelper {
	public static camelToSnake(obj: any): any {
		if (obj !== null && typeof obj === 'object') {
			if (Array.isArray(obj)) {
				return obj.map((item) => this.camelToSnake(item));
			} else {
				const snakeObject: any = {};
				Object.keys(obj).forEach((key) => {
					const snakeKey = key.replace(/[A-Z]/g, (letter) => `_${letter.toLowerCase()}`);
					snakeObject[snakeKey] = this.camelToSnake(obj[key]);
				});

				return snakeObject;
			}
		}

		return obj;
	}

	public static snakeToCamel(obj: any): any {
		if (obj !== null && typeof obj === 'object') {
			if (Array.isArray(obj)) {
				return obj.map((item) => this.snakeToCamel(item));
			} else {
				const camelObject: any = {};
				Object.keys(obj).forEach((key) => {
					const camelKey = key.replace(/_([a-z])/g, (_, letter) => letter.toUpperCase());
					camelObject[camelKey] = this.snakeToCamel(obj[key]);
				});

				return camelObject;
			}
		}

		return obj;
	}
}
