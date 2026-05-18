using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Engine;
using Game;

namespace Financial
{
	public class ShopWidget : CanvasWidget
	{
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

		public override void MeasureOverride(Vector2 parentAvailableSize)
		{
			int max = (this.m_inventory is ComponentCreativeInventory) ? 10 : 7;
			this.m_inventory.VisibleSlotsCount = MathUtils.Clamp((int)((parentAvailableSize.X - 320f - 25f) / 72f), 7, max);
			if (this.m_inventory.VisibleSlotsCount != this.m_inventoryGrid.Children.Count)
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

		public override void Update()
		{
			this.m_moneyLabel.Text = LanguageControl.GetContentWidgets("ShopWidget", 2) + " " + this.m_subsystemFinancial.Money.ToString();
			this.m_shoplevelLabel.Text = LanguageControl.GetContentWidgets("ShopWidget", 3) + " " + this.m_subsystemFinancial.ShopLevel.ToString();
			this.m_chooseitemcountLabel.Text = LanguageControl.GetContentWidgets("ShopWidget", 16) + " " + this.ItemCount.ToString();
			int num = Terrain.ExtractContents(this.ItemValue);
			Block block = BlocksManager.Blocks[num];
			this.m_chooseitemLabel.Text = LanguageControl.GetContentWidgets("ShopWidget", 15) + " " + block.GetDisplayName(null, this.ItemValue);
			this.m_itemvalueicon.Value = this.ItemValue;

			if (this.m_chooseButton.IsClicked)
			{
				DialogsManager.ShowDialog(null, new ChooseItemsDialog(this.m_subsystemFinancial, delegate (int value)
				{
					this.ItemValue = value;
				}));
			}

			if (this.m_choosecountButton.IsClicked)
			{
				DialogsManager.ShowDialog(null, new ChooseItemsCountDialog(delegate (int count)
				{
					this.ItemCount = count;
				}));
			}

			if (this.m_upgradeshopButton.IsClicked)
			{
				int num2 = (this.m_subsystemFinancial.ShopLevel + 1) * 400;
				if (this.m_subsystemFinancial.Money > (double)num2)
				{
					if (this.m_subsystemFinancial.UpgradeShop())
					{
						SubsystemFinancial subsystemFinancial = this.m_subsystemFinancial;
						int shopLevel = subsystemFinancial.ShopLevel;
						subsystemFinancial.ShopLevel = shopLevel + 1;
						this.m_subsystemFinancial.Money -= (double)num2;
					}
					else
					{
						this.m_componentPlayer.ComponentGui.DisplaySmallMessage(LanguageControl.GetContentWidgets("ShopWidget", 13), Color.White, true, true);
					}
				}
				else
				{
					this.m_componentPlayer.ComponentGui.DisplaySmallMessage(LanguageControl.GetContentWidgets("ShopWidget", 14) + " " + num2.ToString(), Color.White, true, true);
				}
			}

			if (this.m_soldButton.IsClicked)
			{
				foreach (Money money in MoneyManager.m_moneys)
				{
					if (this.m_componentShop.m_slots[this.m_componentShop.SoldSlotIndex].Value == money.ItemValue && this.m_componentShop.m_slots[this.m_componentShop.SoldSlotIndex].Count > 0)
					{
						if (this.m_subsystemFinancial.ShopLevel >= money.ShopLevel)
						{
							this.m_subsystemFinancial.AddMoney(money.returnMoney * (double)this.m_componentShop.m_slots[this.m_componentShop.SoldSlotIndex].Count);
							this.m_componentShop.RemoveSlotItems(this.m_componentShop.SoldSlotIndex, this.m_componentShop.m_slots[this.m_componentShop.SoldSlotIndex].Count);
						}
						else
						{
							this.m_componentPlayer.ComponentGui.DisplaySmallMessage(LanguageControl.GetContentWidgets("ShopWidget", 13), Color.White, true, true);
						}
					}
				}
			}

			if (this.m_componentShop.m_slots[this.m_componentShop.SoldSlotIndex].Count > 0)
			{
				using (List<Money>.Enumerator enumerator = MoneyManager.m_moneys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Money money2 = enumerator.Current;
						if (this.m_componentShop.m_slots[this.m_componentShop.SoldSlotIndex].Value == money2.ItemValue)
						{
							if (this.m_subsystemFinancial.ShopLevel >= money2.ShopLevel)
							{
								this.m_returnmoneyLabel.Text = LanguageControl.GetContentWidgets("ShopWidget", 12) + " " + (money2.returnMoney * (double)this.m_componentShop.m_slots[this.m_componentShop.SoldSlotIndex].Count).ToString();
							}
							else
							{
								this.m_returnmoneyLabel.Text = LanguageControl.GetContentWidgets("ShopWidget", 13);
							}
						}
					}
					goto IL_3EF;
				}
			}
			this.m_returnmoneyLabel.Text = LanguageControl.GetContentWidgets("ShopWidget", 14);

		IL_3EF:
			if (this.m_buyButton.IsClicked)
			{
				if (this.ItemValue != 0)
				{
					if (this.ItemCount > 0)
					{
						using (List<Money>.Enumerator enumerator = MoneyManager.m_moneys.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Money money3 = enumerator.Current;
								if (this.ItemValue == money3.ItemValue)
								{
									if ((double)this.ItemCount * money3.itemMoney < this.m_subsystemFinancial.Money)
									{
										this.m_subsystemFinancial.Money -= (double)this.ItemCount * money3.itemMoney;
										this.m_subsystemFinancial.Project.FindSubsystem<SubsystemPickables>(true).AddPickable(this.ItemValue, this.ItemCount, this.m_componentPlayer.ComponentBody.Position, default(Vector3?), default(Matrix?));
									}
									else
									{
										this.m_componentPlayer.ComponentGui.DisplaySmallMessage(LanguageControl.GetContentWidgets("ShopWidget", 2) + " " + LanguageControl.Get("Usual", "error"), Color.White, true, true);
									}
								}
							}
							return;
						}
					}
					this.m_componentPlayer.ComponentGui.DisplaySmallMessage(LanguageControl.GetContentWidgets("ShopWidget", 14), Color.White, true, true);
					return;
				}
				this.m_componentPlayer.ComponentGui.DisplaySmallMessage(LanguageControl.GetContentWidgets("ShopWidget", 15), Color.White, true, true);
			}
		}

		public GridPanelWidget m_inventoryGrid;
		public ButtonWidget m_chooseButton;
		public ButtonWidget m_choosecountButton;
		public ButtonWidget m_buyButton;
		public ButtonWidget m_soldButton;
		public ButtonWidget m_upgradeshopButton;
		public InventorySlotWidget m_soldSlot;
		public BlockIconWidget m_itemvalueicon;
		public LabelWidget m_chooseitemLabel;
		public LabelWidget m_chooseitemcountLabel;
		public LabelWidget m_moneyLabel;
		public LabelWidget m_returnmoneyLabel;
		public LabelWidget m_shoplevelLabel;
		public ComponentPlayer m_componentPlayer;
		public ComponentShop m_componentShop;
		public SubsystemFinancial m_subsystemFinancial;
		public IInventory m_inventory;
		public int ItemValue;
		public int ItemCount;
	}
}
