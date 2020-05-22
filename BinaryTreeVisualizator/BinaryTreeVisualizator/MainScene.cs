using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BinaryTreeVisualizator.Tree;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tweens;
using Nez.UI;

namespace BinaryTreeVisualizator
{

    public class Helpers
    {
        public static IEnumerator WaitAndFreeze(Entity target)
        {
            yield return Coroutine.WaitForSeconds(0.1f);
            target.UpdateInterval = UInt32.MaxValue;
        }
    }

    public class MainScene : Scene
    {
        private GameUIHelper _uiHelper;
        private Entity _domain;
        private BinaryTree<int> _tree;
        private Dictionary<int, Entity> _treeElements;
        private List<Entity> _lines;
        
        public override void OnStart()
        {
            base.OnStart();
           
            /*var gridEntity  = CreateEntity("grid");
            gridEntity.AddComponent(new SpringGrid(new Rectangle((int)0, (int)0, (int) (Screen.Width), (int) (Screen.Height)), new Vector2(20))
            {
                GridMinorThickness = 0,
                GridMajorThickness = 4,
                GridMajorColor = Color.Gray
            });
            var grid = gridEntity.GetComponent<SpringGrid>();
            grid.RenderLayer = 9999;
            Core.StartCoroutine(Helpers.WaitAndFreeze(gridEntity));*/

            _uiHelper = new GameUIHelper(this.Content);
            var ui = CreateEntity("UI").AddComponent<UICanvas>();
            var mainTable = ui.Stage.AddElement(new Table());
            
            var input = _uiHelper.CreateInputField(mainTable, "", OnCommandEnter);
            
            mainTable.Row();
            mainTable.Pack();

            _tree = new BinaryTree<int>();
            _treeElements = new Dictionary<int, Entity>(10);
            _lines = new List<Entity>();
            
            _domain = CreateEntity("Domain");
            //_domain.AddComponent<ViewController>();
            _domain.Position = new Vector2(Screen.Width/2f, Screen.Height/2f);
            Camera.Entity.AddComponent<ViewController>();
            
            


        }

        private void OnCommandEnter(TextField field)
        {
            var cmd = field.GetText().Split(' ');
            switch (cmd[0].ToLower())
            {
                case "add":
                    AddElement(int.Parse(cmd[1]));
                    break;
                case "remove":
                    RemoveElement(int.Parse(cmd[1]));
                    break;
            }
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

        public override void Initialize()
        {
            base.Initialize();
            ClearColor = Color.Black;
        }

        private void AddElement(int value)
        {
            if (_tree.Contains(value)) return;
            
            _tree.Add(value);
            RebuildTree(true);
        }
        LineRenderer line;
        Entity lineEntity;
        private void RebuildTree(bool afterAdding)
        {
            Entity current;
            RemoveAllLines();


            _tree.Draw((x, y, v) =>
            {
                current = !_treeElements.ContainsKey(v) ? CreateElement(v) : _treeElements[v];
                //current.LocalPosition = new Vector2(x*50, y*50);
                current.Transform.TweenLocalPositionTo( new Vector2(x*50, y*50)).Start();
            }, _tree.Count);

            Core.StartCoroutine(DrawAllLines(afterAdding));
            _domain.Position = new Vector2(Screen.Width/2f-(_tree.Count*50), Screen.Height/2f);
        }

        private IEnumerator DrawAllLines(bool afterAdding)
        {
            yield return Coroutine.WaitForSeconds(0.5f);
            foreach (var item in _treeElements)
            {
                var elem = _tree.FindWithParent(item.Key, out var parent);
                if (elem != null && parent != null)
                {
                    lineEntity = CreateEntity("Line", new Vector2(Screen.Width / 2f, Screen.Height / 2f));
                    // lineEntity.Transform.Parent = _domain.Transform;
                    lineEntity.LocalPosition = Vector2.Zero;
                    line = lineEntity.AddComponent<LineRenderer>();
                    line.LayerDepth = 1;

                    //line.SetUseWorldSpace(false);

                    var from = _treeElements[item.Key].Position;
                    var to = _treeElements[parent.Value].Position;
                   

                    line.AddPoint(from, 3);
                    line.AddPoint(to, 3);
                    //Console.WriteLine($"{v} {parent.Value}");
                    _lines.Add(lineEntity);
                }
            }
        }
        
        private Entity CreateElement(int val)
        {
            var element = CreateEntity("TreeElement"+val).AddComponent(new TreeElement(val));
            element.Transform.Parent = _domain.Transform;
            
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