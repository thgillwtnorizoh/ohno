using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
namespace Ohno.QA09;
public static partial class Tests
{
    static string[] ExpectedPackNames()=>new[]{"normal","soft","drum"}.SelectMany(b=>new[]{"hitnormal","hitwhistle","hitfinish","hitclap"}.Select(l=>b+"-"+l+".wav")).Order(StringComparer.Ordinal).ToArray();
    sealed record Pcm(string Name,int FileBytes,int Rate,int Channels,int Bits,int DataOffset,int DataBytes,string Sha256);
    static Pcm ReadWave(string path)
    {
        byte[] b=File.ReadAllBytes(path);
        string Four(int offset)=>Encoding.ASCII.GetString(b,offset,4);
        uint U32(int offset)=>System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(b.AsSpan(offset,4));
        ushort U16(int offset)=>System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(b.AsSpan(offset,2));
        Yes(b.Length>=44,"minimum canonical PCM length");Eq(Four(0),"RIFF");Eq(Four(8),"WAVE");Eq((long)U32(4)+8,b.Length);
        int pos=12,fmt=-1,data=-1,dataBytes=0,fmtCount=0,dataCount=0;var chunks=new List<string>();
        while(pos+8<=b.Length)
        {
            string name=Four(pos);uint size=U32(pos+4);Yes(size<=int.MaxValue,"bounded chunk size");int start=pos+8;Yes((long)start+size<=b.Length,"chunk lies within RIFF bounds");chunks.Add(name);
            if(name=="fmt "){fmt=start;fmtCount++;Eq(size,16u);}
            if(name=="data"){data=start;dataBytes=(int)size;dataCount++;}
            pos=checked(start+(int)size+((int)size&1));
        }
        Eq(pos,b.Length);Eq(fmtCount,1);Eq(dataCount,1);Seq(chunks,"fmt ","data");
        Eq(U16(fmt), (ushort)1);Eq(U16(fmt+2),(ushort)1);Eq(U32(fmt+4),44100u);Eq(U32(fmt+8),88200u);Eq(U16(fmt+12),(ushort)2);Eq(U16(fmt+14),(ushort)16);
        Eq(dataBytes,5292*2);Eq(dataBytes%U16(fmt+12),0);Eq(b.Length,44+5292*2);
        var pcm=new Pcm(Path.GetFileName(path),b.Length,(int)U32(fmt+4),U16(fmt+2),U16(fmt+14),data,dataBytes,Hash(b));Obs(pcm);return pcm;
    }
    static void AssetCases()
    {
        Add("W01 twelve generated names and independent RIFF/PCM structure","ENGINEERING generator; independent chunk reader, no playback",()=>{
            using var expected=JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root,"re/qa/qa09/Inputs/generated-pack-expectations.json")));Seq(expected.RootElement.GetProperty("files").EnumerateArray().Select(x=>x.GetString()!),ExpectedPackNames());Eq(expected.RootElement.GetProperty("fileBytes").GetInt32(),10628);
            var actual=Directory.GetFiles(packA).Select(Path.GetFileName).Select(x=>x!).Order(StringComparer.Ordinal);Seq(actual,ExpectedPackNames());foreach(string name in ExpectedPackNames())ReadWave(Path.Combine(packA,name));
        });
        Add("W02 bounded PCM sample data independent of header claims","ENGINEERING waveform sanity, no audio decoding/listening",()=>{
            foreach(string name in ExpectedPackNames())
            {
                string path=Path.Combine(packA,name);var p=ReadWave(path);byte[] b=File.ReadAllBytes(path);var samples=new short[p.DataBytes/2];
                for(int i=0;i<samples.Length;i++)samples[i]=System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(b.AsSpan(p.DataOffset+2*i,2));
                Eq(samples[0],(short)0);Eq(samples[^1],(short)0);Yes(samples.Any(x=>Math.Abs((int)x)>100),"nonzero generated sample data");Yes(samples.All(x=>Math.Abs((int)x)<=8500),"declared finite synthesized amplitude bound");
            }
        });
        Add("W03 controlled two-run generator byte reproducibility","ENGINEERING same runtime/input reproducibility only",()=>{
            Seq(Directory.GetFiles(packB).Select(Path.GetFileName).Select(x=>x!).Order(StringComparer.Ordinal),ExpectedPackNames());
            foreach(string name in ExpectedPackNames()){string a=Hash(File.ReadAllBytes(Path.Combine(packA,name))),b=Hash(File.ReadAllBytes(Path.Combine(packB,name)));Obs(new{name,packA=a,packB=b});Eq(a,b);}
        });
        Add("W04 generated pack all banks/layers integrate as defaults","E038 resource keys with ENGINEERING independent pack provider",()=>{
            var trace=new List<string>();var map=new Files("map",trace);var pack=new IndependentSamplePack(new LocalSampleFiles(packA));var loader=new Loader();var r=new CircleSampleResources(map,null,pack,loader,new(14,false,false,true,true,"Default",true,System.Globalization.CultureInfo.InvariantCulture));r.Prepare([],[]);Eq(loader.Loaded.Count,12);
            foreach(int set in new[]{1,2,3})foreach(int bit in new[]{1,2,4,8}){int handle=r.Resolve(bit,set,0);Yes(handle>0,"prepared generated pack handle");var s=loader.Loaded[handle-101];Eq(s.Origin,SampleOrigin.Default);Eq(s.Mode,SampleLoadMode.EmbeddedBytes);Eq(Hash(s.Bytes),Hash(File.ReadAllBytes(Path.Combine(packA,s.Name+".wav"))));}
        });
        Add("W05 generated files differ from all twelve original fingerprints","E038 fingerprint-only comparison; original bytes never opened",()=>{
            using var doc=JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root,"re/evidence/38/hitsound-resources.json")));var store=doc.RootElement.GetProperty("embedded_standard_resources").EnumerateArray().Single(x=>x.GetProperty("assembly").GetString()=="osu!gameplay.dll");
            var original=store.GetProperty("keys").EnumerateObject().Select(x=>x.Value.GetProperty("sha256").GetString()).ToHashSet();foreach(string name in ExpectedPackNames())Yes(!original.Contains(Hash(File.ReadAllBytes(Path.Combine(packA,name)))),"independent synthesized fingerprint "+name);
        });
        Add("W06 generator refuses overwrite without Force","ENGINEERING immutable existing generated-input controls",()=>{
            string before=string.Join(',',ExpectedPackNames().Select(n=>Hash(File.ReadAllBytes(Path.Combine(packA,n)))));
            var start=new ProcessStartInfo(Environment.GetEnvironmentVariable("QA09_POWERSHELL")??"powershell.exe"){RedirectStandardOutput=true,RedirectStandardError=true,UseShellExecute=false,CreateNoWindow=true};
            foreach(string arg in new[]{"-NoProfile","-File",Path.Combine(root,"reconstructed/GenerateIndependentHitSounds.ps1"),"-PackDirectory",packA})start.ArgumentList.Add(arg);
            using var process=Process.Start(start)!;string stdout=process.StandardOutput.ReadToEnd(),stderr=process.StandardError.ReadToEnd();Yes(process.WaitForExit(30000),"bounded no-media generator run");Obs(new{process.ExitCode,stdout,stderr});Yes(process.ExitCode!=0,"overwrite rejected");Eq(before,string.Join(',',ExpectedPackNames().Select(n=>Hash(File.ReadAllBytes(Path.Combine(packA,n))))));
        });
    }
    static string Source(string name)=>File.ReadAllText(Path.Combine(root,name));
    static string Between(string source,string start,string end)
    {int a=source.IndexOf(start,StringComparison.Ordinal);Yes(a>=0,"source boundary "+start);int b=source.IndexOf(end,a+start.Length,StringComparison.Ordinal);Yes(b>a,"source boundary "+end);return source[a..b];}
    static void Ordered(string source,params string[] sites)
    {int from=0;foreach(string site in sites){int at=source.IndexOf(site,from,StringComparison.Ordinal);Yes(at>=0,"ordered source site "+site);from=at+site.Length;}}
    static void SourceBoundaries()
    {
        Add("A01 source binding positive hook pre-completion/accounting","SOURCE INSPECTION E012/E037; dynamically discriminated by I01",()=>{
            string s=Between(Source("reconstructed/CircleGame/Gameplay/CircleSession.cs"),"private void Judge(CircleState circle","private Judgement Classify");Ordered(s,"circle.Result = Classify","PositiveCircleSound?.Invoke","circle.JudgedAt = time","RecordWholeResult");
        });
        Add("A02 Windows OnCircle uses prepared lookup no resource I/O","SOURCE INSPECTION ENGINEERING adapter; selection executes in S16",()=>{
            string s=Between(Source("reconstructed/CircleGame/Platform/WindowsCircleEffects.cs"),"private void OnCircle(","private void Play(");Ordered(s,"prepared.Source.TimingError","prepared.Circles.TryGetValue","sample.Metadata.Error","CircleSampleSelection.Request");Yes(!s.Contains("File.",StringComparison.Ordinal)&&!s.Contains(".Open(",StringComparison.Ordinal),"no direct file/media opening on hook");
        });
        Add("A03 Windows async opens before Play boundary","SOURCE INSPECTION only; codecs/MediaPlayer not invoked",()=>{
            string s=Source("reconstructed/CircleGame/Platform/WindowsCircleEffects.cs");string prep=Between(s,"public static async Task<WindowsCircleEffects> PrepareAsync","public void Attach(");Ordered(prep,"await library.PrepareAsync","Task.Run","File.WriteAllBytes","await Task.WhenAll","await result.OpenAsync");Yes(!prep.Contains(".Play(",StringComparison.Ordinal),"no Play during preparation");Yes(prep.Contains("MediaOpened",StringComparison.Ordinal)&&prep.Contains("MediaFailed",StringComparison.Ordinal)&&prep.Contains("WaitAsync(TimeSpan.FromSeconds(15), token)",StringComparison.Ordinal),"bounded async media-opening/failure/cancel source");
        });
        Add("A04 Windows attach restart disposal bind production queue","SOURCE INSPECTION only; L01-L11 execute actual queue component",()=>{
            string s=Source("reconstructed/CircleGame/Platform/WindowsCircleEffects.cs");string attach=Between(s,"public void Attach(","private void OnCircle(");Ordered(attach,"PositiveCircleSound -= OnCircle","session = replacement","PositiveCircleSound += OnCircle");Yes(s.Contains("playback.Restart(); Attach(replacement);",StringComparison.Ordinal),"restart invalidates before attach");string close=s[s.IndexOf("public void Dispose()",StringComparison.Ordinal)..];Ordered(close,"PositiveCircleSound -= OnCircle","playback.Dispose()","Close(voice)","prepared.Dispose()");Yes(s.Contains("if (!value) playback.Pause();",StringComparison.Ordinal),"disable invalidates queue through wrapper");
        });
        Add("A05 application config placement malformed handling optional effects","SOURCE INSPECTION actual application; no window launched",()=>{
            string s=Source("reconstructed/CircleGame/Program.cs");Yes(s.Contains("Path.Combine(AppContext.BaseDirectory, \"hitsounds.json\")",StringComparison.Ordinal),"config beside executable");Yes(s.Contains("new HitSoundSettings { Enabled = false }",StringComparison.Ordinal),"malformed config disables optional effects");string load=Between(s,"private async Task LoadMapAsync(","private void KeyPressed(");Ordered(load,"Beatmap.Load(path)","ScoreConfiguration.FromBeatmap(map)","await candidateAudio.PrepareAsync","if (effectsConfigurationError is null)","WindowsCircleEffects.PrepareAsync","surface.Load(map");Yes(load.Contains("candidateEffects?.Dispose()",StringComparison.Ordinal)&&load.Contains("request.IsCancellationRequested",StringComparison.Ordinal),"canceled replacement cleanup source");
        });
        Add("A06 actual host fresh Load Restart Pause effects binding","SOURCE INSPECTION; fresh factory executed in I01-I11",()=>{
            string s=Source("reconstructed/CircleGame/Rendering/PlayfieldView.cs");string load=Between(s,"public void Load(","public void Restart()");Ordered(load,"CircleSession.CreateFreshOrdinary(map, scoring)","effects?.Dispose()","effects = preparedEffects","effects.Attach(replacement)","Session = replacement");string restart=Between(s,"public void Restart()","public void TogglePause()");Ordered(restart,"Pause()","CircleSession.CreateFreshOrdinary(Session.Map, Session.Scoring)","effects?.Restart(replacement)");string pause=Between(s,"public void Pause()","private void AudioChanged()");Ordered(pause,"clock.Pause()","audio?.Pause()","effects?.Pause()");
        });
        Add("A07 no slider spinner sound shortcut or native middleware claim","SOURCE INSPECTION scope boundaries; mixed session I08",()=>{
            string session=Source("reconstructed/CircleGame/Gameplay/CircleSession.cs");Eq(session.Split("PositiveCircleSound?.Invoke",StringSplitOptions.None).Length-1,1);string library=Source("reconstructed/CircleGame/Platform/LocalHitSoundAssets.cs");Yes(!library.Contains("DllImport",StringComparison.Ordinal)&&!library.Contains("osz2",StringComparison.Ordinal),"only admitted local providers, no original archive/native integration");
        });
        Add("A08 independent generator source has synthesis not asset reads","SOURCE INSPECTION generated-source provenance, W01-W06 execute bytes",()=>{
            string s=Source("reconstructed/GenerateIndependentHitSounds.ps1");Yes(s.Contains("[Math]::Sin",StringComparison.Ordinal)&&s.Contains("[IO.BinaryWriter]",StringComparison.Ordinal),"synthesized PCM source");Yes(!s.Contains("ReadAllBytes",StringComparison.Ordinal)&&!s.Contains("Copy-Item",StringComparison.Ordinal)&&!s.Contains("MediaPlayer",StringComparison.Ordinal),"no sample asset input or playback");
        });
        Add("A09 unavailable archive input cannot replace valid component session","ENGINEERING no .osz2 parser; remaining session remains usable",()=>{
            var m=Map();var session=Fresh(m);string path=Path.Combine(NewDirectory("archive"),"unsupported.osz2");File.WriteAllBytes(path,[0x50,0x4b,0x03,0x04]);Throws<InvalidDataException>(()=>Beatmap.Load(path));session.Press(m.Circles[0].Position,2000);Eq(session.Circles[0].Result,Judgement.Hit300);
        });
    }
}
