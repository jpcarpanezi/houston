using Houston.Core.Enums;

namespace Houston.Core.Commands.ConnectorFunctionCommands {
	public class CreateConnectorFunctionInputCommand {
		public InputTypeEnum InputType { get; set; }

		public string Name { get; set; } = null!;

		public string Placeholder { get; set; } = null!;

		public string Replace { get; set; } = null!;

		public bool Required { get; set; }

		public string? DefaultValue { get; set; } = null!;

		public string[]? Values { get; set; } = null!;

		public bool AdvancedOption { get; set; }

		public CreateConnectorFunctionInputCommand() { }

		public CreateConnectorFunctionInputCommand(InputTypeEnum inputType, string name, string placeholder, string replace, bool required, string? defaultValue, string[]? values, bool advancedOption) {
			InputType = inputType;
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Placeholder = placeholder ?? throw new ArgumentNullException(nameof(placeholder));
			Replace = replace ?? throw new ArgumentNullException(nameof(replace));
			Required = required;
			DefaultValue = defaultValue;
			Values = values;
			AdvancedOption = advancedOption;
		}
	}
}
