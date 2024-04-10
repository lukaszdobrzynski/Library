﻿using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.Rules;

namespace Library.Modules.Lending.Domain.Book;

public class BookCategory : ValueObject
{
    private string _value;

    public static BookCategory Circulating => new (nameof(Circulating));
    public static BookCategory Restricted => new(nameof(Restricted));
    
    private BookCategory(string value)
    {
        _value = value;
    }

    public static BookCategory Create(string value)
    {
        CheckRule(new BookCategoryMustBeDefinedRule(value));

        return new BookCategory(value);
    }
}