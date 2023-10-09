import { InputType } from "../../enums/input-type.enum";

export interface CreateConnectorFunctionInputCommand {
	inputType: InputType;
	name: string;
	placeholder: string;
	replace: string;
	required: boolean;
	defaultValue: string | null;
	values: string[];
	advancedOption: boolean;
}
