
public class Tests
{
    [Test]
#pragma warning disable TUnitAssertions0005 // Assert.That(...) should not be used with a constant value
    public async Task Works() => await Assert.That(true).IsTrue();
#pragma warning restore TUnitAssertions0005 // Assert.That(...) should not be used with a constant value

}
