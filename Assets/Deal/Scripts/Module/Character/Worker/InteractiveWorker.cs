using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using Deal.Env;
using UnityEngine;

namespace Deal
{
    public class InteractiveWorker : InteractiveBase
    {
        public override void OnUIDisplayShow()
        {
            Building_Storage _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.Storage) as Building_Storage;

            if (_Storage)
            {
                Data_Storage _Data = _Storage.GetData<Data_Storage>();

                if (_Data.IsAseetsFull())
                {
                    CharacterChatBubble chatBubble = this._displayUI.GetComponent<CharacterChatBubble>();
                    chatBubble.ShowLabel("储物仓已满");
                }
                else
                {
                    this._displayUI.SetActive(false);
                }

            }
        }

    }

}

