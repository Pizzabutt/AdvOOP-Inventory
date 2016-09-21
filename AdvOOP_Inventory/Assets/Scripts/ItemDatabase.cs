using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using LitJson;

public class Stat
{
	public int Power { get; set; }
	public int Defence { get; set; }
	public int Vitality { get; set; }

	public Stat(Stat __Stats)
	{
		this.Power = __Stats.Power;
		this.Defence = __Stats.Defence;
		this.Vitality = __Stats.Vitality;
	}
	public Stat(JsonData __data)
	{
		Power = (int)__data["power"];
		Defence = (int)__data["defence"];
		Vitality = (int)__data["vitality"];
	}
}

public class Item
{
	public int ID { get; set; }
	public string Title { get; set; }
	public int Value { get; set; }
	public Stat Stats { get; set; }

	public string Description { get; set; }
	public bool Stackable { get; set; }
	public int Rarity { get; set; }
	public Sprite Sprite { get; set; }
	public GameObject gameObject { get; set; }

	public Item()
	{
		ID = -1;
	}

	public Item(JsonData _data)
	{
		ID = (int)_data["id"];
		Title = _data["title"].ToString();
		Value = (int)_data["value"];
		Stats = new Stat(_data["stats"]);

		Description = _data["description"].ToString();
		Stackable = (bool)_data["stackable"];
		Rarity = (int)_data["rarity"];
		string fileName = _data["sprite"].ToString();
		Sprite = Resources.Load<Sprite>("Sprites/Items/" + fileName);
	}
}

public class ItemDatabase : MonoBehaviour
{
	private static ItemDatabase instance = null;

	private Dictionary<string, Item> database = new Dictionary<string, Item>();
	private JsonData itemData;

	void Awake()
	{
		if(instance ==  null)
		{
			instance = this;

			string jsonFilePath_ = Application.dataPath + "/StreamingAssets/Items.json";
			string jsonText_ = File.ReadAllText(jsonFilePath_);
			itemData = JsonMapper.ToObject(jsonText_);

			constructDatabase();
		}
		else
		{
			Destroy(gameObject);
		}

	}
	void Update()
	{

	}

	void constructDatabase()
	{
		for(int i = 0; i < itemData.Count; i++)
		{
			JsonData data_ = itemData[i];
			Item newItem_ = new Item(data_);
			database.Add(newItem_.Title, newItem_);
		}
	}

	public static Item getItem(string _itemName)
	{
		Dictionary<string, Item> database_ = instance.database;
		if(database_.ContainsKey(_itemName))
		{
			return database_[_itemName];
		}
		return null;
	}
	public static Dictionary<string, Item> getDatabase()
	{
		return instance.database;
	}
}
