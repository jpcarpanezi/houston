using Houston.Core.Attributes;
using Houston.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Houston.Core.Entities.MongoDB {
	[BsonCollection("user")]
	public class User : EntityBase {
		[BsonRequired]
		[BsonElement("name")]
		public string Name { get; set; } = null!;

		[BsonRequired]
		[BsonElement("email")]
		public string Email { get; set; } = null!;

		[BsonRequired]
		[BsonElement("password")]
		public string Password { get; set; } = null!;

		[BsonRequired]
		[BsonElement("user_role")]
		public UserRoleEnum UserRole { get; set; }

		[BsonRequired]
		[BsonElement("is_first_access")]
		[BsonDefaultValue(true)]
		public bool IsFirstAccess { get; set; }

		[BsonRequired]
		[BsonElement("is_active")]
		public bool IsActive { get; set; }

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

		public User() { }

		public User(ObjectId id, string name, string email, string password, bool isFirstAccess, UserRoleEnum userRole, bool isActive, ObjectId createdBy, DateTime creationDate, ObjectId updatedBy, DateTime lastUpdate) : base(id) {
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Email = email ?? throw new ArgumentNullException(nameof(email));
			Password = password ?? throw new ArgumentNullException(nameof(password));
			IsFirstAccess = isFirstAccess;
			UserRole = userRole;
			IsActive = isActive;
			CreatedBy = createdBy;
			CreationDate = creationDate;
			UpdatedBy = updatedBy;
			LastUpdate = lastUpdate;
		}
	}
}
