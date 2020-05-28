using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TinyAlgorithmVisualizer.Algorithms.DataStructures
{
    internal class MyListNode<T>
    {
        internal T Value;
        internal MyListNode<T> Previous;
        internal MyListNode<T> Next;
        internal bool Empty = true;

        public MyListNode<T> First() => Previous == null ? this : Previous.First();

        internal MyListNode<T> Last() => Next == null ? this : Next.Last();

        internal MyListNode(T item, MyListNode<T> prev, MyListNode<T> next)
        {
            Value = item;
            Previous = prev;
            Next = next;
            Empty = false;
        }

        public void SelfRemove()
        {
            var prev = Previous;
            if (Previous != null)
                Previous.Next = Next;
            if (Next != null)
                Next.Previous = prev;
        }
    }

    public class MyList<T> : IEnumerable<T>
    {
        private MyListNode<T> _data;
        public T Value => _data.Value;

        public MyList()
        {
        }

        public void AddToFront(T item)
        {
            if (CheckSelf(item)) return;

            if (_data.Previous == null)
            {
                _data.Previous = new MyListNode<T>(item, null, _data);
            }
            else AddToFront(item, _data.Previous);
        }

        private void AddToFront(T item, MyListNode<T> node)
        {
            if (node.Previous == null)
            {
                node.Previous = new MyListNode<T>(item, null, node);
            }
            else AddToFront(item, node.Previous);
        }


        public void AddToEnd(T item)
        {
            if (CheckSelf(item)) return;

            if (_data.Next == null)
            {
                _data.Next = new MyListNode<T>(item, _data, null);
            }
            else AddToEnd(item, _data.Next);
        }

        private void AddToEnd(T item, MyListNode<T> node)
        {
            if (node.Next == null)
            {
                node.Next = new MyListNode<T>(item, node, null);
            }
            else AddToEnd(item, node.Next);
        }

        private bool CheckSelf(T val)
        {
            if (_data != null) return false;

            _data = new MyListNode<T>(val, null, null);
            return true;
        }


        public int? IndexOf(T item)
        {
            var current = _data.First();
            var ind = 0;

            while (current != null)
            {
                if (!Equals(current.Value, item))
                {
                    ind++;
                    current = current.Next;
                }
                else return ind;
            }

            return null;
        }


        public void SwapCorners()
        {
            var t = _data.First().Value;
            _data.First().Value = _data.Last().Value;
            _data.Last().Value = t;
        }

        public void Remove(T item)
        {
            var side = 2;
            var current = _data;

            while (side > 0)
            {
                if (side == 2)
                {
                    if (current.Previous == null)
                    {
                        side--;
                        continue;
                    }

                    current = current.Previous;
                }

                if (side == 1)
                {
                    if (current.Next == null)
                    {
                        side--;
                        continue;
                    }

                    current = current.Next;
                }

                if (!Equals(current.Value, item)) continue;

                current.SelfRemove();
                return;
            }
        }

        public bool RemoveAt(int index)
        {
            var current = _data.First();
            var c = 0;
            while (current != null)
            {
                if (c == index)
                {
                    current.SelfRemove();
                    return true;
                }

                current = current.Next;
                c++;
            }

            return false;
        }

        public bool IsEmpty => Count==0;

        public int Count => this.Count();

        private void SelfRemove()
        {
            var prev = _data.Previous;
            if (_data.Previous != null)
                _data.Previous.Next = _data.Next;
            if (_data.Next != null)
                _data.Next.Previous = prev;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_data == null) yield break;
            var current = _data.First();
            while (current != null)
            {
                Console.WriteLine(current.Value);
                yield return current.Value;
                current = current.Next;
            }

            Console.WriteLine();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}