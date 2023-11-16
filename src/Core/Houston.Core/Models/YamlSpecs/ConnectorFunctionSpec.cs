namespace Houston.Core.Models.YamlSpecs {
	public class ConnectorFunctionSpec {
		[YamlMember(Alias = "connector")]
		public string Connector { get; set; } = null!;

		[YamlMember(Alias = "function")]
		public string Function { get; set; } = null!;

		[YamlMember(Alias = "version")]
		public string Version { get; set; } = null!;
		
		[YamlMember(Alias = "friendly_name")]
		public string FriendlyName { get; set; } = null!;
		
		[YamlMember(Alias = "description")]
		public string? Description { get; set; }

		[YamlMember(Alias = "inputs")]
		public Dictionary<string, ConnectorFunctionInputsSpec> Inputs { get; set; } = new();

		[YamlMember(Alias = "runs")]
		public ConnectorFunctionRunsSpec Runs { get; set; } = null!;
	}

	public class ConnectorFunctionInputsSpec {
		[YamlMember(Alias = "type")]
		public string Type { get; set; } = null!;

		[YamlMember(Alias = "label")]
		public string Label { get; set; } = null!;

		[YamlMember(Alias = "placeholder")]
		public string Placeholder { get; set; } = null!;
		
		[YamlMember(Alias = "values")]
		public string[]? Values { get; set; }

		[YamlMember(Alias = "default")]
		public string? Default { get; set; }
		
		[YamlMember(Alias = "required")]
		public bool Required { get; set; }
		
		[YamlMember(Alias = "advanced")]
		public bool Advanced { get; set; }
	}

	public class ConnectorFunctionRunsSpec {
		[YamlMember(Alias = "using")]
		public string Using { get; set; } = null!;
	}
}
