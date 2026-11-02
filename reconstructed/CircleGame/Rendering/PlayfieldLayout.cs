using System.Numerics;

namespace Ohno.CircleGame.Rendering;

// E13/E14 normal-mode q=1 algebra inside an independently letterboxed 4:3 stage.
public readonly record struct PlayfieldLayout(float StageX, float StageY, float Width, float Height)
{
    public float Scale => Height / 480f;
    public Vector2 Offset => new((Width - 512f * Scale) / 2f,
        ((Height - 384f * Scale) / 4f) * 3f - 16f * Scale);
    public Vector2 ToScreen(Vector2 point) => point * Scale + Offset + new Vector2(StageX, StageY);
    public Vector2 ToMap(Vector2 point) => (point - new Vector2(StageX, StageY) - Offset) * (1f / Scale);
    public static PlayfieldLayout Fit(double width, double height)
    {
        float h = (float)Math.Max(1, Math.Min(height, width * 0.75));
        float w = h * 4f / 3f;
        return new(((float)width - w) / 2f, ((float)height - h) / 2f, w, h);
    }
}
