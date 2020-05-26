using System.Collections;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Color = Microsoft.Xna.Framework.Color;

namespace BinaryTreeVisualizator
{
    public class TreeElement: Component, IUpdatable
    {

        private readonly string _value;
        
        private Texture2D _texture;
        private SpriteRenderer _spriteRenderer;

        private Entity _textLabel;
        private TextComponent _textComponent;
        private SpriteOutlineRenderer _outlineRenderer;

        public TreeElement(int value)
        {
            _value = value.ToString();
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            
            _texture = Entity.Scene.Content.Load<Texture2D>(Content.Circle);
            _spriteRenderer = Entity.AddComponent(new SpriteRenderer(_texture));
            _spriteRenderer.LayerDepth = 0;
            _spriteRenderer.RenderLayer = 1;
            _spriteRenderer.Transform.SetScale(0.75f);
            _textLabel = Entity.Scene.CreateEntity("TextLabel");
            _textLabel.Parent = Entity.Transform;

            _outlineRenderer = Entity.AddComponent(new SpriteOutlineRenderer(_spriteRenderer)
            {
                OutlineColor = new Color(61, 9, 107),
                OutlineWidth = 10
            });
            _outlineRenderer.RenderLayer = 9999;

            _textComponent = _textLabel.AddComponent<TextComponent>();
            _textComponent.SetFont(Entity.Scene.Content.Load<IFont>(Content.DefaultTitleFont));
            _textComponent.VerticalOrigin = VerticalAlign.Center;
            _textComponent.HorizontalOrigin = HorizontalAlign.Center;
            _textComponent.Text = _value;
            _textComponent.LayerDepth = 0;
            _textComponent.Color = Color.Black;
            _textComponent.Transform.SetScale(0.5f);
            _textComponent.Transform.Parent = Entity.Transform;
            _textComponent.Transform.LocalPosition  = new Vector2(_spriteRenderer.Bounds.Width/6f-_textComponent.Bounds.Width
                ,_spriteRenderer.Bounds.Height/2f-_textComponent.Bounds.Height);
            //_textComponent.Transform.LocalPosition = Vector2.Zero;


        }
        public void Highlight(int seconds)
        {
            Core.StartCoroutine(HighlightIt(seconds));
        }

        private IEnumerator HighlightIt(int seconds)
        {
            var col = _outlineRenderer.OutlineColor;
            _outlineRenderer.OutlineColor = Color.Red;
            yield return Coroutine.WaitForSeconds(seconds);
            _outlineRenderer.OutlineColor = col;
        }
        
        public void Update()
        {
            
        }
    }
}