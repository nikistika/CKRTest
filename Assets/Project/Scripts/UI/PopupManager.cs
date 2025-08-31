using Project.Scripts.Core;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class PopupManager : MonoBehaviour
    {
        [SerializeField] private GameObject popupPanel;
        [SerializeField] private TextMeshProUGUI popupTitle;
        [SerializeField] private TextMeshProUGUI popupDescription;

        public void ShowPopup(FactDetail factDetail)
        {
            popupPanel.SetActive(true);
            popupTitle.text = factDetail.Title;
            popupDescription.text = factDetail.Description;
        }

        public void HidePopup()
        {
            popupPanel.SetActive(false);
        }
    }
}