using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public interface IProperty
    {
        // 万物皆字符串
        void SetValue(string v);

        string GetValue();
    }

    
    class TextProperty : IProperty
    {
        Text _ctrl;
        public TextProperty(Text ctrl)
        {
            _ctrl = ctrl;
        }

        public void SetValue(string v)
        {
            if (v == null)
                return;

            _ctrl.text = v;
        }

        public string GetValue()
        {
            return _ctrl.text;
        }
    }

}
