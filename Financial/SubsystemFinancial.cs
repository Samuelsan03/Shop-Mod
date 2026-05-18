using System;
using GameEntitySystem;
using TemplatesDatabase;

namespace Financial
{
	public class SubsystemFinancial : Subsystem
	{
		public double Money { get; set; }
		public int ShopLevel { get; set; }

		public void AddMoney(double money)
		{
			if (money > 0.0)
			{
				this.Money += money;
			}
		}

		public bool UpgradeShop()
		{
			return (float)this.ShopLevel >= 0f && (float)this.ShopLevel <= 10f;
		}

		public override void Load(ValuesDictionary valuesDictionary)
		{
			base.Load(valuesDictionary);
			this.Money = valuesDictionary.GetValue<double>("Money");
			this.ShopLevel = valuesDictionary.GetValue<int>("ShopLevel");
		}

		public override void Save(ValuesDictionary valuesDictionary)
		{
			valuesDictionary.SetValue<double>("Money", this.Money);
			valuesDictionary.SetValue<int>("ShopLevel", this.ShopLevel);
		}
	}
}
