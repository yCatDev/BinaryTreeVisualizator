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

        //public MyListNode<T> First() => Previous == null ? this : Previous.First();
        //public MyListNode<T> Last() => Next == null ? this : Next.Last();

        internal MyListNode(T item, MyListNode<T> prev, MyListNode<T> next)
        { 
            Value = item;
            Previous = prev;
            Next = next;
        }
        public void SelfRemove()
        {
            if (Previous != null && Next != null)
            {
                var prev = Previous;
                Previous.Next = Next;
                Next.Previous = prev;
            }
            else if(Previous!=null && Next==null)
            {
                Previous.Next = null;
            }else if (Previous == null && Next != null)
            {
                Next.Previous = null;
            }
        }
        
    }

    public class MyList<T> : IEnumerable<T>
    {
        private MyListNode<T> _data;
        private MyListNode<T> _head, _tail;
        public T Value => _data.Value;

        public MyList()
        {
        }

        public void AddToFront(T item)
        {
            if (CheckSelf(item)) return;

            if (_data.Previous == null)
            {
                _head = _data.Previous = new MyListNode<T>(item, null, _data);
            }
            else AddToFront(item, _data.Previous);
        }

        private void AddToFront(T item, MyListNode<T> node)
        {
            if (node.Previous == null)
            {
                _head = node.Previous = new MyListNode<T>(item, null, node);
                
            }
            else AddToFront(item, node.Previous);
        }
        
        public void AddToEnd(T item)
        {
            if (CheckSelf(item)) return;

            if (_data.Next == null)
            {
                _tail = _data.Next = new MyListNode<T>(item, _data, null);
            }
            else AddToEnd(item, _data.Next);
        }

        private void AddToEnd(T item, MyListNode<T> node)
        {
            if (node.Next == null)
            {
                _tail = node.Next = new MyListNode<T>(item, node, null);
            }
            else AddToEnd(item, node.Next);
        }

        private bool CheckSelf(T val)
        {
            if (_data != null) return false;

            _head = _tail = _data = new MyListNode<T>(val, null, null);
            return true;
        }


        public int? IndexOf(T item)
        {
            var current = _head;
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
            var t = _head.Value;
            _head.Value = _tail.Value;
            _tail.Value = t;
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
            return RemoveInternal(0, index, _head);
        }

        private bool RemoveInternal(int c, int index, MyListNode<T> node)
        {
            if (c>index) return false;
            if (c!=index) return RemoveInternal(c+1, index, node.Next);

            if (node.Next==null&&node.Previous==null)
            {
                _head = _tail = _data = null;
            }
            else
            {
                if (node.Next != null)
                {
                    if (node.Previous != null)
                    {
                        var prev = node.Previous;
                        prev.Next = node.Next;
                        node.Next.Previous = prev;
                    }
                    else
                    {
                        _head = node.Next;
                        node.Next.Previous = null;
                        
                    }
                }
                else
                {
                    if (node.Next != null)
                    {
                        var next = node.Next;
                        next.Previous = node.Previous;
                        node.Previous.Next = next;
                    }
                    else
                    {
                        _tail = node.Previous;
                        node.Previous.Next = null;
                    }
                }
            }


            return true;
        }
        
        /*public bool RemoveAt(int index)
        {
            var current = _data.First();
            var c = 0;
            while (current != null)
            {
                if (c == index)
                {
                    current = current.Next;
                }

                current = current.Next;
                c++;
            }

            return false;
        }*/

        public bool IsEmpty => Count==0;

        public int Count => this.Count();

       /* private void SelfRemove()
        {
            if (_data.Previous != null && _data.Next != null)
            {
                var prev = _data.Previous;
                _data.Previous.Next = _data.Next;
                _data.Next.Previous = prev;
            }

            if (_data.Previous == null && _data.Next != null)
            {
                _data = _data.Next;
            }
            if (_data.Previous != null && _data.Next == null)
            {
                _data = _data.Next;
            }
        }*/

        public IEnumerator<T> GetEnumerator()
        {
            if (_head == null) yield break;
            var current = _head;
            Console.WriteLine($"Head is {_head.Value}");
            while (current != null)
            {
                Console.Write($"{current.Value} ");
                yield return current.Value;
                current = current.Next;
            }
            Console.WriteLine($"Tail is {_tail.Value}");
            Console.WriteLine();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}