using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using Game;
using XmlUtilities;

namespace Financial
{
	// Token: 0x02000008 RID: 8
	public static class MoneyManager
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002F38 File Offset: 0x00001138
		public static void Initialize()
		{
			foreach (XElement xelement in ContentManager.Get<XElement>("Financial", null).Descendants("Items"))
			{
				Money money = new Money();
				string attributeValue = XmlUtils.GetAttributeValue<string>(xelement, "Name");
				money.ItemValue = MoneyManager.DecodeValue(attributeValue);
				money.itemMoney = XmlUtils.GetAttributeValue<double>(xelement, "Money");
				money.ShopLevel = XmlUtils.GetAttributeValue<int>(xelement, "ShopLevel");
				money.returnMoney = XmlUtils.GetAttributeValue<double>(xelement, "ReturnMoney");
				MoneyManager.m_moneys.Add(money);
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002FF0 File Offset: 0x000011F0
		public static int DecodeValue(string result)
		{
			string[] array = result.Split(new char[]
			{
				':'
			});
			Block block = BlocksManager.FindBlockByTypeName(array[0], true);
			int data = (array.Length >= 2) ? int.Parse(array[1], CultureInfo.InvariantCulture) : 0;
			return Terrain.MakeBlockValue(block.BlockIndex, 0, data);
		}

		// Token: 0x04000027 RID: 39
		public static List<Money> m_moneys = new List<Money>();
	}
}
