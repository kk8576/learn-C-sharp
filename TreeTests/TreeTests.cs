using System;
using Xunit;
using Trees;
using System.Collections.Generic;

namespace TreeTests
{
    public class BinarySearchTreeTest
    {
        [Fact]
        public void BinarySearchTreeTestInsert()
        {
            int[] dataItems = {
                8, 6, 7, 5, 1, 3, 9, 4, 0, 2
            };

            BinarySearchTree<int> tree = new BinarySearchTree<int>();

            tree.Insert(dataItems);

            int[] inorderItemsArray = tree.ToArray();
            List<int> inorderItemsList = tree.ToList();

            int[] inorderItemsArrayCorrect = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            Assert.Equal(inorderItemsArray, inorderItemsArrayCorrect);
        }
    }
}
