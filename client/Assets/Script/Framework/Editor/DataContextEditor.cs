using UnityEditor;using UnityEngine;

namespace Framework
{
    [CustomEditor(typeof(DataContext))]
    class DataContextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DataContext ctx = target as DataContext;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Detect Type"))
            {
                ctx.Detect();
            }


            ctx.Type = (WidgetType)EditorGUILayout.EnumPopup( ctx.Type);

            GUILayout.EndHorizontal();

            switch( ctx.Type )
            {
                case WidgetType.InputField:
                case WidgetType.Text:
                    {
                        ctx.SyncType = (DataSyncType)EditorGUILayout.EnumPopup("SyncType", ctx.SyncType);
                        break;
                    }
            }


            switch (ctx.Type)
            {
                case WidgetType.View:
                    {
                        EditorGUILayout.LabelField("View: " + ViewTemplate.ClassName(ctx));
                        EditorGUILayout.LabelField("Presenter: " + PresenterTemplate.ClassName(ctx));
                        break;
                    }
                case WidgetType.ScrollRect:
                    {
                        EditorGUILayout.LabelField("Instance in View: _" + ctx.Name);
                        EditorGUILayout.LabelField("Property in Presenter: " + ctx.Name);
                        EditorGUILayout.LabelField("Item View: " + ViewTemplate.ClassItemName( ctx ) );
                        EditorGUILayout.LabelField("Item Presenter: " + PresenterTemplate.ClassItemName(ctx));
                        break;
                    }
                case WidgetType.Button:
                    {
                        EditorGUILayout.LabelField("Command: " + PresenterTemplate.Command( ctx ));                        
                        break;
                    }
                case WidgetType.InputField:
                case WidgetType.Text:
                    {
                        EditorGUILayout.LabelField("Instance in View: _" + ctx.Name);
                        EditorGUILayout.LabelField("Property in Presenter: " + ctx.Name);
                        break;
                    }
            }



            if (GUILayout.Button("Add To Child"))
            {
                ctx.AddToTopChild();
            }

            if (GUILayout.Button("Remove Child"))
            {
                if (EditorUtility.DisplayDialog("移除确认?", "移除所有子节点的DataContext", "是", "否"))
                {
                    ctx.RemoveAllChild();
                }
                
            }

            if (GUILayout.Button("Detect Child"))
            {
                ctx.DetectAllChild();
            }

            base.OnInspectorGUI();
        }
    }

}