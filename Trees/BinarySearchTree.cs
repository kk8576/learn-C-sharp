using System;
using System.Collections.Generic;

namespace Trees
{
    public class BinarySearchTree<T> where T : IComparable<T>
    {
        private int _count;
        private BinaryTreeNode<T> _root;

        public BinarySearchTree()
        {
            Root = null;
            _count = 0;
        }

        public BinarySearchTree(BinaryTreeNode<T> root)
        {
            _root = root;
            _count = 0;
        }

        public BinaryTreeNode<T> Root
        {
            get {return _root;}
            internal set {_root = value;}
        }

        public int Count
        {
            get
            {   
                return _count;
            }
        }

        private void _replaceNodeInParent(BinaryTreeNode<T> node, BinaryTreeNode<T> newNode = null)
        {
            if (node.Parent != null)
            {
                if (node.IsLeftChild)
                    node.Parent.LeftChild = newNode;
                else
                    node.Parent.RightChild = newNode;
            }
            else 
            {
                Root = newNode;
            }

            if (newNode != null)
            {
                newNode.Parent = node.Parent;
            }
        }

        private BinaryTreeNode<T> _findNextLargerNode(BinaryTreeNode<T> node)
        {
            if (node == null)
                return null;

            if (node.HasRightChild)
            {
                BinaryTreeNode<T> currentNode = node.RightChild;
                while (currentNode.HasLeftChild)
                {
                    currentNode = currentNode.LeftChild;
                }

                return currentNode;
            }
            else
            {
                if (node.IsLeftChild)
                {
                    return node.Parent;
                }
                else
                {
                    if (node.Parent != null)
                        return node.Parent.Parent;
                }
            }

            return null;
        }

        private BinaryTreeNode<T> _findNextSmallerNode(BinaryTreeNode<T> node)
        {
            if (node == null)
                return null;

            if (node.HasLeftChild)
            {
                BinaryTreeNode<T> currentNode = node.LeftChild;
                while (currentNode.HasRightChild)
                {
                    currentNode = currentNode.RightChild;
                }

                return currentNode;
            }
            else
            {
                if (node.IsRightChild)
                {
                    return node.Parent;
                }
            }

            return null;
        }

        private bool _insertNode(BinaryTreeNode<T> currentNode, BinaryTreeNode<T> newNode)
        {
            if (currentNode == null)
            {
                return false;
            }

            if (currentNode.HasLeftChild && currentNode > newNode)
            {
                _insertNode(currentNode.LeftChild, newNode);
            }
            else if (!currentNode.HasLeftChild && currentNode > newNode)
            {
                currentNode.LeftChild = newNode;
                newNode.Parent = currentNode;
                _count++;
            }
            else if (currentNode.HasRightChild && currentNode < newNode)
            {
                _insertNode(currentNode.RightChild, newNode);
            }
            else if (!currentNode.HasRightChild && currentNode < newNode)
            {
                currentNode.RightChild = newNode;
                newNode.Parent = currentNode;
                _count++;
            }
            else
            {
                return false;
            }

            return true;
        }

        private bool _removeNode(BinaryTreeNode<T> node)
        {
            if (node == null)
                return false;

            if (node.ChildrenCount == 2)
            {
                var largerNode = _findNextLargerNode(node);
                node.Value = largerNode.Value;
                return (true && _removeNode(largerNode));
            }

            if (node.HasLeftChild)
            {
                _replaceNodeInParent(node, node.LeftChild);
                _count--;
            }
            else if (node.HasRightChild)
            {
                _replaceNodeInParent(node, node.RightChild);
                _count--;
            }
            else
            {
                _replaceNodeInParent(node, null);
                _count--;
            }

            return true;
        }

        private BinaryTreeNode<T> _findNode(BinaryTreeNode<T> currentNode, T itemData)
        {
            if (currentNode == null)
                return null;

            if (currentNode.Value.CompareTo(itemData) == 0)
            {
                return currentNode;
            }
            else if (currentNode.Value.CompareTo(itemData) > 0)
            {
                if (currentNode.HasLeftChild)
                    return _findNode(currentNode.LeftChild, itemData);
                else
                    return null;
            }
            else if (currentNode.Value.CompareTo(itemData) < 0)
            {
                if (currentNode.HasRightChild)
                    return _findNode(currentNode.RightChild, itemData);
                else
                    return null;
            }

            return null;
        }

        private int _treeHeight(BinaryTreeNode<T> currentNode)
        {
            if (currentNode == null)
                return 0;

            if (currentNode.ChildrenCount == 0)
                return 1;

            if (currentNode.ChildrenCount == 2)
                return (1 + Math.Max(_treeHeight(currentNode.LeftChild), 
                                     _treeHeight(currentNode.RightChild)));

            if (currentNode.HasLeftChild)
                return (1 + _treeHeight(currentNode.LeftChild));
            
            if (currentNode.HasRightChild)
                return (1 + _treeHeight(currentNode.RightChild));

            return 0;
        }

        private BinaryTreeNode<T> _findNodeMin(BinaryTreeNode<T> currentNode)
        {
            if (currentNode == null)
                return null;

            if (currentNode.HasLeftChild)
            {
                return _findNodeMin(currentNode.LeftChild);
            }
            else
            {
                return currentNode;   
            }
        }

        private BinaryTreeNode<T> _findNodeMax(BinaryTreeNode<T> currentNode)
        {
            if (currentNode == null)
                return null;

            if (currentNode.HasRightChild)
            {
                return _findNodeMax(currentNode.RightChild);
            }
            else
            {
                return currentNode;   
            }
        }

        /// <summary>
        /// A recursive private method. Used in the public FindAll(predicate) functions.
        /// Implements in-order traversal to find all the matching elements in a subtree.
        /// </summary>
        /// <param name="currentNode">Node to start searching from.</param>
        /// <param name="match"></param>
        private void _findAll(BinaryTreeNode<T> currentNode, Predicate<T> match, ref List<T> list)
        {
            if (currentNode == null)
                return;

            // call the left child
            _findAll(currentNode.LeftChild, match, ref list);

            if (match(currentNode.Value)) // match
            {
                list.Add(currentNode.Value);
            }

            // call the right child
            _findAll(currentNode.RightChild, match, ref list);
        }

        /// <summary>
        /// In-order traversal of the subtrees of a node. Returns every node it vists.
        /// </summary>
        /// <param name="currentNode">Node to traverse the tree from.</param>
        /// <param name="list">List to add elements to.</param>
        private void _inOrderTraverse(BinaryTreeNode<T> currentNode, ref List<T> list)
        {
            if (currentNode == null)
                return;

            // call the left child
            _inOrderTraverse(currentNode.LeftChild, ref list);

            // visit node
            list.Add(currentNode.Value);

            // call the right child
            _inOrderTraverse(currentNode.RightChild, ref list);
        }

        /// <summary>
        /// Checks if tree is empty.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty
        {
            get { return (_count == 0); }
        }

        /// <summary>
        /// Returns the height of the tree.
        /// Time-complexity: O(n), where n = number of nodes.
        /// </summary>
        /// <returns>Hight</returns>
        public int Height
        {
            get
            {
                if (IsEmpty)
                    return 0;

                var currentNode = Root;
                return _treeHeight(currentNode);
            }
        }

        /// <summary>
        /// Clears all elements from tree.
        /// </summary>
        public void Clear()
        {
            Root = null;
            _count = 0;
        }

        public bool Insert(T dataItem)
        {
            BinaryTreeNode<T> newNode = new BinaryTreeNode<T>(dataItem);

            if (Root == null)
            {
                Root = newNode;
                _count++;

                return true;
            }

            return _insertNode(Root, newNode);
        }

        public bool Insert(T[] dataItems)
        {
            bool status = true;

            for (int i = 0; i != dataItems.Length; ++i)
            {
                bool status0 = Insert(dataItems[i]);
                if (!status0)
                    status = false;
            }

            return status;
        }

        public bool Insert(List<T> dataItems)
        {
            bool status = true;

            for (int i = 0; i != dataItems.Count; ++i)
            {
                bool status0 = Insert(dataItems[i]);
                if (!status0)
                    status = false;
            }

            return status;
        }

        public bool Remove(T dataItem)
        {
            BinaryTreeNode<T> deleteNode = _findNode(Root, dataItem);
            return _removeNode(deleteNode);
        }

        public bool Remove(T[] dataItems)
        {
            bool status = true;
            
            for (int i = 0; i != dataItems.Length; ++i)
            {
                bool status0 = Remove(dataItems[i]);
                if (!status0)
                    status = false;
            }

            return status;
        }

        public bool Remove(List<T> dataItems)
        {
            bool status = true;

            for (int i = 0; i != dataItems.Count; ++i)
            {
                bool status0 = Remove(dataItems[i]);
                if (!status0)
                    status = false;
            }

            return status;
        }

        /// <summary>
        /// Returns a list of the nodes' value.
        /// </summary>
        public List<T> ToList()
        {
            var list = new List<T>();
            _inOrderTraverse(Root, ref list);

            return list;
        }
        public T[] ToArray()
        {
            return ToList().ToArray();
        }

        /// <summary>
        /// Finds the minimum in tree 
        /// </summary>
        /// <returns>Min</returns>
        public T FindMin()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty.");

            return _findNodeMin(Root).Value;
        }

        /// <summary>
        /// Finds the maximum in tree 
        /// </summary>
        /// <returns>Max</returns>
        public T FindMax()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty.");

            return _findNodeMax(Root).Value;
        }

        /// <summary>
        /// Checks for the existence of an item
        /// </summary>
        public bool Contains(T item)
        {
            return (_findNode(Root, item) != null);
        }

        public BinaryTreeNode<T> FindNode(T item)
        {
            return _findNode(Root, item);
        }

        /// <summary>
        /// Removes the min value from tree.
        /// </summary>
        public void RemoveMin()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty.");

            var node = _findNodeMin(Root);
            _removeNode(node);
        }

        /// <summary>
        /// Removes the max value from tree.
        /// </summary>
        public void RemoveMax()
        {
            if (IsEmpty)
                throw new Exception("Tree is empty.");

            var node = _findNodeMax(Root);
            _removeNode(node);
        }
    }
}