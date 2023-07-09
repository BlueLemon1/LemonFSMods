using FSLoader;
using I2.Loc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LemonMods
{
    [ModInfo("Lemon_Send_Outside","Send Outside Button","Lemon",1,0)]
    internal class LemonSendOutsideBtn : Mod
    {
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
            if (gameObject.FindChild("ModBtn_SendOutside") == null)
            {
                VaultGUIManager instance = MonoSingleton<VaultGUIManager>.Instance;
                if (instance == null)
                {
                    return;
                }
                GameSettingsWindow gameSettingsWindow = instance.m_gameSettingsWindow;
                if (gameSettingsWindow == null)
                {
                    return;
                }
                Transform transform2 = gameSettingsWindow.gameObject.FindChild("BTN Close");
                if (transform2 == null)
                {
                    return;
                }
                GameObject gameObject2 = transform2.gameObject;
                GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject2);
                gameObject3.name = "ModBtn_SendOutside";
                gameObject3.transform.parent = gameObject.transform;
                gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
                gameObject3.transform.localPosition = new Vector3(75f, -245f, 1f);
                UIButton component = gameObject3.GetComponent<UIButton>();
                component.onClick.Clear();
                component.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.SendDwellerOutside)));
                UILabel componentInChildren = gameObject3.GetComponentInChildren<UILabel>();
                componentInChildren.text = "OUTSIDE";

            }
        }

        private GameObject FindButton(DwellerInfoWindow window)
        {
            GameObject gameObject = window.gameObject.FindChild("DwellerInfoPanel").gameObject;
            Transform transform = gameObject.FindChild("ModBtn_SendOutside");
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
                bool cantSendOut = MonoSingleton<Vault>.Instance.EmergencyState.InEmergency || dweller.CurrentState.Type == EDwellerState.WaitingApproval || dweller.Pregnant || !dweller.IsInVault || dweller.IsChild;
                gameObject.SetActive(dweller != null && !cantSendOut);
                //gameObject.SetActive(false);
            }
        }

        private void SendDwellerOutside()
        {
            //MonoSingleton<VaultGUIManager>.Instance.m_messageOneButton.ShowMessage("111");
            DwellerInfoWindow dwellerInfoWindow = MonoSingleton<VaultGUIManager>.Instance.m_DwellerInfoWindow;
            GameObject gameObject = this.FindButton(dwellerInfoWindow);
            Dweller currentDweller = dwellerInfoWindow.CurrentDweller;

            if (MonoSingleton<Vault>.Instance.EmergencyState.InEmergency)
            {
                return;
            }
            if (!currentDweller.CanDoAction(EDwellerAction.BeSentToWasteland)){
                MonoSingleton<VaultGUIManager>.Instance.m_messageOneButton.ShowMessage(ScriptLocalization.Get("Wasteland_Cant_Send", false),null);
                return;
            }
            MonoSingleton<VaultGUIManager>.Instance.m_wastelandEquipmentWindow.Show(currentDweller);

            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
            //dwellerInfoWindow.FillData(currentDweller);
            //dwellerInfoWindow.Show();
        }

    }
}
