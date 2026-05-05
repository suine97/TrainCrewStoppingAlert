using System;

namespace TrainCrewStoppingAlert
{
    /// <summary>
    /// Utilityクラス
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// 偶数かどうかを判定する
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsEven(int num)
        {
            return (num % 2 == 0);
        }
    }

    /// <summary>
    /// float型拡張クラス
    /// </summary>
    public static class FloatExtensions
    {
        public static bool IsZero(this float self)
        {
            return self.IsZero(float.Epsilon);
        }

        public static bool IsZero(this float self, float epsilon)
        {
            return Math.Abs(self) < epsilon;
        }
    }
}
