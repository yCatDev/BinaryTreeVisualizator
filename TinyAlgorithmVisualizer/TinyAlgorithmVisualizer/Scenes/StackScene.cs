using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace TinyAlgorithmVisualizer.Scenes
{
    public class StackScene : BasicDemoScene
    {
        private Algorithms.DataStructures.Stack<int> _stack;
        private List<KeyValuePair<int, Entity>> _drawElements;
        private List<Entity> _lines;
        private bool _prevent = false;
        private int _id = 0;

        public override void Initialize()
        {
            base.Initialize();
            _stack = new Algorithms.DataStructures.Stack<int>();
            _drawElements = new List<KeyValuePair<int, Entity>>();
            _lines = new List<Entity>();
        }

        protected override void OnCommandEnter(TextField field)
        {
            if (_prevent) return;
            var cmd = field.GetText().Split(' ');
            switch (cmd[0].ToLower())
            {
                case "push":
                    if (!IsDigitsOnly(cmd[1])) return;
                    AddElement(int.Parse(cmd[1]));
                    break;
                case "pop":
                    if (_stack.IsEmpty)
                    {
                        field.SetTextForced("Stack is empty!");
                        break;
                    }
                    RemoveElement();
                    break;
                case "count":
                    field.SetTextForced($"In stack {_stack.Count} elements");
                    break;
                case "menu":
                    Core.StartSceneTransition(new FadeTransition(() => new Menu()));
                    _prevent = true;
                    break;
            }
        }

        private void AddElement(int val)
        {
            _stack.Push(val);
            CreateElement(val, GetMissingIndex());
            RebuildStructure();
        }
        private void RemoveElement()
        {
            _stack.Pop();
            //if (!_tree.Contains(value)) return;
            _drawElements[0].Value.Transform.TweenScaleTo(Vector2.Zero, 0.5f).Start();
            _drawElements[0].Value.Destroy(1);
            _drawElements.RemoveAt(0);
            /*_drawElements[ind].Value.Transform.TweenScaleTo(Vector2.Zero, 0.5f).Start();
            _drawElements[ind].Value.Destroy(1);
            _drawElements.RemoveAt(ind);*/

            RebuildStructure();
        }

        private int GetMissingIndex()
        {
            var tmp = _stack.ToArray();
            for (int i = 0; i < tmp.Length; i++)
            {
                if ( i>_drawElements.Count-1|| _drawElements[i].Key != tmp[i])
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
            Entity last = default;
            foreach (var item in _drawElements)
            {
                if (last == null)
                {
                    last = item.Value;
                    continue;
                }
                var lineEntity = CreateEntity("Line", new Vector2(Screen.Width / 2f, Screen.Height / 2f));
                // lineEntity.Transform.Parent = _domain.Transform;
                lineEntity.LocalPosition = Vector2.Zero;
                var line = lineEntity.AddComponent<LineRenderer>();
                line.LayerDepth = 1;
                line.RenderLayer = 999;

                //line.SetUseWorldSpace(false);

                var from = last.Position;
                var to = item.Value.Position;


                line.AddPoint(from, 3);
                line.AddPoint(to, 3);

                //line.SetStartEndColors(new Color(61, 9, 107),new Color(61, 9, 107));

                //Console.WriteLine($"{v} {parent.Value}");
                _lines.Add(lineEntity);
            }
        }

        private Entity CreateElement(int val, int i)
        {
            var element = CreateEntity("StackElement" + val).AddComponent(new DrawElement(val, false));
            element.Transform.Parent = Domain.Transform;
            element.Transform.LocalPosition = new Vector2(100, -Screen.Height / 2f);
            var scaleTo = new Vector2(0.75f, 0.75f);
            element.Transform.Scale = Vector2.Zero;
            element.Transform.TweenScaleTo(scaleTo, 0.5f).Start();


            _drawElements.Insert(i, new KeyValuePair<int,Entity>(val, element.Entity));

            return element.Entity;
        }

        private void RebuildStructure()
        {
           
            RemoveAllLines();
            var tmp = _stack.ToArray();
            for (int i = 0; i < tmp.Length; i++)
            {
                //Console.WriteLine(tmp[i]);
                
                var current = _drawElements[i].Value;
                //current = !_drawElements.ContainsKey(v) ?  : _drawElements[v];
                current.Transform.TweenLocalPositionTo(new Vector2(0, i*100), 0.5f).Start();
            } 

            //Малюємо звязуючи лінії
            Core.StartCoroutine(DrawAllLines());
            //Центруємо дерево на єкрані
            //Domain.Position = new Vector2(Screen.Width / 2f - (tmp.Length * 50), Screen.Height / 2f);
        }
    }
}