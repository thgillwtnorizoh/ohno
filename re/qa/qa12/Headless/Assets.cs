using System.Buffers.Binary;
using System.Text;
using System.Text.Json;
namespace Ohno.QA12;
public static partial class Tests
{
    static void Assets()
    {
        if(packA.Length==0)return; // Supplemental component-only preflight; authoritative wrapper always supplies both packs.
        foreach(var (name,frames) in new[]{("spinnerspin.wav",22050),("spinnerbonus.wav",13230)})Add("W01 RIFF structure "+name,"ENGINEERING independent asset;not decoder/audio proof",()=>{
            var b=File.ReadAllBytes(Path.Combine(packA,name));Eq(b.Length,44+2*frames);Eq(Encoding.ASCII.GetString(b,0,4),"RIFF");Eq(BinaryPrimitives.ReadUInt32LittleEndian(b.AsSpan(4)),(uint)b.Length-8);Eq(Encoding.ASCII.GetString(b,8,4),"WAVE");int pos=12;var ids=new List<string>();while(pos<b.Length){Yes(pos+8<=b.Length,"bounded chunk header");uint size=BinaryPrimitives.ReadUInt32LittleEndian(b.AsSpan(pos+4));long end=(long)pos+8+size;Yes(end<=b.Length,"bounded chunk data");string id=Encoding.ASCII.GetString(b,pos,4);ids.Add(id);if(id=="fmt "){Eq(size,16u);Eq(BinaryPrimitives.ReadUInt16LittleEndian(b.AsSpan(pos+8)),(ushort)1);Eq(BinaryPrimitives.ReadUInt16LittleEndian(b.AsSpan(pos+10)),(ushort)1);Eq(BinaryPrimitives.ReadUInt32LittleEndian(b.AsSpan(pos+12)),44100u);Eq(BinaryPrimitives.ReadUInt32LittleEndian(b.AsSpan(pos+16)),88200u);Eq(BinaryPrimitives.ReadUInt16LittleEndian(b.AsSpan(pos+20)),(ushort)2);Eq(BinaryPrimitives.ReadUInt16LittleEndian(b.AsSpan(pos+22)),(ushort)16);}if(id=="data"){Eq(size,(uint)(2*frames));Eq(size/2,(uint)frames);Near(size/88200d,name=="spinnerspin.wav"?.5:.3);Yes(b.AsSpan(pos+8,(int)size).ToArray().Any(v=>v!=0),"nonempty waveform");}pos=checked((int)end+(int)(size&1));}Eq(pos,b.Length);Seq(ids,"fmt ","data");evidence.Add(new{name,sha256=Hash(b),frames});
        },"Independent asset control");
        Add("W02 two deterministic generator invocations","ENGINEERING same host deterministic output",()=>{foreach(string name in new[]{"spinnerspin.wav","spinnerbonus.wav"})Eq(Hash(File.ReadAllBytes(Path.Combine(packA,name))),Hash(File.ReadAllBytes(Path.Combine(packB,name))));},"Independent asset control");
        Add("W03 all21 prior fingerprints preserved","ENGINEERING generated before spinner addition;QA11 historical manifest",()=>{using var old=JsonDocument.Parse(Read("re/qa/qa11/verification/assets.json"));foreach(string pack in new[]{packA,packB}){var before=JsonSerializer.Deserialize<Dictionary<string,string>>(File.ReadAllText(Path.Combine(pack,"baseline21.json")))!;Eq(before.Count,21);foreach(var historical in old.RootElement.GetProperty("actual_loops").EnumerateArray().Concat(old.RootElement.GetProperty("previous_fifteen").EnumerateArray()))Eq(Hash(File.ReadAllBytes(Path.Combine(pack,historical.GetProperty("file").GetString()!))),historical.GetProperty("sha256").GetString());foreach(var kv in before)Eq(Hash(File.ReadAllBytes(Path.Combine(pack,kv.Key))),kv.Value);Eq(Directory.GetFiles(pack,"*.wav").Length,23);}},"Independent asset control");
    }
}
