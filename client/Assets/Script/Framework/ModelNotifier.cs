using System;
using System.Collections.Generic;

namespace Framework
{
    public class ModelNotifier
    {
        Dictionary<string, Action> _eventMap = new Dictionary<string, Action>();

        public void Register( string name, Action callback )
        {
            
            Action a = Get( name );
            if ( a == null )
            {
                _eventMap.Add(name, callback);
            }
            else
            {
                a += callback;
            }

        }

        Action Get( string name )
        {
            Action a;
            if ( _eventMap.TryGetValue(name, out a ) )
            {
                return a;
            }

            return null;
        }

        public void Notify( string name )
        {
            Action a = Get( name );
            if ( a != null )
            {
                a.Invoke();
            }            
        }

    }


}