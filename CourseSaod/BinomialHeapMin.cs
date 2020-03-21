#define DEBUG_INSERT
#define DEBUG_MERGE

#undef DEBUG_INSERT
#undef DEBUG_MERGE


using System.Collections.Generic;
using System;

namespace CourseSaod
{
    class BinomialHeapMin
    {
        #region PrivateZoneHeap
        public class HeapNode
        {
            #region PrivateZoneNode
            public HeapNode parent;
            public HeapNode child;
            public HeapNode rightBrother;
            public HeapNode leftBrother;
            #endregion

            #region PublicZone
            public int Key { get; set; } //вес
            public int Degree { get; set; } //степень, кол-во детей.
            public HeapNode(int key)
            {
                Key = key;
                leftBrother = this;
                rightBrother = this;
            }

            public void attachLeft(HeapNode node)
            {
                this.leftBrother.rightBrother = node;
                node.leftBrother = this.leftBrother;
                this.leftBrother = node;
                node.rightBrother = this;
            }

            public void attachRight(HeapNode node)
            {
                this.rightBrother.leftBrother = node;
                node.rightBrother = this.rightBrother;
                this.rightBrother = node;
                node.leftBrother = this;
            }
            #endregion
        }

        public HeapNode head;
        public HeapNode _minNode;
        public Stack<HeapNode> stackTwoNodes = new Stack<HeapNode>();// стек, где храним указатели на "двоечки" в пирамиде
        //public LinkedList

        public int RootCount{get
            {
                int i = 0;
                HeapNode cur = head;
                do
                {
                    i++;
                    cur = cur.rightBrother;
                } while (cur != head);
                return i;
            }
        }

        public BinomialHeapMin() => head = null;
        BinomialHeapMin(int key)
        {
            head = new HeapNode(key);
            _minNode = head;
        }

        BinomialHeapMin(HeapNode n)
        {
            HeapNode newHead = n.leftBrother;

            _minNode = n.leftBrother;
            HeapNode curNode = newHead;
            HeapNode befCurNode = newHead.leftBrother;
            HeapNode temp;

            newHead.leftBrother = n;
            n.rightBrother = newHead;

            do
            {
                curNode.parent = null;
                if (curNode.Key < _minNode.Key) _minNode = curNode;
                curNode.rightBrother = befCurNode;
                temp = befCurNode.leftBrother;
                befCurNode.leftBrother = curNode;

                curNode = curNode.rightBrother;
                befCurNode = temp;
            } while (curNode != newHead);
            head = newHead;
        }

        public void Insert(int key)
        {
            // абстракция к избыточной двоичной СС, чтобы избавиться от каскадного слияния
            // 0 - дерева такого ранга нет
            // 1 - дерево есть такого ранга
            // 2 - есть два дерева такого ранга, ссылку на левое занесем в стек
            // при каждой вставке берем верхнее в стее в стеке дерево( если стек не пустой ) и связываем с правым его соседом по правило бин пирамиды
            // таким образом получаем вставку за 1.5 присоедениний и 1 сравнение 
            HeapNode temp = new HeapNode(key);

            if(this.head == null)
            {
                head = temp;
                _minNode = temp;
                return;
            }

            // update minPointer
            if (this._minNode.Key >= key)
                this._minNode = temp;

            // проверим есть ли у нас двоечки
            if (stackTwoNodes.Count != 0)
            {
                // если есть тогда соединяем(фиксим) их
                HeapNode curRes = stackTwoNodes.Pop().leftBrother;
                if (fix(curRes.rightBrother)) // используются ссылка с соседнего узла, чтобы в стек добавить нужный узел
                    stackTwoNodes.Push(curRes.rightBrother);
            }

            // новый вставляем в начало списка корней 
            this.head.attachLeft(temp);
            this.head = temp;
            // если узла не было просто вставим, если был, тогда сразу его подсоединим, чтобы не валялось двоечки сначала
            if (this.head.Degree == this.head.rightBrother.Degree)
            {
                if (fix(this.head))
                    stackTwoNodes.Push(this.head);
            }
            
#if DEBUG_INSERT
            Console.Write("[INSERT] ");
            BinomialHeapMin.HeapNode temp1 = head;
            Console.Write($"Вствляю {key} MinNode {_minNode.Key} | ");
            do
            {
                Console.Write(temp1.Key + $"({temp1.Degree})" + " ");
                temp1 = temp1.rightBrother;
            } while (temp1 != head);
            Console.WriteLine();
            Console.WriteLine();
#endif
        }
        static void join(HeapNode smallestNode, HeapNode largestNode)
        {
            largestNode.parent = smallestNode;
            largestNode.parent.Degree++;
            //меняем ссылки
            if (smallestNode.child != null)
            {
                largestNode.rightBrother = smallestNode.child;
                largestNode.leftBrother = smallestNode.child.leftBrother; // делаем цикличным
                smallestNode.child.leftBrother.rightBrother = largestNode;
                smallestNode.child.leftBrother = largestNode;
            }
            else
            {
                largestNode.rightBrother = largestNode;
                largestNode.leftBrother = largestNode;
            }
            smallestNode.child = largestNode;
            //возвращаем новый узел в наш список корней.
            smallestNode.leftBrother = smallestNode;
            smallestNode.rightBrother = smallestNode;
        }

        public BinomialHeapMin Merge(BinomialHeapMin mergebleHeap)
        {
            if (this == null || this.head == null) return mergebleHeap;
            if (mergebleHeap == null || mergebleHeap.head == null) return this;

            BinomialHeapMin smallHeap, largeHeap;
            // узнаем, в какой пирамидке меньше деревьев в списке корней
            if (this.RootCount <= mergebleHeap.RootCount)
            {
                smallHeap = this;
                largeHeap = mergebleHeap;
            }
            else
            {
                smallHeap = mergebleHeap;
                largeHeap = this;
            }

            if (this._minNode.Key < mergebleHeap._minNode.Key)
                largeHeap._minNode = this._minNode;
            else if (this._minNode.Key == mergebleHeap._minNode.Key && this._minNode.Degree < mergebleHeap._minNode.Degree)
                largeHeap._minNode = this._minNode;
            else
                largeHeap._minNode = mergebleHeap._minNode;


            var curSH = smallHeap.head;
            var curLH = largeHeap.head;

            // каждое дерево из самой короткой пирамиды присоеденим к длинной
            do
            {
                var h = curSH.rightBrother;
                // будем двигаться пока не найдем подходящее место для вставки этого узла
                if (curSH.Degree >= largeHeap.head.leftBrother.Degree) 
                    // если все деревья в длинном меньше, чем дерево из второго, вставим в конец
                    largeHeap.head.attachLeft(curSH);
                else
                {
                    HeapNode prevLH;
                    // храним предыдущее дерево, чтобы проверить наличие двух двоек подряд
                    for (prevLH = curLH; curLH.Degree <= curSH.Degree; curLH = curLH.rightBrother)
                        if (curLH.Degree > prevLH.Degree && curSH.Degree != curLH.Degree)
                            prevLH = curLH;
                    curLH.attachLeft(curSH);
                    
                    var temp = curLH.leftBrother; // узел, который вставили
                    // если имеем 3 узла подряд (т.е. до этого их было 2)
                    if (temp.leftBrother.leftBrother.Degree == temp.Degree)
                    {
                        if (fix(temp.leftBrother)) // объединим правых два узла
                            largeHeap.stackTwoNodes.Push(curLH.leftBrother);
                    }
                    // если был 1 узел, а теперь их 2
                    else if (temp.leftBrother.Degree == temp.Degree)
                    {
                        //если перед нашим узлом стоит 2 узла меньшей степени, то исправим двоечу
                        if (prevLH.Degree == temp.Degree - 1 && prevLH.leftBrother.Degree == prevLH.Degree)
                        {
                            if (fix(temp.leftBrother)) // объединим два узла
                                largeHeap.stackTwoNodes.Push(curLH.leftBrother);
                        }
                        else
                        {  // если было 0, или 1 и перед ней нет двойки, тогда починим справа двойку 
                            if (largeHeap.stackTwoNodes.Count != 0)
                            {
                                HeapNode c = stackTwoNodes.Pop().leftBrother;
                                if (fix(c.rightBrother))
                                    largeHeap.stackTwoNodes.Push(c.rightBrother);
                            }
                        }
                    } // если было 0, или 1 и перед ней нет двойки, тогда починим справа двойку 
                    else
                    {
                        if (largeHeap.stackTwoNodes.Count != 0)
                        {
                            HeapNode c = largeHeap.stackTwoNodes.Pop();
                            if (fix(c))
                                largeHeap.stackTwoNodes.Push(c);
                        }
                    }
                }
                curSH = h;
            } while (curSH != smallHeap.head);



#if DEBUG_MERGE
            Console.Write("[MERGE] ");
            BinomialHeapMin.HeapNode temp = curRes;
            Console.Write(_minNode.Key + " | ");
            do
            {
                Console.Write(temp.Key + $"({temp.Degree})" + " ");
                temp = temp.rightBrother;
            } while (temp != result.head);
            Console.WriteLine();
#endif
            return largeHeap;
        }

        public HeapNode GetMin()
        {
            return _minNode;
        }

        public String ToString()
        {
            String result = "";
            var curNode = this.head;
            int i = 0;
            do
            {
                if (curNode.Degree > i) 
                {
                    result += "0";
                }
                else if (curNode.Degree == i)
                {
                    int x = 1;
                    int t = curNode.Degree;
                    HeapNode temp;
                    for (temp = curNode.rightBrother; temp.Degree == t && temp != head; temp = temp.rightBrother)
                        x++;
                    result += x.ToString();
                    curNode = temp;
                }
                i++;

            } while (i <= this.head.leftBrother.Degree);
            return result;
        }

        public HeapNode ExtractMin()
        {
            int minKey = int.MaxValue;
            HeapNode minNode = null;
            HeapNode beforeMinNode = null;
            HeapNode beforeCurNode = null;
            // ищем минимальный корень, запоминая корень перед ним
            HeapNode curNode = head;
            
            if (head.rightBrother != head)
            {
                do
                {
                    if (_minNode == curNode)
                        beforeMinNode = beforeCurNode;
                    else if (minKey > curNode.Key)
                    {
                        minKey = curNode.Key;
                        minNode = curNode;
                    }
                    beforeCurNode = curNode;
                    curNode = curNode.rightBrother;
                } while (curNode != this.head);
                //удаляем ссылку из списка корней на мин элемент
                if (beforeMinNode == null)
                {
                    if (head == _minNode && head.rightBrother == _minNode)
                        _minNode.rightBrother.leftBrother = head.leftBrother;
                    head = _minNode.rightBrother;
                    if (head.rightBrother == _minNode)
                        head.rightBrother = head;
                    head.leftBrother = _minNode.leftBrother;
                    head.leftBrother.rightBrother = head;
                }
                else
                {
                    _minNode.rightBrother.leftBrother = beforeMinNode;
                    beforeMinNode.rightBrother = _minNode.rightBrother;
                }
            }
            else 
            {
                head = null;
            }

            // сохраняем ребенка удаленного узла, если он был
            BinomialHeapMin tmp = null;
            if (_minNode.child != null)
            {
                tmp = new BinomialHeapMin(_minNode.child);
            }
            // сливаем оставшиеся пирамиду детей удал узла и нашу вместе
            var resultminNode = this._minNode;
            this._minNode = minNode;

            var t = this.Merge(tmp);
            this.head = t?.head;
            this._minNode = t?._minNode;

            return resultminNode;
        }

        private bool fix(HeapNode curRes) // возвращает, нужно ли добавить в список двоек получившийся узел
        {
            if (curRes.Key > curRes.rightBrother.Key)
            {
                //это дерево, куда мы вставляем
                HeapNode smallestNode = curRes.rightBrother;

                HeapNode rightNode = curRes.rightBrother.rightBrother;
                HeapNode leftNode = curRes.leftBrother;

                if (rightNode == curRes)
                    rightNode = smallestNode;

                //подвязываем одно дерево к другому
                join(smallestNode, curRes);

                smallestNode.leftBrother = leftNode;
                smallestNode.leftBrother.rightBrother = smallestNode;
                smallestNode.rightBrother = rightNode;

                if (head == smallestNode.child)
                    head = smallestNode;
                curRes = smallestNode;
            }
            else
            {
                var largestNode = curRes.rightBrother;
                HeapNode rightNode = curRes.rightBrother.rightBrother;
                HeapNode leftNode = curRes.leftBrother;

                if (leftNode == largestNode)
                    curRes.leftBrother = largestNode.leftBrother;

                //подвязываем одно дерево к другому
                join(curRes, largestNode);

                curRes.leftBrother = leftNode;
                curRes.rightBrother = rightNode;
                curRes.rightBrother.leftBrother = curRes;
            }
            if (curRes.rightBrother != curRes && curRes.rightBrother.Degree == curRes.Degree)
                return true;
            return false;
        }
        #endregion
    }
}
