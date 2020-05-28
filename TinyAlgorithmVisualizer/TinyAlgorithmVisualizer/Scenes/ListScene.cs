using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using TinyAlgorithmVisualizer.Algorithms.DataStructures;

namespace TinyAlgorithmVisualizer.Scenes
{
    public class ListScene : BasicDemoScene
    {
        private Algorithms.DataStructures.MyList<int> _list;
        private List<KeyValuePair<int, Entity>> _drawElements;
        private List<Entity> _lines;
        private bool _prevent = false;
        private int _id = 0;

        public override void Initialize()
        {
            base.Initialize();
            _list = new Algorithms.DataStructures.MyList<int>();
            _drawElements = new List<KeyValuePair<int, Entity>>();
            _lines = new List<Entity>();
        }

        protected override void OnCommandEnter(TextField field)
        {
            if (_prevent) return;
            var cmd = field.GetText().Split(' ');
            switch (cmd[0].ToLower())
            {
                case "add":
                    switch (cmd[1].ToLower())
                    {
                        case "front":
                            if (!IsDigitsOnly(cmd[2])) return;
                            AddElement(true,int.Parse(cmd[2]));
                            break;
                        case "back":
                            if (!IsDigitsOnly(cmd[2])) return;
                            AddElement(false,int.Parse(cmd[2]));
                            break;
                    }
                   
                    break;
                case "remove":
                    if (_list.IsEmpty)
                    {
                        field.SetTextForced("List is empty!");
                        break;
                    }
                    if (!IsDigitsOnly(cmd[1])) return;
                    
                    RemoveElement(int.Parse(cmd[1]));
                    break;
                case "removeat":
                    if (_list.IsEmpty)
                    {
                        field.SetTextForced("List is empty!");
                        break;
                    }
                    if (!IsDigitsOnly(cmd[1])) return;
                    var ind = int.Parse(cmd[1]);
                    RemoveElementByInd(ind);
                    break;
                case "count":
                    field.SetTextForced($"In stack {_list.Count} elements");
                    break;
                case "search":
                    var i = _list.IndexOf(int.Parse(cmd[1]));
                    if (i.HasValue)
                    {
                        _drawElements[i.Value].Value.GetComponent<DrawElement>().Highlight(3);
                    }else  field.SetTextForced($"Item not found");

                    break;
                case "clear":
                    Clear();
                    break;
                case "swap":
                    _list.SwapCorners();
                    RebuildStructure();
                    break;
                
                case "menu":
                    Core.StartSceneTransition(new FadeTransition(() => new Menu()));
                    _prevent = true;
                    break;
            }
        }

        private void Clear()
        {
            var tmp = _list.ToArray();
            foreach (var item in tmp)
            {
                _list.RemoveAt(0);
                _drawElements[0].Value.Transform.TweenScaleTo(Vector2.Zero, 0.5f).Start();
                _drawElements[0].Value.Destroy(1);
                _drawElements.RemoveAt(0);
            }
            _list = new MyList<int>();
            RemoveAllLines();
        }
        
        private void AddElement(bool front, int val)
        {
            if (front)
                _list.AddToFront(val);
            else
                _list.AddToEnd(val);
            CreateElement(val, GetMissingIndex());
            RebuildStructure();
        }

        private void RemoveElement(int val)
        {
            var ind = _list.IndexOf(val);
           
            if (ind == null)
                return;
            _list.RemoveAt(ind.Value);
            if (_list.IsEmpty)
                _list = new MyList<int>();
            _drawElements[ind.Value].Value.Transform.TweenScaleTo(Vector2.Zero, 0.5f).Start();
            _drawElements[ind.Value].Value.Destroy(1);
            _drawElements.RemoveAt(ind.Value);

            RebuildStructure();
        }

        private void RemoveElementByInd(int ind)
        {
            if (!_list.RemoveAt(ind)) return;
            if (_list.IsEmpty)
                _list = new MyList<int>();
            _drawElements[ind].Value.Transform.TweenScaleTo(Vector2.Zero, 0.5f).Start();
            _drawElements[ind].Value.Destroy(1);
            _drawElements.RemoveAt(ind);

            RebuildStructure();
        }
        
        private int GetMissingIndex()
        {
            var tmp = _list.ToArray();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (i > _drawElements.Count - 1 || _drawElements[i].Key != tmp[i])
                    return i;
            }

            return tmp.Length;
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
            var element = CreateEntity("StackElement" + val).AddComponent(new DrawElement(val.ToString(), false));
            element.Transform.Parent = Domain.Transform;
            element.Transform.LocalPosition = new Vector2(100, -Screen.Height / 2f);
            var scaleTo = new Vector2(0.75f, 0.75f);
            element.Transform.Scale = Vector2.Zero;
            element.Transform.TweenScaleTo(scaleTo, 0.5f).Start();


            _drawElements.Insert(i, new KeyValuePair<int, Entity>(val, element.Entity));

            return element.Entity;
        }

        private void RebuildStructure()
        {

            RemoveAllLines();
            var tmp = _list.ToArray();
            if (tmp.Length == 0)
                return;
            for (int i = 0; i < tmp.Length; i++)
            {
                //Console.WriteLine(tmp[i]);
                var current = i>=_drawElements.Count ? CreateElement(tmp[i], i):_drawElements[i].Value;
                
                if (i < _drawElements.Count)
                    current.GetComponent<DrawElement>().SetText(tmp[i].ToString());
                current.Transform.TweenLocalPositionTo(new Vector2(i*100, 0), 0.5f).Start();
            }

            //Малюємо звязуючи лінії
            Core.StartCoroutine(DrawAllLines());
            //Центруємо дерево на єкрані
            //Domain.Position = new Vector2(Screen.Width / 2f - (tmp.Length * 50), Screen.Height / 2f);
        }
    }
}