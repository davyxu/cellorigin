using UnityEngine;

public class Singleton<T> where T : new()
{

    // Instead of compile time check, we provide a run time check
    // to make sure there is only one instance.
    protected Singleton() 
    {
        Debug.Assert(null == _instance); 
    }

    protected static T _instance = new T();

    public static T Instance
    {
        get { return _instance; }
    }
}
