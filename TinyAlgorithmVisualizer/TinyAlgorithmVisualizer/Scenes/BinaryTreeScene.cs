﻿using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using TinyAlgorithmVisualizer.Algorithms.Tree;

namespace TinyAlgorithmVisualizer.Scenes
{

    public class BinaryTreeScene : BasicDemoScene
    {
        private BinaryTree<int> _tree;
        private Dictionary<int, Entity> _treeElements;
        private List<Entity> _lines;

        public override void Initialize()
        {
            base.Initialize();
            _tree = new BinaryTree<int>();
            _treeElements = new Dictionary<int, Entity>();
            _lines = new List<Entity>();
        }

        private bool _prevent = false;
        protected override void OnCommandEnter(TextField field)
        {
            if (_prevent) return;
            var cmd = field.GetText().Split(' ');
            switch (cmd[0].ToLower())
            {
                case "add":
                    if (!IsDigitsOnly(cmd[1])) return;
                    AddElement(int.Parse(cmd[1]));
                    break;
                case "remove":
                    if (!IsDigitsOnly(cmd[1])) return;
                    RemoveElement(int.Parse(cmd[1]));
                    break;
                case "clear":
                    ClearTree();
                    break;
                case "count":
                    field.SetTextForced($"In tree {_tree.Count} elements");
                    break;
                case "find-next":
                    if (!IsDigitsOnly(cmd[1])) return;
                    HighlightElement(_tree.FindNext(int.Parse(cmd[1])));
                    break;
                case "depth":
                    field.SetTextForced("Depth is "+_tree.GetDepth());
                    break;
                case "find-prev":
                    if (!IsDigitsOnly(cmd[1])) return;
                    HighlightElement(_tree.FindPrevious(int.Parse(cmd[1])));
                    break;
                case "search":
                    if (!IsDigitsOnly(cmd[1])) return;
                    if (_tree.Contains(int.Parse(cmd[1])))
                        HighlightElement(int.Parse(cmd[1]));
                    else
                        field.SetTextForced("Not found");
                    break;
                case "preorder":
                    HighlightPreoder(field);
                    break;
                case "leaves":
                    HighlightLeaves();
                    break;
                case "menu":
                    Core.StartSceneTransition(new FadeTransition(() => new Menu()));
                    _prevent = true;
                    break;
            }
        }


        private void HighlightLeaves()
        {
            _tree.FindLeaves((x)=>HighlightElement(x));
        }

        public void HighlightPreoder(TextField field)
        {
            Core.StartCoroutine(HighlightPreoderBehaviour(field, 1));
        }

        private IEnumerator HighlightPreoderBehaviour(TextField field, int seconds)
        {
            field.SetTextForced("");
            field.SetDisabled(true);
            var tmp = new List<int>();
            _tree.PreOrderTraversal((x)=>tmp.Add(x));
            foreach (var i in tmp)
            {
                HighlightElement(i, seconds);
                field.SetTextForced(field.GetText() + " " + i);
                yield return Coroutine.WaitForSeconds(seconds);
            }

            field.SetDisabled(false);
        }
        
        public void HighlightElement(int value, int seconds = 3)
        {
            if (_treeElements.ContainsKey(value))
                _treeElements[value].GetComponent<DrawElement>().Highlight(seconds);
        }
        
        private void ClearTree()
        {
            _tree.Clear();
            foreach (var item in _treeElements)
            {
                _treeElements[item.Key].Transform.TweenScaleTo(Vector2.Zero, 0.5f).Start();
                _treeElements[item.Key].Destroy(1);
                _treeElements.Remove(item.Key);
            }
            _treeElements.Clear();
            RebuildTree(false);
        }
        
       
        private void RemoveElement(int value)
        {
            if (!_tree.Contains(value)) return;
            
            _tree.Remove(value);
            _treeElements[value].Transform.TweenScaleTo(Vector2.Zero, 0.5f).Start();
            _treeElements[value].Destroy(1);
            _treeElements.Remove(value);
            RebuildTree(false);
        }
        

        private void AddElement(int value)
        {
            if (_tree.Contains(value)) return;
            
            _tree.Add(value);
            RebuildTree(true);
        }

      
        
        private void RebuildTree(bool afterAdding)
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
                var current = !_treeElements.ContainsKey(v) ? CreateElement(v) : _treeElements[v];
                //Запускаємо переміщення ноди в нову позицію
                current.Transform.TweenLocalPositionTo( new Vector2(x*50, y*50), 0.5f).Start();
            }, _tree.Count); //Викликаємо відрисовку
            
            //Малюємо звязуючи лінії
            Core.StartCoroutine(DrawAllLines(afterAdding));
            //Центруємо дерево на єкрані
            Domain.Position = new Vector2(Screen.Width/2f-(_tree.Count*50), Screen.Height/2f);
        }

        private IEnumerator DrawAllLines(bool afterAdding)
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
                    var to = _treeElements[parent.Value].Position;
                   

                    line.AddPoint(from, 3);
                    line.AddPoint(to, 3);

                    //line.SetStartEndColors(new Color(61, 9, 107),new Color(61, 9, 107));
                   
                    //Console.WriteLine($"{v} {parent.Value}");
                    _lines.Add(lineEntity);
                }
            }
        }
        
        private Entity CreateElement(int val)
        {
            var element = CreateEntity("TreeElement"+val).AddComponent(new DrawElement(val.ToString()));
            element.Transform.Parent = Domain.Transform;
            element.Transform.LocalPosition = new Vector2(100, -Screen.Height/2 );
            var scaleTo = new Vector2(0.75f, 0.75f);
            element.Transform.Scale = Vector2.Zero;
            element.Transform.TweenScaleTo(scaleTo, 0.5f).Start();
            
            
            _treeElements.Add(val, element.Entity);
            
            return element.Entity;
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

        public override void Update()
        {
           
            base.Update();
        }
    }
}