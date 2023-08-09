using Dictionary.API.Mapping;
using Dictionary.Application.Services.DictionaryServices;
using Microsoft.AspNetCore.Mvc;

namespace Dictionary.API.Controllers;

[ApiController]
public class DictionaryController : ControllerBase
{
    private readonly IDictionaryService _dictionaryService;
    public DictionaryController(IDictionaryService dictionaryService)
    {
        _dictionaryService = dictionaryService;
    }

    [HttpGet(ApiEndpoints.Dictionary.Get)]
    public async Task<IActionResult> GetWord([FromRoute] string request, CancellationToken cancellationToken)
    {
        var response = await _dictionaryService.GetWordAsync(request, cancellationToken);

        if (response is null)
            return NotFound();
        
        return Ok(response.MapToWordResponse());
    }
}