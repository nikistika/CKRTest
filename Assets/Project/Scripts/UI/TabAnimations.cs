using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class TabAnimations : MonoBehaviour
    {
        [SerializeField] private GameObject weatherTab;
        [SerializeField] private GameObject factsTab;

        public void SwitchToWeatherTab()
        {
            weatherTab.SetActive(true);
            factsTab.SetActive(false);
            
            var localPosition = weatherTab.transform.localPosition;
            localPosition = new Vector3(100, localPosition.y, localPosition.z);
            weatherTab.transform.localPosition = localPosition;
            weatherTab.transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutQuad);
        }

        public void SwitchToFactsTab()
        {
            factsTab.SetActive(true);
            weatherTab.SetActive(false);
            
            var localPosition = weatherTab.transform.localPosition;
            factsTab.transform.localPosition = new Vector3(100, localPosition.y, localPosition.z);
            factsTab.transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutQuad);
        }
    }
}