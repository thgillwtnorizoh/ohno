using static Ohno.CircleGame.Difficulty.StarMath;
namespace Ohno.CircleGame.Difficulty;

public static class DifficultyAggregation
{
    public static double Ordered(IEnumerable<double> values,double strength)
    {
        double total=0;int i=0;
        foreach(double value in values.Where(x=>x>0).OrderByDescending(x=>x))
        {double top=1+strength/(1+i),bottom=Math.Pow(i,.9)+1+strength/(1+i);total+=value*(top/bottom);i++;}
        Finite(total);return total;
    }
    public static(double[] Values,int Count) FirstMinute(IEnumerable<double> values,IReadOnlyList<DifficultyPair> pairs)
    {
        var positive=values.Where(x=>x>0).ToArray();int count=0;
        if(pairs.Count>0)foreach(var p in pairs){if(p.Time>pairs[0].Time+60000)break;count++;}
        for(int i=0;i<Math.Min(positive.Length,count);i++)
        {double weight=Math.Log10(1+9*Math.Clamp((double)i/count,0,1));positive[i]*=0+(1d-0)*weight;}
        return(positive,count);
    }
    public static double Duration(IEnumerable<DurationSegment> segments)
    {
        double total=0,t=0;
        foreach(var s in segments){double next=t+s.Duration/400,w=Math.Pow(.9,t)-Math.Pow(.9,next);total+=s.Value*w;t=next;}
        return total/(1-.9);
    }
    public static double Final(double q0,double q1,double q2)
    {
        double a=Math.Pow(q0,.63)*.02275,b=Math.Sqrt(q1)*.0675,c=Math.Sqrt(q2)*.0675;
        double e0=4*Cube(a),e1=4*Cube(b),e2=4*Cube(c);
        // No-mod optional energy is0; M080c returns component2 unchanged.
        return Math.Pow(Norm(1.1,e0,e1,e2)*1.12,.3333333333333333);
    }
}
public sealed class DurationEnvelope(CalculationBudget? budget=null)
{
    private double start,next,peak,retained;
    private bool finalized;
    private readonly List<DurationSegment> segments=[];
    private readonly LinkedList<(double Value,double Time)> candidates=new();
    private void Record(double duration)
    {
        budget?.Step();var segment=new DurationSegment(peak,Math.Round(duration,MidpointRounding.ToEven));
        int lo=0,hi=segments.Count-1;int? found=null;
        while(lo<=hi)
        {int mid=lo+((hi-lo)>>1);double v=segments[mid].Value;if(v==peak){found=mid;break;}if(v>peak)lo=mid+1;else hi=mid-1;}
        segments.Insert(found??lo,segment);retained+=duration;
        while(retained>(11/(1-.9))*400)
        {budget?.Step();var dropped=segments[^1];segments.RemoveAt(segments.Count-1);retained-=dropped.Duration;}
    }
    public double Process(DifficultyPair p,double oldState,Func<double> raw)
    {
        if(finalized)throw new InvalidOperationException("Envelope already finalized.");
        if(p.Index==0){start=p.Time;next=start+400;peak=raw();return peak;}
        while(p.Time>next)
        {
            Record(next-start);start=next;
            if(candidates.First is{} first)
            {var item=first.Value;candidates.RemoveFirst();next=item.Time+400;peak=oldState*Math.Pow(.2,(start-p.Previous()!.Time)/1000);peak=Math.Max(peak,item.Value);}
            else{next=start+400;peak=oldState*Math.Pow(.2,(start-p.Previous()!.Time)/1000);}
        }
        double value=raw();
        if(value>peak){candidates.Clear();Record(p.Time-start);start=p.Time;next=start+400;peak=value;}
        else{while(candidates.Last is{} last&&last.Value.Value<value)candidates.RemoveLast();candidates.AddLast((value,p.Time));}
        return value;
    }
    public(double Value,EnvelopeTrace Trace) Finish()
    {
        if(!finalized){Record(next-start);finalized=true;}
        var data=segments.Where(s=>s.Value>0).ToList();int i=0;double elapsed=0;
        while(i<data.Count&&elapsed<4000)
        {
            budget?.Step();var entry=data[i];double part=0;
            while(part<entry.Duration)
            {
                budget?.Step();double weight=Math.Log10(1+9*Math.Clamp((elapsed+part)/4000,0,1));
                double amount=Math.Round(Math.Min(20,entry.Duration-part),MidpointRounding.ToEven);
                data.Add(new(entry.Value*(.727+(1-.727)*weight),amount));part+=20;
            }
            elapsed+=entry.Duration;i++;
        }
        var processed=data.Skip(i).OrderByDescending(s=>s.Value).ToArray();double total=DifficultyAggregation.Duration(processed);
        Finite(total);return(total,new(segments.ToArray(),processed,i,retained));
    }
}
