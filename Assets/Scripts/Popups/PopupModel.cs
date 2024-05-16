public class PopupModel : IPopupModel
{
    public string RightButton { get; private set; }
    public string LeftButton { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }

    public PopupModel(string rightButton, string leftButton, string title, string description)
    {
        LeftButton = leftButton;
        RightButton = rightButton;
        Title = title;
        Description = description;
    }
}