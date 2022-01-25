using System;
using System.Collections.Immutable;
using FluentAssertions;
using NUnit.Framework;
using PDS.Collections;
using PDS.Implementation.Collections;
using PDS.Implementation.UndoRedo;
using PDS.UndoRedo;

namespace PDS.Tests
{
    [TestFixture]
    public class GenericImmutableStackTests
    {
        private static Type[] _genericTestTypes =
        {
            typeof(PersistentStack<>), typeof(PersistentLinkedList<>), typeof(UndoRedoStack<>),
            typeof(UndoRedoLinkedList<>)
        };

        private static Type[] _genericStackTestTypes = {typeof(PersistentStack<>), typeof(UndoRedoStack<>)};
        private static Type[] _undoRedoTestTypes = {typeof(UndoRedoStack<>), typeof(UndoRedoLinkedList<>)};

        [Test(Description = "Test IImmutableStack implementation")]
        [TestCaseSource(nameof(_genericTestTypes))]
        public void ImplementationIImmutableStackTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var stack = (IImmutableStack<int>) Activator.CreateInstance(classType)!;

            ImmutableStackTest(stack);
        }

        [Test(Description = "Test IPersistentStack implementation")]
        [TestCaseSource(nameof(_genericTestTypes))]
        public void ImplementationIPersistentStackTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var stack = (IPersistentStack<int>) Activator.CreateInstance(classType)!;

            PersistentStackTests(stack);
        }

        [Test(Description = "Test IUndoRedoStack implementation")]
        [TestCaseSource(nameof(_undoRedoTestTypes))]
        public void ImplementationIUndoRedoStackTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var stack = (IUndoRedoStack<int>) Activator.CreateInstance(classType)!;

            UndoRedoStackTest(stack);
        }

        [Test(Description = "Test IUndoRedoStack implementation")]
        [TestCaseSource(nameof(_genericStackTestTypes))]
        public void ImplementationIStackTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var stack = (IPersistentStack<int>) Activator.CreateInstance(classType)!;

            PeekPopStackTest(stack);
        }

        private static void ImmutableStackTest(IImmutableStack<int> a)
        {
            a.IsEmpty.Should().BeTrue();

            var b = a.Push(0);

            a.IsEmpty.Should().BeTrue();
            b.IsEmpty.Should().BeFalse();
            b.Peek().Should().Be(0);

            var c = b.Pop();

            b.IsEmpty.Should().BeFalse();
            b.Peek().Should().Be(0);
            c.IsEmpty.Should().BeTrue();

            var d = b.Push(1);

            b.IsEmpty.Should().BeFalse();
            b.Peek().Should().Be(0);

            d.IsEmpty.Should().BeFalse();
            d.Peek().Should().Be(1);

            d.Clear().IsEmpty.Should().BeTrue();
        }

        private static void PersistentStackTests(IPersistentStack<int> a)
        {
            a.IsEmpty.Should().BeTrue();
            a.Count.Should().Be(0);

            var b = a.Push(0);

            b.Count.Should().Be(1);

            a.IsEmpty.Should().BeTrue();
            b.IsEmpty.Should().BeFalse();
            b.Peek().Should().Be(0);

            var c = b.Pop();

            b.IsEmpty.Should().BeFalse();
            b.Peek().Should().Be(0);
            c.IsEmpty.Should().BeTrue();

            var d = b.Push(1);

            b.IsEmpty.Should().BeFalse();
            b.Peek().Should().Be(0);

            d.IsEmpty.Should().BeFalse();
            d.Peek().Should().Be(1);

            d.Clear().IsEmpty.Should().BeTrue();
        }

        private void UndoRedoStackTest(IUndoRedoStack<int> a)
        {
            a.IsEmpty.Should().Be(true);
            a.Count.Should().Be(0);

            a.CanRedo.Should().BeFalse();
            a.CanUndo.Should().BeFalse();

            Action undo = () => a.Undo();
            undo.Should().Throw<InvalidOperationException>().WithMessage("Undo stack is empty");

            Action redo = () => a.Redo();
            redo.Should().Throw<InvalidOperationException>().WithMessage("Redo stack is empty");

            var b = a.Push(0);
            b.CanUndo.Should().BeTrue();

            var undoB = b.Undo();
            undoB.CanRedo.Should().BeTrue();

            var redoUndoB = undoB.Redo();
            redoUndoB.CanUndo.Should().BeTrue();
            redoUndoB.CanRedo.Should().BeFalse();

            b.Count.Should().Be(1);

            a.IsEmpty.Should().Be(true);
            b.IsEmpty.Should().Be(false);
            b.Peek().Should().Be(0);

            var c = b.Pop();

            b.IsEmpty.Should().Be(false);
            b.Peek().Should().Be(0);
            c.IsEmpty.Should().Be(true);

            var d = b.Push(1);

            b.IsEmpty.Should().Be(false);
            b.Peek().Should().Be(0);

            d.IsEmpty.Should().Be(false);
            d.Peek().Should().Be(1);

            d.Clear().IsEmpty.Should().Be(true);
        }

        private static void PeekPopStackTest(IPersistentStack<int> a)
        {
            a.IsEmpty.Should().Be(true);
            a.Count.Should().Be(0);

            Action peek = () => a.Peek();
            peek.Should().Throw<InvalidOperationException>().WithMessage("Empty stack");

            Action pop = () => a.Pop();
            pop.Should().Throw<InvalidOperationException>().WithMessage("Empty stack");
        }
    }
}