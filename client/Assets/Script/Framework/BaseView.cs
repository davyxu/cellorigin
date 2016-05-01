using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseView : MonoBehaviour
{
    
    public virtual void Bind( BasePresenter presenter)
    {

    }
}


public class BaseItemView : BaseView
{
    public object Key;
}