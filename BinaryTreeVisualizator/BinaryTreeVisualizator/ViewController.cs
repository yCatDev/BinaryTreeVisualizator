using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace BinaryTreeVisualizator
{
    public class ViewController: Component, IUpdatable
    {
        private VirtualButton _left, _right, _up, _down;
        
        public override void OnEnabled()
        {
            base.OnEnabled();
            _left = new VirtualButton();
            _left.AddKeyboardKey(Keys.Left);
            
            _right = new VirtualButton();
            _right.AddKeyboardKey(Keys.Right);
            
            _up = new VirtualButton();
            _up.AddKeyboardKey(Keys.Up);
            
            _down = new VirtualButton();
            _down.AddKeyboardKey(Keys.Down);
        }

        public void Update()
        {
            if (_up.IsDown)
            {
                Entity.Position = new Vector2(Entity.Position.X, Entity.Position.Y - 1);
            }
            if (_down.IsDown)
            {
                Entity.Position = new Vector2(Entity.Position.X, Entity.Position.Y + 1);
            }
            if (_left.IsDown)
            {
                Entity.Position = new Vector2(Entity.Position.X - 1, Entity.Position.Y);
            }
            if (_right.IsDown)
            {
                Entity.Position = new Vector2(Entity.Position.X + 1, Entity.Position.Y);
            }
        }
    }
}