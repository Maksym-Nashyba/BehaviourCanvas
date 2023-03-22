using System.Collections.Generic;
using System.Linq;

namespace BehaviourCanvas.Code.Runtime
{
    public static class LINQExtensions
    {
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> sequence)
        {
            return sequence.Where(e => e != null);
        }
    }
}