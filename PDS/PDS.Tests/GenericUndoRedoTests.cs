using System;
using FluentAssertions;
using NUnit.Framework;
using PDS.Implementation.UndoRedo;
using PDS.UndoRedo;

namespace PDS.Tests
{
    [TestFixture]
    public class GenericUndoRedoTests
    {
        [Test(Description = "Test UndoRedoSet implementation")]
        public void ImplementationUndoRedoSetTest()
        {
            var undoRedoStructure = new UndoRedoSet<int>();

            UndoRedoTest(undoRedoStructure);
        }
        
        [Test(Description = "Test UndoRedoList implementation")]
        public void ImplementationUndoRedoListTest()
        {
            var undoRedoStructure = new UndoRedoList<int>();

            UndoRedoTest(undoRedoStructure);
        }
        
        [Test(Description = "Test UndoRedoLinkedList implementation")]
        public void ImplementationUndoRedoLinkedListTest()
        {
            var undoRedoStructure = new UndoRedoLinkedList<int>();

            UndoRedoTest<IUndoRedoLinkedList<int>>(undoRedoStructure);
            UndoRedoTest<IUndoRedoStack<int>>(undoRedoStructure);
        }
        
        [Test(Description = "Test UndoRedoLinkedSet implementation")]
        public void ImplementationUndoRedoStackTest()
        {
            var undoRedoStructure = new UndoRedoStack<int>();

            UndoRedoTest(undoRedoStructure);
        }

        private void UndoRedoTest<T>(IUndoRedoDataStructure<int, T> structure) where T : IUndoRedoDataStructure<int, T>
        {
            structure.CanRedo.Should().BeFalse();
            structure.CanUndo.Should().BeFalse();
            
            structure.TryRedo(out _).Should().BeFalse();
            structure.TryUndo(out _).Should().BeFalse();

            Action redo = () => structure.Redo();
            redo.Should().Throw<InvalidOperationException>().WithMessage("Redo stack is empty");

            Action undo = () => structure.Undo();
            undo.Should().Throw<InvalidOperationException>().WithMessage("Undo stack is empty");

            structure = structure.Add(55);

            structure.CanUndo.Should().BeTrue();
            structure.TryUndo(out var str1).Should().BeTrue();
            str1.CanRedo.Should().BeTrue();
            str1.CanUndo.Should().BeFalse();
            
            var undoed = structure.Undo();
            undoed.CanRedo.Should().BeTrue();

            undoed.TryRedo(out var str2).Should().BeTrue();
            str2.CanRedo.Should().BeFalse();
            str2.CanUndo.Should().BeTrue();

            var redoed = undoed.Redo();
            redoed.CanUndo.Should().BeTrue();
            redoed.CanRedo.Should().BeFalse();
        }
    }
}