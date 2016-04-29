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

    Button _Add;
    Button _Remove;
    Button _Modify;


    void InitUI()
    {
        _ViewModel = new CharListViewModel();

        var trans = this.transform;
        _CharName = trans.Find("CharName").GetComponent<InputField>();
        _SelectChar = trans.Find("SelectChar").GetComponent<Button>();
        _CreateChar = trans.Find("CreateChar").GetComponent<Button>();

        _CharList = trans.Find("CharList").GetComponent<ListControl>();


        _Add = trans.Find("Add").GetComponent<Button>();
        _Add.onClick.AddListener(_ViewModel.Command_Add);
        _Remove = trans.Find("Remove").GetComponent<Button>();
        _Remove.onClick.AddListener(_ViewModel.Command_Remove);
        _Modify = trans.Find("Modify").GetComponent<Button>();
        _Modify.onClick.AddListener(_ViewModel.Command_Modify);


        _CreateChar.onClick.AddListener(_ViewModel.Command_CreateChar);
        _SelectChar.onClick.AddListener(_ViewModel.Command_SelectChar);


        // CharList
        _ViewModel.CharInfoList.OnItemTotalChanged += delegate {
            _CharList.Clear();

            _ViewModel.CharInfoList.Visit((key, value) =>
            {
                _CharList.Add<SimpleCharInfoView, SimpleCharInfoViewModel>(key, value);
                

                return true;
            });
        };

        _ViewModel.CharInfoList.OnItemAdded += (key, value) =>
        {
            _CharList.Add<SimpleCharInfoView, SimpleCharInfoViewModel>(key, value);
        };

        _ViewModel.CharInfoList.OnItemRemoved += ( key ) =>
        {
            _CharList.Remove(key);
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
