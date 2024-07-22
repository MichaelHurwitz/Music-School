using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_School_Sql.Service
{
    internal static class PracticeXmlService
    {
        public static Func<List<string>, bool> startWithA = (list) =>
            list.Any(str => str.StartsWith("a"));

        public static Func<List<string>, bool> isEmptyString = (list) =>
            list.Any(str => string.IsNullOrEmpty(str));

        public static Func<List<string>, bool> containsA = (list) =>
            list.All(str => str.Contains("a"));

        public static Func<List<string>, List<string>> strListUpper = (list) =>
            list.Select(str => str.ToUpper()).ToList();

        public static Func<List<string>, List<string>> strListUpperLinq = (list) =>
            (from str in list select str.ToUpper()).ToList();

        public static Func<List<string>, List<string>> longerThen3 = (list) =>
            list.Where(x => x.Length > 3).ToList();
        
        public static Func<List<string>, List<string>> longerThen3Query = (list) =>
            (from str in list where str.Length > 3 select str).ToList();

        public static Func<List<string>, string> StringifyList = (list) =>
            list.Aggregate(string.Empty, (acc, n) => $"{acc} {n}");

        public static Func<List<string>, int> SumLengths = (list) =>
            list.Aggregate(0, (acc, n) => acc + n.Length);

        public static Func<List<string>, List<string>> WhereAbbove3 = (list) =>
            list.Aggregate(new List<string>(), (acc, n) => n.Length > 3 ?[.. acc, n] : acc);

        public static Func<List<string>, List<int>> selectLength = (list) =>
            list.Aggregate(new List<int>(), (acc, n) => [.. acc, n.Length]);

    }
}
