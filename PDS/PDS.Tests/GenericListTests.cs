using System;
using System.Collections.Immutable;
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
    public class GenericListTests
    {
        private static Type[] _genericListTypes = {typeof(PersistentList<>), typeof(UndoRedoList<>)};
        private static Type[] _genericUndoRedoListTestTypes = {typeof(UndoRedoList<>)};

        [Test(Description = "Test IPersistentLinkedList implementation")]
        [TestCaseSource(nameof(_genericListTypes))]
        public void PersistentListImplementationTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var list = (IPersistentList<int>) Activator.CreateInstance(classType)!;

            PersistentListTest(list);
        }

        [Test(Description = "Test IImmutableList implementation")]
        [TestCaseSource(nameof(_genericListTypes))]
        public void ImmutableListImplementationTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var list = (IPersistentList<int>) Activator.CreateInstance(classType)!;

            ImmutableListTest(list);
        }
        
        [Test(Description = "Test IUndoRedoLinkedList implementation")]
        [TestCaseSource(nameof(_genericUndoRedoListTestTypes))]
        public void ImplementationUndoRedoListTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var list = (IUndoRedoList<int>) Activator.CreateInstance(classType)!;

            UndoRedoListTest(list);
        }

        private static void PersistentListTest(IPersistentList<int> a)
        {
            a.IsEmpty.Should().BeTrue();

            var b = a.Add(1);
            b.Count.Should().Be(1);
            b[0].Should().Be(1);

            const int size = 256;
            var c = Enumerable.Range(0, size).Aggregate(b, (current, i) => current.Add(i));
            var d = b.AddRange(Enumerable.Range(0, size));

            c.Count.Should().Be(257);
            d.Count.Should().Be(257);

            c.Should().BeEquivalentTo(d, opt => opt.WithStrictOrdering());

            Action removeAt0 = () => a.RemoveAt(0);
            removeAt0.Should().Throw<ArgumentOutOfRangeException>();
            
            Action removeAtNegativePos = () => a.RemoveAt(-1);
            removeAtNegativePos.Should().Throw<ArgumentOutOfRangeException>();

            var e = c.RemoveAt(0);
            var j = c.RemoveAt(c.Count - 1);
            e.Count.Should().Be(j.Count);
            
            var f = Enumerable.Range(0, 2048).Aggregate(a, (current, i) => current.Add(i));
            f.Count.Should().Be(2048);
            f.Should().BeEquivalentTo(Enumerable.Range(0, 2048), opt => opt.WithStrictOrdering());
            f[1].Should().Be(1);

            // ReSharper disable once NotAccessedVariable
            int aa;
            Action getAtNegativePos = () => aa = a[-1];
            getAtNegativePos.Should().Throw<ArgumentOutOfRangeException>();
            
            Action setAtNegativePos = () => a.SetItem(-1, 0);
            setAtNegativePos.Should().Throw<ArgumentOutOfRangeException>();

            var k = f.SetItem(1, 1000);
            k[1].Should().Be(1000);

            var l = f.SetItem(f.Count, 333);
            l[f.Count].Should().Be(333);

            var o = l.SetItem(f.Count, 111);
            o[f.Count].Should().Be(111);
        }

        private static void ImmutableListTest(IImmutableList<int> a)
        {
            a.Count.Should().Be(0);

            var b = a.Add(1);
            b.Count.Should().Be(1);
            b[0].Should().Be(1);

            const int size = 256;
            var c = Enumerable.Range(0, size).Aggregate(b, (current, i) => current.Add(i));
            var d = b.AddRange(Enumerable.Range(0, size));

            c.Count.Should().Be(257);
            d.Count.Should().Be(257);

            c.Should().BeEquivalentTo(d, opt => opt.WithStrictOrdering());

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action removeAt0 = () => a.RemoveAt(0);
            removeAt0.Should().Throw<ArgumentOutOfRangeException>();
            
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action removeAtNegativePos = () => a.RemoveAt(-1);
            removeAtNegativePos.Should().Throw<ArgumentOutOfRangeException>();

            var e = c.RemoveAt(0);
            var j = c.RemoveAt(c.Count - 1);
            e.Count.Should().Be(j.Count);
            
            var f = Enumerable.Range(0, 2048).Aggregate(a, (current, i) => current.Add(i));
            f.Count.Should().Be(2048);
            f.Should().BeEquivalentTo(Enumerable.Range(0, 2048), opt => opt.WithStrictOrdering());
            f[1].Should().Be(1);

            // ReSharper disable once NotAccessedVariable
            int aa;
            Action getAtNegativePos = () => aa = a[-1];
            getAtNegativePos.Should().Throw<ArgumentOutOfRangeException>();

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action setAtNegativePos = () => a.SetItem(-1, 0);
            setAtNegativePos.Should().Throw<ArgumentOutOfRangeException>();

            var k = f.SetItem(1, 1000);
            k[1].Should().Be(1000);

            var l = f.SetItem(f.Count, 333);
            l[f.Count].Should().Be(333);

            var o = l.SetItem(f.Count, 111);
            o[f.Count].Should().Be(111);
        }
        
        private void UndoRedoListTest(IUndoRedoList<int> a)
        {
            a.Count.Should().Be(0);
            a.CanRedo.Should().BeFalse();
            a.CanUndo.Should().BeFalse();

            var b = a.Add(1);
            b.Count.Should().Be(1);
            b[0].Should().Be(1);
            b.CanRedo.Should().BeFalse();
            b.CanUndo.Should().BeTrue();

            var undoB = b.Undo();
            undoB.CanRedo.Should().BeTrue();
            undoB.Count.Should().Be(0);
            var redoB = undoB.Redo();
            redoB.Count.Should().Be(1);
            redoB.CanUndo.Should().BeTrue();

            Action redoExc = () => a.Redo();
            redoExc.Should().Throw<InvalidOperationException>();
            
            Action undoExc = () => a.Undo();
            undoExc.Should().Throw<InvalidOperationException>();

            a.TryRedo(out var newA).Should().BeFalse();
            newA.Should().BeSameAs(a);
            
            a.TryUndo(out var uA).Should().BeFalse();
            uA.Should().BeSameAs(a);

            const int size = 256;
            var c = Enumerable.Range(0, size).Aggregate(b, (current, i) => current.Add(i));
            var d = b.AddRange(Enumerable.Range(0, size));

            c.Count.Should().Be(257);
            d.Count.Should().Be(257);

            c.Should().BeEquivalentTo(d, opt => opt.WithStrictOrdering());

            c.TryUndo(out var uC).Should().BeTrue();
            uC.Count.Should().Be(c.Count - 1);
            uC.CanUndo.Should().BeTrue();
            
            uC.TryRedo(out var newC).Should().BeTrue();
            newC.Count.Should().Be(c.Count);
            newC.Should().BeEquivalentTo(c, opt => opt.WithStrictOrdering());

            Action removeAt0 = () => a.RemoveAt(0);
            removeAt0.Should().Throw<ArgumentOutOfRangeException>();
            
            Action removeAtNegativePos = () => a.RemoveAt(-1);
            removeAtNegativePos.Should().Throw<ArgumentOutOfRangeException>();

            var e = c.RemoveAt(0);
            var j = c.RemoveAt(c.Count - 1);
            e.Count.Should().Be(j.Count);
            
            var f = Enumerable.Range(0, 2048).Aggregate(a, (current, i) => current.Add(i));
            f.Count.Should().Be(2048);
            f.Should().BeEquivalentTo(Enumerable.Range(0, 2048), opt => opt.WithStrictOrdering());
            f[1].Should().Be(1);

            // ReSharper disable once NotAccessedVariable
            int aa;
            Action getAtNegativePos = () => aa = a[-1];
            getAtNegativePos.Should().Throw<ArgumentOutOfRangeException>();
            
            Action setAtNegativePos = () => a.SetItem(-1, 0);
            setAtNegativePos.Should().Throw<ArgumentOutOfRangeException>();

            var k = f.SetItem(1, 1000);
            k[1].Should().Be(1000);

            var l = f.SetItem(f.Count, 333);
            l[f.Count].Should().Be(333);

            var o = l.SetItem(f.Count, 111);
            o[f.Count].Should().Be(111);
        }
    }
}