using System;
using System.Collections.Generic;
using Engine;
using Game;

namespace Financial
{
	// Token: 0x02000009 RID: 9
	public class FinancialModLoader : ModLoader
	{
		// Token: 0x06000020 RID: 32 RVA: 0x00003047 File Offset: 0x00001247
		public override void __ModInitialize()
		{
			ModsManager.RegisterHook("OnLoadingFinished", this);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003054 File Offset: 0x00001254
		public override void OnLoadingFinished(List<Action> actions)
		{
			actions.Add(delegate
			{
				MoneyManager.Initialize();
				Log.Information(LanguageControl.Get("FinancialModLoader", 1));
			});
		}
	}
}
