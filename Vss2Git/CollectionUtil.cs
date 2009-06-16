/* Copyright 2009 HPDI, LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Hpdi.Vss2Git
{
    /// <summary>
    /// A set of utility methods for operating on collections.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public static class CollectionUtil
    {
        public static bool IsEmpty(IEnumerable items)
        {
            var itemCollection = items as ICollection;
            if (itemCollection != null)
            {
                return itemCollection.Count == 0;
            }
            else
            {
                return !items.GetEnumerator().MoveNext();
            }
        }

        public static int CountItems(IEnumerable items)
        {
            var itemCollection = items as ICollection;
            if (itemCollection != null)
            {
                return itemCollection.Count;
            }
            else
            {
                int count = 0;
                foreach (var item in items)
                {
                    ++count;
                }
                return count;
            }
        }

        public delegate T TransformFunction<F, T>(F obj);

        public static IEnumerable<T> Transform<F, T>(IEnumerable<F> items, TransformFunction<F, T> func)
        {
            foreach (var item in items)
            {
                yield return func(item);
            }
        }

        public static string Join(string separator, IEnumerable items)
        {
            var buf = new StringBuilder();
            Join(buf, separator, items);
            return buf.ToString();
        }

        public static void Join(StringBuilder buf, string separator, IEnumerable items)
        {
            var first = true;
            foreach (var item in items)
            {
                if (!first)
                {
                    buf.Append(separator);
                }
                else
                {
                    first = false;
                }
                buf.Append(item);
            }
        }
    }
}
