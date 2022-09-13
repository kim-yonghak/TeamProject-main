using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityChanAdventure.KyungSeo;

namespace UnityChanAdventure.FeelJoon
{
    public class ItempTooltipUI : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Text titleText; // ������ �̸� �ؽ�Ʈ
        [SerializeField] private Text contentText; // ������ ���� �ؽ�Ʈ
        [SerializeField] private Image itemImage; // ������ ������
        // [SerializeField] private GameObject itemBuffText;
        [SerializeField] private Text[] itemBuffValueTexts;

        [SerializeField] private ItemObjectDatabase[] itemDatabases;

        private RectTransform myRectTransform;
        private CanvasScaler canvasScaler;

        #endregion Variables

        #region Unity Methods
        void Awake()
        {
            Init();
            Hide();
        }

        #endregion Unity Methods

        #region Helper Methods
        private void Init()
        {
            TryGetComponent(out myRectTransform);
            myRectTransform.pivot = new Vector2(0f, 1f);
            canvasScaler = GetComponentInParent<CanvasScaler>();

            DisableAllChildrenRaycastTarget(transform);
        }

        private void DisableAllChildrenRaycastTarget(Transform parent)
        {
            parent.TryGetComponent(out Graphic graphic);
            if (graphic != null)
            {
                graphic.raycastTarget = false;
            }

            int childCount = parent.childCount;
            if (childCount.Equals(0))
            {
                return;
            }

            for (int i = 0; i < childCount; i++)
            {
                DisableAllChildrenRaycastTarget(parent.GetChild(i));
            }
        }

        public void SetItemInfo(Item item)
        {
            if (item.id.Equals(-1))
            {
                return;
            }

            ItemObject itemObject = GetItemObject(item);

            titleText.text = itemObject.data.name;
            contentText.text = itemObject.description;
            itemImage.sprite = itemObject.icon;

            int buffsLength = item.buffs.Length;

            for (int i = 0; i < buffsLength; i++)
            {
                itemBuffValueTexts[i].gameObject.SetActive(true);

                if(itemObject.data.buffs[i].stat.Equals(CharacterAttribute.Strength))
                {
                    itemBuffValueTexts[i].text = $"Strength : {item.buffs[i].value}";
                }
                else if (itemObject.data.buffs[i].stat.Equals(CharacterAttribute.Defensive))
                {
                    itemBuffValueTexts[i].text = $"Defensive : {item.buffs[i].value}";
                }
                else if (itemObject.data.buffs[i].stat.Equals(CharacterAttribute.CriticalRate))
                {
                    itemBuffValueTexts[i].text = $"CriticalRate : {item.buffs[i].value}";
                }
                else if (itemObject.data.buffs[i].stat.Equals(CharacterAttribute.Health))
                {
                    itemBuffValueTexts[i].text = $"Health : {item.buffs[i].value}";
                }
                else if (itemObject.data.buffs[i].stat.Equals(CharacterAttribute.Mana))
                {
                    itemBuffValueTexts[i].text = $"Mana : {item.buffs[i].value}";
                }
            }

            for (int i = buffsLength; i < itemBuffValueTexts.Length; i++)
            {
                itemBuffValueTexts[i].gameObject.SetActive(false);
            }
        }

        public void SetRectPosition(RectTransform slotRect)
        {
            // ĵ���� �����Ϸ��� ���� �ػ� ����
            float wRatio = Screen.width / canvasScaler.referenceResolution.x;
            float hRatio = Screen.height / canvasScaler.referenceResolution.y;
            float ratio =
                wRatio * (1f - canvasScaler.matchWidthOrHeight) +
                hRatio * (canvasScaler.matchWidthOrHeight);

            float slotWidth = slotRect.rect.width * ratio;
            float slotHeight = slotRect.rect.height * ratio;

            // ���� �ʱ� ��ġ(���� ���ϴ�) ����
            myRectTransform.position = slotRect.position + new Vector3(slotWidth, -slotHeight);
            Vector2 pos = myRectTransform.position;

            // ������ ũ��
            float width = myRectTransform.rect.width * ratio;
            float height = myRectTransform.rect.height * ratio;

            // ����, �ϴ��� �߷ȴ��� ����
            bool rightTruncated = pos.x + width > Screen.width;
            bool bottomTruncated = pos.y - height < 0f;

            ref bool R = ref rightTruncated;
            ref bool B = ref bottomTruncated;

            if (R && !B) // ������ �߷��� ��
            {
                myRectTransform.position = new Vector2(pos.x - width - slotWidth, pos.y);
            }
            else if (!R && B) // �ϴ��� �߷��� ��
            {
                myRectTransform.position = new Vector2(pos.x, pos.y + height + slotHeight); ;
            }
            else if (R && B) // ����, �ϴ��� �߷��� ��
            {
                myRectTransform.position = new Vector2(pos.x - width - slotWidth, pos.y + height + slotHeight);
            }
        }

        private ItemObject GetItemObject(Item item)
        {
            ItemObject resultItemObj = null;

            switch (item.itemType)
            {
                case ItemType.Sword:
                    resultItemObj = itemDatabases[(int)ItemType.Sword].itemObjects[item.id - itemCodeList.swordCode];
                    break;
                case ItemType.Bow:
                    resultItemObj = itemDatabases[(int)ItemType.Bow].itemObjects[item.id - itemCodeList.bowCode];
                    break;
                case ItemType.Food:
                    resultItemObj = itemDatabases[(int)ItemType.Food].itemObjects[item.id - itemCodeList.foodCode];
                    break;
            }

            return resultItemObj;
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        #endregion Helper Methods
    }
}