using System.ComponentModel.DataAnnotations;

namespace Library.API.Modules.Catalogue;

public class ValidatePolymorphicAdditionalQueryAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is BookSearchTextAdditionalQueryRequest textQuery)
        {
            if (textQuery.Term == null)
            {
                return new ValidationResult($"{nameof(BookSearchTextAdditionalQueryRequest.Term)} " +
                                            $"is required for {nameof(BookSearchAdditionalQueryType.Text)} query.");
            }

            if (textQuery.SearchSource == null)
            {
                return new ValidationResult($"{nameof(BookSearchTextMainQueryRequest.SearchSource)} " +
                                            $"is required for {nameof(BookSearchAdditionalQueryType.Text)} query.");
            }

            if (textQuery.SearchType == null)
            {
                return new ValidationResult($"{nameof(BookSearchTextMainQueryRequest.SearchType)} " +
                                            $"is required for {nameof(BookSearchAdditionalQueryType.Text)} query.");
            }
        }
        
        if (value is BookSearchDateRangeAdditionalQueryRequest { SearchSource: null })
        {
            return new ValidationResult($"{nameof(BookSearchDateRangeAdditionalQueryRequest.SearchSource)} " +
                                        $"is required for {nameof(BookSearchAdditionalQueryType.DateRange)} query.");
        }
        
        if (value is BookSearchDateSequenceAdditionalQueryRequest sequenceQuery)
        {
            if (sequenceQuery.SearchSource == null)
            {
                return new ValidationResult($"{nameof(BookSearchDateSequenceAdditionalQueryRequest.SearchSource)} " +
                                            $"is required for {nameof(BookSearchAdditionalQueryType.DateSequence)} query.");
            }

            if (sequenceQuery.SequenceOperator == null)
            {
                return new ValidationResult($"{nameof(BookSearchDateSequenceAdditionalQueryRequest.SequenceOperator)} " +
                                            $"is required for {nameof(BookSearchAdditionalQueryType.DateSequence)} query.");
            }
        }
        
        return ValidationResult.Success;
    }
}