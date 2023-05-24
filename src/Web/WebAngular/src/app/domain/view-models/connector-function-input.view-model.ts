import { InputType } from "../enums/input-type.enum";

export interface ConnectorFunctionInputViewModel {
	id: string;
	connectorFunctionId: string;
	name: string;
	placeholder: string;
	type: InputType;
	required: boolean;
	replace: string;
	values: string[] | null;
	defaultValue: string | null;
	advancedOption: boolean;
	createdBy: string;
	creationDate: string;
	updatedBy: string;
	lastUpdate: string;
}
