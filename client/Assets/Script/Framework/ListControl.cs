using UnityEngine;

public class ListControl : MonoBehaviour 
{

    public GameObject Content;
    public GameObject ItemPrefab;

    public void Add<ViewType, PresenterType>(object key, PresenterType value)
        where ViewType : BaseItemView 
        where PresenterType: BasePresenter
    {
        var newItem = GameObject.Instantiate<GameObject>(ItemPrefab);
        newItem.transform.SetParent(Content.transform, false );
        
        var view = newItem.gameObject.AddComponent<ViewType>();
        view.Key = key;
        view.Bind(value);
    }

    public ViewType Get<ViewType>(object key)
        where ViewType: BaseItemView
    {
        var list = Content.GetComponentsInChildren<ViewType>();
        foreach (ViewType item in list)
        {
            if ( item.Key.Equals(key ) )
            {
                return item;
            }
        }

        return null;
    }

    public void Remove<ViewType>(object key)
        where ViewType : BaseItemView
    {
        var list = Content.GetComponentsInChildren<ViewType>();
        foreach (ViewType item in list)
        {
            if (item.Key.Equals(key) )
            {
                GameObject.Destroy(item.gameObject);
                break;
            }
        }
    }

    public void Clear<ViewType>()
        where ViewType : BaseItemView
    {
        var list = Content.GetComponentsInChildren<ViewType>();
        foreach (ViewType item in list)
        {
            GameObject.Destroy(item);
        }
    }
}
