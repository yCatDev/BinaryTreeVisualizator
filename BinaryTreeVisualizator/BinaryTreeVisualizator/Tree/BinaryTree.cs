using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTreeVisualizator.Tree
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        private BinaryTreeNode<T> _head;
        private int _count;

        public void Add(T value)
        {
            // Случай 1: Если дерево пустое, просто создаем корневой узел.
            if (_head == null)
            {
                _head = new BinaryTreeNode<T>(value);
            }
            // Случай 2: Дерево не пустое => 
            // ищем правильное место для вставки.
            else
            {
                AddTo(_head, value, 0);
            }

            _count++;
        }

        // Рекурсивная ставка.
        private void AddTo(BinaryTreeNode<T> node, T value, int depth)
        {
            // Случай 1: Вставляемое значение меньше значения узла
            if (value.CompareTo(node.Value) < 0)
            {
                // Если нет левого поддерева, добавляем значение в левого ребенка,
                if (node.Left == null)
                {
                    node.Left = new BinaryTreeNode<T>(value);
                }
                else
                {
                    // в противном случае повторяем для левого поддерева.
                    AddTo(node.Left, value, ++depth);
                }
            }
            // Случай 2: Вставляемое значение больше или равно значению узла.
            else
            {
                // Если нет правого поддерева, добавляем значение в правого ребенка,
                if (node.Right == null)
                {
                    node.Right = new BinaryTreeNode<T>(value);
                }
                else
                {
                    // в противном случае повторяем для правого поддерева.
                    AddTo(node.Right, value, ++depth);
                }
            }
        }

        public bool Contains(T value)
        {
            // Поиск узла осуществляется другим методом.
            BinaryTreeNode<T> parent;
            return FindWithParent(value, out parent) != null;
        }

        /// 
        /// Находит и возвращает первый узел с заданным значением. Если значение
        /// не найдено, возвращает null. Также возвращает родителя найденного узла (или null)
        /// для использования в методе Remove.
        /// 
        public BinaryTreeNode<T> FindWithParent(T value, out BinaryTreeNode<T> parent)
        {
            // Попробуем найти значение в дереве.
            BinaryTreeNode<T> current = _head;
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

        public bool Remove(T value)
        {
            BinaryTreeNode<T> current, parent;

            // Находим удаляемый узел.
            current = FindWithParent(value, out parent);

            if (current == null)
            {
                return false;
            }

            _count--;

            // Случай 1: Если нет детей справа, левый ребенок встает на место удаляемого.
            if (current.Right == null)
            {
                if (parent == null)
                {
                    _head = current.Left;
                }
                else
                {
                    int result = parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        // Если значение родителя больше текущего,
                        // левый ребенок текущего узла становится левым ребенком родителя.
                        parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {
                        // Если значение родителя меньше текущего, // левый ребенок текущего узла становится правым ребенком родителя.
                        parent.Right = current.Left;
                    }
                }
            } // Случай 2: Если у правого ребенка нет детей слева // то он занимает место удаляемого узла.
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;
                if (parent == null)
                {
                    _head = current.Right;
                }
                else
                {
                    int result = parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        // Если значение родителя больше текущего,
                        // правый ребенок текущего узла становится левым ребенком родителя.
                        parent.Left = current.Right;
                    }
                    else if (result < 0)
                    {
                        // Если значение родителя меньше текущего, // правый ребенок текущего узла становится правым ребенком родителя.
                        parent.Right = current.Right;
                    }
                }
            } // Случай 3: Если у правого ребенка есть дети слева, крайний левый ребенок // из правого поддерева заменяет удаляемый узел.
            else
            {
                // Найдем крайний левый узел.
                BinaryTreeNode<T> leftmost = current.Right.Left;
                BinaryTreeNode<T> leftmostParent = current.Right;
                while (leftmost.Left != null)
                {
                    leftmostParent = leftmost;
                    leftmost = leftmost.Left;
                } // Левое поддерево родителя становится правым поддеревом крайнего левого узла.

                leftmostParent.Left =
                    leftmost.Right; // Левый и правый ребенок текущего узла становится левым и правым ребенком крайнего левого.
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;
                if (parent == null)
                {
                    _head = leftmost;
                }
                else
                {
                    int result = parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        // Если значение родителя больше текущего,
                        // крайний левый узел становится левым ребенком родителя.
                        parent.Left = leftmost;
                    }
                    else if (result < 0)
                    {
                        // Если значение родителя меньше текущего,
                        // крайний левый узел становится правым ребенком родителя.
                        parent.Right = leftmost;
                    }
                }
            }

            return true;
        }

        #region Old methods

        public void PreOrderTraversal(Action<T> action)
        {
            PreOrderTraversal(action, _head);
        }

        private void PreOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {
            if (node == null) return; //Вихід із рекурсії
            
            action(node.Value); //Пошук в центрі
            PreOrderTraversal(action, node.Left); //Пошук зліва 
            PreOrderTraversal(action, node.Right); //Пошук справа
        }

        public void PostOrderTraversal(Action<T> action)
        {
            PostOrderTraversal(action, _head);
        }

        private void PostOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
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
            InOrderTraversal(action, _head);
        }

        private void InOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                InOrderTraversal(action, node.Left);

                action(node.Value);

                InOrderTraversal(action, node.Right);
            }
        }

        #endregion

        public IEnumerator<T> InOrderTraversal()
        {
            if (_head != null)
            {
                // Стек для сохранения пропущенных узлов.
                Stack stack = new Stack();

                BinaryTreeNode<T> current = _head;

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
                        current = stack.Pop() as BinaryTreeNode<T>;
                        goLeftNext = false;
                    }
                }
            }
        }


        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            _head = null;
            _count = 0;
        }

        public int Count => _count;


        /// <summary>
        /// Реализация префиксного обхода для отрисовки
        /// </summary>
        /// <param name="onDraw"></param>
        /// <param name="x"></param>
        public void Draw(Action<int, int, T> onDraw, int x = 0)
        {
            DrawElement(onDraw, x, 0, _head);
        }

        private void DrawElement(Action<int, int, T> onDraw, int x, int y, BinaryTreeNode<T> node, int delta = 0)
        {
            if (node != null)
            {
                if (delta == 0) delta = x / 2;
                onDraw(x, y, node.Value); //Идем в корень
                DrawElement(onDraw, x - delta, y + 3, node.Left, delta / 2); //Влево
                DrawElement(onDraw, x + delta, y + 3, node.Right, delta / 2); //Вправо
            }
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
        
        
        //В двоичном дереве поиска найти элемент, предшествующий данному.
        public T FindPrevious(T val)
        {
            //запись корневого элемента во временную переменную
            var temp = _head;
            //возврат результата функции поиска элемента среди дочерних
            return FindPrevious(val, temp);
        }

        //функция поиска элемента среди дочерних
        private T FindPrevious(T val, BinaryTreeNode<T> temp)
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