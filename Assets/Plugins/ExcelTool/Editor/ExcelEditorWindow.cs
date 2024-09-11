using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using OfficeOpenXml;
using System.Text;
using UnityEditor.Callbacks;
using System.Linq;
using System.Reflection;


namespace ExcelTool
{

    /// <summary>
    /// excel表格导出工具窗口
    /// </summary>
    public class ExcelEditorWindow : EditorWindow
    {

        private static ExcelEditorWindow window;
        /// <summary>
        /// 是否绘制窗口
        /// </summary>
        private static bool isOpen = true;
        private static ExcelSettings clsSetting;
        private static string strSettingPath = "";
        private const string strSettingName = "ExcelSetting.txt";
        private const string strCacheName = "Cache.txt";
        private const float fltInputWidth = 700.0f;
        private static DataInfo clsInfo;

        [MenuItem("Tools/Excel配置表工具")]
        public static void FnInit()
        {
            ToolEditorWindow.GetWindow<ExcelEditorWindow>("Excel配置表工具", null, -100, -150, 1000, 500);
            clsInfo = new DataInfo() { infos = new List<SheetInfo>() };
            PrInit();
            isOpen = true;
        }

        private void OnGUI()
        {
            try
            {
                if (!isOpen) return;

                ToolEditorWindow.GetInputField("Excel文件路径 : ", ref clsSetting.strExcelPath, null, width: fltInputWidth);
                ToolEditorWindow.GetInputField("脚本生成路径 : ", ref clsSetting.strCsPath, null, width: fltInputWidth);
                ToolEditorWindow.GetInputField("数据生成路径 : ", ref clsSetting.strDataPath, null, width: fltInputWidth);
                ToolEditorWindow.GetInputField("ScriptObject类的名字 : ", ref clsSetting.strMgrClassName, null, width: fltInputWidth);
                ToolEditorWindow.GetInputField("命名空间 : ", ref clsSetting.strNameSpace, null, width: fltInputWidth);
                //ToolEditorWindow.GetInputField("Config类继承 : ", ref clsSetting.strInherit, null, width: fltInputWidth);
                ToolEditorWindow.GetInputField("缓存数据路径 : ", ref clsSetting.strCachePath, null, width: fltInputWidth);
                ToolEditorWindow.GetDoubleBtn("保存数据", OnSaveConfig, "重置数据", OnResetConfig);
                ToolEditorWindow.GetSingleBtn("导入配置表", OnImportExcelData);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(IOException) && e.ToString().Contains("~$"))
                {
                    Debug.LogWarning("导入配置失败，有表格正在被打开中！请关闭后尝试！");
                }
                else
                {
                    Debug.LogWarning("绘制窗口失败,停止绘制" + e);
                }

                isOpen = false;
            }
        }

        #region 事件监听

        /// <summary>
        /// 导入配置            
        /// </summary>
        private void OnImportExcelData()
        {
            PrReadExcel();
            EditorUtility.RequestScriptReload();
        }

        private void OnResetConfig()
        {
            clsSetting = new ExcelSettings();
        }

        private void OnSaveConfig()
        {
            clsSetting.PuBeforeSave();
            PuSaveByBin(clsSetting, strSettingName, strSettingPath);
        }

        #endregion

        #region UI设置

        /// <summary>
        /// UI设置            
        /// </summary>
        /*        private static void PrUISetting(float height = fltUIHeight)
                {
                    GUI.skin.textField.fixedHeight = height;
                }*/

        #endregion

        #region ScriptsObject


        /// <summary>
        /// 生成ScriptObject            
        /// </summary>
        public static void PrCreateNewSObject()
        {
            /*            TestObjects config = ScriptableObject.CreateInstance(typeof(TestObjects)) as TestObjects;
                        config.enemies = new PropertyEnemy[] { new PropertyEnemy() { id = "1", name = "Enemy1"}, new PropertyEnemy() { id = "2", name = "Enemy2" } };
                        AssetDatabase.CreateAsset(config, "Assets/Resources/Config/Test.asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();*/

        }

        #endregion

        #region Excel

        private static string GetSObjName => string.Concat(clsSetting.strMgrClassName, "SObj");

        /// <summary>
        /// 重新加载后读取配置信息，生成ScriptObjectTODO            
        /// </summary>
        [DidReloadScripts]
        public static void PrOnReloadToCreate()
        {
            int id = EditorPrefs.GetInt("ExcelEditorWindow");
            if (id < 1) return;

            string name = EditorPrefs.GetString("ExcelEditorWindow_Mgr");
            if (IsNull(name)) return;

            PrInit();
            if (!Directory.Exists(clsSetting.strDataPath)) Directory.CreateDirectory(clsSetting.strDataPath);
            ScriptableObject sObj = CreateInstance(PuGetTypeName(name, clsSetting.strNameSpace));
            var dicValues = PrGetNameField(sObj.GetType());
            clsInfo = PuLoadByBin<DataInfo>(strCacheName, clsSetting.strCachePath);
            Dictionary<string, Array> dic = new Dictionary<string, Array>();
            foreach (var info in clsInfo.infos)
            {
                if (info.listInfos.Count > 1)
                {
                    //Debug.Log($"类型 ： {info.name}");
                    dic[string.Concat(PuGetFirstLowerString(info.name), "s")] = PrCreateTypeArray(info);
                }
            }

            foreach (var key in dicValues.Keys)
            {
                //Debug.Log($"类型 ： {key}|{dicValues[key].FieldType}|{dic[key].GetValue(0).GetType()}");
                dicValues[key].SetValue(sObj, dic[key]);
            }

            EditorPrefs.SetInt("ExcelEditorWindow", 0);
            AssetDatabase.CreateAsset(sObj, Path.Combine(clsSetting.strDataPath, string.Concat(name, ".asset")));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"<color=yellow>生成{name}.asset 成功！</color>");
        }

        /// <summary>
        /// 读Excel            
        /// </summary>
        private static void PrReadExcel()
        {
            ExcelPackage excelPackage;
            foreach (var info in GetFileInfos(clsSetting.strExcelPath))
            {
                excelPackage = new ExcelPackage(info);
                PrGetDicValues(info.Name.Substring(0, info.Name.LastIndexOf('.')), excelPackage.Workbook.Worksheets[1]);
            }
            //Enum先生成脚本
            foreach (var info in clsInfo.infos)
            {
                if (info.listInfos.Count < 2)
                {
                    PrCreateClass(info, clsSetting.strCsPath);
                }
            }

            foreach (var info in clsInfo.infos)
            {
                if (info.listInfos.Count > 1)
                {
                    PrCreateClass(info, clsSetting.strCsPath);
                }
            }


            if (clsSetting.strCachePath == null || clsSetting.strCachePath.Length < 3)
            {
                clsSetting.strCachePath = FnGetDefaultPath();
            }

            PuSaveByBin(clsInfo, strCacheName, clsSetting.strCachePath);
            PrCreateScriptObjectClass();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorPrefs.SetInt("ExcelEditorWindow", 1);
            Debug.Log("<color=yellow>生成脚本成功！</color>");
        }

        /// <summary>
        /// 将值提取成字典            
        /// </summary>
        private static void PrGetDicValues(string fileName, ExcelWorksheet worksheet)
        {
            //Debug.Log("fileName : " + worksheet.Dimension.End.Column);
            clsInfo.infos.Add(new SheetInfo(fileName));
            List<ColumnInfo> rows = new List<ColumnInfo>();
            System.Object obj;
            int columnCount = worksheet.Dimension.End.Column;
            int rowCount = worksheet.Dimension.End.Row;
            string strValue;
            //获取数据，方便赋值
            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= columnCount; j++)
                {
                    obj = worksheet.Cells[i, j].Value;
                    strValue = obj == null ? "" : obj.ToString();
                    strValue = strValue.Trim();
                    switch (i)
                    {
                        case 1:
                            rows.Add(new ColumnInfo() { listDatas = new List<string>(), description = strValue });
                            //Debug.Log($"{clsInfo.infos[^1].name}|第{i}行,第{j}列 | {strValue}");
                            break;
                        case 2:
                            rows[j - 1].name = strValue;
                            break;
                        case 3:
                            rows[j - 1].type = strValue;
                            break;
                        default:
                            rows[j - 1].listDatas.Add(strValue);
                            break;
                    }
                }
            }

            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].name == null || rows[i].name.Length < 2)
                {
                    rows.RemoveAt(i);
                    i--;
                }
            }

            clsInfo.infos.Last().listInfos = rows;
            clsInfo.infos.Last().PuUpdateWrongData();
        }

        #endregion

        #region I/O

        public static string GetAppPath => UnityEngine.Application.dataPath;

        /// <summary>
        /// 获取默认路径
        /// </summary>
        /// <returns></returns>
        public static string FnGetDefaultPath()
        {
            string defaultPath = UnityEngine.Application.dataPath;
            string[] guidArr = AssetDatabase.FindAssets("ExcelEditorWindow");
            if (guidArr.Length > 0)
            {
                defaultPath = AssetDatabase.GUIDToAssetPath(guidArr[0]);
                defaultPath = Path.Combine(defaultPath.Substring(0, defaultPath.LastIndexOf("/Editor")), "Data");
            }
            else
            {
                Debug.LogError("默认路径错误？！");
            }

            return defaultPath;
        }

        public static string PuReadStrFromFile(string path)
        {
            string content = null;
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8);
                content = sr.ReadToEnd();
                sr.Close();
                fs.Close();
            }
            catch (IOException)
            {
                Debug.Log("路径文件不存在 : " + path);
            }
            return content;
        }

        public static bool PuWriteFileFromStr(string path, string str)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.CreateNew);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

                sw.Write(str);
                sw.Flush();

                sw.Close();
                fs.Close();

                if (File.Exists(path)) return true;

            }
            catch (IOException e)
            {
                Debug.LogError("保存失败 : " + e.Message);
            }

            return false;
        }

        /// <summary>
        /// [Serializable]Class二进制方法：存档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clsT"></param>
        /// <param name="path"></param>
        public static bool PuSaveByBin<T>(T clsT, string file, string path)
        {
            try
            {
                //Debug.Log("保存路径 : " + path);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                file = Path.Combine(path, file);
                //if (File.Exists(file)) File.Delete(file);
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fileStream = File.Create(file);
                bf.Serialize(fileStream, clsT);
                fileStream.Close();
                /*            if (File.Exists(file))
                            {
                                //Debug.Log("保存成功！");
                            }*/
            }
            catch (Exception e)
            {
                Debug.LogError("保存失败 : " + e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// [Serializable]Class二进制方法：读档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T PuLoadByBin<T>(string fileName, string path)
        {
            path = Path.Combine(path, fileName);
            T save = default;
            if (File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fileStream = File.Open(path, FileMode.Open);
                save = (T)bf.Deserialize(fileStream);
                fileStream.Close();
            }
            else
            {
                Debug.Log("存档文件不存在");
            }
            return save;
        }

        /// <summary>
        /// 获取文件夹下所有路径
        /// </summary>
        public static List<FileInfo> GetFileInfos(string path, string filter = ".xlsx")
        {
            List<FileInfo> files = new List<FileInfo>();
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (dirInfo.Exists)
            {
                FileInfo[] infors = dirInfo.GetFiles();
                for (int i = 0; i < infors.Length; i++)
                {
                    if (infors[i].FullName.EndsWith(filter, false, null))
                    {
                        files.Add(infors[i]);
                    }
                }
            }

            return files;
        }

        #endregion

        #region private
        
        /// <summary>
        /// 根据类型获取new            
        /// </summary>
        public static Array PrCreateTypeArray(SheetInfo info)
        {
            Type mType = PrGetType(info.name, clsSetting.strNameSpace);
            //Type mType = Type.ReflectionOnlyGetType($"Assembly-CSharp.{PuGetTypeName(info.name, clsSetting.strNameSpace)}", true, false);
            //Debug.Log($"获取的类型 : {PuGetTypeName(info.name, clsSetting.strNameSpace)}|{mType}");
            Array arrClones = Array.CreateInstance(mType, info.listInfos[0].listDatas.Count);
            for (int i = 0; i < arrClones.Length; i++)
            {
                arrClones.SetValue(Activator.CreateInstance(mType), i);
            }

            var dicValue = PrGetNameField(mType);
            string name, value, type;
            for (int i = 0; i < info.listInfos.Count; i++)
            {
                name = info.listInfos[i].name;
                type = PrGetRealType(info.listInfos[i].type);
                if (dicValue.ContainsKey(name) || dicValue.ContainsKey(PuGetFirstLowerString(name)))
                {
                    for (int j = 0; j < arrClones.Length; j++)
                    {
                        value = info.listInfos[i].listDatas[j];
                        //Debug.Log($"{info.name}|{type}|{value}");
                        if (IsNull(value, 1)) continue;
                        try
                        {
                            switch (type)
                            {
                                case "int":
                                    dicValue[name].SetValue(arrClones.GetValue(j), int.Parse(value));
                                    break;
                                case "float":
                                    dicValue[name].SetValue(arrClones.GetValue(j), float.Parse(value));
                                    break;
                                case "bool":
                                    dicValue[name].SetValue(arrClones.GetValue(j), bool.Parse(value));
                                    break;
                                case "int[]":
                                    string[] valArr = value.Split(',');
                                    int[] intArr = Array.ConvertAll(valArr, delegate (string s) { return int.Parse(s); });
                                    dicValue[name].SetValue(arrClones.GetValue(j), intArr);
                                    break;
                                case "float[]":
                                    string[] strArr = value.Split(',');
                                    float[] fltArr = Array.ConvertAll(strArr, delegate (string s) { return float.Parse(s); });
                                    dicValue[name].SetValue(arrClones.GetValue(j), fltArr);
                                    break;
                                case "string[]":
                                    string[] sArr = value.Split(',');
                                    dicValue[name].SetValue(arrClones.GetValue(j), sArr);
                                    break;
                                case "UnityEngine.Sprite":
                                    dicValue[name].SetValue(arrClones.GetValue(j), AssetDatabase.LoadAssetAtPath<Sprite>(value));
                                    break;
                                case "UnityEngine.GameObject":
                                    dicValue[name].SetValue(arrClones.GetValue(j), AssetDatabase.LoadAssetAtPath<GameObject>(value));
                                    break;
                                case "UnityEngine.AudioClip":
                                    dicValue[name].SetValue(arrClones.GetValue(j), AssetDatabase.LoadAssetAtPath<AudioClip>(value));
                                    break;
                                case "enum":
                                    int current = int.Parse(value) - 1;
                                    dicValue[PuGetFirstLowerString(name)].SetValue(arrClones.GetValue(j), Enum.Parse(PrGetType(info.listInfos[i].PuGetEnumName, clsSetting.strNameSpace), current.ToString()));
                                    //dicValue[PuGetFirstLowerString(name)].SetValue(arrClones.GetValue(j), PuGetEnumValues(info.listInfos[i].PuGetEnumName, value));
                                    break;
                                default:
                                    dicValue[name].SetValue(arrClones.GetValue(j), value);
                                    break;
                            }
                            //Debug.Log($"{info.name}|{name}|{dicValue[name].GetValue(arrClones.GetValue(j))}");
                        }
                        catch (Exception e)
                        {
                            Debug.Log($"{info.name}|{name}|{info.listInfos[i].listDatas[j]} 绑定失败 : {e}");
                        }
                    }
                }
            }

            return arrClones;
        }

        /// <summary>
        /// 初始化            
        /// </summary>
        public static void PrInit()
        {
            strSettingPath = FnGetDefaultPath();
            //Debug.Log(strSettingPath);
            clsSetting = PuLoadByBin<ExcelSettings>(strSettingName, strSettingPath);
            if (clsSetting == null)
            {
                clsSetting = new ExcelSettings();
            }
        }

        /// <summary>
        /// 生成数据管理的ScriptObject类            
        /// </summary>
        public static void PrCreateScriptObjectClass()
        {
            string sObjeFileName = GetSObjName;
            EditorPrefs.SetString("ExcelEditorWindow_Mgr", sObjeFileName);
            string filePath = Path.Combine(clsSetting.strCsPath, string.Concat(sObjeFileName, ".cs"));
            if (File.Exists(filePath)) { File.Delete(filePath); }
            //生成数据管理类
            List<ColumnInfo> list = new List<ColumnInfo>();
            foreach (var info in clsInfo.infos)
            {
                if (info.listInfos.Count > 1)
                {   //非枚举生成数组变量
                    list.Add(new ColumnInfo()
                    {
                        name = string.Concat(PuGetFirstLowerString(info.name), "s"),
                        description = info.description,
                        type = string.Concat(info.name, "[]")
                    });
                    //Debug.Log($"添加{list[^1].name}");
                }
            }

            PuWriteFileFromStr(filePath,
                PrCombineClass(sObjeFileName,
                    "数据管理类 : 脚本自动生成，勿手动修改",
                    PrCombineParams(list), "", "UnityEngine.ScriptableObject", nameSpace: clsSetting.strNameSpace));
            //判断是否需要生成数据管理类逻辑处理类
            filePath = Path.Combine(clsSetting.strCsPath, string.Concat(clsSetting.strMgrClassName, ".cs"));
            if (File.Exists(filePath)) return;

            //不存在则生成
            PuWriteFileFromStr(filePath,
                PrCombineClass(clsSetting.strMgrClassName,
                "数据管理类的逻辑部分",
                "", "partial", sObjeFileName, nameSpace: clsSetting.strNameSpace));
        }

        /// <summary>
        /// 拼接并生成脚本
        /// </summary>
        /// <param name="name">文件名</param>
        /// <param name="dic">字段名：类型</param>
        private static void PrCreateClass(SheetInfo sheetInfo, string path)
        {
            string filePath = Path.Combine(path, string.Concat(sheetInfo.name, ".cs"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if (sheetInfo.listInfos.Count < 2)
            {
                //枚举
                string[] param = new string[sheetInfo.listInfos[0].listDatas.Count];
                for (int i = 0; i < param.Length; i++)
                {
                    param[i] = sheetInfo.listInfos[0].listDatas[i];
                }

                PuWriteFileFromStr(filePath, PrCombineEnum(sheetInfo.name, sheetInfo.description, clsSetting.strNameSpace, param));
            }
            else
            {
                PuWriteFileFromStr(filePath, PrCombineClass(sheetInfo.name, sheetInfo.description, PrCombineParams(sheetInfo.listInfos), nameSpace: clsSetting.strNameSpace));
            }

            //Debug.Log($"生成脚本 : {sheetInfo.name} 成功！");
        }

        /// <summary>
        /// 枚举类的拼凑
        /// </summary>
        /// <param name="name"></param>
        /// <param name="summary">说明</param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static string PrCombineEnum(string name, string summary, string nameSpace, string[] param)
        {
            bool isNameSpace = nameSpace != null;
            string prefix = isNameSpace ? "\t" : "";
            StringBuilder content = new StringBuilder("");
            if (isNameSpace)
            {
                content.AppendLine($"namespace {nameSpace}");
                content.AppendLine("{");
            }

            content.AppendLine($"{prefix}/// <summary>");
            content.AppendLine($"{prefix}/// {summary}");
            content.AppendLine($"{prefix}/// <summary>");
            //content.AppendLine("[System.Serializable]");
            content.AppendLine($"{prefix}public enum {PuGetEnumName(name)}");
            content.Append($"\n{prefix}");
            content.Append("{\n");
            for (int i = 0; i < param.Length; i++)
            {
                //Debug.Log($"{name} | 枚举字段 : {param[i]}");
                content.AppendLine($"{prefix}\t{PuGetFirstUperString(param[i])},");
            }

            content.Append($"{prefix}");
            content.Append("}\n");
            if (isNameSpace) content.AppendLine("}");
            return content.ToString();
        }

        /// <summary>
        /// 类的拼凑
        /// </summary>
        /// <param name="name"></param>
        /// <param name="summary">说明</param>
        /// <param name="inherit">继承</param>
        /// <param name="param"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private static string PrCombineClass(string name, string summary, string param, string extra = null, string inherit = null, string method = null, string nameSpace = null)
        {
            //Debug.Log($"拼接类 : {nameSpace} | {name}");
            bool isNameSpace = nameSpace != null;
            string prefix = isNameSpace ? "\t" : "";
            StringBuilder content = new StringBuilder("");
            //content.AppendLine($"using System;");
            if (isNameSpace)
            {
                content.AppendLine($"namespace {nameSpace}");
                content.AppendLine("{");
            }

            content.AppendLine($"{prefix}/// <summary>");
            content.AppendLine($"{prefix}/// {summary}");
            content.AppendLine($"{prefix}/// <summary>");
            content.AppendLine($"{prefix}[System.Serializable]");
            content.Append($"{prefix}public {(extra == null ? "" : extra)} class {name}");
            if (inherit != null)
            {
                content.Append($" : {inherit}");
            }

            content.Append($"\n{prefix}");
            content.Append("{\n");
            content.AppendLine($"{prefix}\t#region 变量\n");
            content.AppendLine(param);
            content.AppendLine($"\n{prefix}\t#endregion\n");
            if (method != null)
            {
                content.AppendLine($"{prefix}\t#region 方法\n");
                content.AppendLine(method);
                content.AppendLine($"\n{prefix}\t#endregion\n");
            }

            content.Append($"{prefix}");
            content.Append("}\n");
            if (isNameSpace) content.AppendLine("}");
            return content.ToString();
        }

        /// <summary>
        /// 变量的拼凑            
        /// </summary>
        private static string PrCombineParams(List<ColumnInfo> listInfos)
        {
            StringBuilder content = new StringBuilder();
            foreach (var info in listInfos)
            {
                content.AppendLine("\t/// <summary>");
                content.AppendLine($"\t/// {info.description}");
                content.AppendLine("\t/// <summary>");
                content.AppendLine($"\t[UnityEngine.SerializeField, UnityEngine.Header(\"{info.description}\")]");
                if (info.type == "enum")
                {
                    //Debug.Log($"{info.type}|{info.name}");
                    content.AppendLine($"\tpublic {PuGetEnumParamName(info.name)} {info.name};");
                }
                else
                {
                    content.AppendLine($"\tpublic {PrGetRealType(info.type)} {info.name};");
                }
            }
            return content.ToString();
        }

        /// <summary>
        /// 通过类型名称获取类型
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private static Type PrGetType(string typeName, string nameSpace)
        {
            Debug.Log($"获取类型 {nameSpace}:{typeName}");
            //Debug.Log(typeName);
            typeName = PuGetTypeName(typeName, nameSpace);
            Type type = null;
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly a in assemblies)
            {
                Debug.Log($"-> Name: {a.GetName().Name}");
                Debug.Log($"-> Version: {a.GetName().Version}");
            }
            Assembly defaultAssembly = assemblies.First(a => a.GetName().Name.StartsWith("Assembly-CSharp"));
            type = defaultAssembly.GetType(typeName);
            //Debug.Log($"找到 {defaultAssembly == null}|{type}");
            return type;
            //return Assembly.Load(nameSpace).GetType(string.Concat(nameSpace, ".", typeName));
        }

        /// <summary>
        /// 根据配置获取实际类型            
        /// </summary>
        public static string PrGetRealType(string type)
        {
            string t = type;
            switch (type)
            {
                case "png":
                case "sprite":
                    t = "UnityEngine.Sprite";
                    break;
                case "gameObject":
                    t = "UnityEngine.GameObject";
                    break;
                case "audio":
                    t = "UnityEngine.AudioClip";
                    break;
                case "intArr":
                    t = "int[]";
                    break;
                case "floatArr":
                    t = "float[]";
                    break;
                case "strArr":
                    t = "string[]";
                    break;
            }

            return t;
        }

        /// <summary>
        /// 查找脚本中所有需要绑定成员的名字、数据
        /// </summary>
        /// <param name="type">脚本类型</param>
        /// <returns></returns>
        private static Dictionary<string, FieldInfo> PrGetNameField(Type type)
        {

            Dictionary<string, FieldInfo> dicNameField = new Dictionary<string, FieldInfo>();
            try
            {
                foreach (FieldInfo field in type.GetRuntimeFields())
                {
                    //Debug.Log(field.Name);
                    if (!field.IsNotSerialized)
                    {
                        dicNameField[field.Name] = field;
                    }
                }
            }
            catch (Exception)
            {

            }

            return dicNameField;
        }

        /// <summary>
        /// 判断string 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minLength"></param>
        /// <returns>true是空</returns>
        public static bool IsNull(string str, int minLength = 2)
        {
            return str == null || str.Length < minLength;
        }

        /// <summary>
        /// 获取首字母小写            
        /// </summary>
        public static string PuGetFirstLowerString(string value)
        {
            return string.Concat(value.Substring(0, 1).ToLower(), value.Substring(1));
        }

        /// <summary>
        /// 获取首字母大写            
        /// </summary>
        public static string PuGetFirstUperString(string value)
        {
            return string.Concat(value.Substring(0, 1).ToUpper(), value.Substring(1));
        }

        /// <summary>
        /// 获取枚举类型名            
        /// </summary>
        public static string PuGetEnumParamName(string name)
        {
            string paramName = name.Substring(0, name.IndexOf('_'));
            //Debug.Log($"    ## {paramName}");
            paramName = PuGetEnumName(PuGetFirstUperString(paramName));
            return paramName;
        }

        /// <summary>
        /// 获取类型名            
        /// </summary>
        public static string PuGetTypeName(string name, string nameSpace)
        {
            if (!IsNull(nameSpace)) return string.Concat(nameSpace, ".", name);
            return name;
        }

        /// <summary>
        /// 枚举转化            
        /// </summary>
        public static object PuGetEnumValues(string type, string value)
        {
            Array array = Enum.GetValues(PrGetType(type, clsSetting.strNameSpace));
            //Debug.Log($"{type}|{array.Length} ");
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < array.Length; i++)
            {
                dic[array.GetValue(i).ToString()] = i;
                //Debug.Log($"{type}|{array.GetValue(i)} ");
            }

            if (dic.ContainsKey(value))
            {
                return array.GetValue(dic[value]);
            }

            return default;
        }

        /// <summary>
        /// 获取枚举名称            
        /// </summary>
        public static string PuGetEnumName(string name) => string.Concat("Enum", name);

        #endregion

        [Serializable]
        public class ExcelSettings
        {
            /// <summary>
            /// Excel表格文件路径
            /// </summary>
            public string strExcelPath;
            /// <summary>
            /// 生成数据路径
            /// </summary>
            public string strDataPath;
            /// <summary>
            /// 生成脚本路径
            /// </summary>
            public string strCsPath;
            /// <summary>
            /// 缓存数据路径
            /// </summary>
            public string strCachePath;
            /// <summary>
            /// ScriptObject类的名字
            /// </summary>
            public string strMgrClassName = "ConfigMgr";
            /// <summary>
            /// 主类的样本文件名
            /// </summary>
            public string strClassName = "ClassTemplate.txt";
            /// <summary>
            /// 命名空间
            /// </summary>
            public string strNameSpace = "ExcelData";
            /// <summary>
            /// 默认继承
            /// </summary>
            //public string strInherit = "BaseConfig";

            public ExcelSettings()
            {
                string basePath = Path.Combine(GetAppPath, "Plugins", "ExcelTool", "Example");
                strExcelPath = Path.Combine(basePath, "Config");
                strCsPath = Path.Combine(basePath, "Scripts", "Config");
                strDataPath = Path.Combine("Assets", "Plugins", "ExcelTool", "Example", "Resources", "Config");
                strCachePath = FnGetDefaultPath();
            }


            /// <summary>
            /// 获取ExcelPath            
            /// </summary>
            public string PuGetExcelPath()
            {
                return "";
            }

            /// <summary>
            /// 保存数据前检查            
            /// </summary>
            public void PuBeforeSave()
            {
                if (strCachePath == null || strCachePath.Length < 2)
                {
                    strCachePath = FnGetDefaultPath();
                }

                if (strMgrClassName == null || strMgrClassName.Length < 2)
                {
                    strMgrClassName = "ConfigMgr";
                }
            }
        }

        [System.Serializable]
        public class DataInfo
        {
            public List<SheetInfo> infos;
        }

        [System.Serializable]
        public class SheetInfo
        {
            public string name;
            public string description;
            public List<ColumnInfo> listInfos;

            public SheetInfo(string name)
            {
                if (name.IndexOf("_") > 0)
                {
                    int index = name.IndexOf("_");
                    this.name = name.Substring(0, index);
                    index++;
                    description = name.Substring(index);
                }
                else
                {
                    this.name = name;
                    description = name;
                }
            }

            /// <summary>
            /// 自动矫正错误数据,有必要吗？           
            /// </summary>
            public void PuUpdateWrongData()
            {
                if (listInfos.Count <= 2)
                {   //是枚举
                    //Debug.Log($"{name}是枚举");
                    if (listInfos.Count == 2)
                    {
                        listInfos.RemoveAt(0);
                    }

                    for (int i = 0; i < listInfos[0].listDatas.Count; i++)
                    {
                        if (IsNull(listInfos[0].listDatas[i]))
                        {
                            listInfos[0].listDatas.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }

        [System.Serializable]
        public class ColumnInfo
        {
            public string name;
            public string description;
            public string type;
            public List<string> listDatas;

            /// <summary>
            /// 获取枚举名称            
            /// </summary>
            public string PuGetEnumName => PuGetEnumParamName(name);

            public override string ToString()
            {
                string data = description;
                data += " , " + name;
                data += " , " + type;
                data += " , 数据 ： ";
                for (int i = 0; i < listDatas.Count; i++)
                {
                    data += listDatas[i] + "、";
                }
                return data;
            }

        }
    }
}
