using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ContosoRecipesApi.Models;

public record Recipe
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? RecipeId { get; set; }

    [BsonElement("Name")]
    [JsonPropertyName("Name")]
    public string Title { get; init; }
    public string Description { get; init; }
    public IEnumerable<string> Directions { get; init; }
    public IEnumerable<string> Tags { get; init; }

    [BsonRequired]
    public IEnumerable<Ingredient> Ingredients { get; init; }
    public DateTime Updated { get; init; }
}

public record Ingredient
{
    [BsonRequired]
    public string Name { get; init; }
    public int Amount { get; init; }
    public string Unit { get; init; }
}
