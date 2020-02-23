# ItsyBitsy.Collections.ReadOnlyArray
A .NET library that provides a read-only array type that is convertable from `T[]` and `ImmutableArray<T>`.

[![Build status](https://ci.appveyor.com/api/projects/status/tavhkffeor3heiep/branch/master?svg=true)](https://ci.appveyor.com/project/carlreinke/itsybitsy-collections-readonlyarray/branch/master) [![Test coverage](https://codecov.io/gh/carlreinke/ItsyBitsy.Collections.ReadOnlyArray/branch/master/graph/badge.svg)](https://codecov.io/gh/carlreinke/ItsyBitsy.Collections.ReadOnlyArray) [![NuGet package](https://img.shields.io/nuget/vpre/ItsyBitsy.Collections.ReadOnlyArray?logo=nuget)](https://www.nuget.org/packages/ItsyBitsy.Collections.ReadOnlyArray/)

## Why not just use `ReadOnlyMemory<T>`/`ReadOnlySpan<T>`?
In most cases, you *should* just use `ReadOnlyMemory<T>`/`ReadOnlySpan<T>`.  However, there are a few scenarios in which you might want to use `ReadOnlyArray<T>` instead:
  1. You are targetting .NET Framework, where `ReadOnlyMemory<T>`/`ReadOnlySpan<T>` is slower than `ReadOnlyArray<T>`, and performance matters.
  2. You need to ensure that the underlying memory is a managed array and not some other kind of storage.
  3. You need to reference the array from a field of a non-`ref` `struct` and don't want to allocate an instance of `ReadOnlyMemory<T>`.
