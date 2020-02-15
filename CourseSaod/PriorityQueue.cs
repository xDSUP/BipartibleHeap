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
            #region PrivateZoneNode
            public HeapNode parent;
            public HeapNode child;
            public HeapNode brother;
            #endregion

            #region PublicZone
            public int Key { get; } //вес
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
        public BinomialHeap() => head = null;
        BinomialHeap(int key) => head = new HeapNode(key);
        BinomialHeap(HeapNode n) => head = n;

        public void Insert(int key)
        {
            var tempNode = new HeapNode(key);
            if(head == null) { head = tempNode; return; }
            BinomialHeap temp = new BinomialHeap(tempNode);
            head = this.Merge(temp).head;
        }
        public BinomialHeap Merge(BinomialHeap mergebleHeap)
        {
            if (this == null) return mergebleHeap;
            if (mergebleHeap == null) return this;
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
                        smallestNode.child = curRes; // делаем наш вставляемый ребенком
                        curRes = smallestNode; // делаем текущим к ссылке на следующее дерево
                        
                        //из списка корней удаляем ссылку на вставленное дерево
                        if (result.head != curRes.child)
                        {
                            // просто проходит по списку корней, пока не находит 
                            for (c = result.head; c.brother != curRes; c = c.brother) ;
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
                    continue;
                }
                curRes = curRes.brother;
            }
            return result;
        }

        public int GetMin()
        {
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

            var tmp = new BinomialHeap(minNode.child);
            // убираем ссылку на удаленный элемент из его детей
            for (var curNode = tmp.head; curNode != null; curNode = curNode.brother)
                curNode.parent = null;
            // сливаем оставшиеся пирамиды вместе
            this.head = this.Merge(tmp).head;

            return minKey;
        }

        public void Show()
        {
            for (var c = head; c != null; c = c.brother)
                c.Show();
        }
        #endregion
    }
}
