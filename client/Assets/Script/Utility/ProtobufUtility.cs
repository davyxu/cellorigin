using System;
using UnityEngine;
using System.IO;

public class ProtobufUtility
{
    public static T LoadBinaryFile<T>(string filename) where T : global::ProtoBuf.IExtensible
    {
        try
        {
            FileStream reader = File.OpenRead(filename);

            var data = ProtoBuf.Serializer.Deserialize<T>(reader);

            reader.Dispose();

            return data;
        }
        catch (FileNotFoundException)
        {
            return default(T);
        }
    }

    public static void SaveBinaryFile<T>(string filename, T data) where T : global::ProtoBuf.IExtensible
    {
        FileStream writer = System.IO.File.OpenWrite(filename);

        ProtoBuf.Serializer.Serialize<T>(writer, data);

        writer.Flush();
        writer.Close();
        writer.Dispose();
    }


    public static T LoadTextFile<T>(string filename) where T : global::ProtoBuf.IExtensible
    {
        var text = File.ReadAllText(filename);

        return ProtobufText.Serializer.Deserialize<T>(text);
    }

}