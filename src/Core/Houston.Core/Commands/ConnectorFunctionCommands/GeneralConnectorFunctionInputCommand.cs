using Houston.Core.Enums;

namespace Houston.Core.Commands.ConnectorFunctionCommands {
	public class GeneralConnectorFunctionInputCommand {
		public InputTypeEnum InputType { get; set; }

		public string Name { get; set; } = null!;

		public string Placeholder { get; set; } = null!;

		public string Replace { get; set; } = null!;

		public bool Required { get; set; }

		public string? DefaultValue { get; set; } = null!;

		public List<string>? Values { get; set; } = null!;

		public bool AdvancedOption { get; set; }
	}
}
