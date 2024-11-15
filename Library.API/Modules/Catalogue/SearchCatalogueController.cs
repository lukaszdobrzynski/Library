using System.Threading.Tasks;
using Library.Modules.Catalogue.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Modules.Catalogue;

[Route("api/catalogue/search")]
[ApiController]
public class SearchCatalogueController : ControllerBase
{
    private readonly ICatalogueModule _catalogueModule;
    
    public SearchCatalogueController(ICatalogueModule catalogueModule)
    {
        _catalogueModule = catalogueModule;
    }

    [HttpPost]
    [Route("all")]
    public async Task<IActionResult> All(BookSearchRequest request)
    {
        var query = request.ToSearchBooksQuery();
        var result = await _catalogueModule.ExecuteQueryAsync(query);

        return Ok(result);
    }
}