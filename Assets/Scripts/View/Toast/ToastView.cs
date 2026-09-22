using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Events;
using TMPro;

namespace Game.Views
{
    /// <summary>
    /// 屏幕底部弹出提示。同一时刻只显示一条最新的，
    /// 多条事件相互覆盖（够用且简洁）。
    /// </summary>
    public class ToastView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI toastText;
        [SerializeField] private float showDuration = 1f;
        private Coroutine _routine;
        private void OnEnable()
        {
            EventBus.Subscribe<UiToastEvent>(OnToast);
            if (toastText != null) toastText.gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            EventBus.Unsubscribe<UiToastEvent>(OnToast);
        }
        private void OnToast(UiToastEvent e)
        {
            if (toastText == null) return;
            if (_routine != null) StopCoroutine(_routine);
            _routine = StartCoroutine(ShowRoutine(e.Message));
        }
        IEnumerator ShowRoutine(string message)
        {
            toastText.text = message;
            toastText.gameObject.SetActive(true);
            yield return new WaitForSeconds(showDuration);
            toastText.gameObject.SetActive(false);
            _routine = null;
        }
    }
}
