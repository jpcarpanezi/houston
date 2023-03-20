using Houston.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Houston.Core.Entities.MongoDB {
	public class ConnectorFunctionInput : EntityBase {
		[BsonRequired]
		[BsonElement("name")]
		public string Name { get; set; } = null!;

		[BsonRequired]
		[BsonElement("input_type")]
		public InputTypeEnum InputType { get; set; }

		[BsonRequired]
		[BsonElement("required")]
		public bool Required { get; set; }

		[BsonRequired]
		[BsonElement("placeholder")]
		public string Placeholder { get; set; } = null!;

		[BsonRequired]
		[BsonElement("replace")]
		public string Replace { get; set; } = null!;

		[BsonRequired]
		[BsonElement("default_value")]
		public string? DefaultValue { get; set; } = null!;

		[BsonElement("values")]
		public List<string>? Values { get; set; } = null!;

		[BsonRequired]
		[BsonDefaultValue(false)]
		[BsonElement("advanced_option")]
		public bool AdvancedOption { get; set; }

		[BsonRequired]
		[BsonElement("created_by")]
		public ObjectId CreatedBy { get; set; }

		[BsonRequired]
		[BsonElement("creation_date")]
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime CreationDate { get; set; }

		[BsonRequired]
		[BsonElement("updated_by")]
		public ObjectId UpdatedBy { get; set; }

		[BsonRequired]
		[BsonElement("last_update")]
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime LastUpdate { get; set; }

		public ConnectorFunctionInput() { }

		public ConnectorFunctionInput(string name, InputTypeEnum inputType, string placeholder, string replace, string defaultValue, List<string>? values, bool advancedOption, ObjectId createdBy, DateTime creationDate, ObjectId updatedBy, DateTime lastUpdate) {
			Name = name ?? throw new ArgumentNullException(nameof(name));
			InputType = inputType;
			Placeholder = placeholder ?? throw new ArgumentNullException(nameof(placeholder));
			Replace = replace ?? throw new ArgumentNullException(nameof(replace));
			DefaultValue = defaultValue ?? throw new ArgumentNullException(nameof(defaultValue));
			Values = values;
			AdvancedOption = advancedOption;
			CreatedBy = createdBy;
			CreationDate = creationDate;
			UpdatedBy = updatedBy;
			LastUpdate = lastUpdate;
		}
	}
}
