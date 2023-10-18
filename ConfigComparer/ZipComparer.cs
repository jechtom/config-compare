using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigComparer
{
    /// <summary>
    /// Compares two streams of ordered items
    /// </summary>
    internal class ZipComparer
    {
        public ZipComparer(IComparer<string> keyComparer, IComparer<string> valueComparer)
        {
            KeyComparer = keyComparer ?? throw new ArgumentNullException(nameof(keyComparer));
            ValueComparer = valueComparer ?? throw new ArgumentNullException(nameof(valueComparer));
        }

        public IComparer<string> KeyComparer { get; }
        public IComparer<string> ValueComparer { get; }

        public IEnumerable<CompareLineResult> Compare(IEnumerable<KeyValuePair<string,string>> left, IEnumerable<KeyValuePair<string, string>> right)
        {
            using var enumLeft = left.GetEnumerator();
            using var enumRight = right.GetEnumerator();

            bool hasAnyLeft = true;
            bool hasAnyRight = true;

            bool skipLeft = false;
            bool skipRight = false;

            while (true)
            {
                if(!skipLeft) hasAnyLeft = hasAnyLeft & enumLeft.MoveNext();
                if (!skipRight) hasAnyRight = hasAnyRight & enumRight.MoveNext();

                skipLeft = false;
                skipRight = false;

                if (!hasAnyLeft && !hasAnyRight)
                {
                    // no more items
                    break;
                }

                if(!hasAnyLeft)
                {
                    // items only in right
                    yield return new CompareLineResult.RightOnly(enumRight.Current);
                    continue;
                }

                if (!hasAnyRight)
                {
                    // items only in left
                    yield return new CompareLineResult.LeftOnly(enumLeft.Current);
                    continue;
                }

                // items in both - compare keys
                var leftCurrent = enumLeft.Current;
                var rightCurrent = enumRight.Current;

                int keyCompareResult = KeyComparer.Compare(leftCurrent.Key, rightCurrent.Key);
                if (keyCompareResult < 0)
                {
                    // left key before right key
                    yield return new CompareLineResult.LeftOnly(leftCurrent);
                    skipRight = true;
                    continue;
                }

                if(keyCompareResult > 0)
                {
                    // right key before left key
                    yield return new CompareLineResult.RightOnly(rightCurrent);
                    skipLeft = true;
                    continue;
                }

                // keys are equal - compare values
                yield return ValueComparer.Compare(leftCurrent.Value, rightCurrent.Value) switch
                {
                    0 => new CompareLineResult.Same(leftCurrent, rightCurrent),
                    _ => new CompareLineResult.Different(leftCurrent, rightCurrent),
                };
            };
        }
    }
}
