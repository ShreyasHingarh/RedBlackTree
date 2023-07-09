using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    public interface ISortedSet<T> : IEnumerable<T>
    {
        IComparer<T> Comparer { get; }
        int Count { get; }
        void Clear();
        bool Add(T item);
        void AddRange(IEnumerable<T> items);
        bool Contains(T item);
        bool Remove(T item);
        T Max();
        T Min();
        T Ceiling(T item);
        T Floor(T item);
        ISortedSet<T> Union(ISortedSet<T> other);
        ISortedSet<T> Intersection(ISortedSet<T> other);
    }

    public interface ISortedSet<TSelf, T> : ISortedSet<T> where TSelf : ISortedSet<TSelf>
    {
        new TSelf Union(ISortedSet<T> other);
        new TSelf Intersection(ISortedSet<T> other);
    }
}
