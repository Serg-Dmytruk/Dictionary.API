using Microsoft.AspNetCore.Mvc;

namespace Dictionary.API.Controllers;

[ApiController]
public class DictionaryController : ControllerBase
{
    public DictionaryController()
    {
        
    }

    /*[HttpGet(ApiEndpoints.Dictionary.Get)]
    public async Task<IActionResult> GetWord([FromRoute] string request)
    {
        
    }*/
}