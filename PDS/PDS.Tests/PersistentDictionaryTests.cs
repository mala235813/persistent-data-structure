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
    public class PersistentDictionaryTests
    {
        [Test]
        public void PersistentDictionaryTest()
        {
            var dict = new PersistentDictionary<int, int>();
            dict.Count.Should().Be(0);
            dict.IsEmpty.Should().BeTrue();
            dict.Keys.Should().BeEmpty();
            dict.Values.Should().BeEmpty();

            var a = dict.Set(0, 50);
            a.Count.Should().Be(1);

            a.TryGetValue(0, out var val1).Should().BeTrue();
            val1.Should().Be(50);

            var negDict = a.Set(-1, 111);
            negDict.Contains(-1).Should().BeTrue();

            a.TryGetValue(49, out _).Should().BeFalse();

            a.GetByKey(0).Should().Be(50);
            a[0].Should().Be(50);

            Action getNonExistentKey = () => a.GetByKey(2);
            getNonExistentKey.Should().Throw<KeyNotFoundException>();

            a.Contains(0).Should().BeTrue();
            a.Contains(2).Should().BeFalse();
            a.Contains(new KeyValuePair<int, int> (0, 50) ).Should().BeTrue();
            a.Contains(new KeyValuePair<int, int> (2, 50) ).Should().BeFalse();

            a.TryAdd(0, 1, out var na).Should().BeFalse();
            a.Should().BeSameAs(na);
            a.TryAdd(1, 3, out var ba).Should().BeTrue();
            ba.Count.Should().Be(a.Count + 1);

            var ub = ba.Update(1, ((k, v) => k + v + 10));
            ub.Count.Should().Be(ba.Count);
            ub[1].Should().Be(14);

            var d2 = a;
            for (int i = 0; i < 100; ++i)
            {
                d2 = d2.Set(i, i * 2);
            }

            d2.Count.Should().Be(100);

            var d4 = a.SetItems(Enumerable.Range(0, 5)
                .Select(x => new KeyValuePair<int, int>(x, x + 10)));
            d4.Count.Should().Be(5);
            
            var d5 = a.SetItems(Enumerable.Range(0, 5)
                .Select(x => new KeyValuePair<int, int>(x, x + 10)).ToArray());
            d5.Count.Should().Be(5);

            d5.TryRemove(6, out _).Should().BeFalse();
            d5.TryRemove(0, out var d6).Should().BeTrue();
            d6.Count.Should().Be(4);
            d6.Keys.Count().Should().Be(4);
            d6.Values.Count().Should().Be(4);
            (d6 as IEnumerable).GetEnumerator().MoveNext().Should().BeTrue();

            Action removeNonExistentKey = () => d5.Remove(50);
            removeNonExistentKey.Should().Throw<ArgumentException>();

            var d7 = d5.Remove(0);
            d7.Count.Should().Be(4);

            var d8 = d5.RemoveRange(Enumerable.Range(0, 2));
            d8.Count.Should().Be(3);

            d8.TryGetKey(3, out var key).Should().BeTrue();
            key.Should().Be(3);
            
            d8.TryGetKey(10, out var key2).Should().BeFalse();
            key2.Should().Be(10);
            
            var d9 = a.AddRange(Enumerable.Range(0, 5)
                .Select(x => new KeyValuePair<int, int>(x, x + 10)));
            d9.Count.Should().Be(5);
            
            var d10 = a.AddRange(Enumerable.Range(0, 5)
                .Select(x => new KeyValuePair<int, int>(x, x + 10)).ToArray());
            d10.Count.Should().Be(5);

            d10.AsEnumerable().Count().Should().Be(5);
            
        }
    }
}