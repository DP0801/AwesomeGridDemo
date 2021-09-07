using System;
using System.Collections.Generic;

namespace AwesomeMvcDemo.Utils
{
    public static class DemoUtils
    {
        public static string MealsUrl = "~/Content/Pictures/Meals/";
        public static string FoodUrl = "~/Content/Pictures/Food/";

        public static void TryError()
        {
            var random = new Random();
            if (random.Next(10) > 5)
            {
                throw new Exception("a demo exception has occurred");
            }
        }

        public static void Error()
        {
            throw new Exception("a demo exception has occurred");
        }

        public static string Encode(object input)
        {
            return Autil.JsonEncode(input);
        }

        public static string RndName()
        {
            var names = new[] { "hi there", "orange", "asdfa", "peas", "cabbage" };
            return names[new Random().Next(names.Length)];
        }

        public static string DetailNstBtn = "<div class='detailnst'><i class='caretc'><i class='o-caret o-cl2'></i></i> .(Id)</div>";

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}