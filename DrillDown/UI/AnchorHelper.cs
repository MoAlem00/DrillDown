using Microsoft.Xna.Framework;

namespace DrillDown;

public enum Anchor
{
    TopLeft, TopCenter, TopRight,
    CenterLeft, Center, CenterRight, 
    BottomLeft, BottomCenter, BottomRight
}
public static class AnchorHelper
{
    public static Vector2 GetOrigin(Anchor anchor,Vector2 size)
    {
        float x = anchor switch
        {
            Anchor.TopLeft or Anchor.CenterLeft or Anchor.BottomLeft => 0,
            Anchor.TopCenter or Anchor.Center or Anchor.BottomCenter => size.X * 0.5f,
            _ => size.X
        };
        float y = anchor switch
        {
            Anchor.TopLeft or Anchor.TopCenter or Anchor.TopRight => 0,
            Anchor.CenterLeft or Anchor.Center or Anchor.CenterRight => size.Y * 0.5f,
            _ => size.Y
        };
        return new Vector2(x, y);
        
    }
}