using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PDS.Collections;
using PDS.Implementation.Collections;
using PDS.Implementation.UndoRedo;
using PDS.UndoRedo;

namespace PDS.Tests
{
    [TestFixture]
    public class GenericLinkedListTests
    {
        public static Type[] GenericLinkedListTypes = { typeof(PersistentLinkedList<>), typeof(UndoRedoLinkedList<>) };
        public static Type[] GenericUndoRedoLinkedListTestTypes = { typeof(UndoRedoLinkedList<>) };

        [Test(Description = "Test IPersistentLinkedList implementation")]
        [TestCaseSource(nameof(GenericLinkedListTypes))]
        public void ImplementationIImmutableStackTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var stack = (IPersistentLinkedList<int>)Activator.CreateInstance(classType)!;

            PersistentLinkedListTest(stack);
        }
        
        [Test(Description = "Test IUndoRedoLinkedList implementation")]
        [TestCaseSource(nameof(GenericUndoRedoLinkedListTestTypes))]
        public void ImplementationIPersistentStackTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var stack = (IUndoRedoLinkedList<int>)Activator.CreateInstance(classType)!;

            UndoRedoLinkedListTest(stack);
        }

        private void PersistentLinkedListTest(IPersistentLinkedList<int> a)
        {
            a.IsEmpty.Should().BeTrue();

            Action removeAt0 = () => a.RemoveAt(0);
            removeAt0.Should().Throw<IndexOutOfRangeException>();

            Action removeAtNegativePos = () => a.RemoveAt(-1);
            removeAtNegativePos.Should().Throw<IndexOutOfRangeException>();

            Func<int> first = () => a.First;
            first.Should().Throw<InvalidOperationException>().WithMessage("Unreachable version");
            
            Func<int> last = () => a.Last;
            last.Should().Throw<InvalidOperationException>().WithMessage("Unreachable version");
        }
        
        private void UndoRedoLinkedListTest(IUndoRedoLinkedList<int> a)
        {
            Action removeAt0 = () => a.RemoveAt(0);
            removeAt0.Should().Throw<IndexOutOfRangeException>();

            Action removeAtNegativePos = () => a.RemoveAt(-1);
            removeAtNegativePos.Should().Throw<IndexOutOfRangeException>();

            Func<int> first = () => a.First;
            first.Should().Throw<InvalidOperationException>().WithMessage("Unreachable version");
            
            Func<int> last = () => a.Last;
            last.Should().Throw<InvalidOperationException>().WithMessage("Unreachable version");

            var b = a.Add(0);
            b.Count.Should().Be(1);
            b.First.Should().Be(0);
            b.Last.Should().Be(0);

            var c = b.RemoveFirst();
            c.Count.Should().Be(0);

            var d = b.RemoveLast();
            d.Count.Should().Be(0);

            var e = a.Insert(0, 0);
            e.Count.Should().Be(1);

            var f = a.AddRange(Enumerable.Range(0, 1));
            f.Count.Should().Be(1);

            f.Get(0).Should().Be(0);

            var g = a.AddFirst(0);
            g.Count.Should().Be(1);
            
            var h = a.AddLast(0);
            h.Count.Should().Be(1);

            // h.Contains(0).Should().BeTrue();

            var j = h.SetItem(0, 5);
            j.First.Should().Be(5);

            Action setItemNegative = () => h.SetItem(-1, 2);
            setItemNegative.Should().Throw<IndexOutOfRangeException>();
            
            Action setItemOutOfRange = () => h.SetItem(2, 2);
            setItemOutOfRange.Should().Throw<IndexOutOfRangeException>();
        }
    }
}