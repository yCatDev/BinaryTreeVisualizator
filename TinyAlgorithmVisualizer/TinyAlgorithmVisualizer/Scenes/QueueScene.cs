using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using TinyAlgorithmVisualizer.Algorithms.DataStructures;

namespace TinyAlgorithmVisualizer.Scenes
{
    public class QueueScene : BasicDemoScene
    {

        private Algorithms.DataStructures.LoopedQueue<int> _queue;
        private List<KeyValuePair<int, Entity>> _drawElements;
        private List<Entity> _lines;
        private bool _prevent = false;
        private int _id = 0;

        public override void Initialize()
        {
            base.Initialize();
            _queue = new Algorithms.DataStructures.LoopedQueue<int>(10);
            _drawElements = new List<KeyValuePair<int, Entity>>();
            _lines = new List<Entity>();
        }

        protected override void OnCommandEnter(TextField field)
        {
            if (_prevent) return;
            var cmd = field.GetText().Split(' ');
            switch (cmd[0].ToLower())
            {
                case "create":
                    if (!IsDigitsOnly(cmd[1])) return;
                    CreateQueue(int.Parse(cmd[1]));
                    break;
                case "push":
                    if (!IsDigitsOnly(cmd[1])) return;
                    AddElement(int.Parse(cmd[1]));
                    break;
                case "pop":
                    if (_queue.IsEmpty)
                    {
                        field.SetTextForced("Stack is empty!");
                        break;
                    }

                    RemoveElement();
                    break;
                case "count":
                    field.SetTextForced($"In stack {_queue.Count} elements");
                    break;
                case "menu":
                    Core.StartSceneTransition(new FadeTransition(() => new Menu()));
                    _prevent = true;
                    break;
            }
        }

        private void AddElement(int val)
        {
            _queue.Enqueue(val);
           // CreateElement(val, GetMissingIndex());
            RebuildStructure();
        }

        private void RemoveElement()
        {
            _queue.Dequeue();
            //if (!_tree.Contains(value)) return;
            _drawElements[0].Value.Transform.TweenScaleTo(Vector2.Zero, 0.5f).Start();
            _drawElements[0].Value.Destroy(1);
            _drawElements.RemoveAt(0);
            /*_drawElements[ind].Value.Transform.TweenScaleTo(Vector2.Zero, 0.5f).Start();
            _drawElements[ind].Value.Destroy(1);
            _drawElements.RemoveAt(ind);*/

            RebuildStructure();
        }

        private void CreateQueue(int size)
        {
            _queue = new LoopedQueue<int>(size);
            var step = 360 / size;
            var angle = 0f;
            for (var i = 0; i < size; i++)
            {
                var e = CreateElement("");
                angle += step;
                e.SetPosition(SetPositionAround(angle, size*25f));
            }
            Core.StartCoroutine(DrawAllLines());
        }

        private Vector2 SetPositionAround(float angle, float dist)
        {
            angle = (float) ((angle ) * (Math.PI/180)); // Convert to radians

            var rotatedX =(float)( Math.Cos(angle) * (0 - 0) - Math.Sin(angle) * (dist-0) + 0);

            var rotatedY = (float)( Math.Sin(angle) * (0 - 0) + Math.Cos(angle) * (dist - 0) + 0);
            
            return new Vector2(rotatedX,rotatedY);
        }
        
        private int GetMissingIndex()
        {
            var tmp = _queue.ToArray();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (i > _drawElements.Count - 1 || _drawElements[i].Key != tmp[i])
                    return i;
            }

            return tmp.Length - 1;
        }

        private void RemoveAllLines()
        {
            foreach (var element in _lines)
            {
                if (element.HasComponent<LineRenderer>())
                    element.GetComponent<LineRenderer>().ClearPoints();
                element.Destroy();
            }
        }

        private IEnumerator DrawAllLines()
        {
            yield return Coroutine.WaitForSeconds(0.5f);
            for (var i = 1; i < _drawElements.Count; i++)
            {
                var current = _drawElements[i];
                var prev = _drawElements[i - 1];
                    
                var lineEntity = CreateEntity("Line", new Vector2(Screen.Width / 2f, Screen.Height / 2f));
                // lineEntity.Transform.Parent = _domain.Transform;
                lineEntity.LocalPosition = Vector2.Zero;
                var line = lineEntity.AddComponent<LineRenderer>();
                line.LayerDepth = 1;
                line.RenderLayer = 999;
                

                var from = prev.Value.Position;
                var to = current.Value.Position;


                line.AddPoint(@from, 3);
                line.AddPoint(to, 3);
                
                _lines.Add(lineEntity);
            }
        }

        private Entity CreateElement(string val)
        {
            var element = CreateEntity("StackElement" + val).AddComponent(new DrawElement(val, false));
            element.Transform.Parent = Domain.Transform;
            element.Transform.LocalPosition = new Vector2(100, -Screen.Height / 2f);
            var scaleTo = new Vector2(0.75f, 0.75f);
            element.Transform.Scale = Vector2.Zero;
            element.Transform.TweenScaleTo(scaleTo, 0.5f).Start();

            if (!string.IsNullOrEmpty(val))
                _drawElements.Add(new KeyValuePair<int, Entity>(int.Parse(val), element.Entity));

            return element.Entity;
        }

        private void RebuildStructure()
        {

            RemoveAllLines();
            var tmp = _queue.ToArray();
            for (int i = 0; i < tmp.Length; i++)
            {
                //Console.WriteLine(tmp[i]);

                var current = _drawElements[i].Value;
                //current = !_drawElements.ContainsKey(v) ?  : _drawElements[v];
                current.Transform.TweenLocalPositionTo(new Vector2(0, i * 100), 0.5f).Start();
            }

            //Малюємо звязуючи лінії
            Core.StartCoroutine(DrawAllLines());
            //Центруємо дерево на єкрані
            //Domain.Position = new Vector2(Screen.Width / 2f - (tmp.Length * 50), Screen.Height / 2f);
        }
    }

}