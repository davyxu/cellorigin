using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public interface IProperty
    {
        void SetValue(object v);

        object GetValue();
    }

    class TextProperty : IProperty
    {
        Text _ctrl;
        public TextProperty(Text ctrl)
        {
            _ctrl = ctrl;
        }

        public void SetValue(object v)
        {
            _ctrl.text = (string)v;
        }

        public object GetValue()
        {
            return _ctrl.text;
        }
    }

}
