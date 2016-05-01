
using System;
using System.Collections.Generic;

public delegate bool VisitHandle<KeyType, ValueType>(KeyType key, ValueType value);

public class ObservableCollection<KeyType, ValueType>
{
    Dictionary<KeyType, ValueType> _itemMap = new Dictionary<KeyType, ValueType>();

    public Action<KeyType, ValueType> OnItemAdded;
    public Action<KeyType> OnItemRemoved;
    public Action OnItemTotalChanged;

    public void Add(KeyType key, ValueType elem)
    {
        _itemMap.Add(key, elem);

        if (OnItemAdded != null)
        {
            OnItemAdded(key, elem);
        }
    }

    public void Remove(KeyType key)
    {
        _itemMap.Remove(key);

        if (OnItemRemoved != null)
        {
            OnItemRemoved(key);
        }
    }


    public ValueType Get(KeyType key)
    {
        ValueType v;
        if ( _itemMap.TryGetValue(key, out v ) )
        {
            return v;
        }

        return default(ValueType);
    }    

    public int Count
    {
        get { return _itemMap.Count; }
    }

    public void Clear()
    {
        _itemMap.Clear();

        if (OnItemTotalChanged != null)
        {
            OnItemTotalChanged();
        }
    }


    public void Visit(VisitHandle<KeyType,ValueType> callback)
    {
        foreach( var kv in _itemMap)
        {
            if ( !callback(kv.Key, kv.Value) )
            {
                return;
            }
        }
    }

}