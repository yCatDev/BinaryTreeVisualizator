using System;
using System.Collections.Generic;
using System.Diagnostics;
using TinyAlgorithmVisualizer.Algorithms.DataStructures;
using TinyAlgorithmVisualizer.Algorithms.Tree;

namespace TinyAlgorithmVisualizer.Algorithms.Greedy
{

    public class Huffman
    {
        // получение кода хаффмана
        public void BuidTable(HuffmanTreeNode root, List<int> TempSymbCode, Dictionary<char, List<int>> table, Action<string> onDraw)
        {
            if (root.Left != null) // если левое поддерево существует
            {
                TempSymbCode.Add(0); // добавляяем в код 0
                
                BuidTable(root.Left, TempSymbCode, table, onDraw); // добавляяем в код 0
            }

            if (root.Right != null) // если правое поддерево существует
            {
                TempSymbCode.Add(1); // добавляяем в код 1
                
                BuidTable(root.Right, TempSymbCode, table, onDraw); // идем на право
            }
            
            if (root.c == '\0') // если у элемента нет символа
            {
                //onDraw.Invoke(root.a.ToString());//<-
                if (TempSymbCode.Count == 0)
                {
                    return;
                }
                
                TempSymbCode.RemoveAt(TempSymbCode.Count - 1);
                return;
            }//else onDraw.Invoke($"{root.c.ToString()} ({root.a.ToString()})");//<-
            
            table[root.c] = TempSymbCode; // добавляем в словарь код
            Console.Write(root.c + " ");
            foreach (var num in TempSymbCode)
            {
                Console.Write(num);
            }

            Console.WriteLine();
            TempSymbCode.RemoveAt(TempSymbCode.Count - 1); // удаляем последний символ с кода(возвратимся к родителю)
        }

        // сортировка символов пузырьком по количеству их вхождений
        private void ListSort(List<HuffmanTreeNode> t)
        {
            HuffmanTreeNode temp;
            for (int i = 0; i < t.Count; i++)
            {
                for (int j = i + 1; j < t.Count; j++)
                {
                    if (t[i].weight > t[j].weight)
                    {
                        temp = t[i];
                        t[i] = t[j];
                        t[j] = temp;
                    }
                }
            }
        }

        public HuffmanTreeNode Process(string str)
        {
            List<HuffmanTreeNode> tmp = new List<HuffmanTreeNode>();
            tmp = Fill(str);
            
            //кодирование
            List<int> TempSymbCode = new List<int>();
            // словарь с кодом для каждого символа
            Dictionary<char, List<int>> table = new Dictionary<char, List<int>>();
            // построение словаря
            var root = Build(tmp);
            BuidTable(root, TempSymbCode, table, null);
            // вывод символов
            //return root.GetBinaryTreeHuffmanTreeNode();
            return root;
        }
        
        private bool ContainsSymb(List<HuffmanTreeNode> t, char c)
        {
            foreach (HuffmanTreeNode n in t)
            {
                if (n.c == c)
                {
                    return true;
                }
            }

            return false;
        }

        private int FindSymb(List<HuffmanTreeNode> t, char c)
        {
            int i = 0;
            foreach (HuffmanTreeNode n in t)
            {
                if (n.c == c)
                {
                    return i;
                }

                i++;
            }

            return i;
        }

      
        
        
        public HuffmanTreeNode Build(List<HuffmanTreeNode> t)
        {
            var tree = new HuffmanTree();
            // создание дерева
            while (t.Count != 1) // пока в дереве более одного элемента
            {
                ListSort(t); // сортировка элементов по возрастанию

                // создание левого поддерева
                var l = t[0];
                // удаление
                t.RemoveAt(0);
                // создание правого поддерева
                var r = t[0];
                //удаление
                t.RemoveAt(0);

                // создание нового элемента с правым и левым поддеревом
                HuffmanTreeNode parent = new HuffmanTreeNode(l, r);
                parent.weight = l.weight + r.weight; // вес родителя
                t.Add(parent);
                Print(t);
            }

            // последний оставшийся в списке - корень дерева
            HuffmanTreeNode root = t[0];
            return root;
        }

        private void Print(List<HuffmanTreeNode> list)
        {
            foreach (var node in list)
            {
                Console.Write($" {node.c}: {node.weight}|");
            }

            Console.WriteLine();
        }

        public List<HuffmanTreeNode> Fill(string s)
        {
            //создание списка с символами строки и количеством их вхождений
            List<HuffmanTreeNode> t = new List<HuffmanTreeNode>();
            // заполнение списка
            for (int i = 0; i < s.Length; i++)
            {
                // текущий символ
                char current = Convert.ToChar(s[i]);
                if (!ContainsSymb(t, current))
                {
                    t.Add(new HuffmanTreeNode(1, current)); // если нет в списке добавляем новый
                }
                else
                {
                    t[FindSymb(t, current)].weight += 1; // если есть в списке увеличиваем число вхождений
                }
                
                Print(t);
                Console.WriteLine("--------------------------------------------------------------------------");
            }

            return t;
        }
    }
}