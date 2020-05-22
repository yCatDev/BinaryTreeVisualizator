using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace BinaryTreeVisualizator
{
    public class ViewController: Component, IUpdatable
    {
        private VirtualButton _left, _right, _up, _down;
        private bool _wasClicked;
        private Vector2 _startPos;
        private Vector2 dragOrigin;

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
            /*if (_up.IsDown)
            {
                Entity.Position = new Vector2(Entity.Position.X, Entity.Position.Y - 10);
            }
            if (_down.IsDown)
            {
                Entity.Position = new Vector2(Entity.Position.X, Entity.Position.Y + 10);
            }
            if (_left.IsDown)
            {
                Entity.Position = new Vector2(Entity.Position.X - 10, Entity.Position.Y);
            }
            if (_right.IsDown)
            {
                Entity.Position = new Vector2(Entity.Position.X + 10, Entity.Position.Y);
            }*/
            if (Input.RightMouseButtonPressed)
            {
                dragOrigin = Input.MousePosition;
                return;
            }
 
            if (!Input.RightMouseButtonDown) return;
 
            var pos =Entity.Scene.Camera.ScreenToWorldPoint(Input.MousePosition - dragOrigin);
            pos.Ceiling();
            pos.X = Math.Clamp(pos.X, -170, 170);
            pos.Y = Math.Clamp(pos.Y, -170, 170);
            Console.WriteLine(pos);
            var move = -new Vector2(pos.X * 0.01f, pos.Y * 0.01f);

            Transform.Position += move;

        }
    }
}