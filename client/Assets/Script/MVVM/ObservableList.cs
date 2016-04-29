
using System;
using System.Collections;
using System.Collections.Generic;

public class ObservableList<T>
{
    Dictionary<object, T> _itemMap = new Dictionary<object,T>();

    public Action<int, object> OnItemAdded;
    public Action OnItemRemoved;
    public Action<int, object> OnItemUpdated;
    public Action OnItemTotalChanged;   

    //public void Add( T elem )
    //{
    //    _item.Add(elem);

    //    if (OnItemAdded != null)
    //    {
    //        OnItemAdded(elem);
    //    }
    //}

    //public void Remove( T elem )
    //{
    //    _item.Remove(elem);

    //    if (OnItemRemoved != null)
    //    {
    //        OnItemRemoved();
    //    }
    //}

    //public void Update(int index, T elem )
    //{
    //    _item[index] = elem;

    //    if ( OnItemUpdated != null )
    //    {
    //        OnItemUpdated(index, elem);
    //    }
    //}

    public T Get( int index )
    {
        return _itemMap[index];
    }    

    public void Clear()
    {
        _itemMap.Clear();

        NotifyTotalChanged();
    }

    public void Visit( Action<object, T> callback )
    {
        foreach( var kv in _itemMap)
        {
            callback(kv.Key, kv.Value);
        }
    }

    public void FromList( List<T> list )
    {
        _itemMap.Clear();

        for(int i = 0;i< list.Count;i++)
        {
            _itemMap.Add(i, list[i]);
        }

        NotifyTotalChanged();
    }

    public void NotifyTotalChanged()
    {
        if (OnItemTotalChanged != null)
        {
            OnItemTotalChanged();
        }
    }

    
}