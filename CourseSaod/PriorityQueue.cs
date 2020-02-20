using System;
using System.Collections.Generic;
using System.Text;

namespace CourseSaod
{
    class BinomialHeap
    {
        #region PrivateZoneHeap
        class HeapNode 
        {
            #region ZoneNode
            public HeapNode parent;
            public HeapNode child;
            public HeapNode brother;
            #endregion

            #region Zone
            public int Key { get; } //вес
            public int Degree{ get; set; } //степень, кол-во детей.
            public HeapNode(int key) => Key = key;
            #endregion
        }

        HeapNode head;
        public BinomialHeap() => head = null;
        BinomialHeap(int key) => head = new HeapNode(key);
        BinomialHeap(HeapNode n) 
        {
            
            HeapNode beforeNode = n;
            HeapNode tmp = n.brother;
            var curNode = n.brother;

            while (curNode != null)
            {
                // меняем порядок узлов из 2-1-0 в 0-1-2
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
            BinomialHeap temp = new BinomialHeap(key);
            head = this.Merge(temp).head;
        }
        public BinomialHeap Merge(BinomialHeap mergebleHeap)
        {
            if (this == null || this.head == null) return mergebleHeap;
            if (mergebleHeap == null || mergebleHeap.head == null) return this;
            BinomialHeap result = new BinomialHeap();
            
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
                if(curThis.Degree < curMergebleHeap.Degree)
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
            while(curThis!= null)
            {
                curRes.brother = curThis;
                curRes = curRes.brother;
                curThis = curThis.brother;
            }
            while(curMergebleHeap != null)
            {
                curRes.brother = curMergebleHeap;
                curRes = curRes.brother;
                curMergebleHeap = curMergebleHeap.brother;
            }
            // объединение деревьев одной степени 
            curRes = result.head;
            while(curRes.brother != null)
            {
                // проверим нет ли 3-х подряд идущих, если есть сливаем последних два
                if(curRes.brother.brother!= null)
                {
                    if (curRes.brother.Degree == curRes.brother.brother.Degree)
                        curRes = curRes.brother;
                }
                if(curRes.Degree == curRes.brother.Degree)
                {
                    //вырезаем curRes из списка корней
                    HeapNode c;
                    if(curRes.Key >= curRes.brother.Key)
                    {
                        //это дерево, куда мы вставляем
                        HeapNode smallestNode = curRes.brother;
                        
                        //подвязываем одно дерево к другому
                        curRes.brother = smallestNode.child; // братом вставляемому будет ребенок нашего темпа
                        curRes.parent = smallestNode;
                        curRes.parent.Degree++;
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
                        lagestNode.parent.Degree++;
                        curRes.child = lagestNode;
                    }
                    continue;
                }
                curRes = curRes.brother;
            }
            return result;
        }

        public int GetMin()
        {
            // слабое место
            int min = head.Key;
            for (var c = head; c.brother != null; c = c.brother)
                if (min > c.brother.Key) min = c.brother.Key;
            return min;
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
                if (minKey > curNode.Key)
                {
                    minKey = curNode.Key;
                    beforeMinNode = beforeCurNode;
                    minNode = curNode;
                }
            }
            //удаляем ссылку из списка корней на мин элемент
            if (beforeMinNode == null)
                head = minNode.brother;
            else
                beforeMinNode.brother = minNode.brother;

            // сохраняем ребенка удаленного узла, если он был
            BinomialHeap tmp = null;
            if (minNode.child != null)
            {
                tmp = new BinomialHeap(minNode.child);
                // убираем ссылку на удаленный элемент из его детей
                for (var curNode = tmp.head; curNode != null; curNode = curNode.brother)
                    curNode.parent = null;
            }
            // сливаем оставшиеся пирамиду детей удал узла и нашу вместе
            this.head = this.Merge(tmp)?.head;

            return minKey;
        }

        #endregion
    }
}
