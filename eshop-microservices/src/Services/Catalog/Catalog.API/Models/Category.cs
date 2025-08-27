using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Models;

public class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("slug")]
    public string Slug { get; set; }

    [BsonElement("parent_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ParentId { get; set; }

    [BsonElement("ancestors")]
    public List<AncestorCategory> Ancestors { get; set; }
}

// Represents an ancestor in the category hierarchy
public class AncestorCategory
{
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("slug")]
    public string Slug { get; set; }
}