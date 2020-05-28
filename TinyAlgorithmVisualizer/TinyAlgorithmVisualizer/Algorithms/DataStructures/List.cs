﻿using System.Collections;
using System.Collections.Generic;

namespace TinyAlgorithmVisualizer.Algorithms.DataStructures
{
    public class MyList<T>: IEnumerable<T>
    {
        public T Value;

        private MyList<T> _previous;
        private MyList<T> _next;
        
        public MyList(T item)
        {
            Value = item;
            _previous = _next = null;
        }
     
        private MyList(T item, MyList<T> prev, MyList<T> next)
        {
            Value = item;
            _previous = prev;
            _next = next;
        }

        public void AddToFront(T item)
        {
            if (_previous == null)
                _previous = new MyList<T>(item, null, this);
            else _previous.AddToFront(item);
        }
        public void AddToEnd(T item)
        {
            if (_next == null)
                _next = new MyList<T>(item, this, null);
            else _next.AddToEnd(item);
        }
        
        
        public int? IndexOf(T item)
        {
            var current = First();
            var ind = 0;
            
            while (current != null)
            {
                if (!Equals(current.Value, item))
                {
                    ind++;
                    current = current._next;
                }
                else return ind;
            }
            return null;
        }

        public MyList<T> First() => _previous == null ? this : _previous.First();

        public MyList<T> Last() => _next == null ? this : _next.Last();

        public void SwapCorners()
        {
            var t = First().Value;
            First().Value = Last().Value;
            Last().Value = t;
        }

        public void Remove(T item)
        {
            var side = 2;
            var current = this;
            
            if (Equals(current.Value, item))
            {
                current.SelfRemove();   
                return;
            }
            
            while (side > 0)
            {
                if (side == 2)
                {
                    if (current._previous == null)
                    {
                        side--;
                        continue;
                    }

                    current = current._previous;
                }

                if (side == 1)
                {
                    if (current._next == null)
                    {
                        side--;
                        continue;
                    }

                    current = current._next;
                }

                if (!Equals(current.Value, item)) continue;
                
                current.SelfRemove();   
                return;
            }
        }

        internal void SelfRemove()
        {
            var prev = _previous;
            if (_previous != null)
                _previous._next = _next;
            if (_next!=null)
                _next._previous = prev;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var head = First();
            var current = head;
            while (current != null)
            {
                yield return current.Value;
                current = current._next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}