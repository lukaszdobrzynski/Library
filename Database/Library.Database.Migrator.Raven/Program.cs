using System.IO.Compression;
using Library.Modules.Catalogue.Models;
using Newtonsoft.Json;
using Raven.Client.Documents;

namespace Library.Database.Migrator.Raven;

class Program
{
    static int Main(string[] args)
    {
        var json1 = GetJsonFromArchive("books1");
        var json2 = GetJsonFromArchive("books2");
        var json3 = GetJsonFromArchive("books3");
        var json4 = GetJsonFromArchive("books4");
        var json5 = GetJsonFromArchive("books5");
        var json6 = GetJsonFromArchive("books6");
        var json7 = GetJsonFromArchive("books7");
        var json8 = GetJsonFromArchive("books8");
        var json9 = GetJsonFromArchive("books9");
        var json10 = GetJsonFromArchive("books10");

        var books = Deserialize(json1, json2, json3, json4, json5, json6, json7, json8, json9, json10);
        
        var store = new DocumentStore
        {
            Database = "Library.Catalogue",
            Urls = new[] { "http://library-one:8080" }
        };
        store.Initialize();
        
        using (var bulkInsert = store.BulkInsert())
        {
            foreach (var book in books)
            {
                bulkInsert.Store(book);
            }
        }
        
        return 0;
    }

    private static string GetJsonFromArchive(string fileName)
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Snapshots/{fileName}");
        
        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
        using (var reader = new StreamReader(gzipStream))
        {
            var jsonString = reader.ReadToEnd();
            return jsonString;
        }
    }

    private static List<Book> Deserialize(params string[] jsonText)
    {
        var bookList = new List<Book>();
        
        foreach (var json in jsonText)
        {
            var books = JsonConvert.DeserializeObject<List<Book>>(json);
            bookList.AddRange(books);
        }

        return bookList;
    }
}