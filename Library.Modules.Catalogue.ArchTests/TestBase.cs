using System;
using System.Collections.Generic;
using System.Reflection;
using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Infrastructure;
using NetArchTest.Rules;
using NUnit.Framework;

namespace Library.Modules.Catalogue.ArchTests;

public abstract class TestBase
{
    protected readonly Assembly ApplicationLayer = typeof(ICatalogueModule).Assembly;

    protected readonly Assembly InfrastructureLayer = typeof(CatalogueModule).Assembly;

    private void AssertEmptyArchTestResult(IEnumerable<Type> types)
    {
        Assert.That(types, Is.Null.Or.Empty);
    }
    
    protected void AssertEmptyArchTestResult(TestResult result)
    {
        AssertEmptyArchTestResult(result.FailingTypes);
    }
    
    private void AssertNotEmptyArchTestResult(IEnumerable<Type> types)
    {
        Assert.That(types, Is.Not.Empty);
    }
    
    protected void AssertNotEmptyArchTestResult(TestResult result)
    {
        AssertNotEmptyArchTestResult(result.FailingTypes);
    }
}