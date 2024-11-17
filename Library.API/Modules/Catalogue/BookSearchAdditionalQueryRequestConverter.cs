using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Library.API.Modules.Catalogue;

public class BookSearchAdditionalQueryRequestConverter : JsonConverter<BookSearchAdditionalQueryRequest>
{
    public override BookSearchAdditionalQueryRequest Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (var jsonDoc = JsonDocument.ParseValue(ref reader))
        {
            var rootElement = jsonDoc.RootElement;

            if (!rootElement.TryGetProperty($"{nameof(BookSearchAdditionalQueryRequest.Type)}", out var typeProperty))
                throw new JsonException($"{nameof(BookSearchAdditionalQueryRequest.Type)} property is required.");

            if (Enum.TryParse(typeof(BookSearchAdditionalQueryType), typeProperty.ToString(), out var type) == false)
                throw new JsonException($"{nameof(BookSearchAdditionalQueryRequest.Type)} {typeProperty} is invalid.");
            
            BookSearchAdditionalQueryRequest request = type switch
            {
                BookSearchAdditionalQueryType.Text => JsonSerializer.Deserialize<BookSearchAdditionalTextQueryRequest>(rootElement.GetRawText(), options),
                BookSearchAdditionalQueryType.DateRange => JsonSerializer.Deserialize<BookSearchAdditionalDateRangeQueryRequest>(rootElement.GetRawText(), options),
                BookSearchAdditionalQueryType.DateSequence => JsonSerializer.Deserialize<BookSearchAdditionalDateSequenceQueryRequest>(rootElement.GetRawText(), options),
                _ => throw new JsonException($"Unknown type '{type}'")
            };
            
            try
            {
                ValidateModel(request);
            }
            catch (ValidationException ex)
            {
                throw new JsonException($"Validation failed: {ex.Message}", ex);
            }

            return request;
        }
    }

    public override void Write(Utf8JsonWriter writer, BookSearchAdditionalQueryRequest value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
    
    private static void ValidateModel(BookSearchAdditionalQueryRequest request)
    {
        var validationContext = new ValidationContext(request);
        Validator.ValidateObject(request, validationContext, validateAllProperties: true);
    }
}