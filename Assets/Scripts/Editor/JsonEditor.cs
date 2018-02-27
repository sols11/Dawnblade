using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using LitJson;

namespace SFramework
{
    public class JsonEditor
    {
        private static string dataBasePath = Application.streamingAssetsPath + @"\Temp\";
        private static Dictionary<string, List<string>> dicWeapon=new Dictionary<string, List<string>>();
        private static List<string> talks = new List<string>() {"第一句话", "第二句话"};

        [MenuItem("Editor/CreateJson")]
        private static void CreateJson()
        {
            dicWeapon.Add("NPC1",talks);
            dicWeapon.Add("NPC2",talks);
            if (File.Exists(dataBasePath + "Temp.json"))   // 检查存在
            {
                File.Delete(dataBasePath + "Temp.json");
                Debug.Log("Temp.json已存在，自动覆盖");
            }
            StreamWriter sw = File.CreateText(dataBasePath + "Temp.json");
            sw.Write(JsonMapper.ToJson(dicWeapon));
            sw.Close();
        }
    }
}