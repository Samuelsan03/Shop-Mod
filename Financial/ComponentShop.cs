using System;
using Engine;
using Engine.Input;
using Game;
using GameEntitySystem;
using TemplatesDatabase;

namespace Financial
{
	public class ComponentShop : ComponentInventoryBase, IUpdateable
	{
		UpdateOrder IUpdateable.UpdateOrder
		{
			get
			{
				return ((IUpdateable)this.m_componentPlayer).UpdateOrder;
			}
		}

		float IUpdateable.FloatUpdateOrder
		{
			get
			{
				return (float)((IUpdateable)this).UpdateOrder;
			}
		}

		public BitmapButtonWidget ShopButton { get; set; }

		public int SoldSlotIndex
		{
			get
			{
				return this.SlotsCount - 1;
			}
		}

		public void Update(float dt)
		{
			bool flag = this.m_componentPlayer.ComponentGui.ModalPanelWidget is ShopWidget;
			this.ShopButton.IsChecked = flag;
			if (this.ShopButton.IsClicked)
			{
				if (flag)
				{
					this.m_componentPlayer.ComponentGui.ModalPanelWidget = null;
					return;
				}
				this.m_componentPlayer.ComponentGui.ModalPanelWidget = new ShopWidget(this, this.m_componentPlayer, this.m_subsystemFinancial);
			}
		}

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

		public ComponentPlayer m_componentPlayer;
		public SubsystemFinancial m_subsystemFinancial;
	}
}
