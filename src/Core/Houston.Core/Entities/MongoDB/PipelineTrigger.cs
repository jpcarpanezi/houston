using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Houston.Core.Entities.MongoDB {
	public class PipelineTrigger {
		[BsonRequired]
		[BsonElement("on")]
		public string On { get; set; } = null!;

		[BsonElement("filter")]
		public string? Filter { get; set; }

		[BsonElement("filter_values")]
		public List<string>? FilterValues { get; set; }

		public PipelineTrigger() { }

		public PipelineTrigger(string on, string? filter, List<string>? filterValues) {
			On = on ?? throw new ArgumentNullException(nameof(on));
			Filter = filter;
			FilterValues = filterValues;
		}
	}
}
