namespace LinQ.LinQ
{
    public static class LinqExtensions
    {
        public static T FirstInLinQ<T>(this IEnumerable<T> source, Func<T, bool> predicate) where T : class
        {
            foreach (var item in source)
            {
                if (predicate(item)) return item;
            }
            return null;
        }
        public static IEnumerable<T> WhereInLinQ<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item)) yield return item;
            }
        }
        // Eg TKey: "Age"
        public static IEnumerable<T> OrderByDescendingInLinQ<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
        {
            List<T> sortedList = new List<T>(source);

            // Sort the list in descending order based on the key
            sortedList.Sort((x, y) => keySelector(y).CompareTo(keySelector(x)));

            return sortedList;
        }

        public static IEnumerable<TResult> SelectInLinQ<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            foreach (var item in source)
            {
                yield return selector(item);
            }
        }
        public static IEnumerable<IGrouping<TKey, TSource>> GroupByInLinQ<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            // Create a dictionary to store the groups
            var groupDict = new Dictionary<TKey, List<TSource>>();
            foreach (var item in source)
            {
                // Get the key for the current item
                var key = keySelector(item);

                // If the key is not in the dictionary, add it with an empty list
                if (!groupDict.ContainsKey(key))
                {
                    groupDict[key] = new List<TSource>();
                }

                // Add the item to the corresponding group
                groupDict[key].Add(item);
            }

            // Convert the dictionary into a collection of groupings
            foreach (var group in groupDict)
            {
                yield return new Grouping<TKey, TSource>(group.Key, group.Value);
            }
        }
        // Custom Grouping class to implement IGrouping<TKey, TSource>
        public class Grouping<TKey, TSource> : IGrouping<TKey, TSource>
        {
            private readonly TKey _key;
            private readonly IEnumerable<TSource> _group;

            public Grouping(TKey key, IEnumerable<TSource> group)
            {
                _key = key;
                _group = group;
            }

            public TKey Key => _key;

            public IEnumerator<TSource> GetEnumerator()
            {
                return _group.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return _group.GetEnumerator();
            }
        }
    }
}
