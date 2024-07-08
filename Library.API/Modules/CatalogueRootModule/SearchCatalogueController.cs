using System;
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

    [HttpPost]
    [Route("all")]
    public async Task<IActionResult> All(BookSearchRequest request)
    {
        var result = await _catalogueModule.ExecuteQueryAsync(new SearchAllBooksQuery
            { Term = request.Term, SearchType = Enum.Parse<BookSearchType>(request.SearchType), PageNumber = request.PageNumber, PageSize = request.PageSize });

        return Ok(result);
    }
}