using System;
using System.Xml.Linq;
using Engine;
using Game;

namespace Financial
{
    // Token: 0x02000002 RID: 2
    public class ChooseItemsCountDialog : Dialog
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
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
            this.m_countSlider.MaxValue = 1E+09f;
            this.m_countSlider.Granularity = 1f;
            this.m_countSlider.Value = (float)this.Count;
        }

        // Token: 0x06000002 RID: 2 RVA: 0x00002124 File Offset: 0x00000324
        public override void Update()
        {
            this.m_countSlider.Text = "Quantity:" + this.Count.ToString();
            this.Count = Math.Clamp((int)this.m_countSlider.Value, 0, 1000000000);
            this.m_addcountButton.IsEnabled = (this.m_countSlider.Value < 1E+09f);
            this.m_removecountButton.IsEnabled = (this.m_countSlider.Value > 0f);
            bool isClicked = this.m_removecountButton.IsClicked;
            bool flag = isClicked;
            if (flag)
            {
                SliderWidget countSlider = this.m_countSlider;
                float value = countSlider.Value;
                countSlider.Value = value - 1f;
            }
            bool isClicked2 = this.m_addcountButton.IsClicked;
            bool flag2 = isClicked2;
            if (flag2)
            {
                SliderWidget countSlider2 = this.m_countSlider;
                float value2 = countSlider2.Value;
                countSlider2.Value = value2 + 1f;
            }
            bool isClicked3 = this.m_okButton.IsClicked;
            bool flag3 = isClicked3;
            if (flag3)
            {
                DialogsManager.HideDialog(this);
                bool flag4 = this.m_handler != null;
                bool flag5 = flag4;
                if (flag5)
                {
                    this.m_handler(this.Count);
                }
            }
        }

        // Token: 0x04000001 RID: 1
        public ButtonWidget m_okButton;

        // Token: 0x04000002 RID: 2
        public ButtonWidget m_addcountButton;

        // Token: 0x04000003 RID: 3
        public ButtonWidget m_removecountButton;

        // Token: 0x04000004 RID: 4
        public SliderWidget m_countSlider;

        // Token: 0x04000005 RID: 5
        public Action<int> m_handler;

        // Token: 0x04000006 RID: 6
        public int Count;
    }
}
