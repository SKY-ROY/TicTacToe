using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    /// <summary>
    /// method to save content of StateData into stream and further into a binary file
    /// </summary>
    public static void SaveState(GlobalSettingsControl myScene)
    {
        BinaryFormatter formatter = new BinaryFormatter();                              //Binary formatter for conversion from string format to binary data
        string path = Application.persistentDataPath + "/this.Sett";                    //directory path for saved file varying with target platform
        FileStream stream = new FileStream(path, FileMode.Create);                      //binary file creation

        StateData data = new StateData(myScene);

        formatter.Serialize(stream, data);                                              //conversion happening
        stream.Close();                                                                 //stream termination to avoid rewriting and overwriting
    }

    /// <summary>
    /// method to Load content of StateData from stream which is deserialized from a binary file
    /// </summary>
    public static StateData LoadState()
    {
        string path = Application.persistentDataPath + "/this.Sett";                    //directory path for saved file varying with target platform
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();                          //Binary formatter for conversion from binary data to string format
            FileStream stream = new FileStream(path, FileMode.Open);                    //opening existing binary file from path specified

            StateData data = formatter.Deserialize(stream) as StateData;                //reverse-conversion happening
            stream.Close();                                                             //stream termination to avoid rewriting and overwriting

            return data;
        }
        else
        {
            Debug.LogError("SAVE file not found in" + path);                            //Error LOG to show absence of file
            return null;
        }
    }
}