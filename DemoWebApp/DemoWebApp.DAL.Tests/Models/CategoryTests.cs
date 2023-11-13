using DemoWebApp.DAL.Models;
using NUnit.Framework;

namespace DemoWebApp.DAL.Tests.Models;

public class CategoryTests
{
    [Test]
    public void Clone_ReturnsShallowCopy_ComparingUsingJsonSerialization()
    {
        // Arrange
        Category category = TestHelper.GetCategory();

        // Act
        var clone = category.Clone();

        // Assert
        Assert.AreNotSame(category, clone);
        Assert.AreEqual(TestHelper.ToJson(category), TestHelper.ToJson(clone));
    }
}
