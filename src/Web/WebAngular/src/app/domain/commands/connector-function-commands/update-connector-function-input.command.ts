import { InputType } from "../../enums/input-type.enum";

export interface UpdateConnectorFunctionInputCommand {
	connectorFunctionInputId: string;
	inputType: InputType;
	name: string;
	placeholder: string;
	replace: string;
	required: boolean;
	defaultValue: string;
	values: string[];
	advanced: boolean;
}
