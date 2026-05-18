using System;
using System.Xml.Linq;
using Engine;
using Game;

namespace Financial
{
	public class ChooseItemsCountDialog : Dialog
	{
		public ChooseItemsCountDialog(Action<int> handler)
		{
			XElement xelement = ContentManager.Get<XElement>("Dialogs/ChooseItemsCountDialog", null);
			base.LoadContents(this, xelement);
			this.m_handler = handler;
			this.m_okButton = this.Children.Find<ButtonWidget>("OkButton", true);
			this.m_countSlider = this.Children.Find<SliderWidget>("CountSlider", true);
			this.m_addcountButton = this.Children.Find<ButtonWidget>("AddCountButton", true);
			this.m_removecountButton = this.Children.Find<ButtonWidget>("RemoveCountButton", true);
			this.m_countSlider.MinValue = 0f;
			this.m_countSlider.MaxValue = 40f;
			this.m_countSlider.Granularity = 1f;
			this.m_countSlider.Value = (float)this.Count;
		}

		public override void Update()
		{
			this.m_countSlider.Text = LanguageControl.GetContentWidgets("ChooseItemsCountDialog", 1) + " " + this.Count.ToString();
			this.Count = MathUtils.Clamp((int)this.m_countSlider.Value, 0, 40);
			this.m_addcountButton.IsEnabled = (this.m_countSlider.Value < 40f);
			this.m_removecountButton.IsEnabled = (this.m_countSlider.Value > 0f);

			if (this.m_removecountButton.IsClicked)
			{
				SliderWidget countSlider = this.m_countSlider;
				float value = countSlider.Value;
				countSlider.Value = value - 1f;
			}
			if (this.m_addcountButton.IsClicked)
			{
				SliderWidget countSlider2 = this.m_countSlider;
				float value = countSlider2.Value;
				countSlider2.Value = value + 1f;
			}
			if (this.m_okButton.IsClicked)
			{
				DialogsManager.HideDialog(this);
				if (this.m_handler != null)
				{
					this.m_handler.Invoke(this.Count);
				}
			}
		}

		public ButtonWidget m_okButton;
		public ButtonWidget m_addcountButton;
		public ButtonWidget m_removecountButton;
		public SliderWidget m_countSlider;
		public Action<int> m_handler;
		public int Count;
	}
}
