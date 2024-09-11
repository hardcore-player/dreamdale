using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using Deal.Env;
using Deal.Tools;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Druid;

namespace Deal
{

    public class Npc_ArenaRank : RoleBase
    {
        public void OnUIClick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIArenaPop, UILayer.Dialog);

        }
    }
}

