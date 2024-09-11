using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#if UNITY_WEBGL && !UNITY_EDITOR
using WeChatWASM;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Druid
{
    /// <summary>
    /// Allows the save and load of objects in a specific folder and file.
    /// </summary>
    public static class SaveLoadManager
    {
        private const string _baseFolderName = "/MMData/";
        private const string _defaultFolderName = "SaveManager";

        /// <summary>
        /// Determines the save path to use when loading and saving a file based on a folder name.
        /// </summary>
        /// <returns>The save path.</returns>
        /// <param name="folderName">Folder name.</param>
        static string DetermineSavePath(string folderName = _defaultFolderName)
        {
            string savePath;
            // depending on the device we're on, we assemble the path
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                savePath = Application.persistentDataPath + _baseFolderName;
            }
            else
            {
                savePath = Application.persistentDataPath + _baseFolderName;
            }
#if UNITY_EDITOR
            savePath = Application.dataPath + _baseFolderName;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
            savePath = WeChatWASM.WX.env.USER_DATA_PATH + _baseFolderName;
#endif

            savePath = savePath + folderName + "/";
            return savePath;
        }

        /// <summary>
        /// Determines the name of the file to save
        /// </summary>
        /// <returns>The save file name.</returns>
        /// <param name="fileName">File name.</param>
        static string DetermineSaveFileName(string fileName)
        {
            return fileName + ".binary";
        }

        /// <summary>
        /// Save the specified saveObject, fileName and foldername into a file on disk.
        /// </summary>
        /// <param name="saveObject">Save object.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="foldername">Foldername.</param>
        public static void Save(object saveObject, string fileName, string foldername = _defaultFolderName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string saveFileName = WeChatWASM.WX.env.USER_DATA_PATH + "/" + fileName + ".txt";
            Debug.Log("SaveJson" + saveFileName);
            //Debug.Log("SaveJson" + LitJson.JsonMapper.ToJson(saveObject));

            WXFileSystemManager fs = WX.GetFileSystemManager();
            fs.WriteFileSync(saveFileName, LitJson.JsonMapper.ToJson(saveObject), "utf-8");
#else
            string savePath = DetermineSavePath(foldername);
            string saveFileName = DetermineSaveFileName(fileName);
            var path = Path.Combine(Application.persistentDataPath, savePath + saveFileName);

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            Debug.Log(path + "fileName" + fileName);
            string json = LitJson.JsonMapper.ToJson(saveObject);
            File.WriteAllText(path, json);

            //string savePath = DetermineSavePath(foldername);
            //string saveFileName = DetermineSaveFileName(fileName);
            //// if the directory doesn't already exist, we create it
            //if (!Directory.Exists(savePath))
            //{
            //    Directory.CreateDirectory(savePath);
            //}



            //// we serialize and write our object into a file on disk
            //BinaryFormatter formatter = new BinaryFormatter();
            //FileStream saveFile = File.Create(savePath + saveFileName);
            //formatter.Serialize(saveFile, saveObject);
            //saveFile.Close();
#endif
        }

        /// <summary>
        /// Load the specified file based on a file name into a specified folder
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="foldername">Foldername.</param>
        public static object Load<T>(string fileName, string foldername = _defaultFolderName)
        {

#if UNITY_WEBGL && !UNITY_EDITOR
            string saveFileName = WeChatWASM.WX.env.USER_DATA_PATH + "/" + fileName + ".txt";
            Debug.Log("LoadJson" + saveFileName);
            WXFileSystemManager fs = WX.GetFileSystemManager();
            if (fs.AccessSync(saveFileName).Equals("access:ok"))
            {
                string playerDataString = fs.ReadFileSync(saveFileName, "utf-8");
                //Debug.Log("LoadJson" + playerDataString);

                if (playerDataString != "")
                {
                    return LitJsonEx.JsonMapper.ToObject<T>(playerDataString);
                }
            }
            return null;
#else

            string savePath = DetermineSavePath(foldername);
            string saveFileName = DetermineSaveFileName(fileName);
            var path = Path.Combine(Application.persistentDataPath, savePath + saveFileName);

            Debug.Log("load path" + path);
            if (!File.Exists(path))
            {
                return null;
            }
            var json = File.ReadAllText(path);

            Debug.Log("json:" + json);
            object returnObject = LitJsonEx.JsonMapper.ToObject<T>(json);

            //string savePath = DetermineSavePath(foldername);
            //string saveFileName = savePath + DetermineSaveFileName(fileName);
            //Debug.Log("Load" + savePath);
            //object returnObject;

            //// if the MMSaves directory or the save file doesn't exist, there's nothing to load, we do nothing and exit
            //if (!Directory.Exists(savePath) || !File.Exists(saveFileName))
            //{
            //    return null;
            //}

            //BinaryFormatter formatter = new BinaryFormatter();
            //FileStream saveFile = File.Open(saveFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            //returnObject = formatter.Deserialize(saveFile);
            //saveFile.Close();

            return returnObject;
#endif
        }

        public static string LoadJson(string fileName, string foldername = _defaultFolderName)
        {

#if UNITY_WEBGL && !UNITY_EDITOR
            string saveFileName = WeChatWASM.WX.env.USER_DATA_PATH + "/" + fileName + ".txt";
            Debug.Log("LoadJson" + saveFileName);
            WXFileSystemManager fs = WX.GetFileSystemManager();
            if (fs.AccessSync(saveFileName).Equals("access:ok"))
            {
                string playerDataString = fs.ReadFileSync(saveFileName, "utf-8");
                //Debug.Log("LoadJson" + playerDataString);

                return playerDataString;
            }
            return null;
#else

            string savePath = DetermineSavePath(foldername);
            string saveFileName = DetermineSaveFileName(fileName);
            var path = Path.Combine(Application.persistentDataPath, savePath + saveFileName);

            Debug.Log("load json path" + path);
            if (!File.Exists(path))
            {
                return null;
            }
            var json = File.ReadAllText(path);

            Debug.Log("load json:" + json);
            return json;
#endif
        }

        /// <summary>
        /// Removes a save from disk
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="folderName">Folder name.</param>
        public static void DeleteSave(string fileName, string folderName = _defaultFolderName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string saveFileName = WeChatWASM.WX.env.USER_DATA_PATH + "/" + fileName + ".txt";
            WeChatWASM.WXFileSystemManager fs = WeChatWASM.WX.GetFileSystemManager();
            if (fs.AccessSync(saveFileName).Equals("access:ok"))
            {
                fs.RmdirSync(saveFileName, true);
            }
#else
            string savePath = DetermineSavePath(folderName);
            string saveFileName = DetermineSaveFileName(fileName);
            File.Delete(savePath + saveFileName);
#endif
        }

        public static void DeleteSaveFolder(string folderName = _defaultFolderName)
        {
#if UNITY_EDITOR
            string savePath = DetermineSavePath(folderName);
            FileUtil.DeleteFileOrDirectory(savePath);
#endif
        }
    }
}