using System;
using System.Collections.Generic;

namespace CS.Lists
{
    /// <summary>
    /// The single-linked list node class
    /// </summary>
    /// <typeparam name="T"> The type T should be IComparable. </typeparam>
    public class SLinkedListNode<T> : IComparable<SLinkedListNode<T>> where T : IComparable<T>
    {
        private T _data;
        private SLinkedListNode<T> _next;

        /// <summary>
        /// The data is the default value of type T, and it is linked with null.
        /// </summary>
        public SLinkedListNode()
        {
            Data = default(T);
            Next = null;
        }

        /// <summary>
        /// The data is equal to dataItem.
        /// </summary>
        /// <param name="dataItem"></param>
        public SLinkedListNode(T dataItem)
        {
            Data = dataItem;
            Next = null;
        }

        public T Data
        {
            get {return _data;}
            set {_data = value;}
        }

        public SLinkedListNode<T> Next
        {
            get {return _next;}
            set {_next = value;}
        }

        public int CompareTo(SLinkedListNode<T> other)
        {
            if (other == null)
                return -1;

            return this.Data.CompareTo(other.Data);
        }
    }


    public class SLinkedList<T> : IEnumerable<T> where T : IComparable<T>
    {
        private int _count;
        private SLinkedListNode<T> _firstNode { get; set;}
        private SLinkedListNode<T> _lastNode { get; set;}

        public int Count
        {
            get {return _count;}
        }

        public SLinkedList()
        {
            _firstNode = null;
            _lastNode = null;
            _count = 0;
        }

        public bool IsEmpty()
        {
            return (Count == 0);
        }

        public T First
        {
            get 
            {
                if (_firstNode == null)
                    return default(T);
                else
                    return _firstNode.Data;
            }
        }

        public T Last
        {
            get 
            {
                if (_lastNode == null)
                    return default(T);
                else
                    return _lastNode.Data;
            }
        }

        public SLinkedListNode<T> Head
        {
            get { return this._firstNode; }
        }

        public void Prepend(T dataItem)
        {
            var newNode = new SLinkedListNode<T>(dataItem);

            if (_firstNode == null)
            {
                _firstNode = newNode;
                _lastNode = newNode;
            }
            else
            {
                newNode.Next = _firstNode;
                _firstNode = newNode;
            }

            _count++;
        }

        public void Append(T dataItem)
        {
            var newNode = new SLinkedListNode<T>(dataItem);

            if (_lastNode == null)
            {
                _firstNode = newNode;
                _lastNode = newNode;
            }
            else
            {
                _lastNode.Next = newNode;
                _lastNode = newNode;
            }

            _count++;
        }

        public void InsertAt(T dataItem, int index)
        {
            if (index == 0)
            {
                Prepend(dataItem);
            }
            else if (index == Count)
            {
                Append(dataItem);
            }
            else if (index > 0 && index < Count)
            {
                var currentNode = _firstNode;
                var newNode = new SLinkedListNode<T>(dataItem);

                for (int i = 1; i != index; ++i)
                {
                    currentNode = currentNode.Next;
                }

                newNode.Next = currentNode.Next;
                currentNode.Next = newNode;

                _count++;
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException();
            }

            if (index == 0)
            {
                _firstNode = _firstNode.Next;
            }
            else
            {
                var currentNode = _firstNode;
                int currentIndex = 0;
                while (currentNode != null)
                {
                    if (currentIndex == index - 1)
                        break;

                    currentNode = currentNode.Next;
                    currentIndex++;
                }

                currentNode.Next = currentNode.Next.Next;
            }
            _count--;
        }

        public void clear()
        {
            _count = 0;
            _firstNode = null;
            _lastNode = null;
        }

        public T GetAt(int index)
        {
            if (index == 0)
                return First;
            else if (index == Count - 1)
                return Last;
            else if (index > 0 && index < Count - 1)
            {
                var currentNode = _firstNode;
                int currentIndex = 0;
                while (currentNode != null)
                {
                    if (currentIndex == index)
                        break;
                    
                    currentNode = currentNode.Next;
                    currentIndex++;
                }

                return currentNode.Data;
            }

            throw new IndexOutOfRangeException();
        }

        public T[] ToArray()
        {
            T[] newArray = new T[Count];
            var currentNode = _firstNode;
            int currentIndex = 0;
            while (currentNode != null)
            {
                newArray[currentIndex] = currentNode.Data;

                currentIndex++;
                currentNode = currentNode.Next;
            }

            return newArray;
        }

        public List<T> ToList()
        {
            List<T> list = new List<T>();
            var currentNode = _firstNode;
            int currentIndex = 0;
            while (currentNode != null)
            {
                list.Add(currentNode.Data);

                currentIndex++;
                currentNode = currentNode.Next;
            }

            return list;
        }

        /// <summary>
        /// Returns the list items as a readable multi--line string.
        /// </summary>
        /// <returns></returns>
        public string ToReadable()
        {
            int i = 0;
            var currentNode = _firstNode;
            string listAsString = string.Empty;

            while (currentNode != null)
            {
                listAsString = String.Format("{0}[{1}] => {2}\r\n", listAsString, i, currentNode.Data);
                currentNode = currentNode.Next;
                ++i;
            }

            return listAsString;
        }

        /********************************************************************************/

        public IEnumerator<T> GetEnumerator()
        {
            return new SLinkedListEnumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new SLinkedListEnumerator(this);
        }

        /********************************************************************************/

        internal class SLinkedListEnumerator : IEnumerator<T>
        {
            private SLinkedListNode<T> _current;
            private SLinkedList<T> _doublyLinkedList;

            public SLinkedListEnumerator(SLinkedList<T> list)
            {
                _doublyLinkedList = list;
                _current = list.Head;
            }

            public T Current
            {
                get { return _current.Data; }
            }

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                _current = _current.Next;

                return (_current != null);
            }

            public void Reset()
            {
                _current = _doublyLinkedList.Head;
            }

            public void Dispose()
            {
                _current = null;
                _doublyLinkedList = null;
            }
        }
    }
}
