using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PDS.Implementation.Collections;

namespace PDS.Playground
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var listA = new PersistentList<int>();
            var listB = listA.Add(15);
            var e = listB[0];
            Debug.Assert(e == 15);
            var listC = listB.Set(0, 33);
            Debug.Assert(listB[0] == 15);
            Debug.Assert(listC[0] == 33);


            var dictA = new PersistentDictionary<int, string>();
            var dictB = dictA.Set(15, "B");
            var dictC = dictA.Set(15, "C");
            Debug.Assert(dictB[15] != dictC[15]);
            var dictD = dictC.SetItems(new[]
                {new KeyValuePair<int, string>(15, "D"), new KeyValuePair<int, string>(87, "A")});
            Debug.Assert(dictD.Count == 2);
            
            var setA = new PersistentSet<string>();
            var setB = setA.Add("aadad");
            var setC = setB.Add("Cadada");
            Debug.Assert(setC.Count == 2);
            var setD = setC.Clear();
            Debug.Assert(setC.Count == 2);
            Debug.Assert(setD.IsEmpty);

            var llA = new PersistentLinkedList<int>();
            var llB = llA.AddLast(15);
            var llC = llB.AddFirst(71);
            Debug.Assert(llB.First != llC.FirstOrDefault());
            var llD = llC.Insert(1, 1000);
            Debug.Assert(llD.First == llC.First && llD.Last == llC.Last);

            var stackA = new PersistentStack<char>();
            var stackB = stackA.Push('a');
            Debug.Assert(stackA.IsEmpty);
            var stackC = stackB.Push('d');
            Debug.Assert(stackC.Peek() == 'd' && stackB.Peek() == 'a');
         }
    }
}