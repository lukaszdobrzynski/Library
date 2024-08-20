using System;

namespace Library.API;

public class ApplicationConfigurationException : Exception
{
    public ApplicationConfigurationException(string message) : base(message)
    {
    }
}