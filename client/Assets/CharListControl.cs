using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharListControl : MonoBehaviour {

    public GameObject Content;
    public GameObject ItemPrefab;

    public void Add( string item )
    {
        var newItem = GameObject.Instantiate<GameObject>(ItemPrefab);
        newItem.transform.SetParent(Content.transform, false );
        var text = newItem.GetComponentInChildren<Text>();
        text.text = item;
    }

    void Start( )
    {
        Add("hello");
        Add("world");
    }
}
