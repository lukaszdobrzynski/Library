﻿namespace Library.Modules.Catalogue.Application.BookSearch;

public class BookSearchQueryParameters
{
    public string Term { get; set; }
    public BookSearchType SearchType { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}