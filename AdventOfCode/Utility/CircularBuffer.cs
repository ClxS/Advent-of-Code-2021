﻿// https://github.com/joaoportela/CircullarBuffer-CSharp
// Changed to be a bit more optimal at the cost of safety
namespace AdventOfCode.Utility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <inheritdoc/>
    /// <summary>
    /// Circular buffer.
    /// 
    /// When writing to a full buffer:
    /// PushBack -> removes this[0] / Front()
    /// PushFront -> removes this[Size-1] / Back()
    /// 
    /// this implementation is inspired by
    /// http://www.boost.org/doc/libs/1_53_0/libs/circular_buffer/doc/circular_buffer.html
    /// because I liked their interface.
    /// </summary>
    public class CircularBuffer<T> : IEnumerable<T>
    {
        private readonly T[] _buffer;
        private readonly int capacity;

        /// <summary>
        /// The _start. Index of the first element in buffer.
        /// </summary>
        private int _start;

        /// <summary>
        /// The _end. Index after the last element in the buffer.
        /// </summary>
        private int _end;

        /// <summary>
        /// The _size. Buffer size.
        /// </summary>
        private int _size;

        public CircularBuffer(int capacity)
            : this(capacity, new T[] { })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularBuffer{T}"/> class.
        /// 
        /// </summary>
        /// <param name='capacity'>
        /// Buffer capacity. Must be positive.
        /// </param>
        /// <param name='items'>
        /// Items to fill buffer with. Items length must be less than capacity.
        /// Suggestion: use Skip(x).Take(y).ToArray() to build this argument from
        /// any enumerable.
        /// </param>
        public CircularBuffer(int capacity, T[] items)
        {
            if (capacity < 1)
            {
                throw new ArgumentException(
                    "Circular buffer cannot have negative or zero capacity.", nameof(capacity));
            }
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (items.Length > capacity)
            {
                throw new ArgumentException(
                    "Too many items to fit circular buffer", nameof(items));
            }

            this._buffer = new T[capacity];

            Array.Copy(items, this._buffer, items.Length);
            this._size = items.Length;

            this._start = 0;
            this._end = this._size == capacity ? 0 : this._size;
            this.capacity = capacity;
        }

        /// <summary>
        /// Maximum capacity of the buffer. Elements pushed into the buffer after
        /// maximum capacity is reached (IsFull = true), will remove an element.
        /// </summary>
        public int Capacity => this._buffer.Length;

        public bool IsFull => this.Size == this.capacity;

        public bool IsEmpty => this.Size == 0;

        /// <summary>
        /// Current buffer size (the number of elements that the buffer has).
        /// </summary>
        public int Size => this._size;

        /// <summary>
        /// Element at the front of the buffer - this[0].
        /// </summary>
        /// <returns>The value of the element of type T at the front of the buffer.</returns>
        public T Front()
        {
            this.ThrowIfEmpty();
            return this._buffer[this._start];
        }

        /// <summary>
        /// Element at the back of the buffer - this[Size - 1].
        /// </summary>
        /// <returns>The value of the element of type T at the back of the buffer.</returns>
        public T Back()
        {
            this.ThrowIfEmpty();
            return this._buffer[(this._end != 0 ? this._end : this.capacity) - 1];
        }

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this._buffer[this._start + (index < this.capacity - this._start ? index : index - this.capacity)];

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this._buffer[this._start + (index < this.capacity - this._start ? index : index - this.capacity)] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index)
        {
            return this._buffer[this._start + (index < this.capacity - this._start ? index : index - this.capacity)];
        }

        /// <summary>
        /// Pushes a new element to the back of the buffer. Back()/this[Size-1]
        /// will now return this element.
        /// 
        /// When the buffer is full, the element at Front()/this[0] will be 
        /// popped to allow for this new element to fit.
        /// </summary>
        /// <param name="item">Item to push to the back of the buffer</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushBack(T item)
        {
            if (this.IsFull)
            {
                this._buffer[this._end] = item;
                this.Increment(ref this._end);
                this._start = this._end;
            }
            else
            {
                this._buffer[this._end] = item;
                this.Increment(ref this._end);
                ++this._size;
            }
        }

        /// <summary>
        /// Pushes a new element to the front of the buffer. Front()/this[0]
        /// will now return this element.
        /// 
        /// When the buffer is full, the element at Back()/this[Size-1] will be 
        /// popped to allow for this new element to fit.
        /// </summary>
        /// <param name="item">Item to push to the front of the buffer</param>
        public void PushFront(T item)
        {
            if (this.IsFull)
            {
                this.Decrement(ref this._start);
                this._end = this._start;
                this._buffer[this._start] = item;
            }
            else
            {
                this.Decrement(ref this._start);
                this._buffer[this._start] = item;
                ++this._size;
            }
        }

        /// <summary>
        /// Removes the element at the back of the buffer. Decreasing the 
        /// Buffer size by 1.
        /// </summary>
        public void PopBack()
        {
            this.ThrowIfEmpty("Cannot take elements from an empty buffer.");
            this.Decrement(ref this._end);
            this._buffer[this._end] = default;
            --this._size;
        }

        /// <summary>
        /// Removes the element at the front of the buffer. Decreasing the 
        /// Buffer size by 1.
        /// </summary>
        public void PopFront()
        {
            this.ThrowIfEmpty("Cannot take elements from an empty buffer.");
            this._buffer[this._start] = default;
            this.Increment(ref this._start);
            --this._size;
        }

        /// <summary>
        /// Copies the buffer contents to an array, according to the logical
        /// contents of the buffer (i.e. independent of the internal 
        /// order/contents)
        /// </summary>
        /// <returns>A new array with a copy of the buffer contents.</returns>
        public T[] ToArray()
        {
            T[] newArray = new T[this.Size];
            int newArrayOffset = 0;
            var segments = new ArraySegment<T>[2] { this.ArrayOne(), this.ArrayTwo() };
            foreach (ArraySegment<T> segment in segments)
            {
                Array.Copy(segment.Array, segment.Offset, newArray, newArrayOffset, segment.Count);
                newArrayOffset += segment.Count;
            }
            return newArray;
        }

        #region IEnumerable<T> implementation
        public IEnumerator<T> GetEnumerator()
        {
            var segments = new ArraySegment<T>[2] { this.ArrayOne(), this.ArrayTwo() };
            foreach (ArraySegment<T> segment in segments)
            {
                for (int i = 0; i < segment.Count; i++)
                {
                    yield return segment.Array[segment.Offset + i];
                }
            }
        }
        #endregion
        #region IEnumerable implementation
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        private void ThrowIfEmpty(string message = "Cannot access an empty buffer.")
        {
            if (this.IsEmpty)
            {
                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        /// Increments the provided index variable by one, wrapping
        /// around if necessary.
        /// </summary>
        /// <param name="index"></param>
        private void Increment(ref int index)
        {
            if (++index == this.capacity)
            {
                index = 0;
            }
        }

        /// <summary>
        /// Decrements the provided index variable by one, wrapping
        /// around if necessary.
        /// </summary>
        /// <param name="index"></param>
        private void Decrement(ref int index)
        {
            if (index == 0)
            {
                index = this.capacity;
            }
            index--;
        }

        // doing ArrayOne and ArrayTwo methods returning ArraySegment<T> as seen here: 
        // http://www.boost.org/doc/libs/1_37_0/libs/circular_buffer/doc/circular_buffer.html#classboost_1_1circular__buffer_1957cccdcb0c4ef7d80a34a990065818d
        // http://www.boost.org/doc/libs/1_37_0/libs/circular_buffer/doc/circular_buffer.html#classboost_1_1circular__buffer_1f5081a54afbc2dfc1a7fb20329df7d5b
        // should help a lot with the code.

        #region Array items easy access.
        // The array is composed by at most two non-contiguous segments, 
        // the next two methods allow easy access to those.

        private ArraySegment<T> ArrayOne()
        {
            if (this.IsEmpty)
            {
                return new ArraySegment<T>(new T[0]);
            }
            else if (this._start < this._end)
            {
                return new ArraySegment<T>(this._buffer, this._start, this._end - this._start);
            }
            else
            {
                return new ArraySegment<T>(this._buffer, this._start, this._buffer.Length - this._start);
            }
        }

        private ArraySegment<T> ArrayTwo()
        {
            if (this.IsEmpty)
            {
                return new ArraySegment<T>(new T[0]);
            }
            else if (this._start < this._end)
            {
                return new ArraySegment<T>(this._buffer, this._end, 0);
            }
            else
            {
                return new ArraySegment<T>(this._buffer, 0, this._end);
            }
        }
        #endregion
    }
}