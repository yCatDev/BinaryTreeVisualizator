using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace TinyAlgorithmVisualizer
{
    public class LineComponent: RenderableComponent
    {
        public override void Render(Batcher batcher, Camera camera)
        {
            DrawLine(batcher, Vector2.Zero, new Vector2(100,100), Color.Green, 10);
        }
        
        private void DrawLine(Batcher batcher, Vector2 start, Vector2 end, Color color, float thickness = 2f)
        {
            var delta = end - start;
            var angle = (float) Math.Atan2(delta.Y, delta.X);
            batcher.Draw(Graphics.Instance.PixelTexture, start + Entity.Transform.Position + LocalOffset,
                Graphics.Instance.PixelTexture.SourceRect, color, angle, new Vector2(0, 0.5f),
                new Vector2(delta.Length(), thickness), SpriteEffects.None, LayerDepth);
        }
    }
}