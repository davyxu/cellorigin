using UnityEngine;

namespace Framework
{
    public class Utility
    {

        public static void BindCollection<CollectionKeyType, PresentType, ViewType>(
            Framework.ObservableCollection<CollectionKeyType, PresentType> presenterCollection,
            Framework.ListControl viewList)
            where ViewType : Framework.BaseView
            where PresentType : Framework.BasePresenter
        {
            presenterCollection.OnItemTotalChanged += delegate
            {
                viewList.Clear<ViewType>();

                presenterCollection.Visit((key, value) =>
                {
                    viewList.Add<ViewType, PresentType>(key, value);
                    return true;
                });
            };

            presenterCollection.OnItemAdded += (key, value) =>
            {
                viewList.Add<ViewType, PresentType>(key, value);
            };

            presenterCollection.OnItemRemoved += (key) =>
            {
                viewList.Remove<ViewType>(key);
            };
        }
    }

}
