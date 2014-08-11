using System;
using prep.matching;
using prep.ranges;

namespace prep.collections
{
  public static class MatchCreationExtensions
  {
    public static IMatchA<ItemToMatch> equal_to<ItemToMatch, AttributeType>(this MatchCreationExtensionPoint<ItemToMatch,AttributeType> extension_point, AttributeType value)
    {
      return extension_point.equal_to_any(value);
    }

    public static IMatchA<ItemToMatch> equal_to_any<ItemToMatch, AttributeType>(this MatchCreationExtensionPoint<ItemToMatch,AttributeType> extension_point, params AttributeType[] values)
    {
      return extension_point.for_value_matcher(new EqualAnyMatch<AttributeType>(values));
    }

    public static IMatchA<ItemToMatch> not_equal_to<ItemToMatch, AttributeType>(this MatchCreationExtensionPoint<ItemToMatch,AttributeType> extension_point, AttributeType value)
    {
      return extension_point.equal_to(value).not();
    }

    public static IMatchA<ItemToMatch> for_value_matcher<ItemToMatch, AttributeType>(this MatchCreationExtensionPoint<ItemToMatch,AttributeType> extension_point, IMatchA<AttributeType> value_condition)
    {
      return new AttributeMatch<ItemToMatch, AttributeType>(extension_point.accessor, extension_point.negate, value_condition);
    }

    public static IMatchA<ItemToMatch> for_condition<ItemToMatch, AttributeType>(this MatchCreationExtensionPoint<ItemToMatch,AttributeType> extension_point, Predicate<ItemToMatch> condition)
    {
      return new ConditionalMatch<ItemToMatch>(condition);
    }

    public static IMatchA<ItemToMatch> falls_in<ItemToMatch, AttributeType>(this MatchCreationExtensionPoint<ItemToMatch,AttributeType> extension_point, IContainValues<AttributeType> range)
      where AttributeType : IComparable<AttributeType>
    {
      return extension_point.for_value_matcher(new FallsIn<AttributeType>(range));
    }

    public static IMatchA<ItemToMatch> greater_than<ItemToMatch, AttributeType>(this MatchCreationExtensionPoint<ItemToMatch,AttributeType> extension_point, AttributeType value)
      where AttributeType : IComparable<AttributeType>
    {
      return extension_point.for_value_matcher(new GreaterThan<AttributeType>(value));
    }

    public static IMatchA<ItemToMatch> less_than<ItemToMatch, AttributeType>(this MatchCreationExtensionPoint<ItemToMatch,AttributeType> extension_point, AttributeType value)
      where AttributeType : IComparable<AttributeType>
    {
      return extension_point.for_value_matcher(new LessThan<AttributeType>(value));
    }

    public static IMatchA<ItemToMatch> between<ItemToMatch, AttributeType>(this MatchCreationExtensionPoint<ItemToMatch,AttributeType> extension_point, AttributeType start, AttributeType end)
      where AttributeType : IComparable<AttributeType>
    {
      throw new NotImplementedException();
    }
  }
}
