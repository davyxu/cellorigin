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

        //_SelectChar.onClick.AddListener(SelectCharA_Click);
        //_CreateChar.onClick.AddListener(SelectCharB_Click);
    }

    void Awake()
    {
        InitUI();

        gameObject.SetActive(false);

        EventEmitter.Instance.Add<Event.ShowCharList>(ev =>
        {
            gameObject.SetActive(true);
        });
    }

    void Start()
    {
        //_ViewModel.Start();
    }

    void OnDisable()
    {

    }
}
