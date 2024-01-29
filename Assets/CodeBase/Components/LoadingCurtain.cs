using System.Collections;
using UnityEngine;

namespace CodeBase.Logic
{
    /// <summary>
    /// Loading curtain, hiding mainly level loading
    /// </summary>
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
        }

        public void Hide() => StartCoroutine(FadeOut());

        private IEnumerator FadeOut()
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= 0.03f;
                yield return new WaitForSeconds(0.03f);
            }
        
            gameObject.SetActive(false); 
        }
    
    
    }
}