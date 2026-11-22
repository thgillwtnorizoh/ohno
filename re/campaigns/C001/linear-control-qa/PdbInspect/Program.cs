using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
// Read the delivered portable PDB, not the development verifier's JSON claim.
using var pe=new PEReader(File.OpenRead(args[0]));
using var provider=MetadataReaderProvider.FromPortablePdbStream(File.OpenRead(args[1]));
var reader=provider.GetMetadataReader();var id=reader.DebugMetadataHeader!.Id;
var entries=pe.ReadDebugDirectory();var cv=entries.Single(x=>x.Type==DebugDirectoryEntryType.CodeView);var codeView=pe.ReadCodeViewDebugDirectoryData(cv);
bool identity=codeView.Guid==new Guid(id.Take(16).ToArray())&&cv.Stamp==BitConverter.ToUInt32(id.Skip(16).Take(4).ToArray())&&codeView.Age==1;
if(!identity)throw new InvalidDataException("Delivered PDB does not match bundled DLL CodeView identity.");
var docs=new List<object>();
foreach(var h in reader.Documents){var d=reader.GetDocument(h);string name=reader.GetString(d.Name).Replace('\\','/');
 if(!name.EndsWith("/Gameplay/Beatmap.cs",StringComparison.Ordinal))continue;
 string hash=Convert.ToHexString(reader.GetBlobBytes(d.Hash));byte[] source=File.ReadAllBytes(Path.Combine(args[2],"reconstructed/CircleGame/Gameplay/Beatmap.cs"));
 string lf=System.Text.Encoding.UTF8.GetString(source).Replace("\r\n","\n"),crlf=lf.Replace("\n","\r\n");
 bool match=new[]{lf,crlf}.Any(s=>Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(s)))==hash);
 if(d.HashAlgorithm.IsNil||reader.GetGuid(d.HashAlgorithm)!=new Guid("8829d00f-11b8-4213-878b-770e8597ac16")||!match)throw new InvalidDataException("Compiled source checksum mismatch.");
 docs.Add(new{name,sourceSha256=hash,match});
}
if(docs.Count!=1)throw new InvalidDataException("Expected one compiled Beatmap document.");
File.WriteAllText(args[3],JsonSerializer.Serialize(new{bundledDllSha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[0]))),publishedPdbSha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[1]))),codeView.Guid,cv.Stamp,codeView.Age,identity,documents=docs,originalExecuted=false,WindowsExecuted=false},new JsonSerializerOptions{WriteIndented=true}));
Console.WriteLine("PASS actual delivered portable PDB ID/GUID/stamp and compiled Beatmap SHA256 match bundled DLL and exact source");
