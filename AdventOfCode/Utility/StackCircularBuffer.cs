// https://github.com/joaoportela/CircullarBuffer-CSharp
// Changed to be a bit more optimal at the cost of safety
namespace AdventOfCode.Utility
{
    using System;
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
    public ref struct StackCircularBuffer<T>
    {
        private readonly Span<T> buffer;
        private readonly int capacity;

        /// <summary>
        /// The _start. Index of the first element in buffer.
        /// </summary>
        private int start;

        /// <summary>
        /// The _end. Index after the last element in the buffer.
        /// </summary>
        private int end;

        /// <summary>
        /// The _size. Buffer size.
        /// </summary>
        private int size;

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
        public StackCircularBuffer(Span<T> items)
            : this(items, items.Length)
        {
        }
        
        public StackCircularBuffer(Span<T> items, int initialSize)
        {
            if (items.Length < 1)
            {
                throw new ArgumentException(
                    "Circular buffer cannot have negative or zero capacity.", nameof(this.capacity));
            }
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            this.buffer = items;
            this.size = initialSize;

            this.start = 0;
            this.end = this.size == items.Length ? 0 : this.size;
            this.capacity = items.Length;
        }

        /// <summary>
        /// Maximum capacity of the buffer. Elements pushed into the buffer after
        /// maximum capacity is reached (IsFull = true), will remove an element.
        /// </summary>
        public int Capacity => this.buffer.Length;

        public bool IsFull => this.Size == this.capacity;

        public bool IsEmpty => this.Size == 0;

        /// <summary>
        /// Current buffer size (the number of elements that the buffer has).
        /// </summary>
        public int Size => this.size;

        /// <summary>
        /// Element at the front of the buffer - this[0].
        /// </summary>
        /// <returns>The value of the element of type T at the front of the buffer.</returns>
        public T Front()
        {
            this.ThrowIfEmpty();
            return this.buffer[this.start];
        }

        /// <summary>
        /// Element at the back of the buffer - this[Size - 1].
        /// </summary>
        /// <returns>The value of the element of type T at the back of the buffer.</returns>
        public T Back()
        {
            this.ThrowIfEmpty();
            return this.buffer[(this.end != 0 ? this.end : this.capacity) - 1];
        }

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.buffer[this.start + (index < this.capacity - this.start ? index : index - this.capacity)];

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.buffer[this.start + (index < this.capacity - this.start ? index : index - this.capacity)] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index)
        {
            return this.buffer[this.start + (index < this.capacity - this.start ? index : index - this.capacity)];
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
                this.buffer[this.end] = item;
                this.Increment(ref this.end);
                this.start = this.end;
            }
            else
            {
                this.buffer[this.end] = item;
                this.Increment(ref this.end);
                ++this.size;
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
                this.Decrement(ref this.start);
                this.end = this.start;
                this.buffer[this.start] = item;
            }
            else
            {
                this.Decrement(ref this.start);
                this.buffer[this.start] = item;
                ++this.size;
            }
        }

        /// <summary>
        /// Removes the element at the back of the buffer. Decreasing the 
        /// Buffer size by 1.
        /// </summary>
        public T PopBack()
        {
            this.ThrowIfEmpty("Cannot take elements from an empty buffer.");
            T value = this.Back();
            this.Decrement(ref this.end);
            this.buffer[this.end] = default!;
            --this.size;
            return value;
        }

        /// <summary>
        /// Removes the element at the front of the buffer. Decreasing the 
        /// Buffer size by 1.
        /// </summary>
        public T PopFront()
        {
            this.ThrowIfEmpty("Cannot take elements from an empty buffer.");
            var value = this.Front();
            this.buffer[this.start] = default!;
            this.Increment(ref this.start);
            --this.size;
            return value;
        }

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
    }
}