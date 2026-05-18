using System;
using System.Collections.Generic;
using System.IO;
using Engine;
using Game;

namespace Financial
{
	public class FinancialModLoader : ModLoader
	{
		public override void __ModInitialize()
		{
			ModsManager.RegisterHook("OnLoadingFinished", this);
		}

		public override void OnLoadingFinished(List<Action> actions)
		{
			actions.Add(delegate ()
			{
				LoadLanguage();
				MoneyManager.Initialize();
				Log.Information(LanguageControl.Get("FinancialModLoader", 1));
			});
		}

		private void LoadLanguage()
		{
			try
			{
				Entity.GetAssetsFile("Financial.json", delegate (Stream stream)
				{
					if (stream != null)
					{
						LanguageControl.loadJson(stream);
					}
				});
			}
			catch
			{
			}
		}
	}
}
