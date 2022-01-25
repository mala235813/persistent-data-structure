using System;
using System.Collections.Immutable;
using FluentAssertions;
using NUnit.Framework;
using PDS.Implementation.Collections;
using PDS.Implementation.UndoRedo;

namespace PDS.Tests
{
    [TestFixture]
    public class GenericImmutableQueueTests
    {
        private static Type[] _genericImmutableQueueTypes = { typeof(PersistentLinkedList<>), typeof(UndoRedoLinkedList<>) };

        [Test(Description = "Test IImmutableQueue implementation")]
        [TestCaseSource(nameof(_genericImmutableQueueTypes))]
        public void ImplementationIImmutableQueueTest(Type stackType)
        {
            var classType = stackType.MakeGenericType(typeof(int));
            var queue = (IImmutableQueue<int>)Activator.CreateInstance(classType)!;

            queue.IsEmpty.Should().BeTrue();
            var q2 = queue.Enqueue(0);
            q2.IsEmpty.Should().BeFalse();

            var q3 = q2.Clear();
            q3.IsEmpty.Should().BeTrue();

            q2.Peek().Should().Be(0);

            var q4 = q2.Dequeue(out var b);
            q4.IsEmpty.Should().BeTrue();
            b.Should().Be(0);
        }
    }
}