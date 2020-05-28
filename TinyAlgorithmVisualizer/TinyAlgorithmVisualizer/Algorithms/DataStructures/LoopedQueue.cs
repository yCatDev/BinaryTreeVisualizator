using System;
using System.Collections;
using System.Collections.Generic;

namespace TinyAlgorithmVisualizer.Algorithms.DataStructures
{
    public class LoopedQueue<T>: IEnumerable<T> 
    {
        private readonly T[] _array;
        private readonly int _arraySize;
        
        private int _headIndex;
        private int _tailIndex;
        private int _length;
        

        public LoopedQueue(int size)
        {
            _array = new T[size];
            _arraySize = size;
            
            _headIndex = _length = 0;
            _tailIndex = -1;
        }
        public bool IsEmpty => _length == 0;

        public bool IsFull => _length == _arraySize;

        public T Dequeue()
        {
            if (IsEmpty) throw new InvalidOperationException("Queue is empty!");

            var dequeued = _array[_headIndex];
            _headIndex = GetLoopedIndex(_headIndex + 1);
            _length--;
            //Console.WriteLine($"Dequeue: Tail {_tailIndex} Head {_headIndex}");
            return dequeued;
        }
        
        public void Enqueue(T item)
        {
            if (IsFull) throw new InvalidOperationException("Queue is full!");
            
            
            _tailIndex = GetLoopedIndex(_tailIndex+1);
            //Console.WriteLine($"Enqueue: Tail {_tailIndex} Head {_headIndex}");
            _length++;
            _array[_tailIndex] = item;
        }

        private int GetLoopedIndex(int index)
        {
            var tmp = index-_arraySize;
            return tmp < 0 ? index : tmp;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = _headIndex; i != GetLoopedIndex(_tailIndex+1); i=GetLoopedIndex(i+1))
            {
                //Console.WriteLine($"Tail {_tailIndex} Head {_headIndex} I {i}");
                yield return _array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
           return GetEnumerator();
        }
    }

}