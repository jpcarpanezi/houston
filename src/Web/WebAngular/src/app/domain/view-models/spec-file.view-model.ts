export interface SpecFileViewModel {
	friendlyName: string;
	function: string;
	version: string;
	connector: string;
	description: string;
	inputs: SpecFileViewInputsViewModel[] | Record<string, SpecFileViewInputsViewModel>;
	runs: {
		using: string;
	};
}

export interface SpecFileViewInputsViewModel {
	replace: string;
	type: string;
	label: string;
	placeholder: string;
	values: string[];
	default: string;
	required: boolean;
	advanced: boolean;
}
