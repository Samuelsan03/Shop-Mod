using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using Game;
using XmlUtilities;

namespace Financial
{
	// Token: 0x02000007 RID: 7
	public static class MoneyManager
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002708 File Offset: 0x00000908
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

		// Token: 0x06000013 RID: 19 RVA: 0x000027C4 File Offset: 0x000009C4
		public static int DecodeValue(string result)
		{
			string[] array = result.Split(new char[]
			{
				':'
			});
            Block block = BlocksManager.GetBlock(array[0]);
            int num = (array.Length >= 2) ? int.Parse(array[1], CultureInfo.InvariantCulture) : 0;
			return Terrain.MakeBlockValue(block.BlockIndex, 0, num);
		}

		// Token: 0x04000012 RID: 18
		public static List<Money> m_moneys = new List<Money>();
	}
}
