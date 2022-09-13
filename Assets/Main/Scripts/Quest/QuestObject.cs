using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanAdventure.FeelJoon
{
    public enum QuestStatus
    {
        None,
        Accepted,
        Completed,
        Rewarded,
    }

    [CreateAssetMenu(fileName ="New Quest", menuName ="Quest System/Quests/New Quest")]
    public class QuestObject : ScriptableObject
    {
        #region Variables
        public Quest data = new Quest();
        public QuestStatus status;

        public int index = 0; // ����Ʈ�� ���� �� ���� ��ġ�� �ִ� index�� ������ ����
        public UnityEngine.UI.Image tracker;

        #endregion Varialbes

        public ItemObject SearchRewardItemObjectWithID(int itemID)
        {
            if (itemID >= itemCodeList.swordCode && itemID < itemCodeList.bowCode)
            {
                return data.rewardItemObjectDatabase.itemObjects[itemID - itemCodeList.swordCode];
            }
            else if (itemID >= itemCodeList.bowCode && itemID < itemCodeList.foodCode)
            {
                return data.rewardItemObjectDatabase.itemObjects[itemID - itemCodeList.bowCode];
            }
            else if (itemID >= itemCodeList.foodCode)
            {
                return data.rewardItemObjectDatabase.itemObjects[itemID - itemCodeList.foodCode];
            }

            return null;
        }
    }
}