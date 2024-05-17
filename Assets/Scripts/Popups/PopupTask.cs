using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class PopupTask : ITask, ITaskFinish
{
    public string Name { get; }
    private readonly IPopupModel _popupModel;
    private readonly Transform _parent;
    private readonly TaskQueue _taskQueue;
    private readonly List<ITask> _rightButtonTask;
    private readonly List<ITask> _leftButtonTask;

    private PopupViewBase _popupView;
    private UniTaskCompletionSource _completionSource = new UniTaskCompletionSource();

    public PopupTask(string name, IPopupModel popupModel, Transform parent, TaskQueue taskQueue, List<ITask> rightButtonTask = null, List<ITask> leftButtonTask = null)
    {
        Name = name;
        _popupModel = popupModel;
        _parent = parent;
        _taskQueue = taskQueue;
        _rightButtonTask = rightButtonTask;
        _leftButtonTask = leftButtonTask;
    }

    public void Run()
    {
        _popupView = Resources.Load<GameObject>(Name).GetComponent<PopupViewBase>();
        _popupView = PopupViewBase.Instantiate(_popupView, _parent);
        _popupView.transform.localScale = Vector3.one;
        _popupView.transform.localPosition = Vector3.zero;
        _popupView.SetFinishTracker(this);
        _popupView.SetRightButtonText(_popupModel.RightButton);
        _popupView.SetLeftButtonText(_popupModel.LeftButton);
        _popupView.SetTitle(_popupModel.Title);
        _popupView.SetDescription(_popupModel.Description);
        _popupView.OnLeftButtonClicked += EnqueueLeftButtonTask;
        _popupView.OnRightButtonClicked += EnqueueRightButtonTask;
    }

    public UniTask WaitComplete()
    {
        return _completionSource.Task;
    }

    public void CompleteTask()
    {
        _popupView.OnLeftButtonClicked -= EnqueueLeftButtonTask;
        _popupView.OnRightButtonClicked -= EnqueueRightButtonTask;
        _completionSource.TrySetResult();
        GameObject.Destroy(_popupView.gameObject); 
    }

    private void EnqueueRightButtonTask()
    {
        EnqueueTasks(_rightButtonTask);
        CompleteTask();
    }

    private void EnqueueLeftButtonTask()
    {
        EnqueueTasks(_leftButtonTask);
        CompleteTask();
    }

    private void EnqueueTasks(List<ITask> tasksToEnqueue)
    {
        if (tasksToEnqueue == null)
        {
            return;
        }

        for (int i = tasksToEnqueue.Count - 1; i >= 0; i--)
        {
            _taskQueue.InsertInFront(tasksToEnqueue[i]);
        }
    }
}