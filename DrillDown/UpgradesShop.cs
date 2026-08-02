using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class UpgradesShop : Shop
{
    private class UpgradeEntry
    {
        public Upgrades upgrade;
        public Text progressText,costText,statsValue;
        public int slot;
        public Func<float> readStat;

        public UpgradeEntry(float[] values, int slot,Func<float> readStat)
        {
            upgrade = new Upgrades(values);
            progressText = Text.CreateDefault();
            costText = Text.CreateDefault();
            statsValue = Text.CreateDefault();
            costText.tm.scale = new Vector2(0.7f, 0.7f);
            this.slot = slot;
            this.readStat = readStat;
        }
    }

    private List<UpgradeEntry> entries = new();

    
    public UpgradesShop(string spriteName, float scale, float worldXPos, Player player) : base(spriteName, scale, worldXPos, player)
    {
        panel = new Panel(new Sprite("Panel1"), 4, 4);
        panel.SetTitle("Upgrades Shop");
        AddUpgrade([0.08f, 0.15f, 0.3f, 0.5f, 1f], 4, "Drill", "Drill", player.UpgradeDrill,()=>player.DrillPower,100,100);
        AddUpgrade([10, 20, 50, 70, 100],5, "Fuel", "FuelIcon",player.UpgradeFuel,()=>player.MaxFuel);
        AddUpgrade([30, 50, 70, 120, 200],6, "Capacity", "Cargo", player.Inventory.UpgradeCapacity,()=>player.Inventory.Capacity,120);
        AddUpgrade([10, 20, 50, 70, 100],7, "Armor", "Hull", player.UpgradeArmor,()=>player.MaxHealth);
        panel.AddCloseButton(3,CloseShop);
    }
    
        
    private void AddUpgrade(float[] values, int slot, string label, string icon, Action<float> apply,Func<float> readStat,int width = 80, int height = 80)
    {
        UpgradeEntry entry = new UpgradeEntry(values, slot,readStat);
        entries.Add(entry);
        panel.AddSpriteButton(slot, label, () => ApplyUpgrade(entry.upgrade, apply), new Sprite(icon), width, height);
    }
    
    private void ApplyUpgrade(Upgrades upgrade, Action<float> applyToPlayer)
    {
        if (!upgrade.TryUpgrade(player)) return;
        applyToPlayer(upgrade.CurrentValue());
    }
    
    public override void DrawPanel(SpriteBatch spriteBatch)
    {
        base.DrawPanel(spriteBatch);
        if (!isOpen) return;
        foreach (var e in entries)
        {
            e.progressText.text = $"{e.upgrade.Level}/{e.upgrade.MaxLevel}";
            e.progressText.tm.position = panel.GetSlotBottomCenter(e.slot);
            e.progressText.Draw(spriteBatch);
            
            e.costText.text = e.upgrade.IsMaxed ? "Maxed Out" : $"Cost: ${e.upgrade.NextCost}";
            e.costText.tm.position = panel.GetSlotCenter(e.slot + 4);
            e.costText.Draw(spriteBatch);

            e.statsValue.text = $"{e.readStat()}";
            e.statsValue.tm.position = panel.GetSlotBottomCenter(e.slot + 4);
            e.statsValue.Draw(spriteBatch);
        }
    }
    
    
}