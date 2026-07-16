using System;
using System.Collections;
using System.Collections.Generic;

namespace DrillDown;

public class Inventory : IEnumerable<Material>
{
    private List<Material> materials;
    private float currentWeight;
    private int materialsSellCost;
    private float capacity;
    private float maxCapacity = 300f;
    private float minCapacity = 0f;

    public Inventory(float startingCapacity)
    {
        materials = new List<Material>();
        capacity = Math.Clamp(capacity + startingCapacity, minCapacity, maxCapacity);
    }

    public bool TryAddMaterial(Material material)
    {
        if (currentWeight + material.Weight > capacity) return false;
        materials.Add(material);
        currentWeight += material.Weight;
        return true;
    }

    public void UpgradeCapacity(float amount)
    {
        capacity = Math.Clamp(capacity + amount, minCapacity, maxCapacity);
    }

    public IEnumerator<Material> GetEnumerator()
    {
        return new List<Material>(materials).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}