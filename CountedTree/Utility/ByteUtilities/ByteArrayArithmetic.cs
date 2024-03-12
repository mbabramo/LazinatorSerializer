using System;
using System.Collections.Generic;

namespace CountedTree.ByteUtilities
{
    public static class ByteArrayArithmetic
    {
        public static byte[] Add(this byte[] A, byte[] B)
        {
            List<byte> array = new List<byte>(A);
            for (int i = 0; i < B.Length; i++)
                array = _add_(array, B[i], i);

            return array.ToArray();
        }
        private static List<byte> _add_(List<byte> A, byte b, int idx = 0, byte rem = 0)
        {
            short sample = 0;
            if (idx < A.Count)
            {
                sample = (short)((short)A[idx] + (short)b);
                A[idx] = (byte)(sample % 256);
                rem = (byte)((sample - A[idx]) % 255);
                if (rem > 0)
                    return _add_(A, (byte)rem, idx + 1);
            }
            else A.Add(b);

            return A;
        }
        
        public static byte[] Subtract(this byte[] A, byte[] B)
        {
            // find which array has a greater value for accurate
            // operation if one knows a better way to find which 
            // array is greater in value, do let me know. 
            // (MyArray.Length is not a good option here because
            // an array {255} - {100 000 000} will not yield a
            // correct answer.)
            int x = A.Length - 1, y = B.Length - 1;
            while (A[x] == 0 && x > -1) { x--; }
            while (B[y] == 0 && y > -1) { y--; }
            bool flag;
            if (x == y) flag = (A[x] > B[y]);
            else flag = (x > y);

            // using this flag, we can determine order of operations
            // (this flag can also be used to return whether
            // the array is negative)
            List<byte> array = new List<byte>(flag ? A : B);
            int len = flag ? B.Length : A.Length;
            for (int i = 0; i < len; i++)
                array = _sub_(array, flag ? B[i] : A[i], i);

            return array.ToArray();
        }
        private static List<byte> _sub_(List<byte> A, byte b, int idx, byte rem = 0)
        {
            short sample = 0;
            if (idx < A.Count)
            {
                sample = (short)((short)A[idx] - (short)b);
                A[idx] = (byte)(sample % 256);
                rem = (byte)(Math.Abs((sample - A[idx])) % 255);
                if (rem > 0)
                    return _sub_(A, (byte)rem, idx + 1);
            }
            else A.Add(b);

            return A;
        }
        
        public static byte[] Multiply(this byte[] A, byte[] B)
        {
            List<byte> ans = new List<byte>();

            byte ov, res;
            int idx = 0;
            for (int i = 0; i < A.Length; i++)
            {
                ov = 0;
                for (int j = 0; j < B.Length; j++)
                {
                    short result = (short)(A[i] * B[j] + ov);

                    // get overflow (high order byte)
                    ov = (byte)(result >> 8);
                    res = (byte)result;
                    idx = i + j;

                    // apply result to answer array
                    if (idx < (ans.Count))
                        ans = _add_(ans, res, idx);
                    else ans.Add(res);
                }
                // apply remainder, if any
                if (ov > 0)
                    if (idx + 1 < (ans.Count))
                        ans = _add_(ans, ov, idx + 1);
                    else ans.Add(ov);
            }

            return ans.ToArray();
        }

    }
}
