using MongoDB.Bson;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Houston.Core.Converters {
	public class ObjectIdConverter : JsonConverter<ObjectId> {
		public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			string? value = reader.GetString();

			if (ObjectId.TryParse(value, out ObjectId objectId)) {
				return objectId;
			} else {
				throw new JsonException($"Invalid ObjectId value: {value}");
			}
		}

		public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options) {
			writer.WriteStringValue(value.ToString());
		}
	}
}
