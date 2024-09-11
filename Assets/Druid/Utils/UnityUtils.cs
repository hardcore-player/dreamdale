using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Druid.Utils
{
    public class UnityUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiPostion"></param>
        /// <returns></returns>
        public static Vector3 UIScreenToWorldPoint(Camera uiCamera, Vector3 uiPostion)
        {
            Vector3 worldPostion = uiCamera.WorldToScreenPoint(uiPostion);
            worldPostion.z = 1f;
            worldPostion = Camera.main.ScreenToWorldPoint(worldPostion);

            return worldPostion;
        }


        public static Vector2 World2UI(RectTransform canvasRectTransform, Camera uiCamera, Vector3 worldPosition)
        {
            Vector3 spos = Camera.main.WorldToScreenPoint(worldPosition);
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, new Vector2(spos.x, spos.y),
                uiCamera, out pos);

            return pos;
        }

        /// <summary>
        /// 世界坐标转换为屏幕坐标
        /// </summary>
        /// <param name="worldPoint">屏幕坐标</param>
        /// <returns></returns>
        public static Vector2 WorldPointToScreenPoint(Vector3 worldPoint)
        {
            // Camera.main 世界摄像机
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPoint);
            return screenPoint;
        }

        /// <summary>
        /// 屏幕坐标转换为世界坐标
        /// </summary>
        /// <param name="screenPoint">屏幕坐标</param>
        /// <param name="planeZ">距离摄像机 Z 平面的距离</param>
        /// <returns></returns>
        public static Vector3 ScreenPointToWorldPoint(Vector2 screenPoint, float planeZ)
        {
            // Camera.main 世界摄像机
            Vector3 position = new Vector3(screenPoint.x, screenPoint.y, planeZ);
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(position);
            return worldPoint;
        }

        // RectTransformUtility.WorldToScreenPoint
        // RectTransformUtility.ScreenPointToWorldPointInRectangle
        // RectTransformUtility.ScreenPointToLocalPointInRectangle
        // 上面三个坐标转换的方法使用 Camera 的地方
        // 当 Canvas renderMode 为 RenderMode.ScreenSpaceCamera、RenderMode.WorldSpace 时 传递参数 canvas.worldCamera
        // 当 Canvas renderMode 为 RenderMode.ScreenSpaceOverlay 时 传递参数 null

        // UI 坐标转换为屏幕坐标
        public static Vector2 UIPointToScreenPoint(Vector3 worldPoint)
        {
            // RectTransform：target
            // worldPoint = target.position;
            Camera uiCamera = UIManager.I.UICamera;

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, worldPoint);
            return screenPoint;
        }

        // 屏幕坐标转换为 UGUI 坐标
        public static Vector3 ScreenPointToUIPoint(RectTransform rt, Vector2 screenPoint)
        {
            Vector3 globalMousePos;
            //UI屏幕坐标转换为世界坐标
            Camera uiCamera = UIManager.I.UICamera;

            // 当 Canvas renderMode 为 RenderMode.ScreenSpaceCamera、RenderMode.WorldSpace 时 uiCamera 不能为空
            // 当 Canvas renderMode 为 RenderMode.ScreenSpaceOverlay 时 uiCamera 可以为空
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPoint, uiCamera, out globalMousePos);
            // 转换后的 globalMousePos 使用下面方法赋值
            // target 为需要使用的 UI RectTransform
            // rt 可以是 target.GetComponent<RectTransform>(), 也可以是 target.parent.GetComponent<RectTransform>()
            // target.transform.position = globalMousePos;
            return globalMousePos;
        }

        // 屏幕坐标转换为 UGUI RectTransform 的 anchoredPosition
        public static Vector2 ScreenPointToUILocalPoint(RectTransform parentRT, Vector2 screenPoint)
        {
            Vector2 localPos;
            Camera uiCamera = UIManager.I.UICamera;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, screenPoint, uiCamera, out localPos);
            // 转换后的 localPos 使用下面方法赋值
            // target 为需要使用的 UI RectTransform
            // parentRT 是 target.parent.GetComponent<RectTransform>()
            // 最后赋值 target.anchoredPosition = localPos;
            return localPos;
        }


        //public static void ChangeSkeletonDataAsset(SkeletonAnimation sa, SkeletonDataAsset sda, string initAnimation)
        //{
        //    sa.skeletonDataAsset = sda;
        //    if (sa.skeletonDataAsset != null)
        //    {
        //        foreach (AtlasAssetBase aa in sa.skeletonDataAsset.atlasAssets)
        //        {
        //            if (aa != null) aa.Clear();
        //        }

        //        sa.skeletonDataAsset.Clear();
        //    }

        //    sa.Initialize(true);
        //    if (initAnimation != null)
        //    {
        //        sa.AnimationState.SetAnimation(0, initAnimation, true);
        //    }
        //}


        //public static void SetGrayMaterial(Image image, bool isGray)
        //{
        //    if (isGray)
        //    {
        //        image.material = new Material(Shader.Find("UI/Gray"));
        //    }
        //    else
        //    {
        //        image.material = null;
        //    }
        //}

        public static void ReSortRendererInUI(GameObject gameObject)
        {
            Canvas canvas = gameObject.GetComponentInParent<Canvas>();

            int targetOrder = canvas.sortingOrder + 1;

            SortingGroup sortingGroup = gameObject.GetComponentInChildren<SortingGroup>();
            sortingGroup.sortingOrder = targetOrder;
            sortingGroup.sortingLayerName = canvas.sortingLayerName;


            Renderer[] renderer = gameObject.GetComponentsInChildren<Renderer>();

            if (renderer != null && renderer.Length > 0)
            {
                foreach (var item in renderer)
                {
                    item.gameObject.layer = LayerMask.NameToLayer("UI");
                    item.sortingOrder = targetOrder;
                    item.sortingLayerName = canvas.sortingLayerName;
                }
            }
        }
    }
}