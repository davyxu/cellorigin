using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class CharListView : MonoBehaviour
{
    CharListViewModel _ViewModel;

    InputField _CharName;
    Button _SelectChar;
    Button _CreateChar;


    void InitUI()
    {
        _ViewModel = new CharListViewModel();

        var trans = this.transform;
        _CharName = trans.Find("CharName").GetComponent<InputField>();
        _SelectChar = trans.Find("SelectChar").GetComponent<Button>();
        _CreateChar = trans.Find("CreateChar").GetComponent<Button>();

        _CreateChar.onClick.AddListener(_ViewModel.Command_CreateChar);
        _SelectChar.onClick.AddListener(_ViewModel.Command_SelectChar);

        // CharName
        _CharName.onValueChanged.AddListener(x =>
        {
            _ViewModel.CharNameForCreate = x;
        });
        // 初始化同步
        _ViewModel.CharNameForCreate = _CharName.text;


    }

    void Awake()
    {
        InitUI();
    }

    void Start()
    {
        _ViewModel.Start();
    }

    void OnDisable()
    {

    }
}
