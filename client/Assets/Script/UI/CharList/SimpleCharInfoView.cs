using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



class SimpleCharInfoView : BaseView
{
    SimpleCharInfoViewModel _ViewModel;

    Text _CharName;


    public SimpleCharInfoViewModel ViewModel
    {
        get { return _ViewModel; }
    }

    public override void Bind(BaseViewModel vm)
    {
        _ViewModel = vm as SimpleCharInfoViewModel;

        var trans = this.transform;
        _CharName = trans.Find("CharName").GetComponent<Text>();        
        

        // CharName
        _ViewModel.OnCharNameChanged += delegate()
        {
            _CharName.text = _ViewModel.CharName;
        };
        _CharName.text = _ViewModel.CharName;


    }
}
