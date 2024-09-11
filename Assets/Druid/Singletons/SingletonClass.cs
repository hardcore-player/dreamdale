using UnityEngine;


namespace Druid
{
    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public class SingletonClass<T> where T : class, new()
    {
        protected static T _instance;


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
                    _instance = new T();
                }

                return _instance;
            }
        }


        public virtual void Setup()
        {

        }

        public virtual void Destroy()
        {

        }

    }
}