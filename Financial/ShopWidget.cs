using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Engine;
using Game;

namespace Financial
{
    // Token: 0x02000008 RID: 8
    public class ShopWidget : CanvasWidget
    {
        // Token: 0x06000015 RID: 21 RVA: 0x00002824 File Offset: 0x00000A24
        public ShopWidget(ComponentShop componentShop, ComponentPlayer componentPlayer, SubsystemFinancial subsystemFinancial)
        {
            this.m_componentPlayer = componentPlayer;
            this.m_componentShop = componentShop;
            this.m_subsystemFinancial = subsystemFinancial;
            this.m_inventory = componentPlayer.ComponentMiner.Inventory;
            XElement xelement = ContentManager.Get<XElement>("Widgets/ShopWidget", null);
            base.LoadContents(this, xelement);
            this.m_inventoryGrid = this.Children.Find<GridPanelWidget>("InventoryGrid", true);
            this.m_moneyLabel = this.Children.Find<LabelWidget>("MoneyLabel", true);
            this.m_chooseButton = this.Children.Find<ButtonWidget>("ChooseButton", true);
            this.m_buyButton = this.Children.Find<ButtonWidget>("BuyButton", true);
            this.m_itemvalueicon = this.Children.Find<BlockIconWidget>("Icon", true);
            this.m_soldButton = this.Children.Find<ButtonWidget>("SoldButton", true);
            this.m_chooseitemLabel = this.Children.Find<LabelWidget>("ChooseItemLabel", true);
            this.m_chooseitemcountLabel = this.Children.Find<LabelWidget>("ChooseItemCountLabel", true);
            this.m_choosecountButton = this.Children.Find<ButtonWidget>("ChooseCountButton", true);
            this.m_soldSlot = this.Children.Find<InventorySlotWidget>("SoldSlot", true);
            this.m_returnmoneyLabel = this.Children.Find<LabelWidget>("ReturnMoneyLabel", true);
            this.m_upgradeshopButton = this.Children.Find<ButtonWidget>("UpgradeShopButton", true);
            this.m_shoplevelLabel = this.Children.Find<LabelWidget>("ShopLevelLabel", true);
            this.m_soldSlot.AssignInventorySlot(componentShop, componentShop.SoldSlotIndex);
        }

        // Token: 0x06000016 RID: 22 RVA: 0x000029B4 File Offset: 0x00000BB4
        public override void MeasureOverride(Vector2 parentAvailableSize)
        {
            int num = (this.m_inventory is ComponentCreativeInventory) ? 10 : 7;
            int value = (int)((parentAvailableSize.X - 320f - 25f) / 72f);
            this.m_inventory.VisibleSlotsCount = value < 7 ? 7 : (value > num ? num : value);
            bool flag = this.m_inventory.VisibleSlotsCount != this.m_inventoryGrid.Children.Count;
            bool flag2 = flag;
            if (flag2)
            {
                this.m_inventoryGrid.Children.Clear();
                this.m_inventoryGrid.RowsCount = 1;
                this.m_inventoryGrid.ColumnsCount = this.m_inventory.VisibleSlotsCount;
                for (int i = 0; i < this.m_inventoryGrid.ColumnsCount; i++)
                {
                    InventorySlotWidget inventorySlotWidget = new InventorySlotWidget();
                    inventorySlotWidget.AssignInventorySlot(this.m_inventory, i);
                    inventorySlotWidget.Size = new Vector2(54f, 54f);
                    inventorySlotWidget.BevelColor = new Color(181, 172, 154) * 0.6f;
                    inventorySlotWidget.CenterColor = new Color(181, 172, 154) * 0.33f;
                    this.m_inventoryGrid.Children.Add(inventorySlotWidget);
                    this.m_inventoryGrid.SetWidgetCell(inventorySlotWidget, new Point2(i, 0));
                }
            }
            base.MeasureOverride(parentAvailableSize);
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00002B30 File Offset: 0x00000D30
        public override void Update()
        {
            this.m_moneyLabel.Text = "Balance:" + this.m_subsystemFinancial.Money.ToString();
            this.m_shoplevelLabel.Text = "Shop Level:" + this.m_subsystemFinancial.ShopLevel.ToString();
            this.m_chooseitemcountLabel.Text = "Purchase Quantity:" + this.ItemCount.ToString();
            int num = Terrain.ExtractContents(this.ItemValue);
            Block block = BlocksManager.Blocks[num];
            this.m_chooseitemLabel.Text = "Selected Item:" + block.GetDisplayName(null, this.ItemValue);
            this.m_itemvalueicon.Value = this.ItemValue;
            bool isClicked = this.m_chooseButton.IsClicked;
            bool flag = isClicked;
            if (flag)
            {
                DialogsManager.ShowDialog(null, new ChooseItemsDialog(this.m_subsystemFinancial, delegate (int value)
                {
                    this.ItemValue = value;
                }));
            }
            bool isClicked2 = this.m_choosecountButton.IsClicked;
            bool flag2 = isClicked2;
            if (flag2)
            {
                DialogsManager.ShowDialog(null, new ChooseItemsCountDialog(delegate (int count)
                {
                    this.ItemCount = count;
                }));
            }
            bool isClicked3 = this.m_upgradeshopButton.IsClicked;
            bool flag3 = isClicked3;
            if (flag3)
            {
                int num2 = (this.m_subsystemFinancial.ShopLevel + 1) * 400;
                bool flag4 = this.m_subsystemFinancial.Money > (double)num2;
                bool flag5 = flag4;
                if (flag5)
                {
                    bool flag6 = this.m_subsystemFinancial.UpgradeShop();
                    bool flag7 = flag6;
                    if (flag7)
                    {
                        SubsystemFinancial subsystemFinancial = this.m_subsystemFinancial;
                        int shopLevel = subsystemFinancial.ShopLevel;
                        subsystemFinancial.ShopLevel = shopLevel + 1;
                        this.m_subsystemFinancial.Money -= (double)num2;
                    }
                    else
                    {
                        this.m_componentPlayer.ComponentGui.DisplaySmallMessage("Shop is already at maximum level, cannot upgrade", Color.White, true, true);
                    }
                }
                else
                {
                    this.m_componentPlayer.ComponentGui.DisplaySmallMessage("Insufficient balance, cannot upgrade shop. Required:" + num2.ToString(), Color.White, true, true);
                }
            }
            bool isClicked4 = this.m_soldButton.IsClicked;
            bool flag8 = isClicked4;
            if (flag8)
            {
                foreach (Money money in MoneyManager.m_moneys)
                {
                    bool flag9 = this.m_componentShop.m_slots[this.m_componentShop.SoldSlotIndex].Value == money.ItemValue && this.m_componentShop.m_slots[this.m_componentShop.SoldSlotIndex].Count > 0;
                    bool flag10 = flag9;
                    if (flag10)
                    {
                        bool flag11 = this.m_subsystemFinancial.ShopLevel >= money.ShopLevel;
                        bool flag12 = flag11;
                        if (flag12)
                        {
                            this.m_subsystemFinancial.AddMoney(money.returnMoney * (double)this.m_componentShop.m_slots[this.m_componentShop.SoldSlotIndex].Count);
                            this.m_componentShop.RemoveSlotItems(this.m_componentShop.SoldSlotIndex, this.m_componentShop.m_slots[this.m_componentShop.SoldSlotIndex].Count);
                        }
                        else
                        {
                            this.m_componentPlayer.ComponentGui.DisplaySmallMessage("Insufficient shop level, cannot sell", Color.White, true, true);
                        }
                    }
                }
            }
            bool flag13 = this.m_componentShop.m_slots[this.m_componentShop.SoldSlotIndex].Count > 0;
            bool flag14 = flag13;
            if (flag14)
            {
                using (List<Money>.Enumerator enumerator2 = MoneyManager.m_moneys.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        Money money2 = enumerator2.Current;
                        bool flag15 = this.m_componentShop.m_slots[this.m_componentShop.SoldSlotIndex].Value == money2.ItemValue;
                        bool flag16 = flag15;
                        if (flag16)
                        {
                            bool flag17 = this.m_subsystemFinancial.ShopLevel >= money2.ShopLevel;
                            bool flag18 = flag17;
                            if (flag18)
                            {
                                this.m_returnmoneyLabel.Text = "Estimated Sale Amount:" + (money2.returnMoney * (double)this.m_componentShop.m_slots[this.m_componentShop.SoldSlotIndex].Count).ToString();
                            }
                            else
                            {
                                this.m_returnmoneyLabel.Text = "Current shop level insufficient, cannot view";
                            }
                        }
                    }
                    goto IL_49C;
                }
            }
            this.m_returnmoneyLabel.Text = "Estimated Sale Amount:0";
        IL_49C:
            bool isClicked5 = this.m_buyButton.IsClicked;
            bool flag19 = isClicked5;
            if (flag19)
            {
                bool flag20 = this.ItemValue != 0;
                bool flag21 = flag20;
                if (flag21)
                {
                    bool flag22 = this.ItemCount > 0;
                    bool flag23 = flag22;
                    if (flag23)
                    {
                        using (List<Money>.Enumerator enumerator3 = MoneyManager.m_moneys.GetEnumerator())
                        {
                            while (enumerator3.MoveNext())
                            {
                                Money money3 = enumerator3.Current;
                                bool flag24 = this.ItemValue == money3.ItemValue;
                                bool flag25 = flag24;
                                if (flag25)
                                {
                                    bool flag26 = (double)this.ItemCount * money3.itemMoney < this.m_subsystemFinancial.Money;
                                    bool flag27 = flag26;
                                    if (flag27)
                                    {
                                        this.m_subsystemFinancial.Money -= (double)this.ItemCount * money3.itemMoney;
                                        this.m_subsystemFinancial.Project.FindSubsystem<SubsystemPickables>(true).AddPickable(this.ItemValue, this.ItemCount, this.m_componentPlayer.ComponentBody.Position, null, null);
                                    }
                                    else
                                    {
                                        this.m_componentPlayer.ComponentGui.DisplaySmallMessage("Insufficient balance", Color.White, true, true);
                                    }
                                }
                            }
                            return;
                        }
                    }
                    this.m_componentPlayer.ComponentGui.DisplaySmallMessage("Purchase quantity cannot be less than or equal to 0", Color.White, true, true);
                }
                else
                {
                    this.m_componentPlayer.ComponentGui.DisplaySmallMessage("No item selected for purchase", Color.White, true, true);
                }
            }
        }

        // Token: 0x04000013 RID: 19
        public GridPanelWidget m_inventoryGrid;

        // Token: 0x04000014 RID: 20
        public ButtonWidget m_chooseButton;

        // Token: 0x04000015 RID: 21
        public ButtonWidget m_choosecountButton;

        // Token: 0x04000016 RID: 22
        public ButtonWidget m_buyButton;

        // Token: 0x04000017 RID: 23
        public ButtonWidget m_soldButton;

        // Token: 0x04000018 RID: 24
        public ButtonWidget m_upgradeshopButton;

        // Token: 0x04000019 RID: 25
        public InventorySlotWidget m_soldSlot;

        // Token: 0x0400001A RID: 26
        public BlockIconWidget m_itemvalueicon;

        // Token: 0x0400001B RID: 27
        public LabelWidget m_chooseitemLabel;

        // Token: 0x0400001C RID: 28
        public LabelWidget m_chooseitemcountLabel;

        // Token: 0x0400001D RID: 29
        public LabelWidget m_moneyLabel;

        // Token: 0x0400001E RID: 30
        public LabelWidget m_returnmoneyLabel;

        // Token: 0x0400001F RID: 31
        public LabelWidget m_shoplevelLabel;

        // Token: 0x04000020 RID: 32
        public ComponentPlayer m_componentPlayer;

        // Token: 0x04000021 RID: 33
        public ComponentShop m_componentShop;

        // Token: 0x04000022 RID: 34
        public SubsystemFinancial m_subsystemFinancial;

        // Token: 0x04000023 RID: 35
        public IInventory m_inventory;

        // Token: 0x04000024 RID: 36
        public int ItemValue;

        // Token: 0x04000025 RID: 37
        public int ItemCount;
    }
}
