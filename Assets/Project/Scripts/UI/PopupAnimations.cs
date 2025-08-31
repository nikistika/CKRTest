using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class PopupAnimations : MonoBehaviour
    {
        [SerializeField] private GameObject popupPanel;

        public void ShowPopupWithAnimation()
        {
            popupPanel.SetActive(true);
            popupPanel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }

        public void HidePopupWithAnimation()
        {
            popupPanel.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnKill(() => popupPanel.SetActive(false));
        }
    }
}