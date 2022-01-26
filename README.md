# Persistent data structures [![build](https://github.com/6gales/persistent-data-structure/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/6gales/persistent-data-structure/actions/workflows/dotnet.yml) [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=6gales_persistent-data-structure&metric=bugs)](https://sonarcloud.io/summary/new_code?id=6gales_persistent-data-structure) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=6gales_persistent-data-structure&metric=coverage)](https://sonarcloud.io/summary/new_code?id=6gales_persistent-data-structure) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=6gales_persistent-data-structure&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=6gales_persistent-data-structure) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=6gales_persistent-data-structure&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=6gales_persistent-data-structure) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=6gales_persistent-data-structure&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=6gales_persistent-data-structure)
C# library that implements persistent array, double linked list, dictionary and more

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
We are comparing our implementation of two sequentional collections: PersistentList and PersistentLinkedList with common C# immutable collections, [ImmutableTreeList](https://github.com/tunnelvisionlabs/dotnet-trees) and [ImmutableLinkedList](https://github.com/madelson/ImmutableLinkedList)

### Create collection from range
First test is adding range of elements into the empty collection.
![First bench results](/resourses/first.png)
ImmutableList, ImmutableArray, ImmutableTreeList, ImmutableLinkedList, PersistentLinkedList, PersistentList

|               Method |   Count |         Mean |       Error |     StdDev | Ratio | RatioSD |
|--------------------- |-------- |-------------:|------------:|-----------:|------:|--------:|
|        ImmutableList | 1000000 |    97.607 ms |  26.5226 ms |  1.4538 ms |  1.00 |    0.00 |
|       ImmutableArray | 1000000 |     1.416 ms |   0.3453 ms |  0.0189 ms |  0.01 |    0.00 |
|    ImmutableTreeList | 1000000 |    69.035 ms |  17.8368 ms |  0.9777 ms |  0.71 |    0.01 |
|  ImmutableLinkedList | 1000000 |    61.346 ms |  11.1700 ms |  0.6123 ms |  0.63 |    0.01 |
| PersistentLinkedList | 1000000 | 1,118.175 ms | 186.6299 ms | 10.2298 ms | 11.46 |    0.27 |
|       PersistentList | 1000000 |    40.586 ms |   1.4044 ms |  0.0770 ms |  0.42 |    0.01 |

### Add items one by one
Second test is same adding, but this time we enforces collections to add items one by one.
![Second bench results](/resourses/second.png)
ImmutableList, ImmutableArray, ImmutableTreeList, ImmutableLinkedList, PersistentLinkedList, PersistentList

|               Method |  Count |         Mean |         Error |       StdDev |    Ratio | RatioSD |
|--------------------- |------- |-------------:|--------------:|-------------:|---------:|--------:|
|        ImmutableList | 100000 |     49.86 ms |     20.387 ms |     1.117 ms |     1.00 |    0.00 |
|       ImmutableArray | 100000 |  2,084.60 ms |  1,024.015 ms |    56.130 ms |    41.83 |    1.84 |
|    ImmutableTreeList | 100000 |     67.44 ms |      6.518 ms |     0.357 ms |     1.35 |    0.04 |
|  ImmutableLinkedList | 100000 | 70,299.05 ms | 20,021.392 ms | 1,097.440 ms | 1,410.46 |   41.46 |
| PersistentLinkedList | 100000 |     95.66 ms |     55.313 ms |     3.032 ms |     1.92 |    0.07 |
|       PersistentList | 100000 |     10.33 ms |     21.709 ms |     1.190 ms |     0.21 |    0.02 |

## Authors
 - [Andrey Pleshkov](https://github.com/6gales)
 - [Artyom Lyamin](https://github.com/YGAR84)