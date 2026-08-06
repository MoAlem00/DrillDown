using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class EffectManager : IUpdatable,IDrawable
{
    private List<Animation> effects = new();
    
    public void Start() { }

    public void Update(GameTime gameTime)
    {
        foreach (var effect in effects)
        {
            effect.Update(gameTime);
        }
        effects.RemoveAll(effect => effect.IsFinished);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var effect in effects)
        {
            effect.Draw(spriteBatch);
        }
    }

    public void SpawnEffect(string effectName, Vector2 position)
    {
        Animation effect = new Animation(effectName);
        effect.sortingOrder = 0.9f;
        effect.anchor = Anchor.Center;
        effect.tm.position = position;
        effect.PlayAnimation(false,12);
        effects.Add(effect);
    }
}