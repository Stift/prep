using prep.matching;

namespace prep.collections
{
    public class AttributeMatch<ItemToMatch, AttributeType> : IMatchA<ItemToMatch>
    {
        IGetTheValueOfAnAttribute<ItemToMatch, AttributeType> get_the_value;
        private readonly bool _negateMatch;
        IMatchA<AttributeType> value_criteria;

        public AttributeMatch(IGetTheValueOfAnAttribute<ItemToMatch, AttributeType> get_the_value, bool negateMatch, IMatchA<AttributeType> value_criteria)
        {
            this.get_the_value = get_the_value;
            _negateMatch = negateMatch;
            this.value_criteria = value_criteria;
        }

        public bool matches(ItemToMatch item)
        {
            var value = get_the_value(item);
            return _negateMatch ? !value_criteria.matches(value) : value_criteria.matches(value);
        }
    }
}