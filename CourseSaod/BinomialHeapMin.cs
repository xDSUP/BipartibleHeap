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
            public HeapNode brother;
            #endregion

            #region PublicZone
            public int Key { get; set; } //вес
            public int Degree
            {
                get
                {
                    //подсчет степени по кол-ву детей
                    int i = 0;
                    for (var c = this.child; c != null; i++, c = c.child) ;
                    return i;
                }
            } //степень, кол-во детей.
            public HeapNode(int key) => Key = key;
            public void CorrectSequense()
            {
                HeapNode n = this;
                HeapNode beforeNode = n;
                HeapNode tmp = n.brother;
                var curNode = n.brother;

                while (curNode != null)
                {
                    if (beforeNode.Degree >= curNode.Degree)
                    {
                        tmp = curNode.brother;
                        curNode.brother = beforeNode;
                        if (beforeNode == n)
                            beforeNode.brother = null;
                        beforeNode = curNode;
                        curNode = tmp;
                        continue;
                    }
                    beforeNode = curNode;
                    curNode = curNode.brother;
                }

            }
            public void Show()
            {
                for (var curNode = this; curNode != null; curNode = curNode.child)
                {
                    for (var curNodeBrother = curNode.brother; curNodeBrother != null; curNodeBrother = curNodeBrother.brother)
                    {
                        // curNodeBrother()
                    }
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
            HeapNode beforeNode = n;
            HeapNode tmp = n.brother;
            var curNode = n.brother;

            while (curNode != null)
            {
                if (beforeNode.Degree >= curNode.Degree)
                {
                    tmp = curNode.brother;
                    curNode.brother = beforeNode;
                    if (beforeNode == n)
                        beforeNode.brother = null;
                    beforeNode = curNode;
                    curNode = tmp;
                    continue;
                }
                beforeNode = curNode;
                curNode = curNode.brother;
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

            //if (this._minNode.Key < mergebleHeap._minNode.Key)
            //    result._minNode = this._minNode;
            //else if (this._minNode.Key == mergebleHeap._minNode.Key && this._minNode.Degree < mergebleHeap._minNode.Degree)
            //    result._minNode = this._minNode;
            //else
            //    result._minNode = mergebleHeap._minNode;

            var curThis = this.head;
            var curMergebleHeap = mergebleHeap.head;

            //установка первого корня в результат
            bool comp = this.head.Degree <= mergebleHeap.head.Degree;
            result.head = comp ? this.head : mergebleHeap.head;
            if (comp)
                curThis = curThis.brother;
            else
                curMergebleHeap = curMergebleHeap.brother;

            HeapNode curRes = result.head;

            // совмещаем список корней
            while (curThis != null && curMergebleHeap != null)
            {
                if (curThis.Degree < curMergebleHeap.Degree)
                {
                    curRes.brother = curThis;
                    curRes = curRes.brother;
                    curThis = curThis.brother;
                }
                else
                {
                    curRes.brother = curMergebleHeap;
                    curRes = curRes.brother;
                    curMergebleHeap = curMergebleHeap.brother;
                }
            }
            while (curThis != null)
            {
                curRes.brother = curThis;
                curRes = curRes.brother;
                curThis = curThis.brother;
            }
            while (curMergebleHeap != null)
            {
                curRes.brother = curMergebleHeap;
                curRes = curRes.brother;
                curMergebleHeap = curMergebleHeap.brother;
            }
            // объединение деревьев одной степени 
            curRes = result.head;
            //Console.WriteLine( " GHB " + result._minNode.Key);
            while (curRes.brother != null)
            {
                // проверим нет ли 3-х подряд идущих, если есть сливаем последних два
                if (curRes.brother.brother != null)
                {
                    if (curRes.brother.Degree == curRes.brother.brother.Degree)
                    {
                        if (curRes.Key < result._minNode.Key) result._minNode = curRes;
                        curRes = curRes.brother;
                    }
                        
                }
                if (curRes.Degree == curRes.brother.Degree)
                {
                    //вырезаем curRes из списка корней
                    HeapNode c;
                    if (curRes.Key > curRes.brother.Key)
                    {
                        //это дерево, куда мы вставляем
                        HeapNode smallestNode = curRes.brother;

                        //подвязываем одно дерево к другому
                        curRes.brother = smallestNode.child; // братом вставляемому будет ребенок нашего темпа
                        curRes.parent = smallestNode;
                        smallestNode.child = curRes; // делаем наш вставляемый ребенком
                        curRes = smallestNode; // делаем текущим к ссылке на следующее дерево

                        //из списка корней удаляем ссылку на вставленное дерево
                        if (result.head != curRes.child)
                        {
                            // просто проходит по списку корней, пока не находит 
                            for (c = result.head; c.brother != curRes.child; c = c.brother) ;
                            c.brother = smallestNode;
                        }
                        else
                        {
                            result.head = smallestNode;
                        }
                    }
                    else
                    {
                        //подвязываем одно дерево к другому
                        var lagestNode = curRes.brother;
                        curRes.brother = lagestNode.brother;
                        lagestNode.brother = curRes.child;
                        lagestNode.parent = curRes;
                        curRes.child = lagestNode;
                    }
                    if (curRes.Key < result._minNode.Key) result._minNode = curRes;
                    continue;
                }
                if (curRes.Key < result._minNode.Key) result._minNode = curRes;
                curRes = curRes.brother;
            }
            //проверим последний
            if (curRes.Key < result._minNode.Key) result._minNode = curRes;
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
            for (HeapNode curNode = head; curNode != null; beforeCurNode = curNode, curNode = curNode.brother)
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
                head = _minNode.brother;
            else
                beforeMinNode.brother = _minNode.brother;

            // сохраняем ребенка удаленного узла, если он был
            BinomialHeapMin tmp = null;
            if (_minNode.child != null)
            {
                tmp = new BinomialHeapMin(_minNode.child);
                // убираем ссылку на удаленный элемент из его детей

                // TODO: Убрать 1 цикл из создания пирамиды в прав порядке.
                tmp._minNode = new HeapNode(int.MaxValue);
                for (var curNode = tmp.head; curNode != null; curNode = curNode.brother)
                {
                    if (tmp._minNode.Key > curNode.Key) 
                        tmp._minNode = curNode;
                    curNode.parent = null;
                }
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
