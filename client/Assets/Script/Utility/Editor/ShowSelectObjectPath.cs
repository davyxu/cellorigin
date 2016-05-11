using UnityEngine;
using UnityEditor;

/// <summary>
/// 在日志中显示选中对象的绝对路径
/// </summary>
public class ShowSelectObjectPath : MonoBehaviour {

    [MenuItem("CellOrigin/查看GameObject路径")]
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
