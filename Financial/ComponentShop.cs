using System;
using Engine;
using Engine.Input;
using Game;
using GameEntitySystem;
using TemplatesDatabase;

namespace Financial
{
	// Token: 0x02000004 RID: 4
	public class ComponentShop : ComponentInventoryBase, IUpdateable
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000024B8 File Offset: 0x000006B8
		UpdateOrder IUpdateable.UpdateOrder
		{
			get
			{
				return this.m_componentPlayer.UpdateOrder;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000024D5 File Offset: 0x000006D5
		// (set) Token: 0x06000009 RID: 9 RVA: 0x000024DD File Offset: 0x000006DD
		public BitmapButtonWidget ShopButton { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000024E8 File Offset: 0x000006E8
		public int SoldSlotIndex
		{
			get
			{
				return this.SlotsCount - 1;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002504 File Offset: 0x00000704
		public void Update(float dt)
		{
			bool flag = this.m_componentPlayer.ComponentGui.ModalPanelWidget is ShopWidget;
			this.ShopButton.IsChecked = flag;
			bool flag2 = this.ShopButton.IsClicked || Keyboard.IsKeyDownOnce(49);
			bool flag3 = flag2;
			if (flag3)
			{
				bool flag4 = flag;
				bool flag5 = flag4;
				if (flag5)
				{
					this.m_componentPlayer.ComponentGui.ModalPanelWidget = null;
				}
				else
				{
					this.m_componentPlayer.ComponentGui.ModalPanelWidget = new ShopWidget(this, this.m_componentPlayer, this.m_subsystemFinancial);
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000259C File Offset: 0x0000079C
		public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
		{
			base.Load(valuesDictionary, idToEntityMap);
			this.m_componentPlayer = base.Entity.FindComponent<ComponentPlayer>();
			this.m_subsystemFinancial = base.Project.FindSubsystem<SubsystemFinancial>(true);
			try
			{
				this.ShopButton = this.m_componentPlayer.ViewWidget.GameWidget.Children.Find<BitmapButtonWidget>("ShopButton", true);
			}
			catch
			{
				this.ShopButton = new BitmapButtonWidget
				{
					Name = "ShopButton",
					Size = new Vector2(68f, 64f),
					Margin = new Vector2(4f, 0f),
					NormalSubtexture = ContentManager.Get<Subtexture>("Textures/ShopButton", null),
					ClickedSubtexture = ContentManager.Get<Subtexture>("Textures/ShopButton_Pressed", null)
				};
				this.m_componentPlayer.ViewWidget.GameWidget.Children.Find<StackPanelWidget>("MoreContents", true).Children.Add(this.ShopButton);
			}
		}

		// Token: 0x0400000C RID: 12
		public ComponentPlayer m_componentPlayer;

		// Token: 0x0400000D RID: 13
		public SubsystemFinancial m_subsystemFinancial;
	}
}
