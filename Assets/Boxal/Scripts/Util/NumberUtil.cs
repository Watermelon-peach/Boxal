using UnityEngine;

namespace Boxal.Util
{
    public static class NumberUtil
    {
        public static string FormatNumber(long num)
        {
            if (num >= 1_000_000_000)
                return (num / 1_000_000_000).ToString("0.#") + "B";
            if (num >= 1_000_000)
                return (num / 1_000_000).ToString("0.#") + "M";
            if (num >= 1_000)
                return (num / 1_000).ToString("0.#") + "K";

            return num.ToString("0");
        }
    }

}
