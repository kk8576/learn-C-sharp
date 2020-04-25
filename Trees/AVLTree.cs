using System;

namespace Trees
{
    public class AVLTree<T> : BinarySearchTree<T> where T : IComparable<T>
    {
        private int _imbalance;
        public AVLTree()
        {
            _imbalance = 0;
        }

        public int Imbalance
        {
            get {return _imbalance;}
        }

        private int _getNodeHeight(AVLTreeNode<T> currentNode)
        {
            if (currentNode == null)
                return -1;

            return currentNode.Height;
        }

        private void _updateNodeHeight(AVLTreeNode<T> currentNode)
        {
            if (currentNode == null)
                return;

            currentNode.Height = 1 + Math.Max(_getNodeHeight(currentNode.LeftChild), 
                                              _getNodeHeight(currentNode.RightChild));
        }

        private void _updateHeightRecursive(AVLTreeNode<T> node)
        {
            if (node == null)
                return;

            // height = 1 + the max between left and right children.
            node.Height = 1 + Math.Max(_getNodeHeight(node.LeftChild), _getNodeHeight(node.RightChild));

            _updateHeightRecursive(node.Parent);
        }

        /// <summary>
        /// Get the imbalance factor: height of right subtree - height of left subtree
        /// </summary>
        /// <param name="node">node is the root of this subtree.</param>
        /// <returns></returns>
        private int _getImbalance(AVLTreeNode<T> node)
        {
            if (node == null)
                return -1;

            return (_getNodeHeight(node.RightChild) - _getNodeHeight(node.LeftChild));
        }

        //        p                                 p            p
        //        |               p                 |            |
        //       *1               |                 *1    =>     *2
        //      /                 *2               /              \ 
        //     *2      ==>       / \              *2               *1
        //    / \               *3  *1              \             /
        //   *3  *4                /                 *4          *4
        //                        *4
        private void _rotateToRight(AVLTreeNode<T> node1)
        {
            AVLTreeNode<T> node2 = node1.LeftChild;
            //AVLTreeNode<T> node3 = node2.LeftChild;
            AVLTreeNode<T> p = node1.Parent;
            AVLTreeNode<T> node4 = node2.RightChild;

            if (p != null)
            {
                if (node1.IsLeftChild)
                {
                    p.LeftChild = node2;
                }
                else
                {
                    p.RightChild = node2;
                }
                node2.Parent = p;
                node2.RightChild = node1;

                node1.Parent = node2;
                node1.LeftChild = node4;
                if (node4 != null)
                    node4.Parent = node1;
            }
            else
            {
                Root = node2;
                node2.Parent = null;
                node2.RightChild = node1;

                node1.Parent = node2;
                node1.LeftChild = node4;
                if (node4 != null)
                    node4.Parent = node1;
            }

            _updateHeightRecursive(node1);
        }

        //         p
        //        / \                   p
        //           *1                / \
        //            \                   *2
        //             *2    ==>         / \ 
        //            / \               *1  *3
        //           *4  *3              \
        //                                *4
        private void _rotateToLeft(AVLTreeNode<T> node1)
        {
            AVLTreeNode<T> node2 = node1.RightChild;
            //AVLTreeNode<T> node3 = node2.RightChild;
            AVLTreeNode<T> p = node1.Parent;
            AVLTreeNode<T> node4 = node2.LeftChild;

            if (p != null)
            {
                if (node1.IsLeftChild)
                {
                    p.LeftChild = node2;
                }
                else
                {
                    p.RightChild = node2;
                }
                node2.Parent = p;
                node2.LeftChild = node1;

                node1.Parent = node2;
                node1.RightChild = node4;
                if (node4 != null)
                    node4.Parent = node1;
            }
            else
            {
                Root = node2;
                node2.Parent = null;
                node2.LeftChild = node1;

                node1.Parent = node2;
                node1.RightChild = node4;
                if (node4 != null)
                    node4.Parent = node1;
            }

            _updateHeightRecursive(node1);
        }


        private void _rebalanceTreeAt(AVLTreeNode<T> node)
        {
            if (node == null)
                return;

            AVLTreeNode<T> currentNode = node;

            while (currentNode != null)
            {
                _updateNodeHeight(currentNode);

                int imb = _getImbalance(currentNode);

                AVLTreeNode<T> left = currentNode.LeftChild;
                AVLTreeNode<T> right = currentNode.RightChild;
                
                if (imb >= 2)
                {
                    if (currentNode.HasRightChild 
                        && _getNodeHeight(right.RightChild) >= _getNodeHeight(right.LeftChild))
                    {
                        _rotateToLeft(currentNode);
                    }
                    else
                    {
                        _rotateToRight(currentNode.RightChild);
                        _rotateToLeft(currentNode);
                    }
                }
                else if (imb <= -2)
                {
                    if (currentNode.HasLeftChild
                        && _getNodeHeight(left.LeftChild) >= _getNodeHeight(left.RightChild))
                    {
                        _rotateToRight(currentNode);
                    }
                    else
                    {
                        _rotateToLeft(currentNode.LeftChild);
                        _rotateToRight(currentNode);
                    }
                }

                currentNode = currentNode.Parent;
            }
        }

        public override bool Insert(T dataItem)
        {
            AVLTreeNode<T> newNode = new AVLTreeNode<T>(dataItem);

            if (Root == null)
            {
                Root = newNode;
                return true;
            }

            var success = base._insertNode(Root, newNode);

            if (!success)
                return false;

            // Rebalance the tree
            _rebalanceTreeAt(newNode);

            return true;
        }

        public override bool Insert(T[] dataItems)
        {
            bool status = true;

            for (int i = 0; i < dataItems.Length; ++i)
            {
                if (!Insert(dataItems[i]))
                    status = false;
            }

            return status;
        }

        public override bool Insert(System.Collections.Generic.List<T> dataItems)
        {
            bool status = true;

            for (int i = 0; i < dataItems.Count; ++i)
            {
                if (!Insert(dataItems[i]))
                    status = false;
            }

            return status;
        }

        public override bool Remove(T dataItem)
        {
            var node = (AVLTreeNode<T>)base._findNode(Root, dataItem);
            var p = node.Parent;
            bool isLeft = node.IsLeftChild;

            if (!_removeNode(node))
            {
                return false;
            }

            if (isLeft)
                if (p.HasLeftChild)
                    _rebalanceTreeAt(p.LeftChild);
                else
                    _rebalanceTreeAt(p);
            else
                if (p.HasRightChild)
                    _rebalanceTreeAt(p.RightChild);
                else
                    _rebalanceTreeAt(p);

            return true;
        }

        public override bool Remove(T[] dataItems)
        {
            bool status = true;

            for (int i = 0; i < dataItems.Length; ++i)
            {
                if (!Remove(dataItems[i]))
                    status = false;
            }

            return status;
        }

        public override bool Remove(System.Collections.Generic.List<T> dataItems)
        {
            bool status = true;

            for (int i = 0; i < dataItems.Count; ++i)
            {
                if (!Remove(dataItems[i]))
                    status = false;
            }

            return status;
        }

        /// <summary>
        /// Removes the min value from tree.
        /// </summary>
        public override void RemoveMin()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty.");

            T minValue = base.FindMin();
            Remove(minValue);
        }

        /// <summary>
        /// Removes the max value from tree.
        /// </summary>
        public override void RemoveMax()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty.");

            T maxValue = base.FindMax();
            Remove(maxValue);
        }

        public new AVLTreeNode<T> FindNode(T dataItem)
        {
            return (AVLTreeNode<T>)base.FindNode(dataItem);
        }
    }
}