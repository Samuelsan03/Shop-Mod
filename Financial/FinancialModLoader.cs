using System;
using System.Collections.Generic;
using Engine;
using Game;

namespace Financial
{
	// Token: 0x02000005 RID: 5
	public class FinancialModLoader : ModLoader
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000026BD File Offset: 0x000008BD
		public override void __ModInitialize()
		{
			ModsManager.RegisterHook("OnLoadingFinished", this);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000026CC File Offset: 0x000008CC
		public override void OnLoadingFinished(List<Action> actions)
		{
			actions.Add(delegate
			{
				MoneyManager.Initialize();
				Log.Information("[Info]Financial mod manager initialized");
			});
		}
	}
}
