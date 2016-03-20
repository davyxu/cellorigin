using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum CodeGenObjectType
{
    Unknown,
    GenAsCanvas,
    GenAsButton,
    GenAsInputField,
    GenAsText,
    GenAsImage,
    GenAsWindow,
}

[ExecuteInEditMode]
public class UIBinder : MonoBehaviour {

    public CodeGenObjectType Type;

    void OnEnable( )
    {
        // 启动探测类型
        Type = DetectType(gameObject);
    }

    static CodeGenObjectType DetectType(GameObject go)
    {
        if (go.GetComponent<Canvas>() != null &&
                go.GetComponent<CanvasScaler>() != null &&
                go.GetComponent<GraphicRaycaster>() != null)
        {
            return CodeGenObjectType.GenAsCanvas;
        }

        // 父级是canvas, 且带有UI尾缀, 识别为窗口
        if (go.transform.parent != null && 
            go.transform.parent.GetComponent<Canvas>() &&
            go.name.EndsWith("UI"))
        {
            return CodeGenObjectType.GenAsWindow;
        }


        if (go.GetComponent<Button>() != null)
        {
            return CodeGenObjectType.GenAsButton;
        }

        if (go.GetComponent<InputField>() != null)
        {
            return CodeGenObjectType.GenAsInputField;
        }

        if (go.GetComponent<Text>() != null)
        {
            return CodeGenObjectType.GenAsText;
        }

        if (go.GetComponent<Image>() != null)
        {
            return CodeGenObjectType.GenAsImage;
        }

        return CodeGenObjectType.Unknown;
    }
}

