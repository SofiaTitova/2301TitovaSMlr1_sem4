using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bwt___mft__ha
{
    public class Tuple : IComparable<Tuple>
    {
        public int OriginalIndex { get; set; }  // хранит оригинальный индекс суффикса
        public int FirstHalf { get; set; }      // хранит ранг первой половины суффикса
        public int SecondHalf { get; set; }     // хранит ранг второй половины суффикса

        public int CompareTo(Tuple other)
        {
            if (this.FirstHalf == other.FirstHalf)
            {
                return this.SecondHalf.CompareTo(other.SecondHalf);
            }
            else
            {
                return this.FirstHalf.CompareTo(other.FirstHalf);
            }
        }

        public override string ToString()
        {
            return $"Tuple{{OriginalIndex={OriginalIndex}, FirstHalf={FirstHalf}, SecondHalf={SecondHalf}}}";
        }
    }

    public class BWTFast
    {
        public StringBuilder GetBWT(string s)
        {
            s += "$";
            StringBuilder bwt = new StringBuilder();
            int[] suffixArray = Compress(s);
            foreach (int i in suffixArray)
            {
                int j = i - 1;
                if (j < 0)
                {
                    j += suffixArray.Length;
                }
                bwt.Append(s[j]);
            }
            return bwt;
        }

        public int[] Compress(string s)
        {
            int N = s.Length;

            int steps = (int)Math.Log(N, 2);

            int[][] rank = new int[steps + 1][];
            for (int i = 0; i <= steps; i++)
            {
                rank[i] = new int[N];
            }

            for (int i = 0; i < N; i++)
            {
                rank[0][i] = s[i] - 'a';
            }

            Tuple[] tuples = new Tuple[N];

            for (int step = 1, cnt = 1; step <= steps; step++, cnt <<= 1)
            {
                for (int i = 0; i < N; i++)
                {
                    Tuple tuple = new Tuple();
                    tuple.FirstHalf = rank[step - 1][i];
                    tuple.SecondHalf = i + cnt < N ? rank[step - 1][i + cnt] : -1;
                    tuple.OriginalIndex = i;

                    tuples[i] = tuple;
                }

                Array.Sort(tuples);

                rank[step][tuples[0].OriginalIndex] = 0;

                for (int i = 1, currRank = 0; i < N; i++)
                {
                    if (tuples[i - 1].FirstHalf != tuples[i].FirstHalf || tuples[i - 1].SecondHalf != tuples[i].SecondHalf)
                    {
                        ++currRank;
                    }
                    rank[step][tuples[i].OriginalIndex] = currRank;
                }
            }

            int[] suffixArray = new int[N];

            for (int i = 0; i < N; i++)
            {
                suffixArray[i] = tuples[i].OriginalIndex;
            }

            return suffixArray;
        }
    }
}
