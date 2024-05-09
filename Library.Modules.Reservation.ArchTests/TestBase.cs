using System.Reflection;
using Library.Modules.Reservation.Application.Contracts;
using Library.Modules.Reservation.Domain.Patrons;
using Library.Modules.Reservation.Infrastructure;
using NetArchTest.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.ArchTests;

public abstract class TestBase
{
    protected readonly Assembly DomainLayer = typeof(Patron).Assembly;

    protected readonly Assembly ApplicationLayer = typeof(CommandBase).Assembly;

    protected readonly Assembly InfrastructureLayer = typeof(ReservationModule).Assembly;

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