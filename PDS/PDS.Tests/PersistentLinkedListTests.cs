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
        }
    }
}