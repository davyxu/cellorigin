using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public class BaseView : MonoBehaviour
    {
        public object ItemKey;

        public virtual void Bind(BasePresenter presenter)
        {

        }
    }


    public class BaseItemView : BaseView
    {

    }


}
