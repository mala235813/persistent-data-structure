using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PDS.Collections;
using PDS.Implementation.UndoRedo;
using PDS.UndoRedo;

namespace PDS.Tests
{
    [TestFixture]
    public class UndoRedoDictionaryTests
    {
        [Test]
        public void UndoRedoDictionaryTest()
        {
            var dict = new UndoRedoDictionary<int, int>();
            dict.Count.Should().Be(0);
            dict.IsEmpty.Should().BeTrue();
            dict.Keys.Should().BeEmpty();
            dict.Values.Should().BeEmpty();

            Action setItemOutOfRange = () => dict.SetItem(-1, 0);
            setItemOutOfRange.Should().Throw<Exception>();

            UndoRedoDictionary<int, int> a = (UndoRedoDictionary<int, int>)dict.SetItem(0, 50);
            a.Count.Should().Be(1);
            a[0].Should().Be(50);

            a.TryGetValue(0, out var val1).Should().BeTrue();
            val1.Should().Be(50);
            
            a.TryGetValue(49, out _).Should().BeFalse();

            // a.GetByKey(0).Should().Be(50);
            //
            // Action getNonExistentKey = () => a.GetByKey(2);
            // getNonExistentKey.Should().Throw<KeyNotFoundException>();

            // a.Contains(0).Should().BeTrue();
            // a.Contains(2).Should().BeFalse();
            a.Contains(new KeyValuePair<int, int> (0, 50) ).Should().BeTrue();
            a.Contains(new KeyValuePair<int, int> (2, 50) ).Should().BeFalse();

            var ca = a as IUndoRedoDictionary<int, int>;
            ca.TryAdd(0, 1, out var na).Should().BeFalse();
            ca.Should().BeSameAs(na);
            ca.TryAdd(1, 3, out var ba).Should().BeTrue();
            ba.Count.Should().Be(a.Count + 1);

            var ub = ba.Update(1, ((k, v) => k + v + 10));
            ub.Count.Should().Be(ba.Count);
            ub[1].Should().Be(14);
            
            var d2 = a;
            for (int i = 0; i < 100; ++i)
            {
                d2 = (UndoRedoDictionary<int, int>)d2.SetItem(i, i * 2);
            }

            d2.Count.Should().Be(100);

            UndoRedoDictionary<int, int> d4 = (UndoRedoDictionary<int, int>)a.SetItems(Enumerable.Range(0, 5)
                .Select(x => new KeyValuePair<int, int>(x, x + 10)));
            d4.Count.Should().Be(5);
            
            UndoRedoDictionary<int, int> d5 = (UndoRedoDictionary<int, int>)a.SetItems(Enumerable.Range(0, 5)
                .Select(x => new KeyValuePair<int, int>(x, x + 10)).ToArray());
            d5.Count.Should().Be(5);

            d5.TryRemove(6, out IUndoRedoDictionary<int, int> _).Should().BeFalse();
            d5.TryRemove(0, out IUndoRedoDictionary<int, int> d6).Should().BeTrue();
            ((UndoRedoDictionary<int, int>)d6).Count.Should().Be(4);
            
            d5.TryRemove(6, out IPersistentDictionary<int, int> _).Should().BeFalse();
            d5.TryRemove(0, out IPersistentDictionary<int, int> d62).Should().BeTrue(); 
            d62.Count.Should().Be(4);

            Action removeNonExistentKey = () => d5.Remove(50);
            removeNonExistentKey.Should().Throw<ArgumentException>();

            UndoRedoDictionary<int, int> d7 = (UndoRedoDictionary<int, int>)d5.Remove(0);
            d7.Count.Should().Be(4);

            UndoRedoDictionary<int, int> d8 = (UndoRedoDictionary<int, int>)d5.RemoveRange(Enumerable.Range(0, 2));
            d8.Count.Should().Be(3);

            d8.TryGetKey(3, out var key).Should().BeTrue();
            key.Should().Be(3);
            
            d8.TryGetKey(10, out var key2).Should().BeFalse();
            key2.Should().Be(10);
            
            UndoRedoDictionary<int, int> d9 = (UndoRedoDictionary<int, int>)a.AddRange(Enumerable.Range(0, 5)
                .Select(x => new KeyValuePair<int, int>(x, x + 10)));
            d9.Count.Should().Be(5);
            
            UndoRedoDictionary<int, int> d10 = (UndoRedoDictionary<int, int>)a.AddRange(Enumerable.Range(0, 5)
                .Select(x => new KeyValuePair<int, int>(x, x + 10)).ToArray());
            d10.Count.Should().Be(5);

            d10.AsEnumerable().Count().Should().Be(5);

            UndoRedoDictionary<int, int> d11 = (UndoRedoDictionary<int, int>)d10.Clear();
            d11.IsEmpty.Should().BeTrue();
        }

        [Test]
        public void TryRemoveAddTest()
        {
            var dict = new UndoRedoDictionary<int, int>();

            var d2 = dict.AddOrUpdate(50, 100);
            d2.Contains(new KeyValuePair<int, int> (50, 100)).Should().BeTrue();

            d2 = d2.AddOrUpdate(50, 150);
            d2.Contains(new KeyValuePair<int, int>(50, 150)).Should().BeTrue();
            d2.Contains(new KeyValuePair<int, int> (50, 100)).Should().BeFalse();
        }
        
        [Test]
        public void UndoRedoDictTest()
        {
            UndoRedoDictionary<int, int> dict = new UndoRedoDictionary<int, int>();
            
            dict.CanRedo.Should().BeFalse();
            dict.CanUndo.Should().BeFalse();
            
            dict.TryRedo(out _).Should().BeFalse();
            dict.TryUndo(out _).Should().BeFalse();

            Action redo = () => dict.Redo();
            redo.Should().Throw<InvalidOperationException>().WithMessage("Redo stack is empty");

            Action undo = () => dict.Undo();
            undo.Should().Throw<InvalidOperationException>().WithMessage("Undo stack is empty");

            dict = (UndoRedoDictionary<int, int>)dict.Add(55, 56);

            dict.CanUndo.Should().BeTrue();
            dict.TryUndo(out var str1).Should().BeTrue();
            str1.CanRedo.Should().BeTrue();
            str1.CanUndo.Should().BeFalse();
            
            var undoed = dict.Undo();
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