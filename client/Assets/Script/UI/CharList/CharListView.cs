using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class CharListView : MonoBehaviour
{
    CharListViewModel _ViewModel;

    InputField _CharName;
    Button _SelectChar;
    Button _CreateChar;

    ListControl _CharList;


    void InitUI()
    {
        _ViewModel = new CharListViewModel();

        var trans = this.transform;
        _CharName = trans.Find("CharName").GetComponent<InputField>();
        _SelectChar = trans.Find("SelectChar").GetComponent<Button>();
        _CreateChar = trans.Find("CreateChar").GetComponent<Button>();

        _CharList = trans.Find("CharList").GetComponent<ListControl>();


        _CreateChar.onClick.AddListener(_ViewModel.Command_CreateChar);
        _SelectChar.onClick.AddListener(_ViewModel.Command_SelectChar);


        // CharList
        _ViewModel.CharInfoList.OnItemTotalChanged += delegate {
            _CharList.Clear();

            _ViewModel.CharInfoList.Visit((key, value) =>
            {
                var item = _CharList.Add();
                item.GetComponent<SimpleCharInfoView>().ViewModel.CharName = value.CharName;
            });

        };

        _ViewModel.CharInfoList.OnItemUpdated += ( index, elem ) =>
        {

            ListItem item = _CharList.Get(index);
            item.GetComponent<SimpleCharInfoView>().ViewModel.CharName = elem as string;

        };

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
