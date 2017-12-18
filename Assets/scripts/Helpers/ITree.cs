using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITree<T>
{

    public ITreeElement<T> root;
    public ITreeElement<T> current;


    public ITreeElement<T> setRoot()
    {
        root = new ITreeElement<T>();
        current = root;

        return root;
    }

    public bool isClear()
    {
        return root.getChildren().Count == 0;
    }

    public void headRoot()
    {
        current = root;
    }

    public void headParent()
    {
        if (current != root)
            current = current.getParent();
    }

    /*public bool treeEquals(ITree<T> tree)
    {
        ITreeElement<T> startHead = current;
        ITreeElement<T> startHeadTree = tree.current;

        headRoot();
        tree.headRoot();

        return checkChildren(tree);
    }

    private bool checkChildren(ITree<T> tree)
    {
        if (tree.current.getChildren().Equals(current.getChildren()))
        {

            foreach (ITreeElement<T> n in treeElement.getChildren())
            {

            }
        }
        else
            return false;
            
    }*/
    

    public class ITreeElement<L>
    {
        public L value;
        ITreeElement<L> parent;
        List<ITreeElement<L>> children;

        public ITreeElement(L value, ITreeElement<L> parent)
        {
            this.value = value;
            this.parent = parent;
            children = new List<ITreeElement<L>>();
        }

        public ITreeElement()
        {
            children = new List<ITreeElement<L>>();
        }

        public List<ITreeElement<L>> getChildren()
        {
            return children;
        }

        public ITreeElement<L> getParent()
        {
            return parent;
        }

        public void addChildren(L value)
        {
            children.Add(new ITreeElement<L>(value, this));
        }

        public bool isLast()
        {
            if (children == null)
                return true;
            else
                return false;
        }
    }

    /*public class TreeElement<L> : IEnumerable
    {
        private ITreeElement<L>[] _treeElement;
        public TreeElement(ITreeElement<L>[] pArray)
        {
            _treeElement = new ITreeElement<L>[pArray.Length];

            for (int i = 0; i < pArray.Length; i++)
            {
                _treeElement[i] = pArray[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ITreeElementEnum<L> GetEnumerator()
        {
            return new ITreeElementEnum<L>(_treeElement);
        }

        public class ITreeElementEnum<K> : IEnumerator
        {
            public ITreeElement<K>[] _treeElement;

            int position = -1;

            public ITreeElementEnum(ITreeElement<K>[] list)
            {
                _treeElement = list;
            }

            public bool MoveNext()
            {
                position++;
                return (position < _treeElement.Length);
            }

            public void Reset()
            {
                position = -1;
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public ITreeElement<K> Current
            {
                get
                {
                    try
                    {
                        return _treeElement[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }
    }*/
}