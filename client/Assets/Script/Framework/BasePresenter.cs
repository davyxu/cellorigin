using System.Collections.Generic;

namespace Framework
{
    public class BasePresenter
    {
        Dictionary<string, IProperty> _property = new Dictionary<string,IProperty>();

        public void RegisterProperty( string name , IProperty proprty)
        {
            _property.Add(name, proprty);
        }

        public IProperty GetProperty( string name )
        {
            IProperty ret;
            if (_property.TryGetValue( name, out ret ))
            {
                return ret;
            }

            return null;
        }
    }
}

