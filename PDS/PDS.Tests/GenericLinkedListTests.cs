using System;
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
        private static Type[] _genericLinkedListTypes = { typeof(PersistentLinkedList<>), typeof(UndoRedoLinkedList<>) };
        private static Type[] _genericUndoRedoLinkedListTestTypes = { typeof(UndoRedoLinkedList<>) };

        [Test(Description = "Test IPersistentLinkedList implementation")]
        [TestCaseSource(nameof(_genericLinkedListTypes))]
        public void ImplementationIImmutableStackTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var stack = (IPersistentLinkedList<int>)Activator.CreateInstance(classType)!;

            PersistentLinkedListTest(stack);
        }
        
        [Test(Description = "Test IUndoRedoLinkedList implementation")]
        [TestCaseSource(nameof(_genericUndoRedoLinkedListTestTypes))]
        public void ImplementationIPersistentStackTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var stack = (IUndoRedoLinkedList<int>)Activator.CreateInstance(classType)!;

            UndoRedoLinkedListTest(stack);
        }

        private static void PersistentLinkedListTest(IPersistentLinkedList<int> a)
        {
            a.IsEmpty.Should().BeTrue();

            Action removeAt0 = () => a.RemoveAt(0);
            removeAt0.Should().Throw<ArgumentOutOfRangeException>();

            Action removeAtNegativePos = () => a.RemoveAt(-1);
            removeAtNegativePos.Should().Throw<ArgumentOutOfRangeException>();

            Func<int> first = () => a.First;
            first.Should().Throw<InvalidOperationException>().WithMessage("Unreachable version");
            
            Func<int> last = () => a.Last;
            last.Should().Throw<InvalidOperationException>().WithMessage("Unreachable version");
        }
        
        private static void UndoRedoLinkedListTest(IUndoRedoLinkedList<int> a)
        {
            Action removeAt0 = () => a.RemoveAt(0);
            removeAt0.Should().Throw<ArgumentOutOfRangeException>();

            Action removeAtNegativePos = () => a.RemoveAt(-1);
            removeAtNegativePos.Should().Throw<ArgumentOutOfRangeException>();

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
            
            var f2 = a.AddRange(Enumerable.Range(0, 1).ToArray());
            f2.Count.Should().Be(1);

            f.Get(0).Should().Be(0);

            var g = a.AddFirst(0);
            g.Count.Should().Be(1);

            var g2 = g.AddFirst(1);
            g2.Count.Should().Be(2);
            
            var h = a.AddLast(0);
            h.Count.Should().Be(1);

            var j = h.SetItem(0, 5);
            j.First.Should().Be(5);

            Action setItemNegative = () => h.SetItem(-1, 2);
            setItemNegative.Should().Throw<ArgumentOutOfRangeException>();
            
            Action setItemOutOfRange = () => h.SetItem(2, 2);
            setItemOutOfRange.Should().Throw<ArgumentOutOfRangeException>();

            var ala = a.AddRange(Enumerable.Range(0, 100));
            var ala1 = ala.SetItem(0, 5);
            ala1.First.Should().Be(5);
            var ala2 = ala.RemoveAt(0);
            ala2.Count.Should().Be(99);
        }
    }
}