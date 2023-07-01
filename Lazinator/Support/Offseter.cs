using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Support
{
    public static class Offseter
    {
        public static (int bucketIndex, int itemIndex) MoveForward(int numBuckets, Func<int, int> numItemsInBucket, int startingBucketIndex, int startingItemIndex, long numItemsToMoveForward)
        {
            long remainingItems = numItemsToMoveForward;
            int currentBucketIndex = startingBucketIndex;
            int currentItemIndex = startingItemIndex;

            while (remainingItems > 0)
            {
                int itemsInCurrentBucket = numItemsInBucket(currentBucketIndex);

                if (itemsInCurrentBucket == 0)
                {
                    do
                    {
                        currentBucketIndex++;
                        if (currentBucketIndex == numBuckets)
                        {
                            return (currentBucketIndex - 1, numItemsInBucket(currentBucketIndex - 1));
                        }
                        itemsInCurrentBucket = numItemsInBucket(currentBucketIndex);
                    } while (itemsInCurrentBucket == 0);

                    currentItemIndex = 0;
                }

                if (currentItemIndex + remainingItems < itemsInCurrentBucket)
                {
                    currentItemIndex += (int)remainingItems;
                    remainingItems = 0;
                }
                else
                {
                    remainingItems -= itemsInCurrentBucket - currentItemIndex;
                    currentBucketIndex++;
                    currentItemIndex = 0;

                    if (currentBucketIndex == numBuckets)
                    {
                        return (currentBucketIndex - 1, numItemsInBucket(currentBucketIndex - 1));
                    }
                }
            }

            return (currentBucketIndex, currentItemIndex);
        }
    }


}
