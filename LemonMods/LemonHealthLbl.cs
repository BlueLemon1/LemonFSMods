using FSLoader;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using Logger = FSLoader.Logger;
using System;

namespace LemonMods
{
    [ModInfo("Lemon_HealthLabel", "Shows Health Label above Healthbar", "Lemon", 1, 0)]
    internal class LemonHealthLbl : Mod
    {
        private UILabel healthLbl;
        Logger logger = new FSLoader.Logger("Lemon_Test.log");

        [Hook("DwellerInfoWindow::Show()")]
        public void Hook_Init(CallContext context)
        {
            DwellerInfoWindow dwellerInfoWindow = (DwellerInfoWindow)context.This;
            if (dwellerInfoWindow == null)
                return;
            if (healthLbl != null)
                return;

            healthLbl = Object.Instantiate(dwellerInfoWindow.m_levelTitle);
            healthLbl.name = "LemonLbl_Health";
            healthLbl.overflowMethod = UILabel.Overflow.ResizeFreely;
            //gameObject3.transform.parent = transform;
            // dwellercontent is kinda the same like DwellerInfoPanel, so it gets found as child
            healthLbl.transform.parent = dwellerInfoWindow.m_dwellerContent.transform; //dwellercontent to automatically hide when for example pet show, no need to do it manually
            healthLbl.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            healthLbl.transform.localPosition = new Vector3(3f, -47.5f, 1f);
            healthLbl.text = "";
            //logger.WriteLine(MonoSingleton<DwellerManager>.Instance.SelectedDweller.name);
            
        }


        [Hook("DwellerInfoWindow::FillData(Dweller)")]
        public void Hook_CheckButton(CallContext context, Dweller dweller)
        {
            //logger.WriteLine(MonoSingleton<GameParameters>.Instance.GUI.ShowWastelandHealthLabel.ToString());
            if (healthLbl != null)
            {
                healthLbl.text = GetHealthLabelText(dweller);

            }
        }
        private string GetHealthLabelText(Dweller dweller)
        {
            int healthMax = (int)dweller.Health.HealthMax;
            int currentLevel = dweller.Experience.CurrentLevel;

            double num = ((double)Mathf.CeilToInt(dweller.Health.BaseHealthMax) - (105.0 + (double)(currentLevel - 1) * 2.5)) / 0.5;
            if (currentLevel > 1)
            {
                num /= currentLevel - 1;
            }
            double num2 = Math.Floor(num * Math.Pow(10.0, 1)) / Math.Pow(10.0, 1);
            return $"{healthMax}HP ({num2}E)";
        }
    }
}
