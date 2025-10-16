using System;
using System.Xml.Linq;
using Game;

namespace Financial
{
	// Token: 0x02000003 RID: 3
	public class ChooseItemsDialog : Dialog
	{
		// Token: 0x06000003 RID: 3 RVA: 0x0000225C File Offset: 0x0000045C
		public ChooseItemsDialog(SubsystemFinancial subsystemFinancial, Action<int> handler)
		{
			XElement xelement = ContentManager.Get<XElement>("Dialogs/ChooseItemsDialog", null);
			base.LoadContents(this, xelement);
			this.m_okButton = this.Children.Find<ButtonWidget>("OkButton", true);
			this.m_itemsList = this.Children.Find<ListPanelWidget>("ItemsList", true);
			this.m_handler = handler;
			this.m_itemsList.ItemWidgetFactory = delegate(object item)
			{
				Money money2 = (Money)item;
				int num = Terrain.ExtractContents(money2.ItemValue);
				Block block = BlocksManager.Blocks[num];
				XElement xelement2 = ContentManager.Get<XElement>("Widgets/MoneyItem", null);
				ContainerWidget containerWidget = (ContainerWidget)Widget.LoadWidget(this, xelement2, null);
				containerWidget.Children.Find<BlockIconWidget>("Icon", true).Value = money2.ItemValue;
				containerWidget.Children.Find<LabelWidget>("Name", true).Text = "Item Name:" + block.GetDisplayName(null, money2.ItemValue);
				containerWidget.Children.Find<LabelWidget>("Money", true).Text = "Item Price:" + money2.itemMoney.ToString() + "   Sell Price:" + money2.returnMoney.ToString();
				return containerWidget;
			};
			this.m_itemsList.ItemClicked = delegate(object item)
			{
				Money money2 = (Money)item;
				this.value = new int?(money2.ItemValue);
			};
			foreach (Money money in MoneyManager.m_moneys)
			{
				bool flag = subsystemFinancial.ShopLevel >= money.ShopLevel;
				bool flag2 = flag;
				if (flag2)
				{
					this.m_itemsList.AddItem(money);
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002358 File Offset: 0x00000558
		public override void Update()
		{
			bool isClicked = this.m_okButton.IsClicked;
			bool flag = isClicked;
			if (flag)
			{
				DialogsManager.HideDialog(this);
				bool flag2 = this.m_handler != null && this.value != null;
				bool flag3 = flag2;
				if (flag3)
				{
					this.m_handler(this.value.Value);
				}
			}
		}

		// Token: 0x04000007 RID: 7
		public ButtonWidget m_okButton;

		// Token: 0x04000008 RID: 8
		public ListPanelWidget m_itemsList;

		// Token: 0x04000009 RID: 9
		public Action<int> m_handler;

		// Token: 0x0400000A RID: 10
		public int? value;
	}
}
