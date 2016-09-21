using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Slot
{
	public GameObject gameObject;
	public Item item;
	public Slot(GameObject __gameObject, Item __item)
	{
		this.gameObject = __gameObject;
		this.item = __item;
	}
}

public class InventoryScript : MonoBehaviour
{
	[Header("UI")]
	public int slotAmount = 16;
	[Header("Prefabs")]
	public GameObject slotPanel;

	public GameObject slotPrefab;
	public GameObject itemPrefab;
	[Header("Items/Slots")]
	public List<Item> items = new List<Item>();
	public List<Slot> slots = new List<Slot>();

	private ItemDatabase itemDatabase;
	void Start()
	{
		itemDatabase = GetComponent<ItemDatabase>();
		for(int i = 0; i < slotAmount; i++)
		{
			GameObject clone_ = Instantiate(slotPrefab);
			clone_.transform.SetParent(slotPanel.transform);
			Slot slot_ = new Slot(clone_, null);
			SlotData slotData_ = clone_.GetComponent<SlotData>();
			slotData_.inventory = this;
			slotData_.slot = slot_;
			slots.Add(slot_);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			addItem("Steel Gloves");
		}
	}

	public void addItem(string _itemName, int _itemAmount = 1)
	{
		Item newItem_ = ItemDatabase.getItem(_itemName);
		Slot newSlot_ = getEmptySlot();

		if(newItem_ != null && newSlot_ != null)
		{
			if(hasStacked(newItem_, _itemAmount))
			{
				return;
			}

			newSlot_.item = newItem_;
			GameObject item_ = Instantiate(itemPrefab);
			item_.transform.position = newSlot_.gameObject.transform.position;
			item_.transform.SetParent(newSlot_.gameObject.transform);
			item_.name = newItem_.Title;

			newItem_.gameObject = item_;
			Image image_ = item_.GetComponent<Image>();
			image_.sprite = newItem_.Sprite;

			ItemData itemData_ = item_.GetComponent<ItemData>();
			itemData_.item = newItem_;
			itemData_.slot = newSlot_;
		}
	}
	public Slot getEmptySlot()
	{
		for(int i = 0; i < slots.Count; i++)
		{
			if(slots[i].item == null)
			{
				return slots[i];
			}
		}
		print("no empty slots bro");
		return null;
	}

	bool hasStacked(Item _itemToAdd, int _itemAmount = 1)
	{
		if(_itemToAdd.Stackable)
		{
			Slot occupiedSlot_ = getSlotWithItem(_itemToAdd);
			if(occupiedSlot_ != null)
			{
				Item item_ = occupiedSlot_.item;
				ItemData itemData_ = item_.gameObject.GetComponent<ItemData>();
				itemData_.amount += _itemAmount;
				Text textElement_ = item_.gameObject.GetComponentInChildren<Text>();
				return true;
			}
		}
		return false;
	}
	Slot getSlotWithItem(Item _item)
	{
		for(int i = 0; i < slots.Count; i++)
		{
			Item currentItem_ = slots[i].item;
			if(currentItem_ != null && currentItem_.Title == _item.Title)
			{
				return slots[i];
			}
		}
		return null;
	}
}
