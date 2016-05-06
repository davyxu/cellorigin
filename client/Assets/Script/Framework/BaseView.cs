using UnityEngine;

namespace Framework
{
    public class BaseView : MonoBehaviour
    {

        public virtual void Bind(BasePresenter presenter)
        {

        }
    }


    public class BaseItemView : BaseView
    {
        public object ItemKey;
    }

}
