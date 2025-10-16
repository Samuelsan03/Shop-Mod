using System;
using GameEntitySystem;
using TemplatesDatabase;

namespace Financial
{
	// Token: 0x02000009 RID: 9
	public class SubsystemFinancial : Subsystem
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000031D0 File Offset: 0x000013D0
		// (set) Token: 0x0600001B RID: 27 RVA: 0x000031D8 File Offset: 0x000013D8
		public double Money { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000031E1 File Offset: 0x000013E1
		// (set) Token: 0x0600001D RID: 29 RVA: 0x000031E9 File Offset: 0x000013E9
		public int ShopLevel { get; set; }

		// Token: 0x0600001E RID: 30 RVA: 0x000031F4 File Offset: 0x000013F4
		public void AddMoney(double money)
		{
			bool flag = money > 0.0;
			bool flag2 = flag;
			if (flag2)
			{
				this.Money += money;
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003228 File Offset: 0x00001428
		public bool UpgradeShop()
		{
			return (float)this.ShopLevel >= 0f && (float)this.ShopLevel <= 10f;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000325C File Offset: 0x0000145C
		public override void Load(ValuesDictionary valuesDictionary)
		{
			base.Load(valuesDictionary);
			this.Money = valuesDictionary.GetValue<double>("Money");
			this.ShopLevel = valuesDictionary.GetValue<int>("ShopLevel");
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000328B File Offset: 0x0000148B
		public override void Save(ValuesDictionary valuesDictionary)
		{
			valuesDictionary.SetValue<double>("Money", this.Money);
			valuesDictionary.SetValue<int>("ShopLevel", this.ShopLevel);
		}
	}
}
