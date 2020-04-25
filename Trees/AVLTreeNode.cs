using System;

namespace Trees
{
    public class AVLTreeNode<T> : BinaryTreeNode<T> where T : IComparable<T>
    {
        private int _height = 0;
        
        public AVLTreeNode()
        {
            Value = default(T);
            Parent = null;
            LeftChild = null;
            RightChild = null;
            Height = 0;
        }

        public AVLTreeNode(T dataItem)
        {
            Value = dataItem;
            Parent = null;
            LeftChild = null;
            RightChild = null;
            Height = 0;
        }

        public new AVLTreeNode<T> Parent
        {
            get { return (AVLTreeNode<T>)base.Parent; }
            set { base.Parent = value; }
        }

        public new AVLTreeNode<T> LeftChild
        {
            get { return (AVLTreeNode<T>)base.LeftChild; }
            set { base.LeftChild = value; }
        }

        public new AVLTreeNode<T> RightChild
        {
            get { return (AVLTreeNode<T>)base.RightChild; }
            set { base.RightChild = value; }
        }

        public int Height
        {
            get {return _height;}
            set {_height = value;}
        }
    }
}