#define DEBUG_INSERT
#define DEBUG_MERGE

#undef DEBUG_INSERT
#undef DEBUG_MERGE



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
            #endregion
        }

        public HeapNode head;
        public HeapNode _minNode;

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

            BinomialHeapMin temp = new BinomialHeapMin(key);
            var t = this.Merge(temp);
            head = t.head;
            _minNode = t._minNode;

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
        public BinomialHeapMin Merge(BinomialHeapMin mergebleHeap)
        {
            if (this == null || this.head == null) return mergebleHeap;
            if (mergebleHeap == null || mergebleHeap.head == null) return this;

            BinomialHeapMin result = new BinomialHeapMin();
            result._minNode = new HeapNode(int.MaxValue);

            if (this._minNode.Key < mergebleHeap._minNode.Key)
                result._minNode = this._minNode;
            else if (this._minNode.Key == mergebleHeap._minNode.Key && this._minNode.Degree < mergebleHeap._minNode.Degree)
                result._minNode = this._minNode;
            else
                result._minNode = mergebleHeap._minNode;

            var curThis = this.head;
            var curMergebleHeap = mergebleHeap.head;

            // формируем новый список корней
            result.head = new HeapNode(-99991); // это лишний узел, к нему все подвяжем а потом удалим его.
            HeapNode curRes = result.head;
            // совмещаем список корней
            bool thisHeadAttached = false, MergeHeadAttached = false;
            // флаги, кот обозначают, что первый узел добавлен в результирующий список
            do
            {
                if (curThis.Degree < curMergebleHeap.Degree)
                {
                    //новому корню переопределим ссылки, чтобы были актуальны
                    curThis.leftBrother = curRes;
                    curRes.rightBrother = curThis;
                    curRes = curRes.rightBrother;
                    curThis = curThis.rightBrother;
                    thisHeadAttached = true;
                }
                else
                {
                    curMergebleHeap.leftBrother = curRes;
                    curRes.rightBrother = curMergebleHeap;
                    curRes = curRes.rightBrother;
                    curMergebleHeap = curMergebleHeap.rightBrother;
                    MergeHeadAttached = true;
                }
            } while (!(curThis == this.head && thisHeadAttached) && 
                    !(curMergebleHeap == mergebleHeap.head && MergeHeadAttached));

            while (!(curThis == this.head && thisHeadAttached))
            {
                curThis.leftBrother = curRes;
                curRes.rightBrother = curThis;
                curRes = curRes.rightBrother;
                curThis = curThis.rightBrother;
                thisHeadAttached = true;
            }

            while (!(curMergebleHeap == mergebleHeap.head && MergeHeadAttached))
            {
                curMergebleHeap.leftBrother = curRes;
                curRes.rightBrother = curMergebleHeap;
                curRes = curRes.rightBrother;
                curMergebleHeap = curMergebleHeap.rightBrother;
            } 

            // поправим все ссылки, чтобы работали корректно.
            result.head = result.head.rightBrother;
            result.head.leftBrother = curRes;
            curRes.rightBrother = result.head;
            curRes = result.head;

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


            // объединение деревьев одной степени 
            //Console.WriteLine( " GHB " + result._minNode.Key);
            while (curRes.rightBrother != result.head)
            {
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
                // проверим нет ли 3-х подряд идущих, если есть сливаем последних два
                if (curRes.rightBrother.rightBrother != null)
                {
                    if (curRes.rightBrother.Degree == curRes.rightBrother.rightBrother.Degree)
                    {
                        if (curRes.rightBrother.rightBrother != curRes)
                        {
                            if (curRes.Key < result._minNode.Key) result._minNode = curRes;
                            curRes = curRes.rightBrother;
                        }
                    }

                }
                if (curRes.Degree == curRes.rightBrother.Degree)
                {
                    //вырезаем curRes из списка корней
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
                    
                        if (result.head == smallestNode.child)
                            result.head = smallestNode;
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
                    if (curRes.child == result._minNode) result._minNode = curRes;

                    continue;
                }
                curRes = curRes.rightBrother;
            }
            //проверим последний
            return result;
        }

        public HeapNode GetMin()
        {
            return _minNode;
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
#endregion
    }
}
