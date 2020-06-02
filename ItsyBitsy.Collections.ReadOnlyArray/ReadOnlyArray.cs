// Copyright (c) 2020 Carl Reinke
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ItsyBitsy.Collections
{
    /// <summary>
    /// Provides methods for creating read-only arrays.
    /// </summary>
    public static class ReadOnlyArray
    {
        /// <summary>
        /// Converts an array to a <see cref="ReadOnlyArray{T}"/>.
        /// </summary>
        public static ReadOnlyArray<T> FromArray<T>(T[]? array) => array;

        /// <summary>
        /// Converts an <see cref="ImmutableArray{T}"/> to a <see cref="ReadOnlyArray{T}"/>.
        /// </summary>
        public static ReadOnlyArray<T> FromImmutableArray<T>(ImmutableArray<T> array) => array;

        /// <summary>
        /// Creates an immutable array that contains the specified elements from a read-only array.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array.</typeparam>
        /// <param name="items">The source array.</param>
        /// <param name="start">The index of the first element to copy from
        ///     <paramref name="items"/>.</param>
        /// <param name="length">The number of elements to copy from <paramref name="items"/>.
        ///     </param>
        /// <returns>An immutable array that contains the specified elements from the source array.
        ///     </returns>
        public static ImmutableArray<T> CreateImmutable<T>(ReadOnlyArray<T> items, int start, int length)
        {
            // This is safe because ImmutableArray.Create does not mutate the array.
            var underlyingArray = Unsafe.As<ReadOnlyArray<T>, T[]>(ref items);
            return ImmutableArray.Create(underlyingArray, start, length);
        }

        /// <summary>
        /// Creates an immutable array that contains the specified elements from a read-only span.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array.</typeparam>
        /// <param name="items">The source span.</param>
        /// <returns>An immutable array that contains the specified elements from the source span.
        ///     </returns>
        public static ImmutableArray<T> CreateImmutable<T>(ReadOnlySpan<T> items)
        {
            var underlyingArray = items.ToArray();
            // This is safe because the mutable array has no other references.
            return Unsafe.As<T[], ImmutableArray<T>>(ref underlyingArray);
        }
    }

    /// <summary>
    /// A read-only array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <remarks>
    /// <see cref="ReadOnlyArray{T}"/> provides a common interface for read-only access to an array
    /// or an <see cref="ImmutableArray{T}"/>.  Unlike <see cref="ImmutableArray{T}"/>,
    /// <see cref="ReadOnlyArray{T}"/> does not guarantee that the array cannot be mutated; it only
    /// guarantees that it cannot be used to mutate the array.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
#pragma warning disable CA1710 // Identifiers should have correct suffix
    public readonly struct ReadOnlyArray<T> : IReadOnlyList<T>, IEquatable<ReadOnlyArray<T>>, IStructuralComparable, IStructuralEquatable, IReadOnlyArray
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
        private const string _objectIsNotReadOnlyArray = "Object is not a read-only array.";
        private const string _operationNotValidOnDefault = "Operation is not valid on an uninitialized instance.";

        /// <summary>
        /// An empty array.
        /// </summary>
        public static readonly ReadOnlyArray<T> Empty = ImmutableArray<T>.Empty;

        // ReadOnlyArray<T> provides the same thread-safety guarantees that
        // ImmutableArray<T> does -- namely, every method must access `this`
        // only once per instance; typically this is an access to `_array`.

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly T[]? _array;

        private ReadOnlyArray(T[]? array)
        {
            _array = array;
        }

        /// <summary>
        /// Determines if two arrays are the same array.
        /// </summary>
        public static bool operator ==(ReadOnlyArray<T> left, ReadOnlyArray<T> right)
        {
            return left._array == right._array;
        }

        /// <summary>
        /// Determines if two arrays are the same array.
        /// </summary>
        public static bool operator ==(ReadOnlyArray<T>? left, ReadOnlyArray<T> right)
        {
            return left.GetValueOrDefault()._array == right._array;
        }

        /// <summary>
        /// Determines if two arrays are the same array.
        /// </summary>
        public static bool operator ==(ReadOnlyArray<T> left, ReadOnlyArray<T>? right)
        {
            return left._array == right.GetValueOrDefault()._array;
        }

        /// <summary>
        /// Determines if two arrays are the same array.
        /// </summary>
        public static bool operator ==(ReadOnlyArray<T>? left, ReadOnlyArray<T>? right)
        {
            return left.GetValueOrDefault()._array == right.GetValueOrDefault()._array;
        }

        /// <summary>
        /// Determines if two arrays are not the same array.
        /// </summary>
        public static bool operator !=(ReadOnlyArray<T> left, ReadOnlyArray<T> right)
        {
            return left._array != right._array;
        }

        /// <summary>
        /// Determines if two arrays are not the same array.
        /// </summary>
        public static bool operator !=(ReadOnlyArray<T>? left, ReadOnlyArray<T> right)
        {
            return left.GetValueOrDefault()._array != right._array;
        }

        /// <summary>
        /// Determines if two arrays are not the same array.
        /// </summary>
        public static bool operator !=(ReadOnlyArray<T> left, ReadOnlyArray<T>? right)
        {
            return left._array != right.GetValueOrDefault()._array;
        }

        /// <summary>
        /// Determines if two arrays are not the same array.
        /// </summary>
        public static bool operator !=(ReadOnlyArray<T>? left, ReadOnlyArray<T>? right)
        {
            return left.GetValueOrDefault()._array != right.GetValueOrDefault()._array;
        }

        /// <summary>
        /// Converts an array to a <see cref="ReadOnlyArray{T}"/>.
        /// </summary>
#pragma warning disable CA2225 // Operator overloads have named alternates
        public static implicit operator ReadOnlyArray<T>(T[]? array)
#pragma warning restore CA2225 // Operator overloads have named alternates
        {
            return new ReadOnlyArray<T>(array);
        }

        /// <summary>
        /// Converts an <see cref="ImmutableArray{T}"/> to a <see cref="ReadOnlyArray{T}"/>.
        /// </summary>
#pragma warning disable CA2225 // Operator overloads have named alternates
        public static implicit operator ReadOnlyArray<T>(ImmutableArray<T> array)
#pragma warning restore CA2225 // Operator overloads have named alternates
        {
            // This is safe because ReadOnlyArray does not allow mutation of the array.
            var underlyingArray = Unsafe.As<ImmutableArray<T>, T[]>(ref array);
            return new ReadOnlyArray<T>(underlyingArray);
        }

        /// <summary>
        /// Gets the element at the specified index in the array.
        /// </summary>
        public T this[int index] => _array![index];

        /// <summary>
        /// Gets a read-only reference to the element at the specified index in the array.
        /// </summary>
        public ref readonly T ItemRef(int index) => ref _array![index];

        T IReadOnlyList<T>.this[int index]
        {
            // This indexed property avoids throwing NullReferenceException on
            // uninitialized instances because that would be unexpected behavior
            // for a struct that implements an interface.

            get
            {
                T[]? array = _array;
                if (array == null)
                    throw new InvalidOperationException(_operationNotValidOnDefault);
                return array[index];
            }
        }

        /// <summary>
        /// Gets a value indicating that the array was not initialized.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsDefault => _array == null;

        /// <summary>
        /// Gets a value indicating that the array is empty.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsEmpty => _array!.Length == 0;

        /// <summary>
        /// Gets a value indicating that the array was not initialized or is empty.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsDefaultOrEmpty
        {
            get
            {
                T[]? array = _array;
                return array == null || array.Length == 0;
            }
        }

        /// <summary>
        /// Gets the number of elements in the array.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Length => _array!.Length;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        int IReadOnlyCollection<T>.Count
        {
            // This property avoids throwing NullReferenceException on
            // uninitialized instances because that would be unexpected behavior
            // for a struct that implements an interface.

            get
            {
                T[]? array = _array;
                if (array == null)
                    throw new InvalidOperationException(_operationNotValidOnDefault);
                return array.Length;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Array? IReadOnlyArray.Array => _array;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                T[]? array = _array;
                return array == null
                    ? "Uninitialized"
                    : string.Format(CultureInfo.CurrentCulture, "Length = {0}", array.Length);
            }
        }

        /// <summary>
        /// Efficiently casts the array to an array of some type that <typeparamref name="T"/>
        /// derives from.
        /// </summary>
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static ReadOnlyArray<T> CastUp<TDerived>(ReadOnlyArray<TDerived> array)
#pragma warning restore CA1000 // Do not declare static members on generic types
            where TDerived : class?, T
        {
            return new ReadOnlyArray<T>(array._array);
        }

        /// <summary>
        /// Casts the array to an array of <typeparamref name="TOther"/>.
        /// </summary>
        public ReadOnlyArray<TOther> As<TOther>()
            where TOther : class?
        {
            return new ReadOnlyArray<TOther>(_array as TOther[]);
        }

        /// <summary>
        /// Creates a read-only memory region over the read-only array.
        /// </summary>
        public ReadOnlyMemory<T> AsMemory() => new ReadOnlyMemory<T>(_array);

        /// <summary>
        /// Creates a read-only span over the read-only array.
        /// </summary>
        public ReadOnlySpan<T> AsSpan() => new ReadOnlySpan<T>(_array);

        /// <summary>
        /// Casts the array to an array of <typeparamref name="TOther"/>.
        /// </summary>
        public ReadOnlyArray<TOther> CastArray<TOther>()
            where TOther : class?
        {
            return new ReadOnlyArray<TOther>((TOther[]?)(object?)_array);
        }

        /// <summary>
        /// Copies the elements of the array to a specified array.
        /// </summary>
        public void CopyTo(T[] destination)
        {
            T[] array = _array!;
            _ = array.Length;  // throw NullReferenceException if array is null.
            Array.Copy(array, 0, destination, 0, array.Length);
        }

        /// <summary>
        /// Copies the elements of the array to a specified array starting at a specified
        /// destination index.
        /// </summary>
        public void CopyTo(T[] destination, int destinationIndex)
        {
            T[] array = _array!;
            _ = array.Length;  // throw NullReferenceException if array is null.
            Array.Copy(array, 0, destination, destinationIndex, array.Length);
        }

        /// <summary>
        /// Copies the specified elements of the array to a specified array starting at a specified
        /// destination index.
        /// </summary>
        public void CopyTo(int sourceIndex, T[] destination, int destinationIndex, int length)
        {
            T[] array = _array!;
            _ = array.Length;  // throw NullReferenceException if array is null.
            Array.Copy(array, sourceIndex, destination, destinationIndex, length);
        }

        /// <summary>
        /// Searches the elements of the array for the first instance of a specified item.
        /// </summary>
        public int IndexOf(T item)
        {
            T[] array = _array!;
            _ = array.Length;  // throw NullReferenceException if array is null.
            return Array.IndexOf(array, item);
        }

        /// <summary>
        /// Searches the specified elements of the array for the first instance of a specified item.
        /// </summary>
        public int IndexOf(T item, int startIndex)
        {
            T[] array = _array!;
            _ = array.Length;  // throw NullReferenceException if array is null.
            return Array.IndexOf(array, item, startIndex);
        }

        /// <summary>
        /// Searches the specified elements of the array for the first instance of a specified item.
        /// </summary>
        public int IndexOf(T item, int startIndex, int count)
        {
            T[] array = _array!;
            _ = array.Length;  // throw NullReferenceException if array is null.
            return Array.IndexOf(array, item, startIndex, count);
        }

        /// <summary>
        /// Searches the elements of the array for the last instance of a specified item.
        /// </summary>
        public int LastIndexOf(T item)
        {
            T[] array = _array!;
            _ = array.Length;  // throw NullReferenceException if array is null.
            return Array.LastIndexOf(array, item);
        }

        /// <summary>
        /// Searches the specified elements of the array for the last instance of a specified item.
        /// </summary>
        public int LastIndexOf(T item, int startIndex)
        {
            T[] array = _array!;
            _ = array.Length;  // throw NullReferenceException if array is null.
            return Array.LastIndexOf(array, item, startIndex);
        }

        /// <summary>
        /// Searches the specified elements of the array for the last instance of a specified item.
        /// </summary>
        public int LastIndexOf(T item, int startIndex, int count)
        {
            T[] array = _array!;
            _ = array.Length;  // throw NullReferenceException if array is null.
            return Array.LastIndexOf(array, item, startIndex, count);
        }

        /// <summary>
        /// Determines if two arrays are the same array.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is ReadOnlyArray<T> other && Equals(other);
        }

        /// <summary>
        /// Determines if two arrays are the same array.
        /// </summary>
        public bool Equals(ReadOnlyArray<T> other)
        {
            return _array == other._array;
        }

        /// <summary>
        /// Gets a hash code for the array.
        /// </summary>
        public override int GetHashCode()
        {
            // This method avoids throwing NullReferenceException on
            // uninitialized instances because that would be unexpected behavior
            // for a struct.

            return _array?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Gets an enumerator that enumerates the elements of the array.
        /// </summary>
        public Enumerator GetEnumerator()
        {
            T[] array = _array!;
            _ = array.Length;  // throw NullReferenceException if array is null.
            return new Enumerator(array);
        }

        /// <summary>
        /// Creates an immutable array from the current contents of the array.
        /// </summary>
        /// <remarks>
        /// <see cref="ToImmutableArray"/> always returns a new <see cref="ImmutableArray{T}"/> even
        /// if the <see cref="ReadOnlyArray{T}"/> was constructed from an
        /// <see cref="ImmutableArray{T}"/>.
        /// </remarks>
        public ImmutableArray<T> ToImmutableArray()
        {
            T[] array = _array!;
            _ = array.Length;  // throw NullReferenceException if array is null.
            return ImmutableArray.Create(array);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            // This method avoids throwing NullReferenceException on
            // uninitialized instances because that would be unexpected behavior
            // for a struct that implements an interface.

            T[]? array = _array;
            if (array == null)
                throw new InvalidOperationException(_operationNotValidOnDefault);
            return ((IEnumerable<T>)array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            // This method avoids throwing NullReferenceException on
            // uninitialized instances because that would be unexpected behavior
            // for a struct that implements an interface.

            T[]? array = _array;
            if (array == null)
                throw new InvalidOperationException(_operationNotValidOnDefault);
            return array.GetEnumerator();
        }

        int IStructuralComparable.CompareTo(object? other, IComparer comparer)
        {
            // This method avoids throwing NullReferenceException on
            // uninitialized instances because that would be unexpected behavior
            // for a struct that implements an interface.

            T[]? array = _array;
            if (other is IReadOnlyArray otherReadOnlyArray)
            {
                Array? otherArray = otherReadOnlyArray.Array;
                if (array == null)
                    return otherArray == null ? 0 : -1;
                else if (otherArray == null)
                    return 1;
                else
                    return ((IStructuralComparable)array).CompareTo(otherArray, comparer);
            }
            throw new ArgumentException(_objectIsNotReadOnlyArray, nameof(other));
        }

        bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
        {
            // This method avoids throwing NullReferenceException on
            // uninitialized instances because that would be unexpected behavior
            // for a struct that implements an interface.

            T[]? array = _array;
            if (other is IReadOnlyArray otherReadOnlyArray)
            {
                Array? otherArray = otherReadOnlyArray.Array;
                if (array == null)
                    return otherArray == null;
                else
                    return ((IStructuralEquatable)array).Equals(otherArray, comparer);
            }
            return false;
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            // This method avoids throwing NullReferenceException on
            // uninitialized instances because that would be unexpected behavior
            // for a struct that implements an interface.

            return ((IStructuralEquatable?)_array)?.GetHashCode(comparer) ?? 0;
        }

        /// <summary>
        /// An array enumerator.
        /// </summary>
#pragma warning disable CA1034 // Nested types should not be visible
#pragma warning disable CA1815 // Override equals and operator equals on value types
        public struct Enumerator
#pragma warning restore CA1815 // Override equals and operator equals on value types
#pragma warning restore CA1034 // Nested types should not be visible
        {
            private readonly T[] _array;

            private int _index;

            internal Enumerator(T[] array)
            {
                _array = array;
                _index = -1;
            }

            /// <summary>
            /// Gets the current element.
            /// </summary>
            public T Current => _array[_index];

            /// <summary>
            /// Advances the enumerator to the next element.
            /// </summary>
            public bool MoveNext()
            {
                _index += 1;
                return _index < _array.Length;
            }
        }
    }
}
