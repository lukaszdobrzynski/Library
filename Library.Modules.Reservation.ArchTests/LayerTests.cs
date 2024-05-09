using NetArchTest.Rules;
using NUnit.Framework;

namespace Library.Modules.Reservation.ArchTests;

public class LayerTests : TestBase
{
    [Test]
    public void DomainLayer_DoesNotDependOn_ApplicationLayer()
    {
        var result = Types.InAssembly(DomainLayer)
            .Should()
            .NotHaveDependencyOn(ApplicationLayer.FullName)
            .GetResult();
        
        AssertEmptyArchTestResult(result);
    }

    [Test]
    public void DomainLayer_DoesNotDependOn_InfrastructureLayer()
    {
        var result = Types.InAssembly(DomainLayer)
            .Should()
            .NotHaveDependencyOn(InfrastructureLayer.FullName)
            .GetResult();
        
        AssertEmptyArchTestResult(result);
    }
    
    [Test]
    public void ApplicationLayer_DoesNotDependOn_InfrastructureLayer()
    {
        var result = Types.InAssembly(ApplicationLayer)
            .Should()
            .NotHaveDependencyOn(InfrastructureLayer.FullName)
            .GetResult();
        
        AssertEmptyArchTestResult(result);
    }

    [Test]
    public void InfrastructureLayer_DependsOn_DomainLayer()
    {
        var result = Types.InAssembly(InfrastructureLayer)
            .Should()
            .HaveDependencyOn(DomainLayer.FullName)
            .GetResult();
        
        AssertNotEmptyArchTestResult(result);
    }
    
    [Test]
    public void InfrastructureLayer_DependsOn_ApplicationLayer()
    {
        var result = Types.InAssembly(InfrastructureLayer)
            .Should()
            .HaveDependencyOn(ApplicationLayer.FullName)
            .GetResult();
        
        AssertNotEmptyArchTestResult(result);
    }
}