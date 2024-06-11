using Library.Modules.Catalogue.Models;
using Newtonsoft.Json;
using Raven.Client.Documents;

namespace Library.Database.Migrator.Raven;

class Program
{
    static int Main(string[] args)
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Snapshots/books.json");
        var jsonText = File.ReadAllText(filePath);

        var books = JsonConvert.DeserializeObject<List<Book>>(jsonText);
        
        var store = new DocumentStore
        {
            Database = "Library.Catalogue",
            Urls = new[] { "http://localhost:8080" }
        };
        store.Initialize();

        foreach (var book in books)
        {
            using (var session = store.OpenSession())
            { 
                session.Store(book);
                session.SaveChanges();
            }    
        }
        
        return 0;
    }
}