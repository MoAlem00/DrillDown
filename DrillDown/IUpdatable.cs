using Microsoft.Xna.Framework;

namespace DrillDown;

public interface IUpdatable
{
    void Start();
    void Update(GameTime gameTime);
}