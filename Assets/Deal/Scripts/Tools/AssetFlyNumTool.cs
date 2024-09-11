using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deal.Tools
{
    public class AssetFlyNumTool
    {
        private int _curNumUnit = 1;
        private int _curTime = 0;

        public void stop()
        {
            _curNumUnit = 1;
            _curTime = 0;
        }

        public int numPerTime(int req, int mine)
        {
            if (_curTime >= 10)
                _curNumUnit = 10;
            if (_curTime >= 19)
                _curNumUnit = 100;
            _curTime++;
            int ret = _curNumUnit;
            if (req < ret)
                ret = req;
            if (mine < ret && mine > 0)
                ret = mine;
            return ret;
        }

    }

}

