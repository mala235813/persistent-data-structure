# Persistent data structures 

[![build](https://github.com/6gales/persistent-data-structure/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/6gales/persistent-data-structure/actions/workflows/dotnet.yml) [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=6gales_persistent-data-structure&metric=bugs)](https://sonarcloud.io/summary/new_code?id=6gales_persistent-data-structure) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=6gales_persistent-data-structure&metric=coverage)](https://sonarcloud.io/summary/new_code?id=6gales_persistent-data-structure) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=6gales_persistent-data-structure&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=6gales_persistent-data-structure) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=6gales_persistent-data-structure&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=6gales_persistent-data-structure) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=6gales_persistent-data-structure&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=6gales_persistent-data-structure)

C# library that implements persistent array, double linked list, dictionary and more

| Package with interfaces     | [![NuGet](https://img.shields.io/nuget/v/PDS.svg)](https://www.nuget.org/packages/PDS/)                               |
| Package with implementation | [![NuGet](https://img.shields.io/nuget/v/PDS.Implementation.svg)](https://www.nuget.org/packages/PDS.Implementation/) |

The fundamental benefit of persistence is that it makes it significantly easier to reason about how the code works, enabling you to quickly write correct and elegant code. Consider a single-threaded program. At a given line of code, you may be wondering about the state of an persistent collection. You can easily determine that by locating those locations in code where the collection was created. There’s usually only one or few such locations. By continuing this process, you’ll get to either a mutable source or the empty collection. It doesn’t matter if the immutable collection has been passed to methods because its structure is guaranteed to be preserved, so you don’t have to consider what happens in those methods.
The main goal of persistent data structures is to write efficient code free from side effects.

## Using examples
### List
List has O(1) random access complexity, path-copying and tail optimiztion.
```cs
var listA = new PersistentList<int>();
var listB = listA.Add(15);
var e = listB[0];
Debug.Assert(e == 15);
var listC = listB.Set(0, 33);
Debug.Assert(listB[0] == 15);
Debug.Assert(listC[0] == 33);
```
### Dictionary
Dictionary built using PersistentList.
```cs
var dictA = new PersistentDictionary<int, string>();
var dictB = dictA.Set(15, "B");
var dictC = dictA.Set(15, "C");
Debug.Assert(dictB[15] != dictC[15]);
var dictD = dictC.SetItems(new[]{new KeyValuePair<int, string>(15, "D"), new KeyValuePair<int, string>(87, "A")});
Debug.Assert(dictD.Count == 2);
```
### Set
Hash set is also built using PersistentList.
```cs
var setA = new PersistentSet<string>();
var setB = setA.Add("aadad");
var setC = setB.Add("Cadada");
Debug.Assert(setC.Count == 2);
var setD = setC.Clear();
Debug.Assert(setC.Count == 2);
Debug.Assert(setD.IsEmpty);
```
### Linked list
PersistentLinkedList uses both path-copying and fat-node approaches to optimize version access and new version creation.
```cs
var llA = new PersistentLinkedList<int>();
var llB = llA.AddLast(15);
var llC = llB.AddFirst(71);
Debug.Assert(llB.First != llC.FirstOrDefault());
var llD = llC.Insert(1, 1000);
Debug.Assert(llD.First == llC.First && llD.Last == llC.Last);
```
### Stack
Simple yet efficient way if we need only one side of collection.
```cs
var stackA = new PersistentStack<char>();
var stackB = stackA.Push('a');
Debug.Assert(stackA.IsEmpty);
var stackC = stackB.Push('d');
Debug.Assert(stackC.Peek() == 'd' && stackB.Peek() == 'a');
```

All examples can be found in PDS.Playground project.<br />
![Demo](/resourses/demo.gif)

## Undo-redo
All collections also have implementation of undo-redo mechanic to rollback changes:
* Undo returns us to state before last modifying operation.
* Redo returns us to state before last undo.
* Any modifying operation clears redo stack.

## Benchmarks
Official stands for System.Collections.Immutable<br />
Tunnel is [ImmutableTreeList from this project](https://github.com/tunnelvisionlabs/dotnet-trees)<br />
Atropos is [previous year nsu course project](https://github.com/evilguest/atropos)<br />
Pds is this project

### Create collection from range
First test is adding range of elements into the empty collection.
![Add range speed](/resourses/addrange.png)
Memory allocation during test:
![Add range alloc](/resourses/addrange.alloc.png)

### Add items one by one
Second test is same adding, but this time we enforces collections to add items one by one.
![Add int speed](/resourses/addint.png)
Memory allocation during test:
![Add int alloc](/resourses/addint.alloc.png)

## Authors
 - [Andrey Pleshkov](https://github.com/6gales)
 - [Artyom Lyamin](https://github.com/YGAR84)