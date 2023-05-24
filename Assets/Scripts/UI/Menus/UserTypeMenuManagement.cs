using Managers;
using UnityEngine;
using Utilities;

namespace UI.Menus
{
    public class UserTypeMenuManagement : MonoBehaviour
    {
        private AudioManagement AudioManagement { get; set; }
        private GameObject PrimaryMenuGameObject { get; set; }
        private GameObject PlayerLoginMenuGameObject { get; set; }
        private GameObject AdminMenuGameObject { get; set; }
        private AdminMenu AdminMenu { get; set; }

        private void Awake()
        {
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>("Interface/MainCamera/Audio/Sounds");
            PrimaryMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PrimaryMenu");
            PlayerLoginMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PrimaryMenu");
            AdminMenuGameObject = Utils.GetGameObjectOrThrow("Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu");
            AdminMenu = Utils.GetComponentOrThrow<AdminMenu>("Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu"); 
        }

        public void SwitchToPlayerLoginMenu()
        {
            AudioManagement.PlayOneShot("ButtonSound");
            
            PrimaryMenuGameObject.SetActive(false);
            PlayerLoginMenuGameObject.SetActive(true);
        }
    
        public void SwitchToAdminMenu()
        {
            AudioManagement.PlayOneShot("ButtonSound");
            
            PrimaryMenuGameObject.SetActive(false);
            AdminMenuGameObject.SetActive(true);
        }

        public void QuitGame()
        {
            AudioManagement.PlayOneShot("ButtonSound");
            
            Application.Quit(0);
        }
    }
}
