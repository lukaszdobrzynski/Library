namespace Library.Modules.Catalogue.Application;

public interface IDomainNotificationsRegistry
{
    string GetName(Type type);

    Type GetType(string name);
}