using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace TinyAlgorithmVisualizer
{
    public class ViewController: Component, IUpdatable
    {
        private VirtualButton _left, _right, _up, _down;
        private bool _wasClicked;
        private Vector2 _startPos;
        private Vector2 _lastMousePos;

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
            if (!Input.RightMouseButtonDown) return;

            var mousePos = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
            if (Vector2.Distance(_lastMousePos, mousePos) > 1f)
            {
                var step = (_lastMousePos - mousePos);
                step.Normalize();
                Transform.Position+=step*10;
                _lastMousePos = mousePos;
            }

        }
    }
}