using System.Collections.Generic;
using System;

public class PriorityQueue<T> where T : class
{
    private List<T> items_ = new List<T>();
    private Dictionary<T, int> costs_ = new Dictionary<T, int>();

    public bool Empty
    {
        get { return costs_.Count == 0; }
    }

    public void Add(T t, int cost)
    {
        int index = items_.Count;
        items_.Add(t);
        costs_.Add(t, cost);
        MoveUp(index);
    }

    public bool Contains(T t)
    {
        return costs_.ContainsKey(t);
    }

    public T Dequeue()
    {
        if (Empty)
            throw new InvalidOperationException();

        if(items_.Count == 1)
        {
            T ret = items_[0];
            items_.Clear();
            costs_.Clear();
            return ret;
        }
        else
        {
            T ret = items_[0];
            costs_.Remove(ret);

            //Swap last item and first item.
            items_[0] = items_[items_.Count - 1];

            //Put the new first item in the right place.
            MoveDown(0);
            return ret;
        }
    }

    public void Update(T t, int cost)
    {
        //We probably don't want to be doing a linear search in the future.
        costs_[t] = cost;
        for(int i = 0; i < items_.Count; ++i)
        {
            if(items_[i] == t)
            {
                //Assume that the item's cost only ever decreases
                MoveUp(i);
                return;
            }
        }

        throw new InvalidOperationException();
    }

    private void MoveUp(int index)
    {
        T myItem = items_[index];
        int myCost = costs_[myItem];
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if(costs_[items_[parent]] <= myCost)
            {
                break;
            }

            items_[index] = items_[parent];
            items_[parent] = myItem;
            index = parent;
        }
    }

    private void MoveDown(int index)
    {
        T myItem = items_[index];
        int myCost = costs_[myItem];
        while (index > 0)
        {
            int child1 = index * 2 + 1;
            int child2 = child1 + 1;
            if (costs_[items_[child1]] >= myCost && costs_[items_[child2]] >= myCost)
            {
                break;
            }
            else if(costs_[items_[child1]] >= myCost)
            {
                items_[index] = items_[child2];
                items_[child2] = myItem;
                index = child2;
            }
            else if (costs_[items_[child2]] >= myCost)
            {
                items_[index] = items_[child1];
                items_[child1] = myItem;
                index = child1;
            }
            else
            {
                if(costs_[items_[child1]] < costs_[items_[child2]])
                {
                    items_[index] = items_[child1];
                    items_[child1] = myItem;
                    index = child1;
                }
                else
                {
                    items_[index] = items_[child2];
                    items_[child2] = myItem;
                    index = child2;
                }
            }

        }
    }
}
