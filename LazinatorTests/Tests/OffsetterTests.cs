using FluentAssertions;
using Lazinator.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LazinatorTests.Tests
{
    using Xunit;
    using FluentAssertions;
    using System;

    public class OffseterTests
    {
        [Fact]
        public void MoveForward_Should_Return_Expected_Result_When_Moving_From_Beginning_Of_First_Bucket()
        {
            // Arrange
            int numBuckets = 5;
            Func<int, int> numItemsInBucket = x => new int[] { 2, 5, 1, 0, 6 }[x];
            int startingBucketIndex = 0;
            int startingItemIndex = 0;
            long numItemsToMoveForward = 10;

            // Reasoning:
            // Starting at beginning of bucket 0 with 10 items to move forward:
            // - In bucket 0, there are 2 items, which is less than 10.
            //   - So, we can move forward 2 items, putting us at the beginning of bucket 1 with 8 moves more.
            // - In bucket 1, there are 5 items, which is less than 8. 
            //   - So, we can move forward 5 items, putting us at the beginning of bucket 2 with 3 moves more.
            // - In bucket 2, there is 1 item, which is less than 3.
            //   - So, we can move forward 1 item, putting us at the beginning of bucket 3 with 2 moves more.
            // - In bucket 3, there are 0 items. So, we move to the beginning of bucket 4 with 2 moves more.
            // - In bucket 4, there are 6 items, which is greater than 2.
            //   - So, we can move our final 2 moves, and we end up at (4, 2).

            // Act
            var result = Offseter.MoveForward(numBuckets, numItemsInBucket, startingBucketIndex, startingItemIndex, numItemsToMoveForward);

            // Assert
            result.Should().Be((4, 2));
        }
        
        [Fact]
        public void MoveForward_Should_Return_Expected_Result_When_Moving_From_Middle_Of_Bucket()
        {
            // Arrange
            int numBuckets = 3;
            Func<int, int> numItemsInBucket = x => new int[] { 5, 4, 7 }[x];
            int startingBucketIndex = 1;
            int startingItemIndex = 2;
            long numItemsToMoveForward = 6;

            // Reasoning:
            // Starting from position (1, 2) with 6 items to move forward:
            // - We are at item 2 in bucket 1, which has a total of 4 items.
            //   - There are 2 items left in the current bucket, which is less than 6.
            //   - So, we move to the end of bucket 1, leaving us with 4 moves more.
            // - In bucket 2, there are 7 items, which is greater than 4.
            //   - So, we can move our final 4 moves, putting us at (2, 4).

            // Act
            var result = Offseter.MoveForward(numBuckets, numItemsInBucket, startingBucketIndex, startingItemIndex, numItemsToMoveForward);

            // Assert
            result.Should().Be((2, 4));
        }

        [Fact]
        public void MoveForward_Should_Return_Expected_Result_When_Moving_From_Middle_Of_Bucket_With_Enough_Items()
        {
            // Arrange
            int numBuckets = 3;
            Func<int, int> numItemsInBucket = x => new int[] { 5, 4, 7 }[x];
            int startingBucketIndex = 1;
            int startingItemIndex = 2;
            long numItemsToMoveForward = 1;

            // Reasoning:
            // Starting from position (1, 2) with 1 item to move forward:
            // - We are at item 2 in bucket 1, which has a total of 4 items.
            //   - There are 2 items left in the current bucket, which is greater than 1.
            //   - So, we can move our final 1 move, putting us at (1, 3).

            // Act
            var result = Offseter.MoveForward(numBuckets, numItemsInBucket, startingBucketIndex, startingItemIndex, numItemsToMoveForward);

            // Assert
            result.Should().Be((1, 3));
        }

        [Fact]
        public void MoveForward_Should_Return_Expected_Result_When_Moving_Through_Empty_Buckets()
        {
            // Arrange
            int numBuckets = 5;
            Func<int, int> numItemsInBucket = x => new int[] { 3, 0, 0, 2, 6 }[x];
            int startingBucketIndex = 0;
            int startingItemIndex = 1;
            long numItemsToMoveForward = 4;

            // Reasoning:
            // Starting at position (0, 1) with 4 items to move forward:
            // - We are at item 1 in bucket 0, which has a total of 3 items.
            //   - There are 2 items left in the current bucket, which is less than 4.
            //   - So, we move to the end of bucket 0, leaving us with 2 moves more.
            // - Buckets 1 and 2 are empty, so we skip to the beginning of bucket 3 with 2 moves more.
            // - In bucket 3, there are 2 items, which is equal to 2.
            //   - So, we can move our final 2 moves, and we end up at the beginning of bucket 4.

            // Act
            var result = Offseter.MoveForward(numBuckets, numItemsInBucket, startingBucketIndex, startingItemIndex, numItemsToMoveForward);

            // Assert
            result.Should().Be((4, 0));
        }

        [Fact]
        public void MoveForward_Should_Return_Expected_Result_When_Moving_From_Last_Item_Of_Bucket()
        {
            // Arrange
            int numBuckets = 4;
            Func<int, int> numItemsInBucket = x => new int[] { 3, 5, 2, 6 }[x];
            int startingBucketIndex = 2;
            int startingItemIndex = 1;
            long numItemsToMoveForward = 1;

            // Reasoning:
            // Starting from position (2, 1) with 1 item to move forward:
            // - We are at the last item in bucket 2.
            //   - So, we move our final 1 move, putting us at the beginning of bucket 3.

            // Act
            var result = Offseter.MoveForward(numBuckets, numItemsInBucket, startingBucketIndex, startingItemIndex, numItemsToMoveForward);

            // Assert
            result.Should().Be((3, 0));
        }

        [Fact]
        public void MoveForward_Should_Return_Expected_Result_When_Moving_Within_Same_Bucket()
        {
            // Arrange
            int numBuckets = 3;
            Func<int, int> numItemsInBucket = x => new int[] { 5, 7, 3 }[x];
            int startingBucketIndex = 0;
            int startingItemIndex = 2;
            long numItemsToMoveForward = 2;

            // Reasoning:
            // Starting from position (0, 2) with 2 items to move forward:
            // - We are at item 2 in bucket 0, which has a total of 5 items.
            //   - There are 3 items left in the current bucket, which is greater than 2.
            //   - So, we can move our final 2 moves, putting us at (0, 4).

            // Act
            var result = Offseter.MoveForward(numBuckets, numItemsInBucket, startingBucketIndex, startingItemIndex, numItemsToMoveForward);

            // Assert
            result.Should().Be((0, 4));
        }

        [Fact]
        public void MoveForward_Should_Return_Expected_Result_When_Moving_With_Single_Item_Buckets()
        {
            // Arrange
            int numBuckets = 5;
            Func<int, int> numItemsInBucket = x => new int[] { 1, 1, 1, 1, 1 }[x];
            int startingBucketIndex = 0;
            int startingItemIndex = 0;
            long numItemsToMoveForward = 4;

            // Reasoning:
            // Starting from position (0, 0) with 4 items to move forward:
            // - Each bucket has only one item.
            // - So, moving 4 items forward will take us to the beginning of bucket 4.

            // Act
            var result = Offseter.MoveForward(numBuckets, numItemsInBucket, startingBucketIndex, startingItemIndex, numItemsToMoveForward);

            // Assert
            result.Should().Be((4, 0));
        }

        [Fact]
        public void MoveForward_Should_Throw_Exception_When_There_Are_Insufficient_Items()
        {
            // Arrange
            int numBuckets = 3;
            Func<int, int> numItemsInBucket = x => new int[] { 2, 3, 1 }[x];
            int startingBucketIndex = 1;
            int startingItemIndex = 2;
            long numItemsToMoveForward = 5;

            // Reasoning:
            // Starting from position (1, 2) with 5 items to move forward:
            // - We are at the last item in bucket 1, and there is only one item in bucket 2.
            // - So, trying to move 5 items forward should throw an exception due to insufficient items.

            // Act
            Action act = () => Offseter.MoveForward(numBuckets, numItemsInBucket, startingBucketIndex, startingItemIndex, numItemsToMoveForward);

            // Assert
            act.Should().Throw<Exception>().WithMessage("Internal error.");
        }


    }



}
