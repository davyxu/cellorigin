using System;
using UnityEngine;
using System.IO;

class LocalSetting
{
    public static T Load<T>( string key )
    {
        var str = PlayerPrefs.GetString(key);
        var data =Convert.FromBase64String(str);

        MemoryStream s = new MemoryStream(data);

        return ProtoBuf.Serializer.Deserialize<T>(s);
    }

    public static void Save<T>(string key, T value)
    {
        MemoryStream data = new MemoryStream();

        ProtoBuf.Serializer.Serialize(data, value);

        var str = Convert.ToBase64String(data.ToArray());
        PlayerPrefs.SetString(key, str);
    }
}