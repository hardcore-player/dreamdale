using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deal.Data
{

    [Serializable]
    public class Data_SaveBase
    {
        public virtual void Load() { }
        public virtual void Update() { }
        public virtual void Save() { }

    }

}
