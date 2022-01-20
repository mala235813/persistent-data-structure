using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NUnit.Framework;
using PDS.Implementation.Collections;

namespace PDS.Tests;

[TestFixture]
public class PersistentStackTests
{
    [Test]
    public void PersistentStack_PushPopTest_IsCorrect()
    {
        var a = PersistentStack<int>.Empty;
        
        Assert.That(a.Count, Is.EqualTo(0));
        Assert.That(a.IsEmpty);

        var b = a.Push(0);
 
        Assert.That(a.Count, Is.EqualTo(0));
        Assert.That(a.IsEmpty);
        Assert.That(b.Count, Is.EqualTo(1));
        Assert.That(b.IsEmpty, Is.False);
        Assert.That(b.Peek(), Is.EqualTo(0));

        var c = b.Pop();
        
        Assert.That(b.Count, Is.EqualTo(1));
        Assert.That(b.IsEmpty, Is.False);
        Assert.That(b.Peek(), Is.EqualTo(0));
        Assert.That(c.Count, Is.EqualTo(0));
        Assert.That(c.IsEmpty);

        var d = b.Push(1);
        
        Assert.That(b.Count, Is.EqualTo(1));
        Assert.That(b.IsEmpty, Is.False);
        Assert.That(b.Peek(), Is.EqualTo(0));
        Assert.That(d.Count, Is.EqualTo(2));
        Assert.That(d.IsEmpty, Is.False);
        Assert.That(d.Peek(), Is.EqualTo(1));
    }

    [Test]
    public void PersistentStack_AsImmutableStack_IsCorrect()
    {
        IImmutableStack<int> a = PersistentStack<int>.Empty;
        
        Assert.That(a.IsEmpty);

        var b = a.Push(0);
 
        Assert.That(a.IsEmpty);
        Assert.That(b.IsEmpty, Is.False);
        Assert.That(b.Peek(), Is.EqualTo(0));

        var c = b.Pop();
        
        Assert.That(b.IsEmpty, Is.False);
        Assert.That(b.Peek(), Is.EqualTo(0));
        Assert.That(c.IsEmpty);

        var d = b.Push(1);
        
        Assert.That(b.IsEmpty, Is.False);
        Assert.That(b.Peek(), Is.EqualTo(0));

        Assert.That(d.IsEmpty, Is.False);
        Assert.That(d.Peek(), Is.EqualTo(1));
        
        Assert.That(d.Clear().IsEmpty);
    }
    
    [Test]
    public void PersistentStack_ClearAddRangeTest_IsCorrect()
    {
        var array = Enumerable.Range(1, 5).ToArray();
        var stack = PersistentStack<int>.Empty.AddRange(array);
        
        Assert.That(stack.Count, Is.EqualTo(5));
        Assert.That(stack.Peek(), Is.EqualTo(5));
        
        CollectionAssert.AreEqual(array.Reverse(), stack);
        
        Assert.That(stack.Add(6).Peek(), Is.EqualTo(6));
        Assert.That(stack.Clear(), Is.Empty);
    }
}