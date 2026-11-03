using Ohno.CircleGame.Gameplay;

namespace Ohno.CircleGame.Rendering;

// ENGINEERING visibility policy: no circle-style approach preview for spinners.
// Native spinner fade/transform timing remains unrecovered. This is read-only and
// must never become an input-admission, held-state or judgement gate.
public static class SpinnerPresentation
{
    public static bool IsVisible(SpinnerState spinner, int time) =>
        spinner.Result == Judgement.Pending && time >= spinner.Definition.StartTime;
}
