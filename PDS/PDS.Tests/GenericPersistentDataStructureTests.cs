using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PDS.Collections;
using PDS.Implementation.Collections;
using PDS.Implementation.UndoRedo;

namespace PDS.Tests
{
    [TestFixture]
    public class GenericPersistentDataStructureTests
    {
        [Test(Description = "Test PersistentLinkedList implementation")]
        public void ImplementationPersistentLinkedListTest()
        {
            var structure = new PersistentLinkedList<int>();

            PersistentDataStructureTest<IPersistentLinkedList<int>>(structure);
            PersistentDataStructureWithStackTest(structure);
        }
        
        [Test(Description = "Test PersistentList implementation")]
        public void ImplementationPersistentListTest()
        {
            var structure = new PersistentList<int>();

            PersistentDataStructureTest(structure);
        }
        
        [Test(Description = "Test PersistentSet implementation")]
        public void ImplementationPersistentSetTest()
        {
            var structure = new PersistentSet<int>();

            PersistentDataStructureTest(structure);
            PersistentDataStructureWithSetTest(structure);
        }
        
        [Test(Description = "Test PersistentStack implementation")]
        public void ImplementationPersistentStackTest()
        {
            var structure = new PersistentStack<int>();

            PersistentDataStructureTest(structure);
            PersistentDataStructureWithStackTest(structure);
        }
        
        [Test(Description = "Test UndoRedoLinkedList implementation")]
        public void ImplementationUndoRedoLinkedListTest()
        {
            var structure = new UndoRedoLinkedList<int>();

            PersistentDataStructureTest<IPersistentLinkedList<int>>(structure);
            PersistentDataStructureWithStackTest(structure);
        }
        
        [Test(Description = "Test UndoRedoList implementation")]
        public void ImplementationUndoRedoListTest()
        {
            var structure = new UndoRedoList<int>();

            PersistentDataStructureTest<IPersistentList<int>>(structure);
        }
        
        [Test(Description = "Test UndoRedoSet implementation")]
        public void ImplementationUndoRedoSetTest()
        {
            var structure = new UndoRedoSet<int>();

            PersistentDataStructureTest<IPersistentSet<int>>(structure);
            PersistentDataStructureWithSetTest(structure);
        }
        
        [Test(Description = "Test UndoRedoStack implementation")]
        public void ImplementationUndoRedoStackTest()
        {
            var structure = new UndoRedoStack<int>();

            PersistentDataStructureTest<IPersistentStack<int>>(structure);
            PersistentDataStructureWithStackTest(structure);
        }

        private void PersistentDataStructureTest<T>(IPersistentDataStructure<int, T> a) where T: IPersistentDataStructure<int, T>
        {
            a.IsEmpty.Should().BeTrue();

            var b = a.Add(0);
            b.Count.Should().Be(1);
            b.IsEmpty.Should().BeFalse();

            var c = a.AddRange(Enumerable.Range(0, 5));
            c.Count.Should().Be(5);
            
            var d = a.AddRange(Enumerable.Range(0, 5).ToArray());
            d.Count.Should().Be(5);
        }
        
        private void PersistentDataStructureWithSetTest(IPersistentDataStructure<int, IPersistentSet<int>> a)
        {
            a.IsEmpty.Should().BeTrue();

            var b = a.Add(0);
            b.Count.Should().Be(1);
            b.IsEmpty.Should().BeFalse();

            var c = a.AddRange(Enumerable.Range(0, 5));
            c.Count.Should().Be(5);
            
            var d = a.AddRange(Enumerable.Range(0, 5).ToArray());
            d.Count.Should().Be(5);
        }
        
        private void PersistentDataStructureWithStackTest(IPersistentDataStructure<int, IPersistentStack<int>> a)
        {
            a.IsEmpty.Should().BeTrue();

            var b = a.Add(0);
            b.Count.Should().Be(1);
            b.IsEmpty.Should().BeFalse();

            var c = a.AddRange(Enumerable.Range(0, 5));
            c.Count.Should().Be(5);
            
            var d = a.AddRange(Enumerable.Range(0, 5).ToArray());
            d.Count.Should().Be(5);

            var e = d.Clear();
            e.Count.Should().Be(0);
        }
    }
}