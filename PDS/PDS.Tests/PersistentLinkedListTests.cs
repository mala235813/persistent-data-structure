using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PDS.Implementation.Collections;

namespace PDS.Tests
{
    public class Tests
    {
        [Test]
        public void PersistentLinkedListTests()
        {
            var list = new PersistentLinkedList<int>();

            var a = list.PushBack(0);
            var b = a.Set(0, 1);
            var c = b.PushBack(2);
            var d = c.RemoveAt(1);

            Assert.That(list.Count, Is.EqualTo(0));
            Assert.That(a.Get(0), Is.EqualTo(0));
            Assert.That(b.Get(0), Is.EqualTo(1));
            Assert.That(c.Count, Is.EqualTo(2));
            Assert.That(c.Get(0), Is.EqualTo(1));
            Assert.That(c.Get(1), Is.EqualTo(2));
            Assert.That(d.Count, Is.EqualTo(1));
            Assert.That(d.Get(0), Is.EqualTo(1));

            var e = list.Insert(0, 2);
            e.First.Should().Be(2);
            e.Last.Should().Be(2);
            e.Count.Should().Be(1);

            e.AsEnumerable().Count().Should().Be(1);

            var x = list.AddRange(Enumerable.Range(0, 10)).SetItem(1, 5);
            x.Get(1).Should().Be(5);

            var x2 = x.RemoveAt(1);
            x2.Count.Should().Be(9);

            var x3 = x2.Insert(2, 99);
            x3.Get(2).Should().Be(99);

            var x4 = x3.Insert(x3.Count, 55);
            x4.Last.Should().Be(55);

            Action insertOutOfRange = () => x3.Insert(-1, 55);
            insertOutOfRange.Should().Throw<IndexOutOfRangeException>();
            Action insertOutOfRange2 = () => x3.Insert(-1, 155);
            insertOutOfRange2.Should().Throw<IndexOutOfRangeException>();
        }
    }
}