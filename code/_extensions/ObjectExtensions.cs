﻿/*********************
*	Rudolf Chrispens
***********************/

#region USE
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
#endregion

public interface IJsonSerialize<T>
{
    void Serialize(T _Data);
}

public interface IJsonDeserialize<T>
{
    bool deserializationFinished { get; set; } // used to check if the JSON struct is loaded correctly after awake
    void Deserialize(T _Data); // should return the json data struct we use and set deserializationFinished flag to true after it is finished.

    bool deserializationCheck(); // should check if deserialization bool is set to finished
}



public static class ObjectExtension
{
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /********************
     * Reflection to get a property from string name  *
     ********************/
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static object GetPropValue(this string _PropertyName, object _Source)
    {
        return _Source.GetType().GetProperty(_PropertyName).GetValue(_Source, null);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /********************
     * Deep Clone an object with Binary Formatter  *
     ********************/
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static T DeepClone<T>(this T a) where T : ISerializable
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, a);
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /********************
     * JSON Object De/Serialization  *
     ********************/
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region JSON
    /// <summary>
    /// This will serialize the object into a a relative path you choose.
    /// </summary>
    /// <param name="_Object"></param>
    /// <param name="_RelativePath"></param>
    public static void SerializeJSON<T>(this T _Object, string _RelativePath) where T : struct
    {
        var setting = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        var json = JsonConvert.SerializeObject(_Object, setting);
        var path = Path.Combine(Application.streamingAssetsPath, _RelativePath);
        File.WriteAllText(path, json);
    }


    /// <summary>
    /// This will deserialize a object from a relative path into the object you are calling it on.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_Root"></param>
    /// <returns></returns>
    public static void DeserializeJSON<T>(this T _Object, string _RelativePath) where T : struct
    {
        var setting = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        var path = Path.Combine(Application.streamingAssetsPath, _RelativePath);
        var fileContent = File.ReadAllText(path);
        var deserializedObject = JsonConvert.DeserializeObject<T>(fileContent);

        _Object = deserializedObject;
    }

    #endregion
}
