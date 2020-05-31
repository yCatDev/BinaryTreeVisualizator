using System.Collections;
using System.Collections.Generic;
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
        private Dictionary<string, Entity> _treeElements;
        private List<Entity> _lines;

        protected override void AfterStartup()
        {
            base.AfterStartup();

            _lines = new List<Entity>();
            _treeElements = new Dictionary<string, Entity>();
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
            
            
            _tree.Draw((x, y, v) =>
            {
                //Якщо в словнику є елемент з таким значенням то використовуємо його якщо ж ні створюємо новий 
                var current = CreateElement(v);
                //Запускаємо переміщення ноди в нову позицію
                current.Transform.TweenLocalPositionTo(new Vector2(x * 50, y * 50), 0.5f).Start();
            }, _tree.Length); //Викликаємо відрисовку*/

            //Малюємо звязуючи лінії
            Core.StartCoroutine(DrawAllLines());
            //Центруємо дерево на єкрані
            //Domain.Position = new Vector2(Screen.Width / 2f - (_tree.Count * 50), Screen.Height / 2f);
        }

        private IEnumerator DrawAllLines()
        {
            
            yield return Coroutine.WaitForSeconds(0.5f);
            foreach (var item in _treeElements)
            {
                var elem = _tree.FindWithParent(item.Key, out var parent);
                if (elem != null && parent != null)
                {
                    var lineEntity = CreateEntity("Line", new Vector2(Screen.Width / 2f, Screen.Height / 2f));
                    // lineEntity.Transform.Parent = _domain.Transform;
                    lineEntity.LocalPosition = Vector2.Zero;
                    var line = lineEntity.AddComponent<LineRenderer>();
                    line.LayerDepth = 1;
                    line.RenderLayer = 999;

                    //line.SetUseWorldSpace(false);

                    var from = _treeElements[item.Key].Position;
                    var to = _treeElements[parent.value].Position;
                   

                    line.AddPoint(from, 3);
                    line.AddPoint(to, 3);

                    //line.SetStartEndColors(new Color(61, 9, 107),new Color(61, 9, 107));
                   
                    //Console.WriteLine($"{v} {parent.Value}");
                    _lines.Add(lineEntity);
                }
            }
        }
        
        private Entity CreateElement(string val)
        {
            var element = CreateEntity("TreeElement"+val).AddComponent(new DrawElement(val.ToString()));
            element.Transform.Parent = Domain.Transform;
            element.Transform.LocalPosition = new Vector2(100, -Screen.Height/2 );
            var scaleTo = new Vector2(0.75f, 0.75f);
            element.Transform.Scale = Vector2.Zero;
            element.Transform.TweenScaleTo(scaleTo, 0.5f).Start();
            
            if (!_treeElements.ContainsKey(val))
                _treeElements.Add(val, element.Entity);
            
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