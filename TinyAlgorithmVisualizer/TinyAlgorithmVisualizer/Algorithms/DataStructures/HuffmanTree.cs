using System;
using System.Linq;
using Nez.AI.GOAP;

namespace TinyAlgorithmVisualizer.Algorithms.DataStructures
{
    
    public class HuffmanTreeNode
    {
        public int weight;
        public char c;
        public string value;
        public HuffmanTreeNode Left;
        public HuffmanTreeNode Right;
        public int Length;

        public string GetValue() => value;
       
        
        public HuffmanTreeNode(int _a, char _c, HuffmanTreeNode _left = null, HuffmanTreeNode _right = null)
        {
            weight = _a;
            c = _c;
            Left = _left;
            Right = _right;
        }

        public HuffmanTreeNode(HuffmanTreeNode _left = null, HuffmanTreeNode _right = null)
        {
            Left = _left;
            Right = _right;
        }

        public HuffmanTreeNode(string value)
        {
            this.value = value;
        }
        
        /// 
        /// Находит и возвращает первый узел с заданным значением. Если значение
        /// не найдено, возвращает null. Также возвращает родителя найденного узла (или null)
        /// для использования в методе Remove.
        /// 
        public HuffmanTreeNode FindWithParent(string svalue, out HuffmanTreeNode parent)
        {
            
            var current = this;
            parent = null;
            // Поки елемент не знайдений
            while (current != null)
            {
                if (!int.TryParse(current.value, out var result))
                {
                    current = null;
                    parent = null;
                    break;
                }

                result = result.CompareTo(int.Parse(svalue));
                if (result > 0)
                {
                    // Якщо шукане значення менше йдемо наліво
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // Якщо шукане значення більше йдемо направо
                    parent = current;
                    current = current.Right;
                }
                else
                {
                    // Якщо співпадають то значення найдене
                    break;
                }

               
            }

            return current;
        }
        
        public void Draw(Action<int, int, string, string> onDraw, int x = 0)
        {
            DrawElement(onDraw, x, 0, this);
        }

        private string lastValue;
        private void DrawElement(Action<int, int, string, string> onDraw, int x, int y, HuffmanTreeNode node, int delta = 0)
        {
            //Прямий обхід
            if (node == null) return;
            
            if (delta == 0) delta = x / 2;
            node.value = node.c == '\0' ? $"{node.weight}" : $"{node.c} ({node.weight})";
            onDraw(x, y, node.value,node.lastValue);
            //Console.WriteLine($"{node.value} {lastValue}");
            

            if (node.Left!=null)
                node.Left.lastValue = node.value;
            if (node.Right!=null)
                node.Right.lastValue = node.value;
            
            
            DrawElement(onDraw, x - delta, y + 3, node.Left, delta / 2);
            DrawElement(onDraw, x + delta, y + 3, node.Right, delta / 2);
        }
    }
    public class HuffmanTree
    {
        private HuffmanTreeNode _head;
        public int Count { get; private set; }
        public HuffmanTreeNode Head => _head;

        
        

        public void AssignRoot(HuffmanTreeNode newRoot)
        {
            newRoot.Left = _head.Left;
            newRoot.Right = _head.Right;
            
            _head = newRoot;
        }
        
        
        
        
        
        public HuffmanTreeNode Add(string value, int side)
        {
            // Случай 1: Если дерево пустое, просто создаем корневой узел.
            if (_head == null)
            {
                _head = new HuffmanTreeNode(value);
                return _head;
            }
            // Случай 2: Дерево не пустое => 
            // ищем правильное место для вставки.
            else
            {
                return AddTo(_head, value, side);
            }
        }
        // Рекурсивная ставка.
        private HuffmanTreeNode AddTo(HuffmanTreeNode node, string value, int side)
        {
            // Случай 1: Вставляемое значение меньше значения узла
            if (side < 0)
            {
                // Если нет левого поддерева, добавляем значение в левого ребенка,
                if (node.Left == null)
                {
                    node.Left = new HuffmanTreeNode(value);
                    Count++;
                    return node.Left;

                }
                else
                {
                    // в противном случае повторяем для левого поддерева.
                    return AddTo(node.Left, value, side);
                }
            }
            // Случай 2: Вставляемое значение больше или равно значению узла.
            else
            {
                // Если нет правого поддерева, добавляем значение в правого ребенка,
                if (node.Right == null)
                {
                    node.Right = new HuffmanTreeNode(value);
                    Count++;
                    return node.Right;
                   
                }
                else
                {
                    // в противном случае повторяем для правого поддерева.
                    return AddTo(node.Right, value, side);
                }
            }
        }
        
        /// <summary>
        /// Реализация префиксного обхода для отрисовки
        /// </summary>
        /// <param name="onDraw"></param>
        /// <param name="x"></param>
        public void Draw(Action<int, int, string, int> onDraw, int x = 0)
        {
            DrawElement(onDraw, x, 0, _head);
        }

        private int _side = 0;
        private void DrawElement(Action<int, int, string, int> onDraw, int x, int y, HuffmanTreeNode node, int delta = 0)
        {
            //Прямий обхід
            if (node == null) return;
            
            if (delta == 0) delta = x / 2;
            onDraw(x, y, node.GetValue(), _side);
            _side = -1;
            DrawElement(onDraw, x - delta, y + 3, node.Left, delta / 2); 
            _side = 1;
            DrawElement(onDraw, x + delta, y + 3, node.Right, delta / 2);
        }
    }
}