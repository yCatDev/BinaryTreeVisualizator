using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTreeVisualizator.Tree
{

    // Класс AVLTreeNode реализует один узел АВЛ дерева 

    public class AVLTreeNode<TNode> : IComparable<TNode>
        where TNode : IComparable
    {
        AVLTree<TNode> _tree;

        AVLTreeNode<TNode> _left; // левый  потомок

        AVLTreeNode<TNode> _right; // правый потомок

        //Конструктор
        public AVLTreeNode(TNode value, AVLTreeNode<TNode> parent, AVLTree<TNode> tree)
        {
            Value = value;
            Parent = parent;
            _tree = tree;
        }

        // Свойства 
        public AVLTreeNode<TNode> Left
        {
            get { return _left; }

            internal set
            {
                _left = value;

                if (_left != null)
                {
                    _left.Parent = this; // установка указателя на родительский элемент
                }
            }
        }

        public AVLTreeNode<TNode> Right
        {
            get { return _right; }

            internal set
            {
                _right = value;

                if (_right != null)
                {
                    _right.Parent = this; // установка указателя на родительский элемент
                }
            }
        }

        // Указатель на родительский узел
        public AVLTreeNode<TNode> Parent { get; internal set; }
        // значение текущего узла 

        public TNode Value { get; private set; }

        // Сравнивает текущий узел по указаному значению, возвращет 1, если значение экземпляра больше переданного значения,  возвращает -1, когда значение экземпляра меньше переданого значения, 0 - когда они равны.     
        public int CompareTo(TNode other)
        {
            return Value.CompareTo(other);
        }

        internal void Balance()
        {
            if (State == TreeState.RightHeavy)
            {
                if (Right != null && Right.BalanceFactor < 0)
                {
                    LeftRightRotation();
                }

                else
                {
                    LeftRotation();
                }
            }
            else if (State == TreeState.LeftHeavy)
            {
                if (Left != null && Left.BalanceFactor > 0)
                {
                    RightLeftRotation();
                }
                else
                {
                    RightRotation();
                }
            }
        }

        private int MaxChildHeight(AVLTreeNode<TNode> node)
        {
            if (node != null)
            {
                return 1 + Math.Max(MaxChildHeight(node.Left), MaxChildHeight(node.Right));
            }

            return 0;
        }

        private int LeftHeight
        {
            get { return MaxChildHeight(Left); }
        }

        private int RightHeight
        {
            get { return MaxChildHeight(Right); }
        }

        private TreeState State
        {
            get
            {
                if (LeftHeight - RightHeight > 1)
                {
                    return TreeState.LeftHeavy;
                }

                if (RightHeight - LeftHeight > 1)
                {
                    return TreeState.RightHeavy;
                }

                return TreeState.Balanced;
            }
        }

        private int BalanceFactor
        {
            get { return RightHeight - LeftHeight; }
        }

        enum TreeState
        {
            Balanced,
            LeftHeavy,
            RightHeavy,
        }

        //Левое вращение
        private void LeftRotation()
        {
            // Сделать правого потомка новым корнем дерева.
            AVLTreeNode<TNode> newRoot = Right;
            ReplaceRoot(newRoot);
            // Поставить на место правого потомка - левого потомка нового корня.    
            Right = newRoot.Left;
            // Сделать текущий узел - левым потомком нового корня.    
            newRoot.Left = this;
        }

        //Правое вращение
        private void RightRotation()
        {
            // Левый узел текущего элемента становится новым корнем
            AVLTreeNode<TNode> newRoot = Left;
            ReplaceRoot(newRoot);
            // Перемещение правого потомка нового корня на место левого потомка старого корня
            Left = newRoot.Right;
            // Правым потомком нового корня, становится старый корень.     
            newRoot.Right = this;
        }

        private void LeftRightRotation()
        {
            Right.RightRotation();
            LeftRotation();
        }

        private void RightLeftRotation()
        {
            Left.LeftRotation();
            RightRotation();
        }

        //Перемещение корня
        private void ReplaceRoot(AVLTreeNode<TNode> newRoot)
        {
            if (this.Parent != null)
            {
                if (this.Parent.Left == this)
                {
                    this.Parent.Left = newRoot;
                }
                else if (this.Parent.Right == this)
                {
                    this.Parent.Right = newRoot;
                }
            }
            else
            {
                _tree.Head = newRoot;
            }

            newRoot.Parent = this.Parent;
            this.Parent = newRoot;
        }
    }

    public class AVLTree<T> : IEnumerable<T>
        where T : IComparable

    {
        // Свойство для корня дерева
        public AVLTreeNode<T> Head { get; internal set; }

        //Количество узлов дерева
        public int Count { get; private set; }

        // Метод добавлет новый узел
        public void Add(T value)
        {
            if (Head == null) // Если дерево пустое - создание корня дерева 
            {
                Head = new AVLTreeNode<T>(value, null, this);
            }
            else // Если дерево не пустое - найти место для добавление нового узла.
            {
                AddTo(Head, value);
            }
            Head.Balance();
            Count++;
        }

        // Алгоритм рекурсивного добавления нового узла в дерево.
        private void AddTo(AVLTreeNode<T> node, T value)
        {
            if (value.CompareTo(node.Value) < 0
            ) //Добавление нового узла в дерево. Если значение добавлемого узла меньше чем значение текущего узла.
            {
                if (node.Left == null) //Создание левого узла, если его нет.
                {
                    node.Left = new AVLTreeNode<T>(value, node, this);
                }
                else
                {
                    // Переходим к следующему левому узлу
                    AddTo(node.Left, value);
                }
            }
            else // Если добавлемое значение больше или равно текущему значению.
            {
                if (node.Right == null) //Создание правого узла, если его нет.
                {
                    node.Right = new AVLTreeNode<T>(value, node, this);
                }
                else
                {
                    // Переход к следующему правому узлу.             
                    AddTo(node.Right, value);
                }
            }
        }

        public bool Contains(T value)
        {
            return Find(value) != null;
        }

        private AVLTreeNode<T> Find(T value)
        {

            AVLTreeNode<T> current = Head; // помещаем текущий элемент в корень дерева

            // Пока текщий узел на пустой 
            while (current != null)
            {
                int result = current.CompareTo(value); // сравнение значения текущего элемента с искомым значением

                if (result > 0)
                {
                    // Если значение меньшне текущего - переход влево 
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // Если значение больше текщего - переход вправо             
                    current = current.Right;
                }
                else
                {
                    // Элемент найден      
                    break;
                }
            }

            return current;
        }

// Метод удаляет элемент по значению
        public bool Remove(T value)
        {
            AVLTreeNode<T> current;
            current = Find(value); // находим узел с удаляемым значением

            if (current == null) // узел не найден
            {
                return false;
            }

            AVLTreeNode<T> treeToBalance = current.Parent; // баланс дерева относительно узла родителя
            Count--; // уменьшение колиества узлов

            //Если удаляемый узел не имеет правого потомка      

            if (current.Right == null)
            {
                if (current.Parent == null) // удаляемый узел является корнем
                {
                    Head = current.Left; // на место корня перемещаем левого потомка

                    if (Head != null)
                    {
                        Head.Parent = null; // убераем ссылку на родителя  
                    }
                }
                else // удаляемый узел не является корнем
                {
                    int result = current.Parent.CompareTo(current.Value);

                    if (result > 0)
                    {
                        // Если значение родительского узла больше значения удаляемого, сделать левого потомка удаляемого узла, левым потомком родителя.  

                        current.Parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {

                        // Если значение родительского узла меньше чем удаляемого,
                        // сделать левого потомка удаляемого узла - правым потомком
                        // родительского узла.current.Parent.Right = current.Left;
                    }
                }
            }

            // Если правый потомок удаляемого узла не имеет левого потомка, тогда правый потомок удаляемого узла становится потомком родительского узла.      

            else if (current.Right.Left == null) // если у правого потомка нет левого потомка
            {
                current.Right.Left = current.Left;

                if (current.Parent == null) // текущий элемент является корнем
                {
                    Head = current.Right;

                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    int result = current.Parent.CompareTo(current.Value);
                    if (result > 0
                    ) // Если значение узла родителя больше чем значение удаляемого узла, сделать правого потомка удаляемого узла, левым потомком его родителя.      
                    {
                        current.Parent.Left = current.Right;
                    }

                    else if (result < 0
                    ) // Если значение родительского узла меньше значения удаляемого,сделать правого потомка удаляемого узла - правым потомком родителя.          
                    {
                        current.Parent.Right = current.Right;
                    }
                }
            }
            // Если правый потомок удаляемого узла имеет левого потомка,заместить удаляемый узел, крайним левым потомком правого потомка.     
            else
            {
                // Нахожление крайнего левого узла для правого потомка удаляемого узла.       
                AVLTreeNode<T> leftmost = current.Right.Left;
                while (leftmost.Left != null)
                {
                    leftmost = leftmost.Left;
                }

                // Родительское правое поддерево становится родительским левым поддеревом.         
                leftmost.Parent.Left = leftmost.Right;
                // Присвоить крайнему левому узлу, ссылки на правого и левого потомка удаляемого узла.         
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;
                if (current.Parent == null)
                {
                    Head = leftmost;
                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    int result = current.Parent.CompareTo(current.Value);

                    if (result > 0)
                    {
                        // Если значение родительского узла больше значения удаляемого,сделать крайнего левого потомка левым потомком родителя удаляемого узла.                 

                        current.Parent.Left = leftmost;
                    }
                    else if (result < 0)
                    {
                        // Если значение родительского узла, меньше чем значение удаляемого,сделать крайнего левого потомка, правым потомком родителя удаляемого узла.                 

                        current.Parent.Right = leftmost;
                    }
                }
            }

            if (treeToBalance != null)
            {
                treeToBalance.Balance();
            }
            else
            {
                if (Head != null)
                {
                    Head.Balance();
                }
            }

            return true;
        }

        public void Clear()
        {
            Head = null; // удаление дерева
            Count = 0;
        }

         public IEnumerator<T> InOrderTraversal()
        {
            // Это нерекурсивный алгоритм.
            // Он использует стек для того, чтобы избежать рекурсии.
            if (Head != null)
            {
                // Стек для сохранения пропущенных узлов.
                Stack stack = new Stack();

                AVLTreeNode<T> current = Head;

                // Когда мы избавляемся от рекурсии, нам необходимо
                // запоминать, в какую стороны мы должны двигаться.
                bool goLeftNext = true;

                // Кладем в стек корень.
                stack.Push(current);

                while (stack.Count > 0)
                {
                    // Если мы идем налево...
                    if (goLeftNext)
                    {
                        // Кладем все, кроме самого левого узла на стек.
                        // Крайний левый узел мы вернем с помощю yield.
                        while (current.Left != null)
                        {
                            stack.Push(current);
                            current = current.Left;
                        }
                    }

                    // Префиксный порядок: left->yield->right.
                    yield return current.Value;

                    // Если мы можем пойти направо, идем.
                    if (current.Right != null)
                    {
                        current = current.Right;

                        // После того, как мы пошли направо один раз,
                        // мы должным снова пойти налево.
                        goLeftNext = true;
                    }
                    else
                    {
                        // Если мы не можем пойти направо, мы должны достать родительский узел
                        // со стека, обработать его и идти в его правого ребенка.
                        current = stack.Pop() as AVLTreeNode<T>;
                        goLeftNext = false;
                    }
                }
            }
        }
         
         /// 
         /// Находит и возвращает первый узел с заданным значением. Если значение
         /// не найдено, возвращает null. Также возвращает родителя найденного узла (или null)
         /// для использования в методе Remove.
         /// 
         public AVLTreeNode<T> FindWithParent(T value, out AVLTreeNode<T> parent)
         {
             // Попробуем найти значение в дереве.
             AVLTreeNode<T> current = Head;
             parent = null;

             // До тех пор, пока не нашли...
             while (current != null)
             {
                 int result = current.CompareTo(value);

                 if (result > 0)
                 {
                     // Если искомое значение меньше, идем налево.
                     parent = current;
                     current = current.Left;
                 }
                 else if (result < 0)
                 {
                     // Если искомое значение больше, идем направо.
                     parent = current;
                     current = current.Right;
                 }
                 else
                 {
                     // Если равны, то останавливаемся
                     break;
                 }
             }

             return current;
         }
         
         /// <summary>
         /// Реализация префиксного обхода для отрисовки
         /// </summary>
         /// <param name="onDraw"></param>
         /// <param name="x"></param>
         public void Draw(Action<int, int, T> onDraw, int x = 0)
         {
             DrawElement(onDraw, x, 0, Head);
         }

         private void DrawElement(Action<int, int, T> onDraw, int x, int y, AVLTreeNode<T> node, int delta = 0)
         {
             if (node != null)
             {
                 if (delta == 0) delta = x / 2;
                 onDraw(x, y, node.Value);
                 DrawElement(onDraw, x - delta, y + 3, node.Left, delta / 2);
                 DrawElement(onDraw, x + delta, y + 3, node.Right, delta / 2);
             }
         }
         
        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}