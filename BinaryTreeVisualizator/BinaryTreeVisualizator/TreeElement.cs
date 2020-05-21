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
        
        public TreeElement(int value)
        {
            _value = value.ToString();
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            
            _texture = Entity.Scene.Content.Load<Texture2D>(Content.Circle);
            _spriteRenderer = Entity.AddComponent(new SpriteRenderer(_texture));
            _spriteRenderer.RenderLayer = 0;
            _textLabel = Entity.Scene.CreateEntity("TextLabel");
            _textLabel.Parent = Entity.Transform;


            _textComponent = _textLabel.AddComponent<TextComponent>();
            _textComponent.SetFont(Entity.Scene.Content.Load<IFont>(Content.DefaultTitleFont));
            _textComponent.Text = _value;
            _textComponent.Color = Color.Black;
            _textComponent.Origin = new Vector2(_textComponent.Bounds.Width/2f,_textComponent.Bounds.Height/2f);
       
            //_textLabel.Position =  new Vector2(_spriteRenderer.Bounds.Width/2f,_spriteRenderer.Bounds.Height/2f);
            _textLabel.SetScale(1);
            
        }

        public void Update()
        {
            
        }
    }
}