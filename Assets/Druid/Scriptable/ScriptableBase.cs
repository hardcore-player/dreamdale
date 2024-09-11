using UnityEngine;

namespace Druid
{

    public abstract class ScriptableBase : ScriptableObject
    {
        [Tooltip("勿动，自动填充")] [SerializeField] public string AddressablePath;

        public virtual void OnCreate()
        {
        }
    }

}