using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float delayBeforeCounting;
        [SerializeField] private UnityEvent<string> updatePprogress;
        [SerializeField] private float minProgressDelay;
        [SerializeField] private float maxProgressDelay;
        [SerializeField] private float delayBeforeClose;
        private static int OpenGalleryAnimationHash = Animator.StringToHash("openGallery");
        private static int CloseAnimationHash = Animator.StringToHash("close");

        public void OpenGallery()
        {
            animator.SetTrigger(OpenGalleryAnimationHash);
            StartCoroutine(ProgressCount());
        }

        private IEnumerator ProgressCount()
        {
            var progress = 0;
            updatePprogress.Invoke($"{++progress}%");
            yield return new WaitForSeconds(delayBeforeCounting);
            var loadSceneAsync = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            loadSceneAsync.allowSceneActivation = false;
            while (progress < 100 || loadSceneAsync.progress < 0.9f)
            {
                updatePprogress.Invoke($"{++progress}%");
                var delay = Random.Range(minProgressDelay, maxProgressDelay);
                yield return new WaitForSeconds(delay);
            }

            animator.SetTrigger(CloseAnimationHash);
            yield return new WaitForSeconds(delayBeforeClose);
            loadSceneAsync.allowSceneActivation = true;
        }
    }
}