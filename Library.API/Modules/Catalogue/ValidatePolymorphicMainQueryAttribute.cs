using System.ComponentModel.DataAnnotations;

namespace Library.API.Modules.Catalogue;

public class ValidatePolymorphicMainQueryAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is BookSearchTextMainQueryRequest textQuery)
        {
            if (textQuery.Term == null)
            {
                return new ValidationResult($"{nameof(BookSearchTextMainQueryRequest.Term)} " +
                                            $"is required for {nameof(BookSearchMainQueryType.Text)} query.");
            }

            if (textQuery.SearchSource == null)
            {
                return new ValidationResult($"{nameof(BookSearchTextMainQueryRequest.SearchSource)} " +
                                            $"is required for {nameof(BookSearchMainQueryType.Text)} query.");
            }

            if (textQuery.SearchType == null)
            {
                return new ValidationResult($"{nameof(BookSearchTextMainQueryRequest.SearchType)} " +
                                            $"is required for {nameof(BookSearchMainQueryType.Text)} query.");
            }
        }
        
        if (value is BookSearchDateRangeMainQueryRequest { SearchSource: null })
        {
            return new ValidationResult($"{nameof(BookSearchDateRangeMainQueryRequest.SearchSource)} " +
                                        $"is required for {nameof(BookSearchMainQueryType.DateRange)} query.");
        }
        
        if (value is BookSearchDateSequenceMainQueryRequest sequenceQuery)
        {
            if (sequenceQuery.SearchSource == null)
            {
                return new ValidationResult($"{nameof(BookSearchDateSequenceMainQueryRequest.SearchSource)} " +
                                            $"is required for {nameof(BookSearchMainQueryType.DateSequence)} query.");
            }

            if (sequenceQuery.SequenceOperator == null)
            {
                return new ValidationResult($"{nameof(BookSearchDateSequenceMainQueryRequest.SequenceOperator)} " +
                                            $"is required for {nameof(BookSearchMainQueryType.DateSequence)} query.");
            }
        }
        
        return ValidationResult.Success;
    }
}