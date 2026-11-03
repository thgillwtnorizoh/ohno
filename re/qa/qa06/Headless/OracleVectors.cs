namespace Ohno.QA06;

// Independent finite E034 table. No production parser/preparer calls here.
// T is the signed Int32 (last-first-breaks)/1000, NOT prepared object max-end.
// K values derive from E034's literal Single-order expression, no fitted owner total.
internal sealed record MetadataVector(string Name, string Rows, string Difficulty, string Events,
    int N, int First, int Last, int Breaks, int Seconds, int K);
internal static class OracleVectors
{
    internal const string Standard = "HPDrainRate:0\nOverallDifficulty:5\nCircleSize:5";
    internal static readonly MetadataVector[] Metadata =
    [
        new("A01 single object zero seconds", "0,0,3000,2,0,L|100:0,1,100", "HPDrainRate:0\nOverallDifficulty:4\nCircleSize:4", "", 1,3000,3000,0,0,3),
        new("A02 sparse hundred second span", "0,0,1000,1,0\n0,0,101000,1,0", Standard,"",2,1000,101000,0,100,1),
        new("A03 positive subsecond span truncates to zero", "0,0,1000,1,0\n0,0,1999,1,0", Standard,"",2,1000,1999,0,0,3),
        new("A04 exact one second span", "0,0,1000,1,0\n0,0,2000,1,0", Standard,"",2,1000,2000,0,1,3),
        new("A05 two second span", "0,0,1000,1,0\n0,0,3000,1,0", Standard,"",2,1000,3000,0,2,2),
        new("A06 unsorted raw first and last are not extrema", "0,0,9000,1,0\n0,0,1000,1,0\n0,0,4000,1,0", Standard,"",3,9000,4000,0,-5,1),
        new("A07 equal starts metadata-only", "0,0,2000,1,0\n0,0,2000,1,0", Standard,"",2,2000,2000,0,0,3),
        new("A08 first unknown sets first before type dispatch", "0,0,1000,16,0\n0,0,5000,1,0", Standard,"",2,1000,5000,0,4,2),
        new("A09 last unknown increments N without changing last", "0,0,1000,1,0\n0,0,5000,1,0\n0,0,9000,16,0", Standard,"",3,1000,5000,0,4,2),
        new("A10 numeric break subtracts from row-start span", "0,0,1000,1,0\n0,0,5000,1,0", "HPDrainRate:5\nOverallDifficulty:5\nCircleSize:5","2,2000,4000",2,1000,5000,2000,2,3),
        new("A11 overlapping breaks both count without union", "0,0,1000,1,0\n0,0,5000,1,0", Standard,"2,1500,3000\n2,2000,3500",2,1000,5000,3000,1,3),
        new("A12 negative break duration is signed", "0,0,1000,1,0\n0,0,5000,1,0", Standard,"2,4000,2000",2,1000,5000,-2000,6,2),
        new("A13 out-of-range break not clipped", "0,0,1000,1,0\n0,0,5000,1,0", Standard,"2,9000,14000",2,1000,5000,5000,-1,1),
        new("A14 negative fractional second truncates toward zero", "0,0,2000,1,0\n0,0,1001,1,0", Standard,"",2,2000,1001,0,0,3),
        new("A15 fractional time converts before elapsed division", "0,0,1000.9,1,0\n0,0,2000.1,1,0", Standard,"",2,1000,2000,0,1,3),
        new("A16 exact even midpoint 2.5", "0,0,1000,1,0\n0,0,5000,1,0", "HPDrainRate:5\nOverallDifficulty:5\nCircleSize:5","",2,1000,5000,0,4,2),
        new("A17 dense density caps at sixteen", string.Join('\n',Enumerable.Range(0,20).Select(i=>$"0,0,{1000+i*250},1,0").Append("0,0,6000,1,0")), Standard,"",21,1000,6000,0,5,3),
        new("A18 unchecked elapsed subtraction overflow", "0,0,-2147483648,1,0\n0,0,2147483647,1,0", Standard,"",2,int.MinValue,int.MaxValue,0,0,3),
        new("A19 unchecked break duration overflow", "0,0,1000,1,0\n0,0,5000,1,0", Standard,"2,-2147483648,2147483647",2,1000,5000,-1,4,2),
    ];
}
