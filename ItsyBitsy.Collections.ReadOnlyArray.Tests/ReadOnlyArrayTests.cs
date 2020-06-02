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
using System.Runtime.CompilerServices;
using Xunit;

namespace ItsyBitsy.Collections.Tests
{
    public static class ReadOnlyArrayTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void FromArray_Always_ReturnsSameValueAsImplicitCast(byte[]? array)
        {
            var instance = ReadOnlyArray.FromArray(array);

            ReadOnlyArray<byte> expectedInstance = array;
            Assert.Equal(expectedInstance, instance);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void FromImmutableArray_Always_ReturnsSameValueAsImplicitCast(byte[]? array)
        {
            var immutableArray = array == null ? default : ImmutableArray.Create(array);

            var instance = ReadOnlyArray.FromImmutableArray(immutableArray);

            ReadOnlyArray<byte> expectedInstance = immutableArray;
            Assert.Equal(expectedInstance, instance);
        }

        [Theory]
        [InlineData(new byte[0], 0, 0, new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 }, 0, 3, new byte[] { 3, 1, 2 })]
        [InlineData(new byte[] { 3, 1, 2 }, 0, 2, new byte[] { 3, 1 })]
        [InlineData(new byte[] { 3, 1, 2 }, 1, 1, new byte[] { 1 })]
        [InlineData(new byte[] { 3, 1, 2 }, 1, 2, new byte[] { 1, 2 })]
        public static void CreateImmutableReadOnlyArray_Always_ReturnsImmutableArrayContainingSelectedItems(byte[] array, int start, int length, IEnumerable<byte> expectedValues)
        {
            ReadOnlyArray<byte> readOnlyArray = array;

            var instance = ReadOnlyArray.CreateImmutable(readOnlyArray, start, length);

            Assert.Equal(expectedValues, instance);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void CreateImmutableReadOnlySpan_Always_ReturnsImmutableArrayContainingItems(byte[]? array)
        {
            ReadOnlySpan<byte> span = array;

            var instance = ReadOnlyArray.CreateImmutable(span);

            IEnumerable<byte> expectedValues = array ?? Array.Empty<byte>();
            Assert.Equal(expectedValues, instance);
        }

        [Fact]
        public static void Empty_Always_IsNotDefault()
        {
            var instance = ReadOnlyArray<byte>.Empty;

            Assert.False(instance.IsDefault);
        }

        [Fact]
        public static void Empty_Always_IsEmpty()
        {
            var instance = ReadOnlyArray<byte>.Empty;

            Assert.True(instance.IsEmpty);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void OpEquality_SameArray_ReturnsSameValueAsArrayOpEquality(byte[]? array)
        {
            ReadOnlyArray<byte> instance1 = array;
            ReadOnlyArray<byte> instance2 = array;
            ReadOnlyArray<byte>? nullable1 = array;
            ReadOnlyArray<byte>? nullable2 = array;

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.Equal(array == array, instance1 == instance2);
            Assert.Equal(array == array, instance2 == instance1);

            Assert.Equal(array == array, instance1 == nullable1);
            Assert.Equal(array == array, nullable1 == instance1);

            Assert.Equal(array == array, nullable1 == nullable2);
            Assert.Equal(array == array, nullable2 == nullable1);
#pragma warning restore CS1718 // Comparison made to same variable
        }

        [Theory]
        [InlineData(null, new byte[0])]
        [InlineData(new byte[0], new byte[0])]
        [InlineData(new byte[0], new byte[] { 3, 1, 2 })]
        [InlineData(new byte[] { 3, 1, 2 }, new byte[] { 3, 1, 2 })]
        public static void OpEquality_DifferentArrays_ReturnsSameValueAsArrayOpEquality(byte[]? array1, byte[]? array2)
        {
            ReadOnlyArray<byte> instance1 = array1;
            ReadOnlyArray<byte> instance2 = array2;
            ReadOnlyArray<byte>? nullable1 = array1;
            ReadOnlyArray<byte>? nullable2 = array2;

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.Equal(array1 == array2, instance1 == instance2);
            Assert.Equal(array2 == array1, instance2 == instance1);

            Assert.Equal(array1 == array2, instance1 == nullable2);
            Assert.Equal(array2 == array1, nullable2 == instance1);

            Assert.Equal(array1 == array2, nullable1 == nullable2);
            Assert.Equal(array2 == array1, nullable2 == nullable1);
#pragma warning restore CS1718 // Comparison made to same variable
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void OpInequality_SameArray_ReturnsSameValueAsArrayOpEquality(byte[]? array)
        {
            ReadOnlyArray<byte> instance1 = array;
            ReadOnlyArray<byte> instance2 = array;
            ReadOnlyArray<byte>? nullable1 = array;
            ReadOnlyArray<byte>? nullable2 = array;

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.Equal(array != array, instance1 != instance2);
            Assert.Equal(array != array, instance2 != instance1);

            Assert.Equal(array != array, instance1 != nullable1);
            Assert.Equal(array != array, nullable1 != instance1);

            Assert.Equal(array != array, nullable1 != nullable2);
            Assert.Equal(array != array, nullable2 != nullable1);
#pragma warning restore CS1718 // Comparison made to same variable
        }

        [Theory]
        [InlineData(null, new byte[0])]
        [InlineData(new byte[0], new byte[0])]
        [InlineData(new byte[0], new byte[] { 3, 1, 2 })]
        [InlineData(new byte[] { 3, 1, 2 }, new byte[] { 3, 1, 2 })]
        public static void OpInequality_DifferentArrays_ReturnsSameValueAsArrayOpEquality(byte[]? array1, byte[]? array2)
        {
            ReadOnlyArray<byte> instance1 = array1;
            ReadOnlyArray<byte> instance2 = array2;
            ReadOnlyArray<byte>? nullable1 = array1;
            ReadOnlyArray<byte>? nullable2 = array2;

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.Equal(array1 != array2, instance1 != instance2);
            Assert.Equal(array2 != array1, instance2 != instance1);

            Assert.Equal(array1 != array2, instance1 != nullable2);
            Assert.Equal(array2 != array1, nullable2 != instance1);

            Assert.Equal(array1 != array2, nullable1 != nullable2);
            Assert.Equal(array2 != array1, nullable2 != nullable1);
#pragma warning restore CS1718 // Comparison made to same variable
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void OpImplicitArray_Always_ReturnsValueWithSameUnderlyingArray(byte[]? array)
        {
            ReadOnlyArray<byte> instance = array;

            byte[] instanceUnderlyingArray = Unsafe.As<ReadOnlyArray<byte>, byte[]>(ref instance);

            Assert.Same(array, instanceUnderlyingArray);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void OpImplicitImmutableArray_Always_ReturnsValueWithSameUnderlyingArray(byte[]? array)
        {
            var immutableArray = ImmutableArray.Create(array);
            ReadOnlyArray<byte> instance = immutableArray;

            byte[] immutableUnderlyingArray = Unsafe.As<ImmutableArray<byte>, byte[]>(ref immutableArray);
            byte[] instanceUnderlyingArray = Unsafe.As<ReadOnlyArray<byte>, byte[]>(ref instance);

            Assert.Same(immutableUnderlyingArray, instanceUnderlyingArray);
        }

        [Fact]
        public static void Item_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance[0]);
        }

        [Theory]
        [InlineData(new byte[0], -1)]
        [InlineData(new byte[0], 0)]
        [InlineData(new byte[] { 3, 1, 2 }, -1)]
        [InlineData(new byte[] { 3, 1, 2 }, 3)]
        public static void Item_IndexOutOfRange_ThrowsIndexOutOfRangeException(byte[] array, int index)
        {
            ReadOnlyArray<byte> instance = array;

            _ = Assert.Throws<IndexOutOfRangeException>(() => instance[index]);
        }

        [Theory]
        [InlineData(new byte[] { 3, 1, 2 }, 0)]
        [InlineData(new byte[] { 3, 1, 2 }, 1)]
        [InlineData(new byte[] { 3, 1, 2 }, 2)]
        public static void Item_IndexInRange_ReturnsSameValueAsArray(byte[] array, int index)
        {
            ReadOnlyArray<byte> instance = array;

            Assert.Equal(array[index], instance[index]);
        }

        [Fact]
        public static void ItemRef_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance.ItemRef(0));
        }

        [Theory]
        [InlineData(new byte[0], -1)]
        [InlineData(new byte[0], 0)]
        [InlineData(new byte[] { 3, 1, 2 }, -1)]
        [InlineData(new byte[] { 3, 1, 2 }, 3)]
        public static void ItemRef_IndexOutOfRange_ThrowsIndexOutOfRangeException(byte[] array, int index)
        {
            ReadOnlyArray<byte> instance = array;

            _ = Assert.Throws<IndexOutOfRangeException>(() => instance.ItemRef(index));
        }

        [Theory]
        [InlineData(new byte[] { 3, 1, 2 }, 0)]
        [InlineData(new byte[] { 3, 1, 2 }, 1)]
        [InlineData(new byte[] { 3, 1, 2 }, 2)]
        public static void ItemRef_IndexInRange_ReturnsSameValueAsArray(byte[] array, int index)
        {
            ReadOnlyArray<byte> instance = array;

            Assert.True(Unsafe.AreSame(ref array[index], ref Unsafe.AsRef(instance.ItemRef(index))));
        }

        [Fact]
        public static void IReadOnlyListT_Item_DefaultInstance_ThrowsInvalidOperationException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<InvalidOperationException>(() => ((IReadOnlyList<byte>)instance)[0]);
        }

        [Theory]
        [InlineData(new byte[0], -1)]
        [InlineData(new byte[0], 0)]
        [InlineData(new byte[] { 3, 1, 2 }, -1)]
        [InlineData(new byte[] { 3, 1, 2 }, 3)]
        public static void IReadOnlyListT_Item_IndexOutOfRange_ThrowsIndexOutOfRangeException(byte[] array, int index)
        {
            ReadOnlyArray<byte> instance = array;

            _ = Assert.Throws<IndexOutOfRangeException>(() => ((IReadOnlyList<byte>)instance)[index]);
        }

        [Theory]
        [InlineData(new byte[] { 3, 1, 2 }, 0)]
        [InlineData(new byte[] { 3, 1, 2 }, 1)]
        [InlineData(new byte[] { 3, 1, 2 }, 2)]
        public static void IReadOnlyListT_Item_IndexInRange_ReturnsSameValueAsArray(byte[] array, int index)
        {
            ReadOnlyArray<byte> instance = array;

            Assert.Equal(((IReadOnlyList<byte>)array)[index], ((IReadOnlyList<byte>)instance)[index]);
        }

        [Fact]
        public static void IsDefault_DefaultInstance_ReturnsTrue()
        {
            ReadOnlyArray<byte> instance = null;

            Assert.True(instance.IsDefault);
        }

        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void IsDefault_NonDefaultInstance_ReturnsFalse(byte[] array)
        {
            ReadOnlyArray<byte> instance = array;

            Assert.False(instance.IsDefault);
        }

        [Fact]
        public static void IsEmpty_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance.IsEmpty);
        }

        [Fact]
        public static void IsEmpty_EmptyInstance_ReturnsTrue()
        {
            ReadOnlyArray<byte> instance = new byte[0];

            Assert.True(instance.IsEmpty);
        }

        [Theory]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void IsEmpty_NonEmptyInstance_ReturnsFalse(byte[] array)
        {
            ReadOnlyArray<byte> instance = array;

            Assert.False(instance.IsEmpty);
        }

        [Fact]
        public static void IsDefaultOrEmpty_DefaultInstance_ReturnsTrue()
        {
            ReadOnlyArray<byte> instance = null;

            Assert.True(instance.IsDefaultOrEmpty);
        }

        [Fact]
        public static void IsDefaultOrEmpty_EmptyInstance_ReturnsTrue()
        {
            ReadOnlyArray<byte> instance = new byte[0];

            Assert.True(instance.IsDefaultOrEmpty);
        }

        [Theory]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void IsDefaultOrEmpty_NonEmptyInstance_ReturnsFalse(byte[] array)
        {
            ReadOnlyArray<byte> instance = array;

            Assert.False(instance.IsDefaultOrEmpty);
        }

        [Fact]
        public static void Length_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance.Length);
        }

        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void Length_NonDefaultInstance_ReturnsSameValueAsArray(byte[] array)
        {
            ReadOnlyArray<byte> instance = array;

            Assert.Equal(array.Length, instance.Length);
        }

        [Fact]
        public static void IReadOnlyCollectionT_Count_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<InvalidOperationException>(() => ((IReadOnlyCollection<byte>)instance).Count);
        }

        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void IReadOnlyCollectionT_Count_NonDefaultInstance_ReturnsSameValueAsArray(byte[] array)
        {
            ReadOnlyArray<byte> instance = array;

            Assert.Equal(((IReadOnlyCollection<byte>)array).Count, ((IReadOnlyCollection<byte>)instance).Count);
        }

        [Theory]
        [InlineData(null)]
        [MemberData(nameof(NonNullableDerivedArray))]
        public static void CastUp_NonNullable_ReturnsValueWithSameUnderlyingArray(Derived[]? array)
        {
            ReadOnlyArray<Derived> instance = array;

            var castUpInstance = ReadOnlyArray<Base>.CastUp(instance);

            Base?[]? castUpUnderlyingArray = Unsafe.As<ReadOnlyArray<Base>, Base[]?>(ref castUpInstance);
            Assert.Same(array, castUpUnderlyingArray);
        }

        [Theory]
        [InlineData(null)]
        [MemberData(nameof(NullableDerivedArray))]
        public static void CastUp_Nullable_ReturnsValueWithSameUnderlyingArray(Derived?[]? array)
        {
            ReadOnlyArray<Derived?> instance = array;

            var castUpInstance = ReadOnlyArray<Base?>.CastUp(instance);

            Base?[]? castUpUnderlyingArray = Unsafe.As<ReadOnlyArray<Base?>, Base?[]?>(ref castUpInstance);
            Assert.Same(array, castUpUnderlyingArray);
        }

        [Theory]
        [InlineData(null)]
        [MemberData(nameof(NonNullableBaseArray))]
        [MemberData(nameof(NonNullableDerivedArray))]
        public static void As_NonNullableBaseToDerived_ReturnsValueWithArrayCastByAs(Base[]? array)
        {
            ReadOnlyArray<Base> instance = array;

            var asInstance = instance.As<Derived>();

            Derived[]? asUnderlyingArray = Unsafe.As<ReadOnlyArray<Derived>, Derived[]>(ref asInstance);
            Assert.Same(array as Derived[], asUnderlyingArray);
        }

        [Theory]
        [InlineData(null)]
        [MemberData(nameof(NonNullableDerivedArray))]
        public static void As_NonNullableDerivedToBase_ReturnsValueWithArrayCastByAs(Derived[]? array)
        {
            ReadOnlyArray<Derived?> instance = array;

            var asInstance = instance.As<Base>();

            Base[]? asUnderlyingArray = Unsafe.As<ReadOnlyArray<Base>, Base[]?>(ref asInstance);
            Assert.Same(array as Base[], asUnderlyingArray);
        }

        [Theory]
        [InlineData(null)]
        [MemberData(nameof(NonNullableBaseArray))]
        [MemberData(nameof(NonNullableDerivedArray))]
        public static void As_NonNullableBaseToOther_ReturnsValueWithArrayCastByAs(Base[]? array)
        {
            ReadOnlyArray<Base> instance = array;

            var asInstance = instance.As<Other>();

            Other[]? asUnderlyingArray = Unsafe.As<ReadOnlyArray<Other>, Other[]?>(ref asInstance);
            Assert.Same((object?)array as Other[], asUnderlyingArray);
        }

        [Theory]
        [InlineData(null)]
        [MemberData(nameof(NullableBaseArray))]
        [MemberData(nameof(NullableDerivedArray))]
        public static void As_NullableBaseToDerived_ReturnsValueWithArrayCastByAs(Base?[]? array)
        {
            ReadOnlyArray<Base?> instance = array;

            var asInstance = instance.As<Derived?>();

            Derived?[]? asUnderlyingArray = Unsafe.As<ReadOnlyArray<Derived?>, Derived?[]>(ref asInstance);
            Assert.Same(array as Derived[], asUnderlyingArray);
        }

        [Theory]
        [InlineData(null)]
        [MemberData(nameof(NullableDerivedArray))]
        public static void As_NullableDerivedToBase_ReturnsValueWithArrayCastByAs(Derived?[]? array)
        {
            ReadOnlyArray<Derived?> instance = array;

            var asInstance = instance.As<Base?>();

            Base?[]? asUnderlyingArray = Unsafe.As<ReadOnlyArray<Base?>, Base?[]?>(ref asInstance);
            Assert.Same(array as Base[], asUnderlyingArray);
        }

        [Theory]
        [InlineData(null)]
        [MemberData(nameof(NullableBaseArray))]
        [MemberData(nameof(NullableDerivedArray))]
        public static void As_NullableBaseToOther_ReturnsValueWithArrayCastByAs(Base?[]? array)
        {
            ReadOnlyArray<Base?> instance = array;

            var asInstance = instance.As<Other?>();

            Other?[]? asUnderlyingArray = Unsafe.As<ReadOnlyArray<Other?>, Other?[]?>(ref asInstance);
            Assert.Same((object?)array as Other?[], asUnderlyingArray);
        }

        [Fact]
        public static void AsMemory_DefaultInstance_ReturnsEmptyMemory()
        {
            ReadOnlyArray<byte> instance = null;

            var memory = instance.AsMemory();

            Assert.Equal(0, memory.Length);
        }

        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void AsMemory_NonDefaultInstance_ReturnsReadOnlySpanWrappingArray(byte[] array)
        {
            ReadOnlyArray<byte> instance = array;

            var memory = instance.AsMemory();

            var expectedMemory = array.AsMemory();
            Assert.Equal(expectedMemory, memory);
        }

        [Fact]
        public static void AsSpan_DefaultInstance_ReturnsEmptyMemory()
        {
            ReadOnlyArray<byte> instance = null;

            var span = instance.AsSpan();

            Assert.Equal(0, span.Length);
        }

        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void AsSpan_NonDefaultInstance_ReturnsReadOnlySpanWrappingArray(byte[] array)
        {
            ReadOnlyArray<byte> instance = array;

            var span = instance.AsSpan();

            var expectedSpan = array.AsSpan();
            Assert.True(expectedSpan == span);
        }

        [Theory]
        [InlineData(null)]
        [MemberData(nameof(NonNullableBaseArray))]
        [MemberData(nameof(NonNullableDerivedArray))]
        public static void CastArray_NonNullableBaseToDerived_ReturnsValueWithArrayCast(Base[]? array)
        {
            ReadOnlyArray<Base> instance = array;

            if (array is null || array is Derived[])
            {
                var castInstance = instance.CastArray<Derived>();

                Derived[]? castUnderlyingArray = Unsafe.As<ReadOnlyArray<Derived>, Derived[]?>(ref castInstance);
                Assert.Same(array, castUnderlyingArray);
            }
            else
            {
                _ = Assert.Throws<InvalidCastException>(() => instance.CastArray<Derived>());
            }
        }

        [Theory]
        [InlineData(null)]
        [MemberData(nameof(NonNullableDerivedArray))]
        public static void CastArray_NonNullableDerivedToBase_ReturnsValueWithArrayCast(Derived[]? array)
        {
            ReadOnlyArray<Derived> instance = array;

            var asInstance = instance.CastArray<Base>();

            Base[]? asUnderlyingArray = Unsafe.As<ReadOnlyArray<Base>, Base[]?>(ref asInstance);
            Assert.Same(array, asUnderlyingArray);
        }

        [Theory]
        [InlineData(null)]
        [MemberData(nameof(NonNullableBaseArray))]
        [MemberData(nameof(NonNullableDerivedArray))]
        public static void CastArray_NonNullableBaseToOther_ReturnsValueWithArrayCast(Base[]? array)
        {
            ReadOnlyArray<Base> instance = array;

            if (array is null)
            {
                var castInstance = instance.CastArray<Other>();

                Other[]? castUnderlyingArray = Unsafe.As<ReadOnlyArray<Other>, Other[]?>(ref castInstance);
                Assert.Same(array, castUnderlyingArray);
            }
            else
            {
                _ = Assert.Throws<InvalidCastException>(() => instance.CastArray<Other>());
            }
        }

        [Theory]
        [InlineData(null)]
        [MemberData(nameof(NullableBaseArray))]
        [MemberData(nameof(NullableDerivedArray))]
        public static void CastArray_NullableBaseToDerived_ReturnsValueWithArrayCast(Base?[]? array)
        {
            ReadOnlyArray<Base?> instance = array;

            if (array is null || array is Derived?[])
            {
                var castInstance = instance.CastArray<Derived?>();

                Derived?[]? castUnderlyingArray = Unsafe.As<ReadOnlyArray<Derived?>, Derived?[]?>(ref castInstance);
                Assert.Same(array, castUnderlyingArray);
            }
            else
            {
                _ = Assert.Throws<InvalidCastException>(() => instance.CastArray<Derived?>());
            }
        }

        [Theory]
        [InlineData(null)]
        [MemberData(nameof(NullableDerivedArray))]
        public static void CastArray_NullableDerivedToBase_ReturnsValueWithArrayCast(Derived?[]? array)
        {
            ReadOnlyArray<Derived?> instance = array;

            var asInstance = instance.CastArray<Base?>();

            Base?[]? asUnderlyingArray = Unsafe.As<ReadOnlyArray<Base?>, Base?[]?>(ref asInstance);
            Assert.Same(array, asUnderlyingArray);
        }

        [Theory]
        [InlineData(null)]
        [MemberData(nameof(NullableBaseArray))]
        [MemberData(nameof(NullableDerivedArray))]
        public static void CastArray_NullableBaseToOther_ReturnsValueWithArrayCast(Base?[]? array)
        {
            ReadOnlyArray<Base?> instance = array;

            if (array is null)
            {
                var castInstance = instance.CastArray<Other?>();

                Other?[]? castUnderlyingArray = Unsafe.As<ReadOnlyArray<Other?>, Other?[]?>(ref castInstance);
                Assert.Same(array, castUnderlyingArray);
            }
            else
            {
                _ = Assert.Throws<InvalidCastException>(() => instance.CastArray<Other?>());
            }
        }

        [Fact]
        public static void CopyTo_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance.CopyTo(Array.Empty<byte>()));
        }

        // The tests for CopyTo are non-exhaustive -- just sanity checks, with the assumption that
        // the underlying implementation calls Array.CopyTo(...).
        [Fact]
        public static void CopyTo_NonDefaultInstance_CopiesToDestination()
        {
            ReadOnlyArray<byte> instance = new byte[] { 3, 1, 2 };
            byte[] destination = new byte[instance.Length];

            instance.CopyTo(destination);

            byte[] expectedDestination = new byte[] { instance[0], instance[1], instance[2] };
            Assert.Equal<byte>(expectedDestination, destination);
        }

        [Fact]
        public static void CopyTo2_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance.CopyTo(Array.Empty<byte>(), 0));
        }

        // The tests for CopyTo are non-exhaustive -- just sanity checks, with the assumption that
        // the underlying implementation calls Array.CopyTo(...).
        [Fact]
        public static void CopyTo2_NonDefaultInstance_CopiesToDestination()
        {
            ReadOnlyArray<byte> instance = new byte[] { 3, 1, 2 };
            byte[] destination = new byte[instance.Length + 1];

            instance.CopyTo(destination, 1);

            byte[] expectedDestination = new byte[] { 0, instance[0], instance[1], instance[2] };
            Assert.Equal<byte>(expectedDestination, destination);
        }

        [Fact]
        public static void CopyTo4_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance.CopyTo(0, Array.Empty<byte>(), 0, 0));
        }

        // The tests for CopyTo are non-exhaustive -- just sanity checks, with the assumption that
        // the underlying implementation calls Array.CopyTo(...).
        [Fact]
        public static void CopyTo4_NonDefaultInstance_CopiesToDestination()
        {
            ReadOnlyArray<byte> instance = new byte[] { 3, 1, 2 };
            byte[] destination = new byte[instance.Length + 2];

            instance.CopyTo(1, destination, 2, 2);

            byte[] expectedDestination = new byte[] { 0, 0, instance[1], instance[2], 0 };
            Assert.Equal<byte>(expectedDestination, destination);
        }

        [Fact]
        public static void IndexOf_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance.IndexOf(0));
        }

        // The tests for IndexOf are non-exhaustive -- just sanity checks, with the assumption that
        // the underlying implementation calls Array.IndexOf(...).
        [Theory]
        [InlineData(new byte[] { 1, 1 }, (byte)1, 0)]
        [InlineData(new byte[] { 1, 1 }, (byte)0, -1)]
        public static void IndexOf_NonDefaulInstance_ReturnsExpectedIndex(byte[] array, byte item, int expectedIndex)
        {
            ReadOnlyArray<byte> instance = array;

            int index = instance.IndexOf(item);

            Assert.Equal(expectedIndex, index);
        }

        [Fact]
        public static void IndexOf2_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance.IndexOf(0, 0));
        }

        // The tests for IndexOf are non-exhaustive -- just sanity checks, with the assumption that
        // the underlying implementation calls Array.IndexOf(...).
        [Theory]
        [InlineData(new byte[] { 1, 1 }, (byte)1, 0, 0)]
        [InlineData(new byte[] { 1, 1 }, (byte)1, 1, 1)]
        [InlineData(new byte[] { 1, 0 }, (byte)1, 1, -1)]
        public static void IndexOf2_NonDefaulInstance_ReturnsExpectedIndex(byte[] array, byte item, int startIndex, int expectedIndex)
        {
            ReadOnlyArray<byte> instance = array;

            int index = instance.IndexOf(item, startIndex);

            Assert.Equal(expectedIndex, index);
        }

        [Fact]
        public static void IndexOf3_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance.IndexOf(0, 0, 0));
        }

        // The tests for IndexOf are non-exhaustive -- just sanity checks, with the assumption that
        // the underlying implementation calls Array.IndexOf(...).
        [Theory]
        [InlineData(new byte[] { 1, 1 }, (byte)1, 0, 2, 0)]
        [InlineData(new byte[] { 1, 1 }, (byte)1, 1, 1, 1)]
        [InlineData(new byte[] { 1, 0 }, (byte)1, 1, 1, -1)]
        [InlineData(new byte[] { 0, 1 }, (byte)1, 0, 1, -1)]
        public static void IndexOf3_NonDefaulInstance_ReturnsExpectedIndex(byte[] array, byte item, int startIndex, int count, int expectedIndex)
        {
            ReadOnlyArray<byte> instance = array;

            int index = instance.IndexOf(item, startIndex, count);

            Assert.Equal(expectedIndex, index);
        }

        [Fact]
        public static void LastIndexOf_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance.LastIndexOf(0));
        }

        // The tests for LastIndexOf are non-exhaustive -- just sanity checks, with the assumption
        // that the underlying implementation calls Array.LastIndexOf(...).
        [Theory]
        [InlineData(new byte[] { 1, 1 }, (byte)1, 1)]
        [InlineData(new byte[] { 1, 1 }, (byte)0, -1)]
        public static void LastIndexOf_NonDefaulInstance_ReturnsExpectedIndex(byte[] array, byte item, int expectedIndex)
        {
            ReadOnlyArray<byte> instance = array;

            int index = instance.LastIndexOf(item);

            Assert.Equal(expectedIndex, index);
        }

        [Fact]
        public static void LastIndexOf2_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance.LastIndexOf(0, 0));
        }

        // The tests for LastIndexOf are non-exhaustive -- just sanity checks, with the assumption that
        // the underlying implementation calls Array.LastIndexOf(...).
        [Theory]
        [InlineData(new byte[] { 1, 1 }, (byte)1, 0, 0)]
        [InlineData(new byte[] { 1, 1 }, (byte)1, 1, 1)]
        [InlineData(new byte[] { 0, 1 }, (byte)1, 0, -1)]
        public static void LastIndexOf2_NonDefaulInstance_ReturnsExpectedIndex(byte[] array, byte item, int startIndex, int expectedIndex)
        {
            ReadOnlyArray<byte> instance = array;

            int index = instance.LastIndexOf(item, startIndex);

            Assert.Equal(expectedIndex, index);
        }

        [Fact]
        public static void LastIndexOf3_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance.LastIndexOf(0, 0, 0));
        }

        // The tests for LastIndexOf are non-exhaustive -- just sanity checks, with the assumption that
        // the underlying implementation calls Array.LastIndexOf(...).
        [Theory]
        [InlineData(new byte[] { 1, 1 }, (byte)1, 0, 1, 0)]
        [InlineData(new byte[] { 1, 1 }, (byte)1, 1, 2, 1)]
        [InlineData(new byte[] { 0, 1 }, (byte)1, 0, 1, -1)]
        [InlineData(new byte[] { 1, 0 }, (byte)1, 1, 1, -1)]
        public static void LastIndexOf3_NonDefaulInstance_ReturnsExpectedIndex(byte[] array, byte item, int startIndex, int count, int expectedIndex)
        {
            ReadOnlyArray<byte> instance = array;

            int index = instance.LastIndexOf(item, startIndex, count);

            Assert.Equal(expectedIndex, index);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void Equals_InstanceSameArray_ReturnsSameValueAsArrayEquals(byte[]? array)
        {
            ReadOnlyArray<byte> instance1 = array;
            ReadOnlyArray<byte> instance2 = array;

            bool arrayEqualsArray = array is null || array.Equals(array);

            Assert.Equal(arrayEqualsArray, ((object)instance1).Equals(instance2));
            Assert.Equal(arrayEqualsArray, ((object)instance2).Equals(instance1));
        }

        [Theory]
        [InlineData(null, new byte[0])]
        [InlineData(new byte[0], new byte[0])]
        [InlineData(new byte[0], new byte[] { 3, 1, 2 })]
        [InlineData(new byte[] { 3, 1, 2 }, new byte[] { 3, 1, 2 })]
        public static void Equals_InstanceDifferentArray_ReturnsSameValueAsArrayEquals(byte[]? array1, byte[]? array2)
        {
            ReadOnlyArray<byte> instance1 = array1;
            ReadOnlyArray<byte> instance2 = array2;

            bool array1EqualsArray2 = array1 is null ? array2 is null : array1.Equals(array2);
            bool array2EqualsArray1 = array2 is null ? array1 is null : array2.Equals(array1);

            Assert.Equal(array1EqualsArray2, ((object)instance1).Equals(instance2));
            Assert.Equal(array2EqualsArray1, ((object)instance2).Equals(instance1));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void Equals_SameArray_ReturnsFalse(byte[]? array)
        {
            ReadOnlyArray<byte> instance1 = array;

            Assert.False(((object)instance1).Equals(array));
        }

        [Theory]
        [InlineData(null, new byte[0])]
        [InlineData(new byte[0], new byte[0])]
        [InlineData(new byte[0], new byte[] { 3, 1, 2 })]
        [InlineData(new byte[] { 3, 1, 2 }, new byte[] { 3, 1, 2 })]
        public static void Equals_DifferentArray_ReturnsFalse(byte[]? array1, byte[]? array2)
        {
            ReadOnlyArray<byte> instance1 = array1;

            Assert.False(((object)instance1).Equals(array2));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void IEquatableEquals_InstanceSameArray_ReturnsSameValueAsArrayEquals(byte[]? array)
        {
            ReadOnlyArray<byte> instance1 = array;
            ReadOnlyArray<byte> instance2 = array;

            bool arrayEqualsArray = array is null || array.Equals(array);

            Assert.Equal(arrayEqualsArray, instance1.Equals(instance2));
            Assert.Equal(arrayEqualsArray, instance2.Equals(instance1));
        }

        [Theory]
        [InlineData(null, new byte[0])]
        [InlineData(new byte[0], new byte[0])]
        [InlineData(new byte[0], new byte[] { 3, 1, 2 })]
        [InlineData(new byte[] { 3, 1, 2 }, new byte[] { 3, 1, 2 })]
        public static void IEquatableEquals_InstanceDifferentArrays_ReturnsSameValueAsArrayEquals(byte[]? array1, byte[]? array2)
        {
            ReadOnlyArray<byte> instance1 = array1;
            ReadOnlyArray<byte> instance2 = array2;

            bool array1EqualsArray2 = array1 is null ? array2 is null : array1.Equals(array2);
            bool array2EqualsArray1 = array2 is null ? array1 is null : array2.Equals(array1);

            Assert.Equal(array1EqualsArray2, instance1.Equals(instance2));
            Assert.Equal(array2EqualsArray1, instance2.Equals(instance1));
        }

        [Fact]
        public static void GetHashCode_DefaultInstance_ReturnsZero()
        {
            ReadOnlyArray<byte> instance = null;

            int result = instance.GetHashCode();

            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void GetHashCode_NonDefaultInstance_ReturnsSameValueAsArrayGetHashCode(byte[] array)
        {
            ReadOnlyArray<byte> instance = array;

            int result = instance.GetHashCode();

            int expectedResult = array.GetHashCode();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public static void GetEnumerator_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance.GetEnumerator());
        }

        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void GetEnumerator_NonDefaultInstance_ReturnsEnumeratorWithSameItemsAsArray(byte[] array)
        {
            ReadOnlyArray<byte> instance = array;

            var instanceEnumerator = instance.GetEnumerator();

            var arrayEnumerator = array.GetEnumerator();
            while (arrayEnumerator.MoveNext())
            {
                Assert.True(instanceEnumerator.MoveNext());
                Assert.Equal(arrayEnumerator.Current, instanceEnumerator.Current);
            }
            Assert.False(instanceEnumerator.MoveNext());
        }

        [Fact]
        public static void ToImmutableArray_DefaultInstance_ThrowsNullReferenceException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<NullReferenceException>(() => instance.ToImmutableArray());
        }

        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void ToImmutableArray_NonDefaultInstance_ReturnsImmutableArrayWithSameItemsAsArray(byte[] array)
        {
            ReadOnlyArray<byte> instance = array;

            var immutableArray = instance.ToImmutableArray();

            Assert.Equal<byte>(instance, immutableArray);
        }

        [Fact]
        public static void IEnumerableT_GetEnumerator_DefaultInstance_ThrowsInvalidOperationException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<InvalidOperationException>(() => ((IEnumerable<byte>)instance).GetEnumerator());
        }

        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void IEnumerableT_GetEnumerator_NonDefaultInstance_ReturnsEnumeratorWithSameItemsAsArray(byte[] array)
        {
            ReadOnlyArray<byte> instance = array;

            var instanceEnumerator = ((IEnumerable<byte>)instance).GetEnumerator();

            var arrayEnumerator = array.GetEnumerator();
            while (arrayEnumerator.MoveNext())
            {
                Assert.True(instanceEnumerator.MoveNext());
                Assert.Equal(arrayEnumerator.Current, instanceEnumerator.Current);
            }
            Assert.False(instanceEnumerator.MoveNext());
        }

        [Fact]
        public static void IEnumerable_GetEnumerator_DefaultInstance_ThrowsInvalidOperationException()
        {
            ReadOnlyArray<byte> instance = null;

            _ = Assert.Throws<InvalidOperationException>(() => ((IEnumerable)instance).GetEnumerator());
        }

        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void IEnumerable_GetEnumerator_NonDefaultInstance_ReturnsEnumeratorWithSameItemsAsArray(byte[] array)
        {
            ReadOnlyArray<byte> instance = array;

            var instanceEnumerator = ((IEnumerable)instance).GetEnumerator();

            var arrayEnumerator = array.GetEnumerator();
            while (arrayEnumerator.MoveNext())
            {
                Assert.True(instanceEnumerator.MoveNext());
                Assert.Equal(arrayEnumerator.Current, instanceEnumerator.Current);
            }
            Assert.False(instanceEnumerator.MoveNext());
        }

        [Theory]
        [InlineData(null, 0)]
        [InlineData(new byte[0], -1)]
        [InlineData(new byte[] { 3, 1, 2 }, -1)]
        public static void IStructuralComparable_CompareTo_DefaultInstanceAndInstanceSameElementType_ReturnsValueWithExpectedSign(byte[]? array, int expectedSign)
        {
            IStructuralComparable_CompareTo_ReturnsValueWithExpectedSign<byte, byte>(null, array, expectedSign);
        }

        [Theory]
        [InlineData(null, 0)]
        [InlineData(new int[0], -1)]
        [InlineData(new int[] { 3, 1, 2 }, -1)]
        public static void IStructuralComparable_CompareTo_DefaultInstanceAndInstanceDifferentElementType_ReturnsValueWithExpectedSign(int[]? array, int expectedSign)
        {
            IStructuralComparable_CompareTo_ReturnsValueWithExpectedSign<byte, int>(null, array, expectedSign);
        }

        [Theory]
        [InlineData(null, 0)]
        [InlineData(new byte[0], 1)]
        [InlineData(new byte[] { 3, 1, 2 }, 1)]
        public static void IStructuralComparable_CompareTo_InstanceAndDefaultInstanceSameElementType_ReturnsValueWithExpectedSign(byte[]? array, int expectedSign)
        {
            IStructuralComparable_CompareTo_ReturnsValueWithExpectedSign<byte, byte>(array, null, expectedSign);
        }

        [Theory]
        [InlineData(null, 0)]
        [InlineData(new int[0], 1)]
        [InlineData(new int[] { 3, 1, 2 }, 1)]
        public static void IStructuralComparable_CompareTo_InstanceAndDefaultInstanceDifferentElementType_ReturnsValueWithExpectedSign(int[] array, int expectedSign)
        {
            IStructuralComparable_CompareTo_ReturnsValueWithExpectedSign<int, byte>(array, null, expectedSign);
        }

        [Theory]
        [InlineData(new byte[0], new byte[0], 0)]
        [InlineData(new byte[] { 3, 1, 2 }, new byte[] { 3, 1, 2 }, 0)]
        [InlineData(new byte[] { 1, 1 }, new byte[] { 1, 2 }, -1)]
        [InlineData(new byte[] { 1, 2 }, new byte[] { 1, 1 }, 1)]
        [InlineData(new byte[] { 1, 1 }, new byte[] { 2, 1 }, -1)]
        [InlineData(new byte[] { 2, 1 }, new byte[] { 1, 1 }, 1)]
        public static void IStructuralComparable_CompareTo_NonDefaultInstancesSameElementTypeSameNumberOfElements_ReturnsValueWithExpectedSign(byte[] array1, byte[] array2, int expectedSign)
        {
            IStructuralComparable_CompareTo_ReturnsValueWithExpectedSign(array1, array2, expectedSign);
        }

        [Theory]
        [InlineData(new byte[0], new int[0], 0)]
        [InlineData(new byte[] { 3, 1, 2 }, new int[] { 3, 1, 2 }, 0)]
        [InlineData(new byte[] { 1, 1 }, new int[] { 1, 2 }, -1)]
        [InlineData(new byte[] { 1, 2 }, new int[] { 1, 1 }, 1)]
        [InlineData(new byte[] { 1, 1 }, new int[] { 2, 1 }, -1)]
        [InlineData(new byte[] { 2, 1 }, new int[] { 1, 1 }, 1)]
        public static void IStructuralComparable_CompareTo_NonDefaultInstancesDifferentElementTypeSameNumberOfElements_ReturnsValueWithExpectedSign(byte[] array1, int[] array2, int expectedSign)
        {
            IStructuralComparable_CompareTo_ReturnsValueWithExpectedSign(array1, array2, expectedSign);
        }

        private static void IStructuralComparable_CompareTo_ReturnsValueWithExpectedSign<T1, T2>(T1[]? array1, T2[]? array2, int expectedSign)
        {
            ReadOnlyArray<T1> instance1 = array1;
            ReadOnlyArray<T2> instance2 = array2;
            var comparer = Comparer<object>.Create((x, y) => ((int)Convert.ChangeType(x, TypeCode.Int32)).CompareTo((int)Convert.ChangeType(y, TypeCode.Int32)));

            int result = ((IStructuralComparable)instance1).CompareTo(instance2, comparer);

            Assert.Equal(expectedSign, Sign(result));

            static int Sign(int value) => value < 0 ? -1 : value > 0 ? 1 : 0;
        }

        [Theory]
        [InlineData(new byte[0], new byte[] { 3, 1, 2 })]
        [InlineData(new byte[] { 3, 1, 2 }, new byte[0])]
        public static void IStructuralComparable_CompareTo_NonDefaultInstancesSameElementTypeDifferentNumberOfElements_ThrowsArgumentException(byte[] array1, byte[] array2)
        {
            ReadOnlyArray<byte> instance1 = array1;
            ReadOnlyArray<byte> instance2 = array2;
            var comparer = Comparer<byte>.Default;

            _ = Assert.Throws<ArgumentException>(() => ((IStructuralComparable)instance1).CompareTo(instance2, comparer));
        }

        [Theory]
        [InlineData(new byte[0], new int[] { 3, 1, 2 })]
        [InlineData(new byte[] { 3, 1, 2 }, new int[0])]
        public static void IStructuralComparable_CompareTo_NonDefaultInstancesDifferentElementTypeDifferentNumberOfElements_ThrowsArgumentException(byte[] array1, int[] array2)
        {
            ReadOnlyArray<byte> instance1 = array1;
            ReadOnlyArray<int> instance2 = array2;
            var comparer = Comparer<object>.Create((x, y) => ((int)Convert.ChangeType(x, TypeCode.Int32)).CompareTo((int)Convert.ChangeType(y, TypeCode.Int32)));

            _ = Assert.Throws<ArgumentException>(() => ((IStructuralComparable)instance1).CompareTo(instance2, comparer));
        }

        [Theory]
        [InlineData(null, new byte[0])]
        [InlineData(new byte[0], new byte[0])]
        [InlineData(new byte[0], new byte[] { 3, 1, 2 })]
        [InlineData(new byte[] { 3, 1, 2 }, new byte[0])]
        public static void IStructuralComparable_CompareTo_ArraySameElementType_ThrowsArgumentException(byte[]? array1, byte[] array2)
        {
            ReadOnlyArray<byte> instance1 = array1;
            var comparer = Comparer<object>.Create((x, y) => ((int)Convert.ChangeType(x, TypeCode.Int32)).CompareTo((int)Convert.ChangeType(y, TypeCode.Int32)));

            _ = Assert.Throws<ArgumentException>(() => ((IStructuralComparable)instance1).CompareTo(array2, comparer));
        }

        [Theory]
        [InlineData(null, new int[0])]
        [InlineData(new byte[0], new int[0])]
        [InlineData(new byte[0], new int[] { 3, 1, 2 })]
        [InlineData(new byte[] { 3, 1, 2 }, new int[0])]
        public static void IStructuralComparable_CompareTo_ArrayDifferentElementType_ThrowsArgumentException(byte[]? array1, int[] array2)
        {
            ReadOnlyArray<byte> instance1 = array1;
            var comparer = Comparer<object>.Create((x, y) => ((int)Convert.ChangeType(x, TypeCode.Int32)).CompareTo((int)Convert.ChangeType(y, TypeCode.Int32)));

            _ = Assert.Throws<ArgumentException>(() => ((IStructuralComparable)instance1).CompareTo(array2, comparer));
        }

        [Theory]
        [InlineData(null, null, true)]
        [InlineData(null, new byte[0], false)]
        [InlineData(new byte[0], null, false)]
        [InlineData(new byte[0], new byte[0], true)]
        [InlineData(new byte[0], new byte[] { 3, 1, 2 }, false)]
        [InlineData(new byte[] { 3, 1, 2 }, new byte[0], false)]
        [InlineData(new byte[] { 3, 1, 2 }, new byte[] { 3, 1, 2 }, true)]
        [InlineData(new byte[] { 1, 1 }, new byte[] { 1, 2 }, false)]
        [InlineData(new byte[] { 1, 2 }, new byte[] { 1, 1 }, false)]
        [InlineData(new byte[] { 1, 1 }, new byte[] { 2, 1 }, false)]
        [InlineData(new byte[] { 2, 1 }, new byte[] { 1, 1 }, false)]
        public static void IStructuralEquatable_Equals_InstanceSameElementType_ReturnsExpectedValue(byte[]? array1, byte[]? array2, bool expectedResult)
        {
            ReadOnlyArray<byte> instance1 = array1;
            ReadOnlyArray<byte> instance2 = array2;
            var equalityComparer = EqualityComparer<byte>.Default;

            bool result = ((IStructuralEquatable)instance1).Equals(instance2, equalityComparer);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, null, true)]
        [InlineData(null, new int[0], false)]
        [InlineData(new byte[0], null, false)]
        [InlineData(new byte[0], new int[0], true)]
        [InlineData(new byte[0], new int[] { 3, 1, 2 }, false)]
        [InlineData(new byte[] { 3, 1, 2 }, new int[0], false)]
        [InlineData(new byte[] { 3, 1, 2 }, new int[] { 3, 1, 2 }, true)]
        [InlineData(new byte[] { 1, 1 }, new int[] { 1, 2 }, false)]
        [InlineData(new byte[] { 1, 2 }, new int[] { 1, 1 }, false)]
        [InlineData(new byte[] { 1, 1 }, new int[] { 2, 1 }, false)]
        [InlineData(new byte[] { 2, 1 }, new int[] { 1, 1 }, false)]
        public static void IStructuralEquatable_Equals_InstanceDifferentElementType_ReturnsExpectedValue(byte[]? array1, int[]? array2, bool expectedResult)
        {
            ReadOnlyArray<byte> instance1 = array1;
            ReadOnlyArray<int> instance2 = array2;
            var equalityComparer = ConvertInt32EqualityComparer.Instance;

            bool result = ((IStructuralEquatable)instance1).Equals(instance2, equalityComparer);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, new byte[0])]
        [InlineData(new byte[0], new byte[0])]
        [InlineData(new byte[0], new byte[] { 3, 1, 2 })]
        [InlineData(new byte[] { 3, 1, 2 }, new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 }, new byte[] { 3, 1, 2 })]
        public static void IStructuralEquatable_Equals_ArraySameElementType_ReturnsFalse(byte[]? array1, byte[]? array2)
        {
            ReadOnlyArray<byte> instance1 = array1;
            var equalityComparer = EqualityComparer<byte>.Default;

            Assert.False(((IStructuralEquatable)instance1).Equals(array2, equalityComparer));
        }

        [Theory]
        [InlineData(null, new int[0])]
        [InlineData(new byte[0], new int[0])]
        [InlineData(new byte[0], new int[] { 3, 1, 2 })]
        [InlineData(new byte[] { 3, 1, 2 }, new int[0])]
        [InlineData(new byte[] { 3, 1, 2 }, new int[] { 3, 1, 2 })]
        public static void IStructuralEquatable_Equals_ArrayDifferentElementType_ReturnsFalse(byte[]? array1, int[]? array2)
        {
            ReadOnlyArray<byte> instance1 = array1;
            var equalityComparer = ConvertInt32EqualityComparer.Instance;

            Assert.False(((IStructuralEquatable)instance1).Equals(array2, equalityComparer));
        }

        [Fact]
        public static void IStructuralEquatable_GetHashCode_DefaultInstance_ReturnsZero()
        {
            ReadOnlyArray<byte> instance = null;
            var equalityComparer = EqualityComparer<byte>.Default;

            int result = ((IStructuralEquatable)instance).GetHashCode(equalityComparer);

            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 3, 1, 2 })]
        public static void IStructuralEquatable_GetHashCode_NonDefaultInstance_ReturnsArrayGetHashCode(byte[] array)
        {
            ReadOnlyArray<byte> instance = array;
            var equalityComparer = EqualityComparer<byte>.Default;

            int result = ((IStructuralEquatable)instance).GetHashCode(equalityComparer);

            int expectedResult = ((IStructuralEquatable)array).GetHashCode(equalityComparer);
            Assert.Equal(expectedResult, result);
        }

        public static readonly TheoryData<Derived[]> NonNullableDerivedArray = new TheoryData<Derived[]>
        {
            new Derived[] { new Derived() },
        };

        public static readonly TheoryData<Derived?[]> NullableDerivedArray = new TheoryData<Derived?[]>
        {
            new Derived?[] { null },
            new Derived[] { new Derived() },
        };

        public class Base
        {
        }

        public static readonly TheoryData<Base[]> NonNullableBaseArray = new TheoryData<Base[]>
        {
            new Base[] { new Base() },
            new Base[] { new Derived() },
            new Derived[] { new Derived() },
        };

        public static readonly TheoryData<Base?[]> NullableBaseArray = new TheoryData<Base?[]>
        {
            new Base?[] { null },
            new Base[] { new Base() },
            new Base[] { new Derived() },
            new Derived?[] { null },
            new Derived[] { new Derived() },
        };

        public class Derived : Base
        {
        }

        public static readonly TheoryData<Other[]> NonNullableOtherArray = new TheoryData<Other[]>
        {
            new Other[] { new Other() },
        };

        public static readonly TheoryData<Other?[]> NullableOtherArray = new TheoryData<Other?[]>
        {
            new Other?[] { null },
            new Other[] { new Other() },
        };

        public class Other
        {
        }

        private sealed class ConvertInt32EqualityComparer : IEqualityComparer
        {
            public static ConvertInt32EqualityComparer Instance { get; } = new ConvertInt32EqualityComparer();

            private ConvertInt32EqualityComparer()
            {
            }

            public new bool Equals(object? x, object? y)
            {
                return !(x is null) && !(y is null) && ((int)Convert.ChangeType(x, TypeCode.Int32)).Equals((int)Convert.ChangeType(y, TypeCode.Int32));
            }

            public int GetHashCode(object obj)
            {
                return (obj is null) ? 0 : ((int)Convert.ChangeType(obj, TypeCode.Int32)).GetHashCode();
            }
        }
    }
}
