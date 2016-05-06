using UnityEditor;using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 在界面上显示UIBinder的两个主要功能
    /// </summary>
    [CustomEditor(typeof(DataContext))]
    class UIBinderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DataContext binder = target as DataContext;

            if (GUILayout.Button("Add Binder To Top Child"))
            {
                binder.AddBinderToAllTopChild();
            }

            if (GUILayout.Button("Remove All Top Child Binder"))
            {
                binder.RemoveAllTopChildBinder();
            }


            base.OnInspectorGUI();
        }
    }

}