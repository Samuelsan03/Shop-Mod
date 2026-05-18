using System;
using System.Xml.Linq;
using Game;

namespace Financial
{
	public class ChooseItemsDialog : Dialog
	{
		public ChooseItemsDialog(SubsystemFinancial subsystemFinancial, Action<int> handler)
		{
			XElement xelement = ContentManager.Get<XElement>("Dialogs/ChooseItemsDialog", null);
			base.LoadContents(this, xelement);
			this.m_okButton = this.Children.Find<ButtonWidget>("OkButton", true);
			this.m_itemsList = this.Children.Find<ListPanelWidget>("ItemsList", true);
			this.m_handler = handler;

			this.m_itemsList.ItemWidgetFactory = delegate (object item)
			{
				Money money2 = (Money)item;
				int num = Terrain.ExtractContents(money2.ItemValue);
				Block block = BlocksManager.Blocks[num];
				XElement xelement2 = ContentManager.Get<XElement>("Widgets/MoneyItem", null);
				ContainerWidget containerWidget = (ContainerWidget)Widget.LoadWidget(this, xelement2, null);
				containerWidget.Children.Find<BlockIconWidget>("Icon", true).Value = money2.ItemValue;
				containerWidget.Children.Find<LabelWidget>("Name", true).Text = LanguageControl.GetContentWidgets("MoneyItem", 1) + " " + block.GetDisplayName(null, money2.ItemValue);
				containerWidget.Children.Find<LabelWidget>("Money", true).Text = LanguageControl.GetContentWidgets("MoneyItem", 2) + " " + money2.itemMoney.ToString() + "   " + LanguageControl.Get("MoneyItem", 3) + " " + money2.returnMoney.ToString();
				return containerWidget;
			};

			this.m_itemsList.ItemClicked = OnItemClicked;

			foreach (Money money in MoneyManager.m_moneys)
			{
				if (subsystemFinancial.ShopLevel >= money.ShopLevel)
				{
					this.m_itemsList.AddItem(money);
				}
			}
		}

		private void OnItemClicked(object item)
		{
			Money money = (Money)item;
			this.value = new int?(money.ItemValue);
		}

		public override void Update()
		{
			if (this.m_okButton.IsClicked)
			{
				DialogsManager.HideDialog(this);
				if (this.m_handler != null && this.value != null)
				{
					this.m_handler.Invoke(this.value.Value);
				}
			}
		}

		public ButtonWidget m_okButton;
		public ListPanelWidget m_itemsList;
		public Action<int> m_handler;
		public int? value;
	}
}
