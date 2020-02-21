using System;
using System.Collections.Generic;
using System.Text;

namespace CourseSaod
{
    class BinomialHeapMin
    {
        #region PrivateZoneHeap
        class HeapNode
        {
            #region PrivateZoneNode
            public HeapNode parent;
            public HeapNode child;
            public HeapNode rightBrother;
            //public HeapNode leftBrother;
            #endregion

            #region PublicZone
            public int Key { get; set; } //вес
            public int Degree { get; set; } //степень, кол-во детей.
            public HeapNode(int key) => Key = key;
            public void CorrectSequense()
            {
                HeapNode n = this;
                HeapNode beforeNode = n;
                HeapNode tmp = n.rightBrother;
                var curNode = n.rightBrother;

                while (curNode != null)
                {
                    if (beforeNode.Degree >= curNode.Degree)
                    {
                        tmp = curNode.rightBrother;
                        curNode.rightBrother = beforeNode;
                        if (beforeNode == n)
                            beforeNode.rightBrother = null;
                        beforeNode = curNode;
                        curNode = tmp;
                        continue;
                    }
                    beforeNode = curNode;
                    curNode = curNode.rightBrother;
                }

            }
            #endregion
        }

        HeapNode head;
        HeapNode _minNode;

        public BinomialHeapMin() => head = null;
        BinomialHeapMin(int key)
        {
            head = new HeapNode(key);
            _minNode = head;
        }
        
        BinomialHeapMin(HeapNode n)
        {
            _minNode = n;
            HeapNode beforeNode = n;
            HeapNode tmp = n.rightBrother;
            var curNode = n.rightBrother;

            while (curNode != null)
            {
                curNode.parent = null;
                if (curNode.Key != _minNode.Key) _minNode = curNode;
                if (beforeNode.Degree >= curNode.Degree)
                {
                    tmp = curNode.rightBrother;
                    curNode.rightBrother = beforeNode;
                    if (beforeNode == n)
                        beforeNode.rightBrother = null;
                    beforeNode = curNode;
                    curNode = tmp;
                    continue;
                }
                beforeNode = curNode;
                curNode = curNode.rightBrother;
            }
            n = beforeNode;
            head = n;
        }

        public void Insert(int key)
        {
            BinomialHeapMin temp = new BinomialHeapMin(key);
            var t = this.Merge(temp);
            head = t.head;
            _minNode = t._minNode;
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

            //установка первого корня в результат
            bool comp = this.head.Degree <= mergebleHeap.head.Degree;
            result.head = comp ? this.head : mergebleHeap.head;
            if (comp)
                curThis = curThis.rightBrother;
            else
                curMergebleHeap = curMergebleHeap.rightBrother;

            HeapNode curRes = result.head;

            // совмещаем список корней
            while (curThis != null && curMergebleHeap != null)
            {
                if (curThis.Degree < curMergebleHeap.Degree)
                {
                    curRes.rightBrother = curThis;
                    curRes = curRes.rightBrother;
                    curThis = curThis.rightBrother;
                }
                else
                {
                    curRes.rightBrother = curMergebleHeap;
                    curRes = curRes.rightBrother;
                    curMergebleHeap = curMergebleHeap.rightBrother;
                }
            }
            while (curThis != null)
            {
                curRes.rightBrother = curThis;
                curRes = curRes.rightBrother;
                curThis = curThis.rightBrother;
            }
            while (curMergebleHeap != null)
            {
                curRes.rightBrother = curMergebleHeap;
                curRes = curRes.rightBrother;
                curMergebleHeap = curMergebleHeap.rightBrother;
            }
            // объединение деревьев одной степени 
            curRes = result.head;
            //Console.WriteLine( " GHB " + result._minNode.Key);
            while (curRes.rightBrother != null)
            {
                // проверим нет ли 3-х подряд идущих, если есть сливаем последних два
                if (curRes.rightBrother.rightBrother != null)
                {
                    if (curRes.rightBrother.Degree == curRes.rightBrother.rightBrother.Degree)
                    {
                        if (curRes.Key < result._minNode.Key) result._minNode = curRes;
                        curRes = curRes.rightBrother;
                    }
                        
                }
                if (curRes.Degree == curRes.rightBrother.Degree)
                {
                    //вырезаем curRes из списка корней
                    HeapNode c;
                    if (curRes.Key > curRes.rightBrother.Key)
                    {
                        //это дерево, куда мы вставляем
                        HeapNode smallestNode = curRes.rightBrother;

                        //подвязываем одно дерево к другому
                        //curRes.leftBrother = smallestNode.child.leftBrother;
                        //smallestNode.child.leftBrother = curRes;
                        curRes.rightBrother = smallestNode.child; // братом вставляемому будет ребенок нашего темпа
                       
                        curRes.parent = smallestNode;
                        curRes.parent.Degree++;
                        smallestNode.child = curRes; // делаем наш вставляемый ребенком
                        curRes = smallestNode; // делаем текущим к ссылке на следующее дерево
                        

                        //из списка корней удаляем ссылку на вставленное дерево
                        if (result.head != curRes.child)
                        {
                            // просто проходит по списку корней, пока не находит 
                            for (c = result.head; c.rightBrother != curRes.child; c = c.rightBrother) ;
                            c.rightBrother = smallestNode;
                        }
                        else
                        {
                            result.head = smallestNode;
                        }
                    }
                    else
                    {
                        //подвязываем одно дерево к другому
                        var lagestNode = curRes.rightBrother;
                        curRes.rightBrother = lagestNode.rightBrother;
                        lagestNode.rightBrother = curRes.child;
                        lagestNode.parent = curRes;
                        lagestNode.parent.Degree++;
                        curRes.child = lagestNode;
                    }
                    if (curRes.child == result._minNode) result._minNode = curRes;
                    continue;
                }
                curRes = curRes.rightBrother;
            }
            //проверим последний
            return result;
        }

        private HeapNode GetMin()
        {
            return _minNode;
        }

        public int ExtractMin()
        {
            int minKey = int.MaxValue;
            HeapNode minNode = null;
            HeapNode beforeMinNode = null;
            HeapNode beforeCurNode = null;
            // ищем минимальный корень, запоминая корень перед ним
            for (HeapNode curNode = head; curNode != null; beforeCurNode = curNode, curNode = curNode.rightBrother)
            {
                if (_minNode == curNode)
                    beforeMinNode = beforeCurNode;
                else if (minKey > curNode.Key)
                {
                    minKey = curNode.Key;
                    minNode = curNode;
                }
            }
            //удаляем ссылку из списка корней на мин элемент
            if (beforeMinNode == null)
                head = _minNode.rightBrother;
            else
                beforeMinNode.rightBrother = _minNode.rightBrother;

            // сохраняем ребенка удаленного узла, если он был
            BinomialHeapMin tmp = null;
            if (_minNode.child != null)
            {
                tmp = new BinomialHeapMin(_minNode.child);
                // убираем ссылку на удаленный элемент из его детей

                // TODO: Убрать 1 цикл из создания пирамиды в прав порядке.
                //tmp._minNode = new HeapNode(int.MaxValue);
                //for (var curNode = tmp.head; curNode != null; curNode = curNode.rightBrother)
                //{
                //    if (tmp._minNode.Key > curNode.Key) 
                //        tmp._minNode = curNode;
                //    curNode.parent = null;
                //}
            }
            // сливаем оставшиеся пирамиду детей удал узла и нашу вместе
            var resultminNode = this._minNode;
            this._minNode = minNode;
            
            var t = this.Merge(tmp);
            this.head = t?.head;
            this._minNode = t?._minNode;
            
            return resultminNode.Key;
        }
        #endregion
    }
}
