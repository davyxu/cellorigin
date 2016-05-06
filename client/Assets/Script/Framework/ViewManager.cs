using UnityEngine;

namespace Framework
{

    public class ViewManager : MonoBehaviour
    {
        static ViewManager _instance;

        public static ViewManager Instance
        {
            get { return _instance; }
        }

        void Awake()
        {
            _instance = this;
        }

        public void Show(string name)
        {
            var prefab = Resources.Load<GameObject>("UI/" + name);
            if (prefab == null)
            {
                Debug.LogError("UIManager.Show UI没找到: " + name);
                return;
            }

            var ins = GameObject.Instantiate<GameObject>(prefab);

            ins.transform.SetParent(gameObject.transform, false);
            ins.SetActive(true);
        }

        public void Hide(string name)
        {
            var targetTrans = transform.FindChild(name);
            if (targetTrans == null)
            {
                Debug.LogError("UIManager.Hide UI没找到: " + name);
                return;
            }

            targetTrans.gameObject.SetActive(false);
        }
    }

}