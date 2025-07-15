using Ordering.Domain.SeedWork;
using Ordering.Domain.Shared;

namespace Ordering.UnitTests.SeedWork;

public abstract class TestBase
{
    /// <summary>
    /// asserts that a specific domain event has been published by the aggregate.
    /// </summary>
    public static T AssertPublishedDomainEvent<T>(Entity aggregate) where T : IDomainEvent
    {
        var domainEvents = DomainEventsTestHelper.GetAllDomainEvents(aggregate).OfType<T>().SingleOrDefault();
        if (domainEvents == null)
        {
            throw new Exception($"{typeof(T).Name} event not published");
        }

        return domainEvents;
    }

    /// <summary>
    /// asserts that a specific domain event has been published by the aggregate and returns a list of all such events.
    /// </summary>
    public static List<T> AssertPublishedDomainEvents<T>(Entity aggregate) where T : IDomainEvent
    {
        var domainEvents = DomainEventsTestHelper.GetAllDomainEvents(aggregate).OfType<T>().ToList();
        if (domainEvents == null)
        {
            throw new Exception($"{typeof(T).Name} event not published");
        }

        return domainEvents;
    }

    /// <summary>
    /// asserts that a specific business rule is broken when executing the provided test delegate.
    /// </summary>
    public static void AssertBrokenRule<TRule>(TestDelegate testDelegate) where TRule : class, IBusinessRule
    {
        var message = $"Expected {typeof(TRule).Name} broken rule";
        var businessRuleValidationException = Assert.Catch<BusinessRuleValidationException>(testDelegate, message);
        if (businessRuleValidationException != null)
        {
            Assert.That(businessRuleValidationException.BrokenRule, Is.TypeOf<TRule>(), message);
        }
    }

    [TearDown]
    public void AfterEachTest()
    {
        SystemClock.Reset();
    }
}
