using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITree<T>
{   

    public ITreeElement<T> current;
    public ITreeElement<T> root;

    public ITreeElement<T> getRoot()
    {
        return root = new ITreeElement<T>();
    }

    public bool isClear()
    {
        return root.getChildren().Count == 0;
    }

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
}