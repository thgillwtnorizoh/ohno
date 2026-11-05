using System.Buffers.Binary;
using System.Text;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
namespace Ohno.QA10;
public static partial class Tests
{
    static readonly string[] banks=["normal","soft","drum"];
    static void Assets()
    {
        Add("W01 exact independent companion names","R12 ENGINEERING asset inventory;not three extra cases",()=>{var expected=banks.SelectMany(b=>new[]{"hitnormal","hitwhistle","hitfinish","hitclap","slidertick"}.Select(l=>b+"-"+l+".wav")).Order().ToArray();Seq(Directory.GetFiles(packA,"*.wav").Select(Path.GetFileName).Order(),expected);});
        Add("W02 independent RIFF chunk and PCM validation","R12 ENGINEERING generated data;not playback",()=>
        {
            foreach(string bank in banks)
            {
                byte[] b=File.ReadAllBytes(Path.Combine(packA,bank+"-slidertick.wav"));Eq(b.Length,10628);Eq(Encoding.ASCII.GetString(b,0,4),"RIFF");Eq(BinaryPrimitives.ReadUInt32LittleEndian(b.AsSpan(4)),10620u);Eq(Encoding.ASCII.GetString(b,8,4),"WAVE");
                var chunks=new List<string>();int pos=12;while(pos<b.Length){Yes(pos+8<=b.Length,"chunk header bounded");string id=Encoding.ASCII.GetString(b,pos,4);int n=checked((int)BinaryPrimitives.ReadUInt32LittleEndian(b.AsSpan(pos+4)));int end=checked(pos+8+n);Yes(end<=b.Length,"chunk data bounded");chunks.Add(id);if(id=="fmt "){Eq(n,16);Eq(BinaryPrimitives.ReadUInt16LittleEndian(b.AsSpan(pos+8)),(ushort)1);Eq(BinaryPrimitives.ReadUInt16LittleEndian(b.AsSpan(pos+10)),(ushort)1);Eq(BinaryPrimitives.ReadUInt32LittleEndian(b.AsSpan(pos+12)),44100u);Eq(BinaryPrimitives.ReadUInt32LittleEndian(b.AsSpan(pos+16)),88200u);Eq(BinaryPrimitives.ReadUInt16LittleEndian(b.AsSpan(pos+20)),(ushort)2);Eq(BinaryPrimitives.ReadUInt16LittleEndian(b.AsSpan(pos+22)),(ushort)16);}if(id=="data"){Eq(n,10584);Yes(b.AsSpan(pos+8,n).ToArray().Any(v=>v!=0),"nonempty waveform");}pos=end+(n&1);}Eq(pos,b.Length);Seq(chunks,"fmt ","data");Obs(new{file=bank+"-slidertick.wav",length=b.Length,sha256=Hash(b)});
            }
        });
        Add("W03 controlled independent generation reproducible","R12 ENGINEERING same-host two invocations",()=>{foreach(string name in Directory.GetFiles(packA,"*.wav").Select(p=>Path.GetFileName(p)!)){var a=File.ReadAllBytes(Path.Combine(packA,name!));var b=File.ReadAllBytes(Path.Combine(packB,name!));Eq(Hash(a),Hash(b));Obs(new{file=name,sha256=Hash(a),length=a.Length});}});
        Add("W04 adding tick pack preserves twelve historical assets","R11/R12 ENGINEERING independently captured pre-add hashes",()=>{var baseline=System.Text.Json.JsonSerializer.Deserialize<Dictionary<string,string>>(File.ReadAllText(Path.Combine(packA,"circle-baseline.json")))!;Eq(baseline.Count,12);foreach(var entry in baseline)Eq(Hash(File.ReadAllBytes(Path.Combine(packA,entry.Key))),entry.Value);});
        Async("W05 generated ticks enter actual local provider","E039 dedicated banks;R12 ENGINEERING catalog",async()=>{var m=SM();using var lib=new HitSoundLibrary(new(){DefaultPackDirectory=packA});using var p=await lib.PrepareAsync(m,CancellationToken.None);var handles=new List<int>();foreach(int set in new[]{1,2,3}){int h=p.Resources.ResolveTick(set,0);Yes(h>0,"independent tick prepared");handles.Add(h);var bytes=p.Samples[h];Eq(Hash(bytes.Bytes),Hash(File.ReadAllBytes(Path.Combine(packA,banks[set-1]+"-slidertick.wav"))));}Eq(handles.Distinct().Count(),3);});
    }
    static void Bindings()
    {
        string Read(string p)=>File.ReadAllText(Path.Combine(root,p));
        Add("B01 both sound streams attach detach restart dispose","R12 ENGINEERING actual Windows adapter SOURCE INSPECTION",()=>{string s=Read("reconstructed/CircleGame/Platform/WindowsCircleEffects.cs");foreach(string t in new[]{"session.SliderSound -= OnSlider","session.SliderSound += OnSlider","session.PositiveCircleSound -= OnCircle","session.PositiveCircleSound += OnCircle","playback.Restart(); Attach(replacement)","session = null; playback.Dispose()","prepared.Dispose()"})Yes(s.Contains(t,StringComparison.Ordinal),t);},"SOURCE INSPECTION");
        Add("B02 synchronous slider callback uses prepared selection","R12 ENGINEERING no decoder/file work in OnSlider SOURCE INSPECTION",()=>{string s=Read("reconstructed/CircleGame/Platform/WindowsCircleEffects.cs");string body=s[s.IndexOf("private void OnSlider",StringComparison.Ordinal)..s.IndexOf("private sealed class SliderSink",StringComparison.Ordinal)];Yes(body.Contains("prepared.Sliders.TryGetValue",StringComparison.Ordinal),"prepared identity");Yes(body.Contains("SliderSampleSelection.Request",StringComparison.Ordinal),"prepared selection");foreach(string forbidden in new[]{"File.","Directory.",".Open(","Wait(",".Result"})Yes(!body.Contains(forbidden,StringComparison.Ordinal),"no "+forbidden);},"SOURCE INSPECTION");
        Add("B03 renderer publishes map ball before screen conversion","R12 ENGINEERING presentation bridge SOURCE INSPECTION",()=>{string s=Read("reconstructed/CircleGame/Rendering/PlayfieldView.cs");int a=s.IndexOf("Vector2 ballMap = definition.BallPosition",StringComparison.Ordinal);int b=s.IndexOf("game.PublishSliderPresentationX(definition, ballMap.X)",StringComparison.Ordinal);int c=s.IndexOf("ToScreen(ballMap)",StringComparison.Ordinal);Yes(a>=0&&b>a&&c>b,"map-space ball publication precedes conversion");},"SOURCE INSPECTION");
        Add("B04 original assets not generator inputs","R12 ENGINEERING asset provenance SOURCE INSPECTION",()=>{string s=Read("reconstructed/GenerateIndependentSliderTicks.ps1");foreach(string t in new[]{"[Math]::Sin","[Math]::Exp","$count = 5292","@(1000,800,400)"})Yes(s.Contains(t,StringComparison.Ordinal),t);foreach(string t in new[]{"ReadAllBytes","osubackup","Start-Process","MediaPlayer"})Yes(!s.Contains(t,StringComparison.Ordinal),"no "+t);},"SOURCE INSPECTION");
    }
}
