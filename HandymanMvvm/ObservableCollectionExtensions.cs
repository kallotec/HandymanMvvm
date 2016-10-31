using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HandymanMvvm
{
    public static class ObservableCollectionExtensions
    {
        public static void Refresh<TSource, TDestination>(this ObservableCollection<TDestination> destination, List<TSource> source, Func<TSource, TDestination, bool> matchCondition, Action<TSource, TDestination> createAndUpdateAction, Action<TDestination> deleteAction = null, Func<TDestination, object> orderBy = null)
            where TSource : class, new()
            where TDestination : class, new()
        {
            // Remove cycle - remove any items that are in the 'to' list but not in the 'from' list
            for (var x = 0; x < destination.Count; x++)
            {
                var to = destination[x];

                //try find any matches
                var isInFromList = source.Any(from => matchCondition(from, to));

                //remove 'to' if not found in 'from' list
                if (!isInFromList)
                {
                    //execute any delete action - could be removing event handlers or anything
                    deleteAction?.Invoke(to);

                    //remove from list
                    destination.RemoveAt(x);
                    x--;
                }
            }

            // Update cycle
            foreach (var from in source)
            {
                var found = false;

                foreach (var to in destination)
                {
                    //try match
                    var match = matchCondition(from, to);
                    if (match)
                    {
                        //update 'to' with data from 'from'
                        createAndUpdateAction(from, to);
                        found = true;
                        break;
                    }
                }

                //add new 'to' item - if no existing item found
                if (found == false)
                {
                    var newItem = new TDestination();
                    createAndUpdateAction(from, newItem);
                    destination.Add(newItem);
                }
            }

            // Sort algorithm
            if (orderBy != null)
            {
                var reorderedList = destination.OrderBy(orderBy).ToList();

                for (var x = 0; x < reorderedList.Count; x++)
                {
                    var item = reorderedList[x];
                    var currIndex = destination.IndexOf(item);

                    if (x == currIndex)
                        continue;

                    // Move
                    destination.Move(currIndex, x);

                    // Restart loop since indexes have changed
                    x = -1;

                }
            }
        }

        /// <summary>
        /// Convenience method for RefreshableModel<TSource> inputs
        /// </summary>
        public static void RefreshModels<TSource, TDestination>(this ObservableCollection<TDestination> destination, List<TSource> source, Func<TSource, TDestination, bool> matchCondition, Action<TDestination> deleteAction = null, Func<TDestination, object> orderBy = null)
            where TSource : class, new()
            where TDestination : RefreshableModel<TSource>, new()
        {
            destination.Refresh(source, matchCondition, (s, d) => d.Refresh(s), deleteAction, orderBy);
        }

    }
}
