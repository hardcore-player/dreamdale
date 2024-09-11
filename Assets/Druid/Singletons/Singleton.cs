using UnityEngine;


namespace Druid
{
    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T _instance;

        protected static bool _inited;

        public bool isInited
        {
            get
            {
                return _inited;
            }
        }

        /// <summary>
        /// Singleton design pattern
        /// </summary>
        /// <value>The instance.</value>
        public static T I
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject(typeof(T).Name);
                        obj.name = typeof(T).Name;
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// On awake, we initialize our instance. Make sure to call base.Awake() in override if you need awake.
        /// </summary>
        protected virtual void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            _instance = this as T;

            //Init();
        }

        public virtual void Init()
        {
            _inited = true;
        }

    }
}