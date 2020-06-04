using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using TinyAlgorithmVisualizer.Algorithms.DataStructures;
using TinyAlgorithmVisualizer.Algorithms.Greedy;
using TinyAlgorithmVisualizer.Algorithms.Tree;

namespace TinyAlgorithmVisualizer.Scenes
{
    public class HuffmanScene: BasicDemoScene
    {
        private Huffman _huffman;
        private HuffmanTreeNode _tree;
        private List<KeyValuePair<string, Entity>> _treeElements;
        private Dictionary<string, int> _garbage = new Dictionary<string, int>();
        private List<Entity> _lines;

        protected override void AfterStartup()
        {
            base.AfterStartup();

            _lines = new List<Entity>();
            _treeElements = new List<KeyValuePair<string, Entity>>();
            _huffman = new Huffman();
        }

        protected override void OnCommandEnter(TextField field)
        {
            var cmd = field.GetText();
            Process(cmd);
        }

        
        private void Process(string str)
        {
            _huffman = new Huffman();
            _tree = (_huffman.Process(str));
            //_tree.PreOrder((x)=>CreateElement(x));
            RebuildTree();
        }
        

        private void RebuildTree()
        {
            /*
             * Принцип роботи оснований на особливості бінарного дерева пошуку:
             * воно не може мати одинакових елементів
             * Отже потрібно створити словник який матиме в ключах значення дерева а в значеннях - об'екти
             */
            RemoveAllLines(); //Видаляємо всі лінії
            foreach (var item in _treeElements)
            {
                item.Value.Destroy();
            }
            _garbage.Clear();
            Console.WriteLine("==========");
            var t = 0;
            _tree.Draw((x, y, v,oldv) =>
            {
                //Якщо в словнику є елемент з таким значенням то використовуємо його якщо ж ні створюємо новий 
                var current = CreateElement(v);
                //Запускаємо переміщення ноди в нову позицію
                current.Transform.TweenLocalPositionTo(new Vector2(x * 50, y * 50), 0.5f).Start();
                Entity elem = current, parent = null;

                

                if (oldv != null && _treeElements.FirstOrDefault(e=>e.Key==oldv).Value!=null)
                {
                    parent = _treeElements.FirstOrDefault(e=>e.Key==oldv).Value;
                    var f = _garbage.TryGetValue(parent.Name, out var res);
                    /*if (f)
                        Console.WriteLine($"{parent.Name} {res}");*/
                    if (_garbage.ContainsKey(parent.Name))
                    {
                        _garbage[parent.Name]++;
                    }else  _garbage.Add(parent.Name, 1);
                }

                Core.StartCoroutine(DrawLine(elem, parent, t++));
            }, _tree.Length*2); //Викликаємо відрисовку*/
            
            _tree = new HuffmanTreeNode();
        }

        private void Remove(string name)
        {
            for (int i = 0; i < _treeElements.Count; i++)
            {
                var item = _treeElements[i];
                if (item.Value.Name == name)
                {
                    _treeElements[i]=new KeyValuePair<string, Entity>("",item.Value);
                    //Console.WriteLine("Removed " + item.Value.Name);
                    break;
                }
            }
        }

        private IEnumerator DrawLine(Entity elem, Entity parent, int t)
        {
            if (elem != null && parent != null)
            {
                //Console.WriteLine($"Connect {elem.Name} with {parent.Name}");
                var parentPos = new Vector2(parent.Position.X, parent.Position.Y);
                if (_garbage.ContainsKey(parent.Name) && _garbage[parent.Name] > 1)
                    Remove(parent.Name);
                yield return Coroutine.WaitForSeconds(0.5f /*+ (t * 0.5f)*/);

                var lineEntity = CreateEntity("Line", new Vector2(Screen.Width / 2f, Screen.Height / 2f));
                // lineEntity.Transform.Parent = _domain.Transform;
                lineEntity.LocalPosition = Vector2.Zero;
                var line = lineEntity.AddComponent<LineRenderer>();
                line.LayerDepth = 1;
                line.RenderLayer = 999;

                //line.SetUseWorldSpace(false);

                var from = elem.Position;
                var to = parent.Position;


                line.AddPoint(from, 3);
                line.AddPoint(to, 3);

                //line.SetStartEndColors(new Color(61, 9, 107),new Color(61, 9, 107));

                //Console.WriteLine($"{v} {parent.Value}");
                _lines.Add(lineEntity);

            }
        }


        private int id = 0;
        private Entity CreateElement(string val)
        {
            var element = CreateEntity("TreeElement"+id.ToString()).AddComponent(new DrawElement(val.ToString()));
            id++;
            element.Transform.Parent = Domain.Transform;
            element.Transform.LocalPosition = new Vector2(100, -Screen.Height/2 );
            var scaleTo = new Vector2(0.75f, 0.75f);
            element.Transform.Scale = Vector2.Zero;
            element.Transform.TweenScaleTo(scaleTo, 0.5f).Start();
                
            _treeElements.Add(new KeyValuePair<string,Entity>(val, element.Entity));
            
            return element.Entity;
        }

        /*private void AddElement(string value)
        {
            //if (_tree.Contains(value)) return;
            
            _tree.Add(value);
            CreateElement(value);
            RebuildTree();
        }*/
        
        private void RemoveAllLines()
        {
            foreach (var element in _lines)
            {
                if (element.HasComponent<LineRenderer>())
                    element.GetComponent<LineRenderer>().ClearPoints();
                element.Destroy();
            }
        }

        
    }
}