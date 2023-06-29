using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common
{
    public class AnimatedButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] [NotNull] private Animator animator;
        [SerializeField] private UnityEvent onClick;
        [SerializeField] private bool interactableAfterPress;
        private bool interactable = true;
        private static int PressedAnimationHash = Animator.StringToHash("pressed");

        private void OnEnable()
        {
            interactable = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (interactable)
            {
                animator.SetBool(PressedAnimationHash, true);
                interactable = interactableAfterPress;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            animator.SetBool(PressedAnimationHash, false);
        }

        public void OnAnimationComplete()
        {
            onClick.Invoke();
        }
    }
}