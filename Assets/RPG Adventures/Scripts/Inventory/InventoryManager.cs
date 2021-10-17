using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG {
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] Transform inventoryPanel;

        public List<InventorySlot> inventory = new List<InventorySlot>();
        int inventorySize;

        private void Awake()
        {
            inventorySize = inventoryPanel.childCount;
            CreateInventory(inventorySize);
        }
        private void CreateInventory(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                inventory.Add(new InventorySlot(i));
                RegisterWeaponButton(i);
            }
        }

        void RegisterWeaponButton(int slotIndex)
        {
            var slotButton = inventoryPanel.GetChild(slotIndex).GetComponent<Button>();
            slotButton.onClick.AddListener(() =>
            {
                UseItem(slotIndex);
            });
        }

        void UseItem(int slotIndex)
        {
            var slot = FindSlotByIndex(slotIndex);
            if (slot == null) return;
            Character.Instance.EquipWeapon(slot);
        }

        public void OnItemPickup(ItemSpawner spawner)
        {
            AddItem(spawner);
        }

        void AddItem(ItemSpawner spawner)
        {
            InventorySlot slot = GetFreeSlot();
            if (slot == null) return;

            slot.Place(spawner.weaponPrefab);

            SetWeaponSprite(spawner, slot);

            Destroy(spawner.gameObject);
        }

        private void SetWeaponSprite(ItemSpawner spawner, InventorySlot slot)
        {
            var weaponImage = inventoryPanel.GetChild(slot.index).GetChild(0).GetComponent<Image>();
            weaponImage.sprite = spawner.weaponImageSprite;

            var tempColor = weaponImage.color;
            tempColor.a = 1f;
            weaponImage.color = tempColor;
        }

        InventorySlot GetFreeSlot()
        {
            return inventory.Find(slot => slot.name == null);
        }

        InventorySlot FindSlotByIndex(int slotIndex)
        {
            return inventory.Find(slot => slot.index == slotIndex);    
        }
    }
}

