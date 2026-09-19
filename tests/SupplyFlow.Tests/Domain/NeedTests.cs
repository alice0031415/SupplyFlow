using SupplyFlow.Procurement.Domain.Needs;

namespace SupplyFlow.Tests.Domain;

public sealed class NeedTests
{
    [Fact]
    public void PublishTender_ShouldChangeStatusAndRaiseDomainEvent()
    {
        // Arrange
        var need = new Need(
            "Ноутбуки",
            10,
            new DateOnly(2026, 10, 20));

        // Act
        need.PublishTender();

        // Assert
        Assert.Equal(NeedStatus.Published, need.Status);
        Assert.Single(need.DomainEvents);

        var domainEvent = Assert.IsType<TenderPublishedDomainEvent>(
            need.DomainEvents.Single());

        Assert.Equal(need.Id, domainEvent.NeedId);
    }

    [Fact]
    public void PublishTender_WhenAlreadyPublished_ShouldThrow()
    {
        // Arrange
        var need = new Need(
            "Ноутбуки",
            10,
            new DateOnly(2026, 10, 20));

        need.PublishTender();

        // Act + Assert
        Assert.Throws<InvalidOperationException>(
            () => need.PublishTender());
    }
}