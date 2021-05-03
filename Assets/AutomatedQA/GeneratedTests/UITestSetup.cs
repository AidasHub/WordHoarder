using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WordHoarder.Utility;

public static class UITestSetup
{
    public static void SetupUITests()
    {
        var data = Application.dataPath + "/Tests/UI Test Data/Data";
        string path = Application.persistentDataPath + "/" + SaveUtility.SaveFolderName;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.Copy(data + "0", path + "/" + SaveUtility.SaveFileName + "0", true);
        File.Copy(data + "1", path + "/" + SaveUtility.SaveFileName + "1", true);
    }
}
