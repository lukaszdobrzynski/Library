using System.Threading.Tasks;
using Library.Modules.Catalogue.Application.BookSearch;
using Library.Modules.Catalogue.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Modules.CatalogueRootModule;

[Route("api/catalogue/search")]
[ApiController]
public class SearchCatalogueController : ControllerBase
{
    private readonly ICatalogueModule _catalogueModule;
    
    public SearchCatalogueController(ICatalogueModule catalogueModule)
    {
        _catalogueModule = catalogueModule;
    }

    [HttpGet]
    [Route("{q}")]
    public async Task<IActionResult> All(string q, int pageNumber, int pageSize)
    {
        var result = await _catalogueModule.ExecuteQueryAsync(new SearchAllBooksQuery
            { Term = q, PageNumber = pageNumber, PageSize = pageSize });

        return Ok(result);
    }
}