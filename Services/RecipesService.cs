using ContosoRecipesApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ContosoRecipesApi.Services;

public class RecipesService
{
    private readonly IMongoCollection<Recipe> _recipesCollection;

    public RecipesService(IOptions<ContosoRecipesDatabaseSettings> contosoRecipesDatabaseSettings)
    {
        var mongoClient = new MongoClient(contosoRecipesDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            contosoRecipesDatabaseSettings.Value.DatabaseName
        );

        _recipesCollection = mongoDatabase.GetCollection<Recipe>(
            contosoRecipesDatabaseSettings.Value.RecipesCollectionName
        );
    }

    public async Task<List<Recipe>> GetAsync(int count) =>
        await _recipesCollection.Find(_ => true).Limit(count).ToListAsync();

    public async Task<Recipe?> GetAsync(string id) =>
        await _recipesCollection.Find(x => x.RecipeId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Recipe recipe) => await _recipesCollection.InsertOneAsync(recipe);

    public async Task UpdateAsync(string id, Recipe updatedRecipe) =>
        await _recipesCollection.ReplaceOneAsync(x => x.RecipeId == id, updatedRecipe);

    public async Task DeleteAsync(string id) =>
        await _recipesCollection.DeleteOneAsync(x => x.RecipeId == id);
}
