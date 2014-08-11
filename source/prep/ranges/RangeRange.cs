using System;
using prep.collections;
using prep.matching;

namespace prep.ranges
{
    public static class RangeFactory
    {

        public static IContainValues<AttributeType> start_from<AttributeType>(AttributeType start) where AttributeType : IComparable<AttributeType>
        {
            return new StartFromContain<AttributeType>(start);
        }

        public static IContainValues<AttributeType> ends_at<AttributeType>(this IContainValues<AttributeType> containValue, AttributeType end) where AttributeType : IComparable<AttributeType>
        {
            return new EndAtContain<AttributeType>(containValue, end);
        }

    }

    internal class StartFromContain<AttributeType> : IContainValues<AttributeType> where AttributeType : IComparable<AttributeType>
    {
        private readonly AttributeType start;
        private IMatchA<AttributeType> matcher;
        private readonly MatchCreationExtensionPoint<AttributeType, AttributeType> accessor;

        public StartFromContain(AttributeType start)
        {
            this.start = start;
            accessor = Match<AttributeType>.with_attribute(any => any);
            matcher = accessor.greater_than(start);
        }

        public bool contains(AttributeType value)
        {
            return matcher.matches(value);
        }

        public IContainValues<AttributeType> inclusive
        {
            get
            {
                matcher = matcher.or(accessor.equal_to(start));
                return this;
            }
        }
    }

    internal class EndAtContain<AttributeType> : IContainValues<AttributeType> where AttributeType : IComparable<AttributeType>
    {
        private readonly IContainValues<AttributeType> containValue;
        private readonly AttributeType end;
        private IMatchA<AttributeType> matcher;
        private readonly MatchCreationExtensionPoint<AttributeType, AttributeType> accessor;

        public EndAtContain(IContainValues<AttributeType> containValue, AttributeType end)
        {
            this.containValue = containValue;
            this.end = end;
            this.containValue = containValue;
            accessor = Match<AttributeType>.with_attribute(any => any);
            matcher = accessor.less_than(end);
        }

        public bool contains(AttributeType value)
        {
            return containValue.contains(value) && (matcher).matches(value);
        }

        public IContainValues<AttributeType> inclusive
        {
            get
            {
                matcher = matcher.or(accessor.equal_to(end));
                return this;
            }
        }
    }
}