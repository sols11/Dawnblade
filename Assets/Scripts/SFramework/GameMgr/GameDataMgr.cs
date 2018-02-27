using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;
using System.Xml;
using System.Security.Cryptography;

namespace SFramework
{
    /// <summary>
    /// 游戏存储数据管理
    /// </summary>
    public class GameDataMgr : IGameMgr
    {
        private string dataFileName = "DreamKeeperSave.xml";//存档文件的名称,自己定// 
        private XmlSaver xs;

        public GameData gameData;//实例

        public SettingData SettingSaveData { get; private set; }

        public GameDataMgr(GameMainProgram gameMain) : base(gameMain)
        {
            xs = new XmlSaver();
            LoadSetting();
        }

        #region Json用存档，用于存储Setting
        public void SaveSetting()
        {
            if(SettingSaveData==null)
                SettingSaveData=new SettingData();
            gameMain.fileMgr.CreateJsonSaveData(GetDataPath() + "/Setting", SettingSaveData);
        }

        public void LoadSetting()
        {
            SettingSaveData = gameMain.fileMgr.LoadJsonSaveData<SettingData>(GetDataPath() + "/Setting");
            if(SettingSaveData == null)
            {
                Debug.Log("未能找到设置，自动创建");
                SettingSaveData = new SettingData(); // 创建初始设置
                SaveSetting();
            }
            else
                Debug.Log("已读取设置");
        }
        #endregion

        /// <summary>
        /// 存档
        /// </summary>
        public void Save(IPlayer currentPlayer)
        {
            if (currentPlayer == null)
            {
                Debug.LogError("没有Player,无法存档");
                return;
            }
            // 拷贝值类型数据
            gameData.Name = currentPlayer.Name;
            gameData.Rank = currentPlayer.Rank;
            gameData.Gold = currentPlayer.Gold;
            gameData.MedicineID = currentPlayer.MedicineID;
            gameData.CanAttack2 = currentPlayer.CanAttack2;
            gameData.CanAttack3 = currentPlayer.CanAttack3;
            gameData.CanAvoid = currentPlayer.CanAvoid;
            gameData.CanDush = currentPlayer.CanDush;
            // File
            string gameDataFile = GetDataPath() + "/" + dataFileName;
            Debug.Log("存档已保存于" + gameDataFile);
            //将存档写入XML文件
            string dataString = xs.SerializeObject(gameData, typeof(GameData));
            xs.CreateXML(gameDataFile, dataString);
        }

        //读档时调用的函数,游戏开始时Player自动读档，如果没有存档那么自动创建
        public void Load(IPlayer currentPlayer)
        {
            if (currentPlayer == null)
            {
                Debug.LogError("没有Player,无法读档");
                return;
            }
            // File
            string gameDataFile = GetDataPath() + "/" + dataFileName;
            if (gameData != null)
            {
                Debug.Log("直接从运行时存档读取数据");
            }
            else if (xs.hasFile(gameDataFile))
            {
                string dataString = xs.LoadXML(gameDataFile);
                GameData gameDataFromXML = xs.DeserializeObject(dataString, typeof(GameData)) as GameData;

                //是合法存档
                if (gameDataFromXML!=null && gameDataFromXML.key == SystemInfo.deviceUniqueIdentifier)
                {
                    //将存档赋给当前实例
                    Debug.Log("已读取存档");
                    gameData = gameDataFromXML;
                }
                else
                {
                    gameData = new GameData(); // 创建初始存档
                    Debug.Log("非法存档，重新创建新存档");
                    //将存档写入XML文件
                    dataString = xs.SerializeObject(gameData, typeof(GameData));
                    xs.CreateXML(gameDataFile, dataString);
                }
            }
            else
            {
                gameData = new GameData(); // 创建初始存档
                Debug.Log("创建新存档于" + gameDataFile);
                //将存档写入XML文件
                string dataString = xs.SerializeObject(gameData, typeof(GameData));
                xs.CreateXML(gameDataFile, dataString);
            }
            // LoadData值类型
            currentPlayer.Name = gameData.Name;
            currentPlayer.Rank = gameData.Rank;
            currentPlayer.Gold = gameData.Gold;
            currentPlayer.MedicineID = gameData.MedicineID;
            currentPlayer.CanAttack2 = gameData.CanAttack2;
            currentPlayer.CanAttack3 = gameData.CanAttack3;
            currentPlayer.CanAvoid = gameData.CanAvoid;
            currentPlayer.CanDush = gameData.CanDush;
            // 引用类型
            currentPlayer.PropNum = gameData.PropNum;
            currentPlayer.Fit = gameData.Fit;
            currentPlayer.HasProp = gameData.HasProp;
            currentPlayer.TasksData = gameData.TasksData;
            currentPlayer.PlayerMedi.FitEquip();
        }

        public void DeleteSaveData()
        {
            if (File.Exists(GetDataPath() + "/" + dataFileName))   // 检查存在
            {
                File.Delete(GetDataPath() + "/" + dataFileName);
            }
        }

        //获取路径// 
        private static string GetDataPath()
        {
            // Your game has read+write access to /var/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/Documents 
            // Application.dataPath returns ar/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/myappname.app/Data             
            // Strip "/Data" from path 
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Debug.Log("Iphone");
                string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
                // Strip application name 
                path = path.Substring(0, path.LastIndexOf('/'));
                return path + "/Documents";
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                Debug.Log("android");
                string path = Application.persistentDataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return path;
            }
            else
                // 这里先用着dataPath，游戏做成了换成persistentDataPath
                return Application.dataPath;
        }
    }
}