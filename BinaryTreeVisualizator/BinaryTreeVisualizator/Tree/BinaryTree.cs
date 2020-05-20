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
                AddTo(_head, value);
            }

            _count++;
        }

// Рекурсивная ставка.
        private void AddTo(BinaryTreeNode<T> node, T value)
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
                    AddTo(node.Left, value);
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
                    AddTo(node.Right, value);
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
        private BinaryTreeNode<T> FindWithParent(T value, out BinaryTreeNode<T> parent)
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

#region НЕ ЧИТАТЬ РЕАЛИЗАЦИЯ ОБХОДОВ ДЕРЕВА
        public void PreOrderTraversal(Action<T> action)
        {
            PreOrderTraversal(action, _head);
        }

        private void PreOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                action(node.Value);
                PreOrderTraversal(action, node.Left);
                PreOrderTraversal(action, node.Right);
            }
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

        public IEnumerator<T> InOrderTraversal()
        {
            // Это нерекурсивный алгоритм.
            // Он использует стек для того, чтобы избежать рекурсии.
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
#endregion

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

        public int Count
        {
            get { return _count; }
        }
    }
}