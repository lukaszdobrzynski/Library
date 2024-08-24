using System;
using System.Text;

namespace Library.Modules.Reservation.IntegrationTests.PlaceBookOnHold;

public class PlaceBookOnHoldInitDbCommandBuilder
{
    private readonly StringBuilder _stringBuilder;
    
    private PlaceBookOnHoldInitDbCommandBuilder(string command)
    {
        _stringBuilder = new StringBuilder(command);
    }

    public static PlaceBookOnHoldInitDbCommandBuilder InitAllTables() => 
        new("CREATE SCHEMA IF NOT EXISTS reservations;" +
            "CREATE TABLE IF NOT EXISTS reservations.patrons (id uuid NOT NULL, patron_type VARCHAR NOT NULL,version_id INTEGER NOT NULL, PRIMARY KEY (id));" +
            "CREATE TABLE IF NOT EXISTS reservations.books (id uuid NOT NULL,library_branch_id uuid NOT NULL,book_category VARCHAR NOT NULL,version_id INTEGER NOT NULL,PRIMARY KEY (id));" +
            "CREATE TABLE IF NOT EXISTS reservations.holds (id uuid NOT NULL,patron_id uuid NOT NULL REFERENCES reservations.patrons (id),book_id uuid NOT NULL REFERENCES reservations.books (id),library_branch_id uuid NOT NULL,created_at date NOT NULL,till date,status VARCHAR NOT NULL,is_active BOOLEAN NOT NULL,request_hold_id uuid NOT NULL,version_id INTEGER NOT NULL,PRIMARY KEY (id));" +
            "CREATE TABLE IF NOT EXISTS reservations.outbox_messages (id uuid NOT NULL,occurred_on timestamp NOT NULL,processed_at timestamp,type VARCHAR NOT NULL,data JSON NOT NULL,PRIMARY KEY (id));" +
            "CREATE TABLE IF NOT EXISTS reservations.checkouts (id uuid NOT NULL,patron_id uuid NOT NULL REFERENCES reservations.patrons (id),book_id uuid NOT NULL REFERENCES reservations.books (id),library_branch_id uuid NOT NULL,due_date DATE NOT NULL,version_id INTEGER NOT NULL,PRIMARY KEY (id));");

    public PlaceBookOnHoldInitDbCommandBuilder WithRegularPatron(Guid patronId)
    {
        _stringBuilder.Append(
            "INSERT INTO reservations.patrons (id, patron_type, version_id)" +
            $"VALUES('{patronId}', 'Regular', 1);");
        return this;
    }

    public PlaceBookOnHoldInitDbCommandBuilder WithCirculatingBook(Guid bookId)
    {
        _stringBuilder.Append("INSERT INTO reservations.books (id, library_branch_id, book_category, version_id)" +
                              $"VALUES('{bookId}', 'ed4a1e1e-0c2b-40ef-8b34-567f2b6ef1e0', 'Circulating', 1);");
        return this;
    }
    
    public string Build() => _stringBuilder.ToString();
}