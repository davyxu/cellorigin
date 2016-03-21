using UnityEngine;
using System.Collections;
using UnityEditor;

public class ShowSelectObjectPath : MonoBehaviour {

    [MenuItem("UITools/查看路径")]
    public static void ShowPath( )
    {
        foreach( Object obj in Selection.objects )
        {
            var go = obj as GameObject;
            if (go == null)
                continue;

            Debug.Log(ObjectUtility.GetGameObjectPath(go));
        }
    }
}
