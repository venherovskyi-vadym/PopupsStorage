using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupViewBase : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private TextMeshProUGUI _rightButtonText;
    [SerializeField] private TextMeshProUGUI _leftButtonText;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    private ITaskFinish _finishTracker;

    public event Action OnLeftButtonClicked;
    public event Action OnRightButtonClicked;

    public void SetFinishTracker(ITaskFinish finishTracker)
    {
        _finishTracker = finishTracker;
    }

    public void SetRightButtonText(string rightButtonText)
    {
        _rightButton.gameObject.SetActive(!string.IsNullOrEmpty(rightButtonText));
        _rightButtonText.text = rightButtonText;
    }

    public void SetLeftButtonText(string leftButtonText)
    {
        _leftButton.gameObject.SetActive(!string.IsNullOrEmpty(leftButtonText));
        _leftButtonText.text = leftButtonText;
    }

    public void SetTitle(string title)
    {
        _titleText.text = title;
    }

    public void SetDescription(string description)
    {
        _descriptionText.text = description;
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(ClosePopup);
        _rightButton.onClick.AddListener(RightButtonClicked);
        _leftButton.onClick.AddListener(LeftButtonClicked);
    }

    private void RightButtonClicked() 
    {
        OnRightButtonClicked?.Invoke();
    }

    private void LeftButtonClicked()
    {
        OnLeftButtonClicked?.Invoke();
    }

    private void ClosePopup()
    {
        if (_finishTracker == null)
        {
            return;
        }

        _finishTracker.CompleteTask();
        _closeButton.onClick.RemoveListener(ClosePopup);
        _rightButton.onClick.RemoveListener(RightButtonClicked);
        _leftButton.onClick.RemoveListener(LeftButtonClicked);
    }

    private void OnDestroy() => ClosePopup();
}