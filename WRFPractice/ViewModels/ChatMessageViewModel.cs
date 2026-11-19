using System;
using CommunityToolkit.Mvvm.ComponentModel;
using WRFPractice.Models;

namespace WRFPractice.ViewModels;

public partial class ChatMessageViewModel : ObservableObject
{
    [ObservableProperty]
    private string _id = Guid.NewGuid().ToString();

    [ObservableProperty]
    private MessageRole _role;

    [ObservableProperty]
    private string _content = string.Empty;

    [ObservableProperty]
    private DateTime _timestamp = DateTime.Now;

    [ObservableProperty]
    private bool _isError;

    [ObservableProperty]
    private string? _errorMessage;

    public string FormattedTime => Timestamp.ToString("HH:mm");

    public ChatMessage ToModel() => new()
    {
        Id = Id,
        Role = Role,
        Content = Content,
        Timestamp = Timestamp,
        IsError = IsError,
        ErrorMessage = ErrorMessage
    };

    public static ChatMessageViewModel FromModel(ChatMessage model) => new()
    {
        Id = model.Id,
        Role = model.Role,
        Content = model.Content,
        Timestamp = model.Timestamp,
        IsError = model.IsError,
        ErrorMessage = model.ErrorMessage
    };
}
