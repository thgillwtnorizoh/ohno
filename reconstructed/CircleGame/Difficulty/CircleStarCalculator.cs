using System.Globalization;
using System.IO;
using Ohno.CircleGame.Selection;
using static Ohno.CircleGame.Difficulty.StarMath;

namespace Ohno.CircleGame.Difficulty;

internal static class StarMath
{
    internal static double Sq(double x)=>x*x;
    internal static double Cube(double x)=>(x*x)*x;
    internal static double Deg(double x)=>(x*Math.PI)/180;
    internal static double Linear(double x,double a,double b)=>Math.Clamp((x-a)/(b-a),0,1);
    internal static double Smooth(double x,double a,double b){double t=Linear(x,a,b);return(t*t)*(3-2*t);}
    internal static double Smoother(double x,double a,double b){double t=Linear(x,a,b);return((t*t)*t)*(t*(6*t-15)+10);}
    internal static double Wide(double x)=>Smooth(x,Deg(140),Deg(40));
    internal static double Narrow(double x)=>Smooth(x,Deg(40),Deg(140));
    internal static double Norm(double power,params double[] values){double sum=0;foreach(double x in values)sum+=Math.Pow(x,power);return Math.Pow(sum,1/power);}
    internal static void Finite(params double[] values){if(values.Any(x=>!double.IsFinite(x)))throw new UnsupportedStarProfileException("Nonfinite mathematical state; no numerical cache entry produced.");}
    internal static double RadiusScale(DifficultyPair p)=>Math.Max(1,1+(double)(float)(((float)(30f-p.Preparation.Radius))/70f));
}
public sealed record MovementTerms(double Movement,double Jump,double VelocityAngle,double Composite);
public static class MovementProcessor
{
    public static double RepeatedAngle(DifficultyPair p,DifficultyPair previous)
    {
        if(p.Angle is null||previous.Angle is null)return 1;
        double total=0;
        for(int k=0;k<6;k++)
        {
            var h=p.Previous(k);if(h is null||Math.Max(p.Strain,h.Strain)>1.1*Math.Min(p.Strain,h.Strain))break;
            if(h.Axis is{} a&&p.Axis is{} b)total+=Math.Cos(8*Math.Min(Deg(11.25),Math.Abs(b-a)));
        }
        // Original division-by-zero/cap arm is expressed finitely with the same selected result.
        double factor=Sq(total==0?1:Math.Min(.5/total,1)),spacing=Smoother(p.Jump,0,100);
        double cosine=Math.Cos(2*Math.Min(Deg(45),Math.Abs(p.Angle.Value-previous.Angle.Value)*spacing));
        double b0=1-(.15*Wide(previous.Angle.Value))*cosine;
        return Sq(b0+(((1-b0)*factor)*.5)*spacing);
    }
    public static double Movement(DifficultyPair p)
    {
        if(p.Index<=1)return 0;
        var prev=p.Previous()!;var older=p.Previous(2);double distance=p.Jump,priorDistance=prev.Jump;
        double v=distance/p.Strain,pv=priorDistance/prev.Strain,value=v*RepeatedAngle(p,prev);
        if(p.Angle is{} aa&&prev.Angle is{} bb)
        {
            double minimum=Math.Min(v,pv),small=0;
            if(Math.Max(p.Strain,prev.Strain)<1.25*Math.Min(p.Strain,prev.Strain))
            {
                small=Wide(aa);small*=.08+.92*(1-Math.Min(small,Cube(Wide(bb))));
                small*=((minimum*Smoother(60000/(p.Strain*2),300,400))*Smoother(distance,0,200));
            }
            double large=Narrow(aa);large*=.25+.75*(1-Math.Min(large,Cube(Narrow(bb))));
            large*=Math.Min(distance/Math.Pow(p.Strain,1.45),priorDistance/Math.Pow(prev.Strain,1.45));
            if(older is not null){float d=CircleDifficultyPreparation.Distance(older.Position,prev.Position);if(d<1f)large*=1-.55*(double)(float)(1f-d);}
            value+=Math.Max(small*2.41,large*9.67);
            double more=((((minimum*Smoother(distance,50,100))*Math.Pow(Linear(distance,300,100),1.8))*Smoother(aa,Deg(110),Deg(60)))*Smoother(priorDistance,50,100));
            more=(more*Math.Pow(Linear(priorDistance,300,100),1.8))*Smoother(bb,Deg(110),Deg(60));value+=more*1.02;
        }
        if(Math.Max(pv,v)!=0)
        {
            double contrast=Smooth(Math.Abs(pv-v)/Math.Max(pv,v),0,1);
            double boost=Math.Min(125/Math.Min(p.Strain,prev.Strain),Math.Abs(pv-v))*contrast;
            boost*=Sq(Math.Min(p.Strain,prev.Strain)/Math.Max(p.Strain,prev.Strain));value+=boost*.9;
        }
        value*=RadiusScale(p);return value*(1/(1-Math.Pow(.03,Math.Pow(p.Strain/1000,.65))));
    }
    public static double Overlap(DifficultyPair a,DifficultyPair b)
    {double d=CircleDifficultyPreparation.Distance(a.Position,b.Position),r=a.Preparation.Radius;return Math.Clamp(1-Sq(Math.Max(d-r,0)/r),0,1);}
    public static double VelocityAngle(DifficultyPair p)
    {
        if(p.Index<=1)return 0;
        var prev=p.Previous()!;var old=p.Previous(1)!;double distance=p.Jump,v=distance/p.Strain,pv=prev.Jump/prev.Strain;
        double value=v*Math.Sqrt(RadiusScale(p));value*=1+Math.Min(.25,Math.Pow((Math.Max(p.Strain,prev.Strain)-Math.Min(p.Strain,prev.Strain))/50,4));
        if(p.Angle is{} a&&prev.Angle is{} b){double x=(Math.Sin(Math.Abs(a-b)/2)*180)/(p.Strain*.1);value*=.8+Math.Sqrt(x/270);}
        double repetition=1;if(p.Index>2)repetition=1-((Overlap(p,prev)*Overlap(p,old))*Overlap(prev,old));
        if(p.Angle is{} angle)value+=(v*Wide(angle))*repetition;
        if(Math.Max(pv,v)!=0)
        {double contrast=Smooth(Math.Abs(pv-v)/Math.Max(pv,v),0,1),boost=Math.Min(125/Math.Min(p.Strain,prev.Strain),Math.Abs(pv-v));value+=((boost*contrast)*repetition)*.52;}
        value=Math.Pow(value,1.45);return value*Smoother(distance,0,50);
    }
    public static MovementTerms Calculate(DifficultyPair p)
    {
        double m=Movement(p),j=((Math.Min(p.Jump,120)/120)*1000)/p.Strain;
        j*=Math.Pow(RadiusScale(p),1.5);j*=1/(1-Math.Pow(.2,p.Strain/1000));
        double v=VelocityAngle(p),x=m*70.9,y=j*2.35,z=v*242,norm=Norm(1.2,x,y);
        // Preserve M19ea's zero and unordered ratio branches without emitting a NaN intermediate.
        double blend=z==0?(norm==0?1:0):norm==0?1:1/(1+Math.Exp(-7.27*Math.Log(z/norm)));
        double value=((norm*blend)+(z*(1-blend)))*1.12,od=(79.5-p.Preparation.FullWindow/2)/6;
        value*=.985+Sq(Math.Max(0,od))/4000;Finite(m,j,v,value);return new(m,j,v,value);
    }
}
public sealed record PatternSnapshot(int Interval,int Count,int Repeats,int Identity)
{public int[] Values=>[Interval,Count,Repeats,Identity];}
public sealed record RhythmStep(int K,int Pair,double Dt,double Prior,double Difference,double HistoryWeight,PatternSnapshot Group,PatternSnapshot PreviousGroup,IReadOnlyList<PatternSnapshot> Saved,bool Active,double Value,double Sum,IReadOnlyList<string> Flags,IReadOnlyList<PatternSnapshot> BeforeSaved);
public sealed record RhythmResult(double Multiplier,int Capacity,int Seed,double Tolerance,double Sum,double Attenuation,IReadOnlyList<RhythmStep> Steps);
public static class RhythmProcessor
{
    private sealed class Pattern(int identity,int interval=int.MaxValue)
    {
        internal readonly int Identity=identity;internal int Interval=interval,Count=1,Repeats=1;
        internal void Append(int dt){if(Interval==int.MaxValue)Interval=Math.Max(dt,25);Count=unchecked(Count+1);}
        internal bool Same(Pattern other,double tolerance)=>Math.Abs(unchecked(Interval-other.Interval))<tolerance&&Count==other.Count;
        internal bool Parity(Pattern other,double tolerance)=>Count>1&&other.Count>1&&Math.Abs(unchecked(Interval-other.Interval))<tolerance&&Count%2==other.Count%2;
        internal PatternSnapshot Snapshot()=>new(Interval,Count,Repeats,Identity);
    }
    public static double DoubleTap(DifficultyPair p,DifficultyPair? following)
    {
        if(following is null)return 0;double dt=Math.Max(1,p.Delta),difference=Math.Abs(Math.Max(1,following.Delta)-dt);
        double timing=dt/Math.Max(dt,difference),window=Math.Pow(Math.Min(1,dt/p.Preparation.FullWindow),5),spacing=Sq(Linear(p.Jump,100,50));
        return 1-Math.Pow(timing,spacing*(1-window));
    }
    public static double IntervalRaw(DifficultyPair p)
    {
        double adjusted=p.Strain/Math.Clamp(p.Strain/p.Preparation.FullWindow/.93,.92,1),bonus=0;
        if(60000/(adjusted*4)>200)bonus=.75*Sq((60000d/4/200-adjusted)/40);
        double value=((1+bonus)*1000)/adjusted;value*=1/(1-Math.Pow(.3,p.Strain/1000));return value*(1-DoubleTap(p,p.Following()));
    }
    public static RhythmResult Calculate(DifficultyPair p,bool capture=true)
    {
        int serial=0;Pattern New(int interval=int.MaxValue)=>new(++serial,interval);
        double total=0,tolerance=p.Preparation.FullWindow*.3,lastValue=0;var group=New();var lastGroup=New();var seen=new List<Pattern>();
        bool active=false;int capacity=Math.Min(p.Index,32),offset=0;var steps=new List<RhythmStep>();
        while(offset<capacity-2&&p.Time-p.Previous(offset)!.Time<5000)offset++;
        int seed=offset;var older=p.Previous(offset);var oldest=p.Previous(offset+1);
        for(int k=offset;k>0;k--)
        {
            var h=p.Previous(k-1)!;double weight=Math.Min((double)(capacity-k)/capacity,(5000-(p.Time-h.Time))/5000);
            double dt=Math.Max(h.Delta,1e-7),prior=Math.Max(older!.Delta,1e-7),difference=Math.Abs(prior-dt);
            if(group.Interval==int.MaxValue)group=New((int)dt);
            double ratio=Math.Max(prior,dt)/Math.Min(prior,dt),fraction=ratio-Math.Truncate(ratio),tri=Math.Clamp((.5-Math.Abs(fraction-.5))*2,0,1);
            double wave=(tri*tri)*(3-2*tri),value=((1+26*Math.Min(.5,wave))*Math.Clamp((difference-tolerance)/tolerance,0,1))*Math.Clamp(2-ratio/8,0,1);
            var before=capture?seen.Select(g=>g.Snapshot()).ToArray():[];var flags=new List<string>();
            if(difference<tolerance){group.Append((int)dt);if(capture)flags.Add("append");}
            if(active)
            {
                if(difference>tolerance)
                {
                    if(group.Parity(lastGroup,tolerance)){value*=.5;if(capture)flags.Add("parity");}
                    if(Math.Max(oldest!.Delta,1e-7)>prior+tolerance&&prior>dt+tolerance){value*=.125;if(capture)flags.Add("descending");}
                    if(lastGroup.Count==group.Count){value*=.5;if(capture)flags.Add("equal-count");}
                    if(prior>dt+tolerance){value*=.65;if(capture)flags.Add("accelerating");}
                    bool found=false;
                    foreach(var saved in seen)
                    {
                        if(!saved.Same(group,tolerance))continue;
                        if(capture)flags.Add("saved-match");
                        if(lastGroup.Same(group,tolerance)){saved.Repeats=unchecked(saved.Repeats+1);if(capture)flags.Add("repeat-increment");}
                        double exponent=2.75/(1+Math.Exp(.24*(58.33-group.Interval)));
                        value*=Math.Min(3d/saved.Repeats,Math.Pow(1d/saved.Repeats,exponent));found=true;break;
                    }
                    if(!found&&group.Count>0){seen.Add(group);if(capture)flags.Add("saved-insert");} // Actual mutable reference, no clone.
                    value*=1-DoubleTap(older,h)*.75;
                    total+=group.Count>1?Math.Sqrt(value*lastValue)*weight:.7*weight;
                    lastValue=value;if(prior+tolerance<dt)active=false;
                    lastGroup=group;group=New((int)dt);
                }
            }
            else if(prior>dt+tolerance){active=true;lastValue=value;group=New((int)dt);if(capture)flags.Add("activate");}
            if(capture)steps.Add(new(k,h.Index,dt,prior,difference,weight,group.Snapshot(),lastGroup.Snapshot(),seen.Select(g=>g.Snapshot()).ToArray(),active,value,total,flags,before));
            oldest=older;older=h;
        }
        double attenuation=Linear(group.Count,22,3),multiplier=Math.Sqrt(4+(total*attenuation)*.95)/2;
        Finite(total,multiplier);return new(multiplier,capacity,seed,tolerance,total,attenuation,steps);
    }
}
public sealed record VisibilityContribution(int Index,double Value);
public sealed record VisibilityAngle(int Index,double Value,double Cap,double Operand);
public sealed record VisibilityResult(double Raw,double Backward,double Forward,double Angle,double? NextJump,double ForwardRoot,double Velocity,double Movement,double Short,IReadOnlyList<VisibilityContribution> Past,IReadOnlyList<VisibilityContribution> Future,IReadOnlyList<VisibilityAngle> Angles);
public static class VisibilityProcessor
{
    public static double Visible(DifficultyPair p,double at)
    {if(at>p.Time)return 0;return Math.Clamp((at-(p.Time-p.Preparation.Preempt))/(400*Math.Min(1,p.Preparation.Preempt/450)),0,1);}
    public static VisibilityResult Calculate(DifficultyPair p,bool capture=true,CalculationBudget? budget=null)
    {
        if(p.Index==0)return new(0,0,0,1,p.Following()?.Jump,0,Math.Max(1,p.Jump/p.Strain),0,0,[],[],[]);
        double backward=0,forward=0,total=0,elapsed=0;var past=new List<VisibilityContribution>();var future=new List<VisibilityContribution>();var angles=new List<VisibilityAngle>();
        for(int k=0;k<p.Index;k++)
        {
            budget?.Step();var h=p.Previous(k)!;if(p.Time-h.Time>3000||h.Time<p.Time-p.Preparation.Preempt)break;
            double v=Visible(p,h.Time)*Smoother(h.Jump,15,150);v*=Math.Clamp(2-(p.Time-h.Time)/1500,0,1);backward+=v;if(capture)past.Add(new(h.Index,v));
        }
        for(var h=p.Following();h is not null;h=h.Following())
        {
            budget?.Step();if(h.Time-p.Time>3000||p.Time<h.Time-h.Preparation.Preempt)break;
            double v=Visible(h,p.Time)*Math.Clamp(2-(h.Time-p.Time)/1500,0,1);forward+=v;if(capture)future.Add(new(h.Index,v));
        }
        int offset=0;DifficultyPair recent=p;DifficultyPair? previous=null,earlier=null;
        while(elapsed<2000)
        {
            budget?.Step();var h=p.Previous(offset);if(h is null)break;
            double temporal=1-Linear(h.Strain,200,2000);
            if(h.Angle is{} hAngle&&p.Angle is{} pAngle)
            {
                double difference=Math.Abs(pAngle-hAngle),angle=Math.PI;
                if(recent.Angle is{} r&&previous?.Angle is{} prev&&earlier?.Angle is{} early)
                {
                    angle=Math.Abs(prev-hAngle)+Math.Abs(early-r);double blend=1;
                    blend*=Linear((Math.Min(hAngle,r)*180)/Math.PI,20,5);blend*=Linear((Math.Max(hAngle,r)*180)/Math.PI,60,120);
                    angle=Math.PI+(.1*angle-Math.PI)*blend;
                }
                double operand=Math.Min(difference,angle)*Smoother(h.Jump,0,50);
                double contribution=Math.Cos(3*Math.Min(Deg(30),operand))*temporal;total+=contribution;
                if(capture)angles.Add(new(h.Index,contribution,angle,operand));
            }
            elapsed=p.Time-h.Time;offset++;earlier=previous;previous=recent;recent=h;
        }
        double angular=total==0?1:Math.Clamp(2/total,.2,1),velocity=Math.Max(1,p.Jump/p.Strain);
        var next=p.Following();double root=Math.Sqrt(forward);if(next is not null)root*=Smoother(next.Jump,15,150);
        double movement=Math.Pow(Math.Max(0,(((Math.Pow(backward+root,1.7)*.4)*angular)*velocity)-2.5),.45)*2.4;
        double temporalTerm=Math.Pow(((500-p.Preparation.Preempt)+Math.Abs(p.Preparation.Preempt-500))/2,2.5)/140000;
        temporalTerm*=angular*velocity;
        double raw=p.Index==0?0:Norm(1.5,temporalTerm,0,movement)*(1/(1-Math.Pow(.8,p.Strain/1000)));
        Finite(raw,backward,forward,angular,movement,temporalTerm);return new(raw,backward,forward,angular,next?.Jump,root,velocity,movement,temporalTerm,past,future,angles);
    }
}
public sealed class CalculationBudget(CancellationToken token=default,int maximumSteps=2000000)
{
    private int used;
    public void Step(){token.ThrowIfCancellationRequested();if(++used>maximumSteps)throw new UnsupportedStarProfileException("Restricted mathematical work budget exceeded.");}
}
public sealed record DurationSegment(double Value,double Duration);
public sealed record EnvelopeTrace(IReadOnlyList<DurationSegment> Segments,IReadOnlyList<DurationSegment> Processed,int Consumed,double Retained);
public sealed record PairCalculation(DifficultyPair Pair,MovementTerms Movement,double IntervalRaw,RhythmResult Rhythm,VisibilityResult Visibility,double[] States,double[] Outputs);
public sealed record StarCalculationTrace(IReadOnlyList<DifficultyPair> Pairs,IReadOnlyList<PairCalculation> Rows,EnvelopeTrace Envelope,double[][] ComponentOutputs,double[] Component2Transformed,int FirstMinuteCount,double[] Aggregates);
public sealed record StarCalculationResult(double CalculatedDouble,float CachedSingle,string SingleBitsLittleEndian,string Display,StarCalculationTrace? Trace=null)
{
    public string Provenance=>"original-derived restricted calculation · E044–E046 mathematical profile";
}
public static class CircleStarCalculator
{
    public static StarCalculationResult Calculate(CatalogChart chart,CancellationToken token=default,bool capture=false)
    {var input=StarAdmission.Read(chart,token);var result=CalculatePrepared(input.Circles,input.Preparation,token,capture);chart.Verify(token);return result;}
    // Explicit prepared-input verification seam, not raw/UI admission of other manager profiles.
    public static StarCalculationResult CalculatePrepared(IReadOnlyList<CircleDefinition> circles,CirclePreparation preparation,CancellationToken token=default,bool capture=true)
    {
        var pairs=CircleDifficultyPreparation.Prepare(circles,preparation);var order=pairs.OrderBy(p=>p.Time).ToArray();
        var budget=new CalculationBudget(token);var envelope=new DurationEnvelope(budget);double[] states=[0,0,0];List<double>[] outputs=[[],[],[]];var rows=new List<PairCalculation>();
        foreach(var p in order)
        {
            budget.Step();var m=MovementProcessor.Calculate(p);
            double Update0(){double decay=Math.Pow(.2,p.Strain/1000);states[0]*=decay;states[0]+=m.Composite*(1-decay);return states[0];}
            double output0=envelope.Process(p,states[0],Update0),raw1=RhythmProcessor.IntervalRaw(p),d=Math.Pow(.3,p.Strain/1000);
            states[1]*=d;states[1]+=(raw1*(1-d))*1.16;var rhythm=RhythmProcessor.Calculate(p,capture);double output1=states[1]*rhythm.Multiplier;
            var visibility=VisibilityProcessor.Calculate(p,capture,budget);d=Math.Pow(.8,p.Delta/1000);states[2]*=d;states[2]+=(visibility.Raw*(1-d))*2.5;
            double[] values=[output0,output1,states[2]];Finite(states);Finite(values);
            for(int k=0;k<3;k++)outputs[k].Add(values[k]);if(capture)rows.Add(new(p,m,raw1,rhythm,visibility,states.ToArray(),values));
        }
        var(q0,env)=envelope.Finish();double q1=DifficultyAggregation.Ordered(outputs[1],20);
        var(transformed,count)=DifficultyAggregation.FirstMinute(outputs[2],order);double q2=DifficultyAggregation.Ordered(transformed,1);
        double result=DifficultyAggregation.Final(q0,q1,q2);Finite(result);float cached=(float)result;
        if(!float.IsFinite(cached))throw new UnsupportedStarProfileException("Nonfinite Single cache result.");
        var bits=BitConverter.GetBytes(cached);if(!BitConverter.IsLittleEndian)Array.Reverse(bits);
        string display=(Math.Floor(Math.Abs((double)cached)*100)/100).ToString("0.##",CultureInfo.InvariantCulture)+"★";
        return new(result,cached,Convert.ToHexString(bits).ToLowerInvariant(),display,capture?new(pairs,rows,env,outputs.Select(x=>x.ToArray()).ToArray(),transformed,count,[q0,q1,q2]):null);
    }
}
