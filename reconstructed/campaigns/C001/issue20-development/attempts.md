Issue20 development attempts, 2026-11-22. No original fixture, oracle, historical
suite or original research was edited.

1. Initial harness compilation referenced nonexistent Audit Outcome.Message,
then Outcome.Reason. Outcome deliberately exposes only status/code/exception type.
Both compiler logs are retained. Direct canonical exception message remains checked
exactly; audit code/type are checked separately and recorded for each case.
2. First execution 5158/5161: copied consumer assertion wrongly required more sound
callbacks than traversals for a genuinely admitted inherited-timing R32 slider.
Existing pending/due-prefix semantics can leave callbacks pending. The corrected
check requires emitted actual events, real parent/resource identities and unchanged
E17 successes/pending grade, without a fabricated callback lower bound. A multi-parent
input did not set NewCombo and a maximum-shape generator emitted95 normalized points
because its first converted point equaled the head. Corrected DEVELOPMENT data uses
explicit NewCombo and96 supplied points with equal first point. Initial test code
and full results preserved as first-*.
3. Expanded first execution 5259/5260: stack positive used a slider-head start gap
outside Route M eligibility and a nonmatching odd-traversal tail. Corrected positive
uses two short even-traversal L96 sliders within the existing stacking threshold;
source/raw/prepared owner translation is checked without modifying stacking code.
Expanded-first-results.json and local-expanded.log preserve the failure.
4. First retained CorpusAudit run could not spawn dotnet because only its absolute
path was invoked and PATH/DOTNET_HOST_PATH were unset. Rerun with explicit host path
passed295/295. Original failed log is retained, not a production discrepancy.
5. Local full modified-source gate5429/5429; exact-source Windows gate still pending
at this introducing publication. Independent Work1 QA is not claimed.

6. First exact Windows run45001890246 at585c624: new headless5430/5430 PASS,
all19 historical summaries2312/2312 PASS and other retained suites passed. New
WPF-only verifier failed on its first unfilled StreamGeometry: native
GetFlattenedPathGeometry returned no point figures, then verifier indexed an empty
array. Executable publication was correctly skipped. Failed logs, original Windows
verifier and headless result packet are retained. No production defect inferred.
Official WPF sources distinguish native flattening from managed serialized-path
readback: StreamGeometry.GetAsPathGeometry uses PathStreamGeometryContext and
ParsePathGeometryData; public PathGeometry.CreateFromGeometry delegates to that
readback. Verifier now checks the actual retained constructor figures/points,
unfilled/open flags and stroked segments via that API, with an explicit empty
check before indexing. Renderer BuildPath remains byte-identical. Windows must
rerun all gates; this correction is not a successful Windows result.
Sources inspected (2026-11-22): dotnet/wpf PresentationCore/System/Windows/Media/
{Geometry.cs,StreamGeometry.cs,PathGeometry.cs}; raw URLs under
https://github.com/dotnet/wpf/tree/main/src/Microsoft.DotNet.Wpf/src/PresentationCore/System/Windows/Media
