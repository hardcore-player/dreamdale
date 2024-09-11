

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine.AddressableAssets;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Druid
{
    public static class Editor_CreateAddressbalePath
    {

        [MenuItem("Tools/刷新Addressbale路径到AddressbalePathEnum")]
        public static void Find()
        {
            AssetDatabase.Refresh();

            HashSet<string> nameHash = new HashSet<string>();

            //获取所有addressable
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            List<AddressableAssetEntry> allEntries =
                new List<AddressableAssetEntry>(settings.groups.SelectMany(g => g.entries));

            //addressablepathenum 文件
            FileStream sf = new FileStream("Assets/Deal/Scripts/AddressbalePathEnum.cs", FileMode.Truncate,
                FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(sf, Encoding.Default);
            sw.Write(
                "//自动生成代码，不要手动修改\n//使用[Tools/刷新Addressbale路径到AddressbalePathEnum]\npublic static class AddressbalePathEnum\n{\n");

            Debug.Log(">>>>>" + allEntries.Count);
            //每条写到cs
            foreach (var foundEntry in allEntries)
            {
                Debug.Log(">>>>>" + foundEntry.AssetPath);
                if (foundEntry.AssetPath.Contains("/Deal/"))
                {
                    Debug.Log(">>>>>");
                    Debug.Log(foundEntry.address);
                    Debug.Log(foundEntry.AssetPath);
                    foundEntry.address = foundEntry.AssetPath;
                    var shotName = addressableShortName(foundEntry.address);
                    Debug.Log(shotName);
                    if (nameHash.Contains(shotName))
                    {
                        EditorUtility.DisplayDialog("重复", "重复! " + foundEntry.AssetPath, "", null);
                        Debug.LogError("重复！" + foundEntry.AssetPath);
                        return;
                    }

                    nameHash.Add(shotName);
                    sw.Write(string.Format("    public static readonly string {0} = \"{1}\";\n", addressableEnumName(shotName),
                        foundEntry.AssetPath));

                    if (foundEntry.MainAsset && foundEntry.MainAsset.GetType().IsSubclassOf(typeof(ScriptableBase)))
                    {
                        var so = AssetDatabase.LoadAssetAtPath<ScriptableBase>(foundEntry.address);
                        so.AddressablePath = foundEntry.address;
                        EditorUtility.SetDirty(so);
                        AssetDatabase.SaveAssets();
                    }
                }
            }

            sw.Write("}");
            sw.Close();
            sf.Close();

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("DONE!", "使用AddressbalePathEnum类的静态成员即可", "", null);
        }

        private static string addressableShortName(string path)
        {
            int index = path.LastIndexOf("/");
            if (index != -1)
            {
                var str = path.Substring(index + 1, path.Length - index - 1);
                return str;
            }

            return "";
        }

        private static string addressableEnumName(string name)
        {
            var index = name.LastIndexOf(".");
            if (index != -1)
            {
                var prefix = name.Substring(index + 1, name.Length - index - 1);
                prefix = prefix.ToUpper();
                var str = name.Substring(0, index);
                return prefix + "_" + str;
            }

            return name;
        }

        //编辑器获取资源文件夹并加载，暂时没用
        private static void FindAssets(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path); //获得文件夹的info
            FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos(); //获得文件夹中文件的info
            foreach (FileSystemInfo item in fileSystemInfos) //FullName从D:盘开始（即地址），Name就是文件名（即名称）。
            {
                if (item is DirectoryInfo) //如果是文件夹
                {
                    FindAssets(item.FullName); //递归调用该方法
                }
                else //如果不是文件夹，即如果是文件
                {
                    if (!item.Name.EndsWith(".meta") &&
                        !item.Name.EndsWith(".DS_Store")) //每个资源都会有个对应的.meta文件，存储资源的ImportSetting
                    {
                        string importerPath =
                            "Assets" + item.FullName.Substring(Application.dataPath.Length); //获得从Assets开始的资源路径
                                                                                             //AssetImporter assetImporter = AssetImporter.GetAtPath(importerPath);//通过上面的资源路径获得资源文件
                    }
                }
            }
        }
    }
}
#endif