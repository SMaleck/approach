﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace _Source.Util
{
    public static class EnumHelper<T>
    {
        private static IEnumerable<T> _enumerable;

        public static IEnumerable<T> Iterator => _enumerable ?? (_enumerable = Enum.GetValues(typeof(T)).Cast<T>());

        public static int Length => Iterator.ToList().Count;

        public static IEnumerable<T> IteratorExcept(params T[] valuesToExclude)
        {
            return Iterator.Where(x => !valuesToExclude.Contains(x)).ToList();
        }
    }
}