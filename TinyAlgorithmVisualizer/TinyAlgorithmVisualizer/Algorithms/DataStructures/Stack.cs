using System;
using System.Collections;
using System.Collections.Generic;

namespace TinyAlgorithmVisualizer.Algorithms.DataStructures
{
    public class Node<T>
    {
        public T Data { get; set; }
        internal Node<T> Next { get; set; }
        
        public Node(T data)
        {
            Data = data;
        }
       
    }
    public class Stack<T> : IEnumerable<T>
    {
        private Node<T> _head;
        private int _count;

        public bool IsEmpty => _count == 0;

        public int Count => _count;

        public void Push(T item)
        {
            var node = new Node<T>(item);
            node.Next = _head;
            _head = node;
            _count++;
        }
        public T Pop()
        {
            if (IsEmpty) throw new InvalidOperationException("Stack is empty!");
            
            var temp = _head;
            _head = _head.Next; 
            _count--;
            return temp.Data;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            var current = _head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
        
        
    }


}