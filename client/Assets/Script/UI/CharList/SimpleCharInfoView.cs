using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



class SimpleCharInfoView : MonoBehaviour
{
    SimpleCharInfoViewModel _ViewModel;

    Text _CharName;


    public SimpleCharInfoViewModel ViewModel
    {
        get { return _ViewModel; }
    }


    void InitUI()
    {
        _ViewModel = new SimpleCharInfoViewModel();

        var trans = this.transform;
        _CharName = trans.Find("CharName").GetComponent<Text>();        
        

        // CharName
        _ViewModel.OnCharNameChanged += delegate()
        {
            _CharName.text = _ViewModel.CharName;
        };


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
