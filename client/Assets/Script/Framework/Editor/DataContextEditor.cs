using UnityEditor;using UnityEngine;

namespace Framework
{
    [CustomEditor(typeof(DataContext))]
    class DataContextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DataContext ctx = target as DataContext;

            if (GUILayout.Button("Smart Fill"))
            {
                ctx.SmartFill();
            }


            if (GUILayout.Button("Add Binder To Top Child"))
            {
                ctx.AddBinderToAllTopChild();
            }

            if (GUILayout.Button("Remove All Top Child Binder"))
            {
                ctx.RemoveAllTopChildBinder();
            }


            base.OnInspectorGUI();
        }
    }

}