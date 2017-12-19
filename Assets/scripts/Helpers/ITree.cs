using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITree<T>
{

    ITreeElement<T> root;

    public ITree()
    {
        root = new ITreeElement<T>();
    }

    public ITreeElement<T> getRoot()
    {
        return root;
    }

    public bool isClear()
    {
        return root.getChildren().Count == 0;
    }

    /*getBranches*/
    public List<ITreeBranch<T>> getBranches(ITreeElement<T> junc)
    {
        List<ITreeBranch<T>> branches = new List<ITreeBranch<T>>();
        List<ITreeElement<T>> lasts = new List<ITreeElement<T>>();
        getLasts(junc, lasts); // out lasts

        foreach (ITreeElement<T> iter in lasts)
        {
            branches.Add(getBranch(iter));
        }
        return branches;
    }

    private void getLasts(ITreeElement<T> elem, List<ITreeElement<T>> lasts)
    {
        if (!elem.isLast())
        {
            foreach (ITreeElement<T> iter in elem.getChildren())
            {
                getLasts(iter, lasts);
            }
        }
        else
            lasts.Add(elem);
    }

    private ITreeBranch<T> getBranch(ITreeElement<T> element)
    {
        if (element.isLast())
        {
            ITreeBranch<T> branch = new ITreeBranch<T>(element.getBranchName());
            while (element != getRoot())
            {
                branch.Add(element);
                element = element.getParent();
            }

            branch.Reverse();
            return branch;
        }
        else
            Debug.LogError("Can't get branch by non-last element  (" + this.ToString() + ")");
        return null;
    }
    /*getBranches*/
}

public class ITreeElement<T>
{
    public T value;
    ITreeElement<T> parent;
    List<ITreeElement<T>> children;

    private string branchName;

    public ITreeElement(T value, ITreeElement<T> parent)
    {
        this.value = value;
        this.parent = parent;
    }

    public ITreeElement()
    {

    }

    public List<ITreeElement<T>> getChildren()
    {
        return children;
    }

    public ITreeElement<T> getParent()
    {
        return parent;
    }

    public ITreeElement<T> addChild(T value)
    {
        ITreeElement<T> child = new ITreeElement<T>(value, this);

        if (children == null)
            children = new List<ITreeElement<T>>();

        children.Add(child);

        return child;
    }

    public bool isLast()
    {
        if (children == null || branchName != null)
            return true;
        else
            return false;
    }

    public override string ToString()
    {
        if (value != null)
            return value.ToString();
        else
            return null;
    }

    public void setBranchName(string name)
    {
        if (isLast())
            branchName = name;
        else
            Debug.LogError("Can't set branch name by non-last element  (" + this.ToString() + ")");
    }

    public string getBranchName()
    {
        return branchName;
    }
}

public class ITreeBranch<T> : List<ITreeElement<T>>
{
    public string name;

    public ITreeBranch(string name)
    {
        this.name = name;
    }

    public override string ToString()
    {
        string res = name + ":  { root ; ";
        foreach (ITreeElement<T> i in this)
        {
            if (!i.isLast())
                res += i.ToString() + " ; ";
            else
                res += i.ToString() + " }";
        }
        return res;
        //return name;
    }
}