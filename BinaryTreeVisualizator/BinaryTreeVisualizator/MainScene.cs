using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using BinaryTreeVisualizator.Tree;
using Microsoft.Xna.Framework;
using Nez;
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
        
        public override void OnStart()
        {
            base.OnStart();
           
            var gridEntity  = CreateEntity("grid");
            gridEntity.AddComponent(new SpringGrid(new Rectangle((int)0, (int)0, (int) (Screen.Width), (int) (Screen.Height)), new Vector2(20))
            {
                GridMinorThickness = 0,
                GridMajorThickness = 4,
                GridMajorColor = Color.Gray
            });
            var grid = gridEntity.GetComponent<SpringGrid>();
            grid.RenderLayer = 9999;
            Core.StartCoroutine(Helpers.WaitAndFreeze(gridEntity));

            _uiHelper = new GameUIHelper(this.Content);
            var ui = CreateEntity("UI").AddComponent<UICanvas>();
            var mainTable = ui.Stage.AddElement(new Table());
            
            var input = _uiHelper.CreateInputField(mainTable, "", OnCommandEnter);
            
            mainTable.Row();
            mainTable.Pack();

            _tree = new BinaryTree<int>();
            _treeElements = new Dictionary<int, Entity>(10);
            
            _domain = CreateEntity("Domain");
            _domain.AddComponent<ViewController>();
            _domain.Position = new Vector2(Screen.Width/2f, Screen.Height/2f);

          

        }

        private void OnCommandEnter(TextField field)
        {
            var cmd = field.GetText().Split(' ');
            if (cmd[0].ToLower() == "add")
            {
                AddElement(int.Parse(cmd[1]));
            }
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
            RebuildTree();
        }

        private void RebuildTree()
        {
            Entity tmp;
            var pos = new Vector2();
            int y = 0;
            int lastSide = 0;
            //Console.WriteLine("New");
            var arr = new List<int>();
            _tree.PreOrderTraversal((x) => arr.Add(x));

            var offset = arr.Count*250;
            var prevLPoint = Vector2.Zero;
            var prevRPoint = Vector2.Zero;
            _tree.ExtendedPreOrderTraversal((x, c, s) =>
            {
                offset /= 2;
                tmp = !_treeElements.ContainsKey(x) ? CreateElement(x) : _treeElements[x];
                switch (s)
                {
                    case Side.Left:
                        tmp.LocalPosition  = new Vector2(-100-offset/2f, 100*c);
                        break;
                    case Side.Right:
                        tmp.LocalPosition  = new Vector2(100+prevRPoint.X-offset/2f, 100*c);
                        break;
                    case Side.Root:
                        tmp.LocalPosition = new Vector2(0, 0);
                        break;
                }

                //Console.WriteLine($"{x} {c} {s}: {offset} \n");
                //Console.WriteLine(Environment.NewLine);
                
                //lastSide = s;
            });
            Console.Clear();
            _tree.PrintTree(15,0, _tree.GetNode());
        }

        private Entity CreateElement(int val)
        {
            var element = CreateEntity("TreeElement"+val).AddComponent(new TreeElement(val));
            element.Transform.Parent = _domain.Transform;
            
            _treeElements.Add(val, element.Entity);
            
            return element.Entity;
        }

        private void DrawLines()
        {
            
        }
        
    }
}