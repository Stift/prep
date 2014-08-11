using prep.matching;

namespace prep.collections
{
    public class MatchCreationExtensionPoint<ItemToMatch, AttributeType>
    {
        public bool negate;
        public IGetTheValueOfAnAttribute<ItemToMatch, AttributeType> accessor { get; set; }
        public MatchCreationExtensionPoint<ItemToMatch, AttributeType> not
        {
            get
            {
                negate = !negate;
                return this;
            }
        }

        public MatchCreationExtensionPoint(IGetTheValueOfAnAttribute<ItemToMatch, AttributeType> accessor)
        {
            this.accessor = accessor;
        }


    }
}