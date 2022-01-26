using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PDS.Implementation.Collections;

namespace PDS.Tests
{
    [TestFixture]
    public class PersistentSetTests
    {
        [Test]
        public void PersistentSetTest()
        {
            var s = new PersistentSet<int>();

            var s2 = s.AddRange(Enumerable.Range(0, 51));
            s2.Count.Should().Be(51);
            
            s2.TryGetValue(50, out var act1).Should().BeTrue();
            act1.Should().Be(50);
            
            s2.TryGetValue(52, out var act2).Should().BeFalse();
            act2.Should().Be(52);
            
            var negS = s2.Add(-1);
            negS.TryGetValue(-1, out _).Should().BeTrue();

            var s3 = s2.Remove(0);
            s3.Count.Should().Be(50);
            s3.Add(34).Count.Should().Be(50);

            Action removeOutOfRange = () => s2.Remove(52);
            removeOutOfRange.Should().Throw<ArgumentException>();

            ((IImmutableSet<int>)s2).Contains(0).Should().BeTrue();
            ((IImmutableSet<int>)s2).Contains(52).Should().BeFalse();

            ((IReadOnlySet<int>)s2).Contains(0).Should().BeTrue();
            ((IReadOnlySet<int>)s2).Contains(52).Should().BeFalse();

            s2.AsEnumerable().Count().Should().Be(51);
            ((IEnumerable)s2).GetEnumerator().Should().NotBeNull();

            var s5 = s2.Clear();
            s5.Count.Should().Be(0);
            s5.IsEmpty.Should().BeTrue();
        }
    }
}