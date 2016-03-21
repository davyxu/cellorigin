using UnityEngine;

public class ObjectUtility
{
    public static string GetGameObjectPath(GameObject obj, GameObject relativeTo = null)
    {
        string path = obj.name;
        while (obj.transform.parent != null)
        {            
            obj = obj.transform.parent.gameObject;

            if (obj == relativeTo)
                break;

            path =  obj.name + "/" + path;
        }

        // 绝对引用加/
        if ( relativeTo == null )
        {
            path = "/" + path;
        }

        return path;
    }
}
