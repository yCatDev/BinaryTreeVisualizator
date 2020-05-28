﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace TinyAlgorithmVisualizer.Algorithms.Tree
{

    // Класс AVLTreeNode реализует один узел АВЛ дерева 

    public class AvlTreeNode<TNode> : IComparable<TNode>
        where TNode : IComparable
    {
        AVLTree<TNode> _tree;

        AvlTreeNode<TNode> _left; // левый  потомок

        AvlTreeNode<TNode> _right; // правый потомок

        //Конструктор
        public AvlTreeNode(TNode value, AvlTreeNode<TNode> parent, AVLTree<TNode> tree)
        {
            Value = value;
            Parent = parent;
            _tree = tree;
        }

        // Свойства 
        public AvlTreeNode<TNode> Left
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

        public AvlTreeNode<TNode> Right
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
        public AvlTreeNode<TNode> Parent { get; internal set; }
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

        private int MaxChildHeight(AvlTreeNode<TNode> node)
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
            AvlTreeNode<TNode> newRoot = Right;
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
            AvlTreeNode<TNode> newRoot = Left;
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
        private void ReplaceRoot(AvlTreeNode<TNode> newRoot)
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
        public AvlTreeNode<T> Head { get; internal set; }

        //Количество узлов дерева
        public int Count { get; private set; }

        // Метод добавлет новый узел
        public void Add(T value)
        {
            if (Head == null) // Если дерево пустое - создание корня дерева 
            {
                Head = new AvlTreeNode<T>(value, null, this);
            }
            else // Если дерево не пустое - найти место для добавление нового узла.
            {
                AddTo(Head, value);
            }
            //Head.Balance();
            Count++;
        }

        // Алгоритм рекурсивного добавления нового узла в дерево.
        private void AddTo(AvlTreeNode<T> node, T value)
        {
            if (value.CompareTo(node.Value) < 0) //Добавление нового узла в дерево. Если значение добавлемого узла меньше чем значение текущего узла.
            {
                if (node.Left == null) //Создание левого узла, если его нет.
                {
                    node.Left = new AvlTreeNode<T>(value, node, this);
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
                    node.Right = new AvlTreeNode<T>(value, node, this);
                }
                else
                {
                    // Переход к следующему правому узлу.             
                    AddTo(node.Right, value);
                }
            }

            node.Balance();
        }

        public bool Contains(T value)
        {
            return Find(value) != null;
        }

        private AvlTreeNode<T> Find(T value)
        {

            AvlTreeNode<T> current = Head; // помещаем текущий элемент в корень дерева

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
            AvlTreeNode<T> current;
            current = Find(value); // находим узел с удаляемым значением

            if (current == null) // узел не найден
            {
                return false;
            }

            AvlTreeNode<T> treeToBalance = current.Parent; // баланс дерева относительно узла родителя
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
                AvlTreeNode<T> leftmost = current.Right.Left;
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
                Head?.Balance();
            }

            return true;
        }

        public void Clear()
        {
            Head = null; // удаление дерева
            Count = 0;
        }
        public void PreOrderTraversal(Action<T> action)
        {
            PreOrderTraversal(action, Head);
        }

        private void PreOrderTraversal(Action<T> action, AvlTreeNode<T> node)
        {
            if (node == null) return; //Вихід із рекурсії
            
            action(node.Value); //Пошук в центрі
            PreOrderTraversal(action, node.Left); //Пошук зліва 
            PreOrderTraversal(action, node.Right); //Пошук справа
        }

        public void PostOrderTraversal(Action<T> action)
        {
            PostOrderTraversal(action, Head);
        }

        private void PostOrderTraversal(Action<T> action, AvlTreeNode<T> node)
        {
            if (node != null)
            {
                PostOrderTraversal(action, node.Left);
                PostOrderTraversal(action, node.Right);
                action(node.Value);
            }
        }


        public void InOrderTraversal(Action<T> action)
        {
            InOrderTraversal(action, Head);
        }

        private void InOrderTraversal(Action<T> action, AvlTreeNode<T> node)
        {
            if (node != null)
            {
                InOrderTraversal(action, node.Left);

                action(node.Value);

                InOrderTraversal(action, node.Right);
            }
        }

         public IEnumerator<T> InOrderTraversal()
        {
            // Это нерекурсивный алгоритм.
            // Он использует стек для того, чтобы избежать рекурсии.
            if (Head != null)
            {
                // Стек для сохранения пропущенных узлов.
                Stack stack = new Stack();

                AvlTreeNode<T> current = Head;

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
                        current = stack.Pop() as AvlTreeNode<T>;
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
         public AvlTreeNode<T> FindWithParent(T value, out AvlTreeNode<T> parent)
         {
             // Попробуем найти значение в дереве.
             AvlTreeNode<T> current = Head;
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
         
         public int GetDepth() => GetDepth(Head);
        
         private int GetDepth(AvlTreeNode<T> node)
         {
             if (node == null) 
                 return 0;
            
             /* Рахуємо максимальну глубину по сторонам */
             var lDepth = GetDepth(node.Left); 
             var rDepth = GetDepth(node.Right); 
  
             /* повертаємо набільшу із них */
             if (lDepth > rDepth) 
                 return (lDepth + 1); 
             else
                 return (rDepth + 1);
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

         private void DrawElement(Action<int, int, T> onDraw, int x, int y, AvlTreeNode<T> node, int delta = 0)
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
        
         //В двоичном дереве поиска найти элемент, следующий за данным.
        public T FindNext(T val)
        {
            //Ищем элемент
            var next = FindWithParent(val, out var parent);
            //Идем вправо
            if (next.Right != null)
            {
                //Проверяем в какую сторону идти
                if (next.Right.Value.CompareTo(val) > 0 || next.Left == null)
                    return next.Right.Value;
            }
            //если вправо не можем то идем либо в лево либо возвращаем значения поиска - ТуПиК
            return next.Left != null ? next.Left.Value : next.Value;
        }
        
        public void FindLeaves(Action<T> onFind)
        {
            FindLeave(Head, onFind);
        }
        
        private void FindLeave(AvlTreeNode<T> node, Action<T> onFind)
        {
            if (node==null) 
                return; 
            
            if (node.Left==null && node.Right==null) 
            { 
                onFind.Invoke(node.Value);
                return; 
            } 
            
            if (node.Left != null) 
                FindLeave(node.Left, onFind); 
          
            if (node.Right != null) 
                FindLeave(node.Right, onFind); 
        }
        
        //В двоичном дереве поиска найти элемент, предшествующий данному.
        public T FindPrevious(T val)
        {
            //запись корневого элемента во временную переменную
            var temp = Head;
            //возврат результата функции поиска элемента среди дочерних
            return FindPrevious(val, temp);
        }

        //функция поиска элемента среди дочерних
        private T FindPrevious(T val, AvlTreeNode<T> temp)
        {
            //Если пытаемся достать значение выше головы возвращаем то что хочем
            if (temp == null) return val;
            
            //если данные в правом или левом дереве равны искомым, возвращаем данные в текущем узле
            if ((temp.Left!=null && val.CompareTo(temp.Left.Value)==0) 
                || (temp.Right!=null && val.CompareTo(temp.Right.Value)==0))
            {
                return temp.Value;
            }
            //если искомые данные меньше днанных в текущем узле, возвращаем результат этой функции для левого поддерева
            else if (val.CompareTo(temp.Value)<0)
            {
                return FindPrevious(val, temp.Left);
            }
            //в противном случае возвращаем результат этой функции для правого поддерева
            else
            {
                return FindPrevious(val, temp.Right);
            }
        }
        
    }
}