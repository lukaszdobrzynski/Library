using NetArchTest.Rules;
using NUnit.Framework;

namespace Library.Modules.Catalogue.ArchTests;

public class LayerTests : TestBase
{
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
    public void InfrastructureLayer_DependsOn_ApplicationLayer()
    {
        var result = Types.InAssembly(InfrastructureLayer)
            .Should()
            .HaveDependencyOn(ApplicationLayer.FullName)
            .GetResult();
        
        AssertNotEmptyArchTestResult(result);
    }
}