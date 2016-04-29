using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ListControl : MonoBehaviour {

    public GameObject Content;
    public ListItem ItemPrefab;

    public ListItem Add( )
    {
        var newItem = GameObject.Instantiate<ListItem>(ItemPrefab);
        newItem.transform.SetParent(Content.transform, false );

        return newItem;
    }

    public ListItem Get( int index )
    {
        var list = Content.GetComponentsInChildren<ListItem>();
        foreach( ListItem item in list )
        {
            if ( item.Key == index )
            {
                return item;
            }
        }

        return null;
    }

    public void Clear( )
    {
        var list = Content.GetComponentsInChildren<ListItem>();
        foreach (ListItem item in list)
        {
            GameObject.Destroy(item);
        }
    }


    //void Start( )
    //{
    //    Add().GetComponentInChildren<Text>().text = "hello";
    //    Add().GetComponentInChildren<Text>().text = "world";
    //}
}
