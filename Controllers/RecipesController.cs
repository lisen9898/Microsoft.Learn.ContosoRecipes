using ContosoRecipesApi.Models;
using ContosoRecipesApi.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ContosoRecipesApi.Controllers;


[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[ApiController]
[Route("api/[controller]")]
public class RecipesController(RecipesService recipesService) : ControllerBase
{
    private readonly RecipesService _recipesService = recipesService;

    /// <summary>
    /// Returns a recipe for a given id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Recipe>> GetRecipeById(string id)
    {
        var recipe = await _recipesService.GetAsync(id);

        if (recipe == null)
        {
            return NotFound();
        }

        return recipe;
    }

    [HttpGet]
    public async Task<ActionResult<List<Recipe>>> GetRecipes(int count)
    {
        var recipeList = await _recipesService.GetAsync(count);
        if (recipeList.Count < 0)
        {
            return NotFound();
        }

        return recipeList;
    }

    [HttpPatch("{id:length(24)}")]
    public async Task<IActionResult> Patch(string id, JsonPatchDocument<Recipe> recipeUpdates)
    {
        var recipe = await _recipesService.GetAsync(id);
        if (recipe == null)
        {
            return NotFound();
        }

        recipeUpdates.ApplyTo(recipe);

        await _recipesService.UpdateAsync(id, recipe);

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecipe([FromBody] Recipe newRecipe)
    {
        await _recipesService.CreateAsync(newRecipe);

        return CreatedAtAction(nameof(GetRecipeById), new { id = newRecipe.RecipeId }, newRecipe);
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<ActionResult> DeleteRecipe(string id)
    {
        var recipe = await _recipesService.GetAsync(id);

        if (recipe == null)
        {
            return NoContent();
        }

        await _recipesService.DeleteAsync(id);
        return NoContent();
    }
}
