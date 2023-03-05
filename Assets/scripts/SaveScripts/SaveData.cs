using UnityEngine;
using System.IO;

public static class SaveData {
    
    public static void Save () {
        string path = Application.persistentDataPath + "/save.json";
    }
}
