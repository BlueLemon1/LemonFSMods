using FSLoader;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using UnityEngine;
using Object = UnityEngine.Object;
using JetBrains.Annotations;

namespace LemonMods
{
    [ModInfo("Lemon_Add_HealthLabel OLD", "Add Health Label OLD", "Lemon", 1, 0)]
    internal class LemonHealthLblOLD : Mod
    {
        private string saved;

        [Hook("DwellerInfoWindow::Show()")]
        public void Hook_Init(CallContext context)
        {
            DwellerInfoWindow dwellerInfoWindow = (DwellerInfoWindow)context.This;
            if (dwellerInfoWindow == null)
            {
                return;
            }
            Transform transform = dwellerInfoWindow.gameObject.FindChild("DwellerInfoPanel");
            if (transform == null)
            {
                return;
            }
            GameObject gameObject = transform.gameObject;
            if (gameObject.FindChild("ModLbl_Healthinfo") == null)
            {
                UILabel uilabel;
                if (MonoSingleton<VaultGUIManager>.IsInstanceValid)
                {
                    uilabel = MonoSingleton<VaultGUIManager>.Instance.m_DwellerInfoWindow.m_dwellerName;
                }
                else if (MonoSingleton<QuestGUIManager>.IsInstanceValid)
                {
                    uilabel = MonoSingleton<QuestGUIManager>.Instance.m_dwellerInfoWindow.m_dwellerName;
                }
                else
                {
                    return;
                }

                GameObject gameObject3 = Object.Instantiate<GameObject>(uilabel.gameObject);
                gameObject3.name = "ModLbl_Healthinfo";
                gameObject3.transform.parent = gameObject.transform;
                gameObject3.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
                gameObject3.transform.localPosition = new Vector3(-47.5f, 105f, 1f);


            }
        }

        private GameObject FindButton(DwellerInfoWindow window)
        {
            GameObject gameObject = window.gameObject.FindChild("DwellerInfoPanel").gameObject;
            Transform transform = gameObject.FindChild("ModLbl_Healthinfo");
            if (transform == null)
            {
                return null;
            }
            return transform.gameObject;
        }


        [Hook("DwellerInfoWindow::FillData(Dweller)")]
        public void Hook_CheckButton(CallContext context, Dweller dweller)
        {

            GameObject gameObject = this.FindButton((DwellerInfoWindow)context.This);
            if (gameObject != null)
            {
                UILabel componentInChildren = gameObject.GetComponentInChildren<UILabel>();

                int healthMax = (int)dweller.Health.HealthMax;
                int currentLevel = dweller.Experience.CurrentLevel;

                double num = ((double)Mathf.CeilToInt(dweller.Health.BaseHealthMax) - (105.0 + (double)(currentLevel - 1) * 2.5)) / 0.5;
                if (currentLevel > 1)
                {
                    num /= currentLevel - 1;
                }
                double num2 = Math.Floor(num * Math.Pow(10.0, 1)) / Math.Pow(10.0, 1);
                this.saved = $"{healthMax}HP ({num2}E)";
                componentInChildren.text = this.saved;
                //gameObject.SetActive(dweller != null && !cantSendOut);
                //gameObject.SetActive(false);
            }

        }
        [Hook("DwellerInfoWindow::ShowPetInfo()")]
        public void Hook_HideLabel(CallContext context)
        {
            GameObject gameObject = this.FindButton((DwellerInfoWindow)context.This);
            if (gameObject != null)
            {
                UILabel componentInChildren = gameObject.GetComponentInChildren<UILabel>();
                //this.saved = componentInChildren.text;
                componentInChildren.text = "";
            }
        }
        [Hook("DwellerInfoWindow::HidePetInfo()")]
        public void Hook_ShowLabel(CallContext context)
        {
            GameObject gameObject = this.FindButton((DwellerInfoWindow)context.This);
            if (gameObject != null)
            {
                UILabel componentInChildren = gameObject.GetComponentInChildren<UILabel>();
                componentInChildren.text =this.saved;
            }
        }
    }
}
