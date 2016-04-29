using UnityEngine;

public class ListControl : MonoBehaviour 
{

    public GameObject Content;
    public ListItem ItemPrefab;

    public ListItem Add<ViewType, ViewModelType>( object key, ViewModelType value) where ViewType: BaseView 
                                                                                   where ViewModelType: BaseViewModel
    {
        var newItem = GameObject.Instantiate<ListItem>(ItemPrefab);
        newItem.transform.SetParent(Content.transform, false );
        newItem.Key = key;

        var valueCom = newItem.gameObject.AddComponent<ViewType>();
        valueCom.Bind(value);

        return newItem;
    }

    public ListItem Get( object key )
    {
        var list = Content.GetComponentsInChildren<ListItem>();
        foreach( ListItem item in list )
        {
            if ( item.Key.Equals(key ) )
            {
                return item;
            }
        }

        return null;
    }

    public void Remove( object key )
    {
        var list = Content.GetComponentsInChildren<ListItem>();
        foreach (ListItem item in list)
        {
            if (item.Key.Equals(key) )
            {
                GameObject.Destroy(item.gameObject);
                break;
            }
        }
    }

    public void Clear( )
    {
        var list = Content.GetComponentsInChildren<ListItem>();
        foreach (ListItem item in list)
        {
            GameObject.Destroy(item);
        }
    }
}
