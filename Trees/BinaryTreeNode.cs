using System;
using System.Collections.Generic;

namespace Trees
{
    public class BinaryTreeNode<T> : IComparable<BinaryTreeNode<T>> where T : IComparable<T>
    {
        private T _data;
        private BinaryTreeNode<T> _parent;
        private BinaryTreeNode<T> _leftChild;
        private BinaryTreeNode<T> _rightChild;

        public BinaryTreeNode()
        {
            _data = default(T);
            _leftChild = null;
            _rightChild = null;
        }

        public BinaryTreeNode(T dataItem)
        {
            _data = dataItem;
            _leftChild = null;
            _rightChild = null;
        }

        public T Value
        {
            get {return _data;}
            set {_data = value;}
        }

        public virtual BinaryTreeNode<T> Parent
        {
            get {return _parent;}
            set {_parent = value;}
        }

        public virtual BinaryTreeNode<T> LeftChild
        {
            get {return _leftChild;}
            set {_leftChild = value;}
        }

        public virtual BinaryTreeNode<T> RightChild
        {
            get {return _rightChild;}
            set {_rightChild = value;}
        }

        public bool HasLeftChild
        {
            get {return (_leftChild != null);}
        }
        public bool HasRightChild
        {
            get {return (_rightChild != null);}
        }
        public bool HasParent
        {
            get {return (_parent != null);}
        }

        public bool IsLeafNode
        {
            get {return (_leftChild == null && _rightChild == null);}
        }

        public int ChildrenCount
        {
            get 
            {
                int count = 0;
                if (_leftChild != null)
                    count++;
                if (_rightChild != null)
                    count++;

                return count;
            }
        }

        public bool IsLeftChild
        {
            get 
            {
                if (Parent == null)
                    return false;
                
                return (this == Parent.LeftChild);
            }
        }

        public bool IsRightChild
        {
            get 
            {
                if (Parent == null)
                    return false;
                
                return (this == Parent.RightChild);
            }
        }

        public int CompareTo(BinaryTreeNode<T> other)
        {
            if (other == null)
                return 1;

            return Value.CompareTo(other.Value);
        }

        // Define the is greater than operator.
        public static bool operator >  (BinaryTreeNode<T> operand1, BinaryTreeNode<T> operand2)
        {
            return operand1.CompareTo(operand2) == 1;
        }
    
        // Define the is less than operator.
        public static bool operator <  (BinaryTreeNode<T> operand1, BinaryTreeNode<T> operand2)
        {
            return operand1.CompareTo(operand2) == -1;
        }

        // Define the is greater than or equal to operator.
        public static bool operator >=  (BinaryTreeNode<T> operand1, BinaryTreeNode<T> operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }
    
        // Define the is less than or equal to operator.
        public static bool operator <=  (BinaryTreeNode<T> operand1, BinaryTreeNode<T> operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }


    }
}
