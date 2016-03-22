using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharListControl : MonoBehaviour {

    public GameObject Content;
    public Button ItemPrefab;

    public void Add( string item )
    {
        var newItem = GameObject.Instantiate<Button>(ItemPrefab);
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
