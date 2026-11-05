using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
internal static class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        // Empty dictionary: production never enters the MediaPlayer constructor/Open branch.
        using var b=new WindowsSliderLoops();var results=new List<object>();int failed=0;
        void Run(string name,Action check){try{check();results.Add(new{name,classification="EXECUTED WINDOWS EMPTY COMPONENT",passed=true});Console.WriteLine("PASS "+name);}catch(Exception e){failed++;results.Add(new{name,classification="EXECUTED WINDOWS EMPTY COMPONENT",passed=false,error=e.ToString()});Console.WriteLine("FAIL "+name+" "+e);}}
        static void Equal<T>(T a,T e){if(!EqualityComparer<T>.Default.Equals(a,e))throw new Exception($"expected {e}, actual {a}");}
        Run("WE01 empty preparation opens no resources",()=>{b.PrepareAsync(new Dictionary<int,SampleBytes>(),"",CancellationToken.None).GetAwaiter().GetResult();Equal(b.Ready,0);Equal(b.Error,null);});
        Run("WE02 missing acquisition sentinels",()=>{foreach(int id in new[]{-1,0,101}){Equal(b.Acquire(id,false),0);Equal(b.Acquire(id,true),0);Equal(b.Activity(id),0);}});
        Run("WE03 missing attributes contained",()=>{foreach(int id in new[]{-1,0,101})foreach(int attr in new[]{2,3,11})b.Attribute(id,attr,.5f);Equal(b.Error,null);});
        Run("WE04 missing play pause stop remain unavailable",()=>{b.Play(101,true);b.Pause(101);b.Stop(101);b.PauseAll();Equal(b.Activity(101),0);Equal(b.Ready,0);Equal(b.Error,null);});
        Run("WE05 empty cancelled preparation does not fabricate decoder work",()=>{using var c=new CancellationTokenSource();c.Cancel();b.PrepareAsync(new Dictionary<int,SampleBytes>(),"",c.Token).GetAwaiter().GetResult();Equal(b.Ready,0);});
        Run("WE06 repeated disposal and post-disposal containment",()=>{b.Dispose();b.Dispose();b.Attribute(101,2,.5f);b.Play(101,true);b.Pause(101);b.Stop(101);b.PauseAll();Equal(b.Acquire(101,false),0);Equal(b.Activity(101),0);Equal(b.Error,null);});
        byte[] bytes=JsonSerializer.SerializeToUtf8Bytes(new{schema="QA11.Windows.empty.v1",sourceSha=Environment.GetEnvironmentVariable("GITHUB_SHA"),runtime=System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,executed=results.Count,passed=results.Count-failed,failed,cases=results,limit="No media bytes supplied, player/decoder/window/device not opened. Nonempty Windows lifecycle SOURCE INSPECTION only."});
        File.WriteAllBytes(args[0],bytes);Console.WriteLine($"QA11 WINDOWS EMPTY: {results.Count-failed} passed, {failed} failed");Console.WriteLine("QA11 WINDOWS SHA256 "+Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant());using var ms=new MemoryStream();using(var zip=new GZipStream(ms,CompressionLevel.SmallestSize,true))zip.Write(bytes);Console.WriteLine("QA11 WINDOWS EVIDENCE "+Convert.ToBase64String(ms.ToArray()));return failed==0?0:1;
    }
}
