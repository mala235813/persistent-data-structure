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
        public static Type[] GenericTestTypes = { typeof(PersistentStack<>), typeof(PersistentLinkedList<>), typeof(UndoRedoStack<>), typeof(UndoRedoLinkedList<>) };
        public static Type[] UndoRedoTestTypes = { typeof(UndoRedoStack<>), typeof(UndoRedoLinkedList<>) }; 
        
        [Test(Description = "Test IImmutableStack implementation")]
        [TestCaseSource(nameof(GenericTestTypes))]
        public void ImplementationIImmutableStackTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var stack = (IImmutableStack<int>)Activator.CreateInstance(classType)!;

            ImmutableStackTest(stack);
        }
        
        [Test(Description = "Test IPersistentStack implementation")]
        [TestCaseSource(nameof(GenericTestTypes))]
        public void ImplementationIPersistentStackTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var stack = (IPersistentStack<int>)Activator.CreateInstance(classType)!;

            IPersistentStackTests(stack);
        }
        
        [Test(Description = "Test IUndoRedoStack implementation")]
        [TestCaseSource(nameof(UndoRedoTestTypes))]
        public void ImplementationIUndoRedoStackTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var stack = (IUndoRedoStack<int>)Activator.CreateInstance(classType)!;

            IUndoRedoStackTest(stack);
        }
        
        private void ImmutableStackTest(IImmutableStack<int> a)
        {
            a.IsEmpty.Should().Be(true);
            var b = a.Push(0);

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
        
        private void IPersistentStackTests(IPersistentStack<int> a)
        {
            a.IsEmpty.Should().Be(true);
            a.Count.Should().Be(0);
            
            var b = a.Push(0);

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
        
        private void IUndoRedoStackTest(IUndoRedoStack<int> a)
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
    }
}