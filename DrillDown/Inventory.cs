using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DrillDown;

public class Inventory : IEnumerable
{
    private Dictionary<Material, int> materials = new();
    private Dictionary<ItemType, Item> items = new();
    private float currentWeight;
    private float capacity;
    private float maxCapacity = 400f;
    private float minCapacity = 0f;
    
    public event Action<float> OnCapacityChange;
    
    public float Capacity => capacity;
    public float MaxCapacity => maxCapacity;
    public Dictionary<Material, int> Materials => materials;
    public Dictionary<ItemType, Item> Items => items;
    
    public Inventory(float startingCapacity)
    {
        items = new Dictionary<ItemType, Item>();
        materials = new Dictionary<Material, int>();
        capacity = Math.Clamp(capacity + startingCapacity, minCapacity, maxCapacity);
        items.Add(ItemType.Teleport,new Teleport(ItemType.Teleport, new Sprite("Teleport"), 10000));
        items.Add(ItemType.Bomb,new Bomb(ItemType.Bomb, new Sprite("Bomb"), 3000));
        items.Add(ItemType.RepairKit,new RepairKit(ItemType.RepairKit, new Sprite("RepairKit"), 2000));
        items.Add(ItemType.FuelTank,new FuelTank(ItemType.FuelTank, new Sprite("FuelTank"), 5000));
    }

    public bool TryAddMaterial(Material material)
    {
        if (currentWeight + material.Weight > capacity)
        {
            Console.WriteLine("Inventory Full!!");
            return false;
        }
        if(materials.ContainsKey(material))
            materials[material]++;
        else 
            materials[material] = 1;
        currentWeight += material.Weight;
        OnCapacityChange?.Invoke(currentWeight);
        return true;
    }

    public Item GetItem(ItemType type)
    {
        if(items.TryGetValue(type, out Item item)) return item;
        return null;
    }

    public void UpgradeCapacity(float amount)
    {
        capacity = Math.Clamp(capacity + amount, minCapacity, maxCapacity);
        OnCapacityChange?.Invoke(currentWeight);
    }
    
    
    public int GetOresTotalValue() => materials.Sum(m => m.Key.SellCost * m.Value);

    public void ClearInventory()
    {
        materials.Clear();
        currentWeight = 0f;
        OnCapacityChange?.Invoke(currentWeight);
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public IEnumerator GetEnumerator()
    {
        return new Dictionary<Material, int>(materials).GetEnumerator();
    }

    
}