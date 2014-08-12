using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using prep.matching_core;

namespace prep.sorting
{
    public static class Sort<ItemToMatch>
    {
        public static IComparer<ItemToMatch> by<AttributeType>(IGetTheValueOfAnAttribute<ItemToMatch, AttributeType> accessor) where AttributeType : IComparable<AttributeType>
        {
            return by(accessor, SortOrders.ascending);
        }
        public static IComparer<ItemToMatch> by<AttributeType>(IGetTheValueOfAnAttribute<ItemToMatch, AttributeType> accessor, SortOrders order) where AttributeType : IComparable<AttributeType>
        {
            return new SortCriteria<ItemToMatch, AttributeType>(null, accessor, order);
        }
        public static IComparer<ItemToMatch> by<AttributeType>(IGetTheValueOfAnAttribute<ItemToMatch, AttributeType> accessor, params AttributeType[] fixedSort)
        {
            //return by(accessor, SortOrders.ascending);
            return new FixedSort<ItemToMatch, AttributeType>(accessor, fixedSort);
        }

    }
    public static class SortExtension
    {

        public static IComparer<ItemToMatch> then_by<ItemToMatch, AttributeType>(this IComparer<ItemToMatch> compareBefore, IGetTheValueOfAnAttribute<ItemToMatch, AttributeType> accessor, SortOrders order) where AttributeType : IComparable<AttributeType>
        {
            //return by(accessor, SortOrders.ascending);
            return new SortCriteria<ItemToMatch, AttributeType>(compareBefore, accessor, order);
        }
        public static IComparer<ItemToMatch> then_by<ItemToMatch, AttributeType>(this IComparer<ItemToMatch> compareBefore, IGetTheValueOfAnAttribute<ItemToMatch, AttributeType> accessor) where AttributeType : IComparable<AttributeType>
        {
            //return by(accessor, SortOrders.ascending);
            return then_by(compareBefore, accessor, SortOrders.ascending);
        }
    }

    public class FixedSort<ItemToMatch, AttributeType> : IComparer<ItemToMatch>
    {
        private readonly IGetTheValueOfAnAttribute<ItemToMatch, AttributeType> accessor;
        private readonly List<AttributeType> fixedSort;

        public FixedSort(IGetTheValueOfAnAttribute<ItemToMatch, AttributeType> accessor, AttributeType[] fixedSort)
        {
            this.accessor = accessor;
            this.fixedSort = fixedSort.ToList();
        }

        public int Compare(ItemToMatch x, ItemToMatch y)
        {
            var valX = fixedSort.IndexOf(accessor(x));
            var valY = fixedSort.IndexOf(accessor(y));
            return valX.CompareTo(valY);
        }
    }

    public class SortCriteria<ItemToMatch, AttributeType> : IComparer<ItemToMatch> where AttributeType : IComparable<AttributeType>
    {
        private readonly IComparer<ItemToMatch> compareBefore;
        private readonly IGetTheValueOfAnAttribute<ItemToMatch, AttributeType> accessor;
        private readonly int flip;

        public SortCriteria(IComparer<ItemToMatch> compareBefore, IGetTheValueOfAnAttribute<ItemToMatch, AttributeType> accessor, SortOrders order)
        {
            this.compareBefore = compareBefore ?? new DummyComparable();
            this.accessor = accessor;
            flip = order == SortOrders.ascending ? 1 : -1;
        }

        class DummyComparable : IComparer<ItemToMatch>
        {
            public int Compare(ItemToMatch x, ItemToMatch y)
            {
                return 0;
            }
        }

        public int Compare(ItemToMatch x, ItemToMatch y)
        {
            var firstCompare = compareBefore.Compare(x, y);
            if (firstCompare != 0) return firstCompare;
            return flip * accessor(x).CompareTo(accessor(y));
        }
    }

    public enum SortOrders
    {
        ascending, descending
    }
}
