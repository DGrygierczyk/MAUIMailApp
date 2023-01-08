using CommunityToolkit.Mvvm.ComponentModel;
using MailApp.Model;
using MimeKit;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using MailApp.View;

namespace MailApp.ViewModel;

[QueryProperty(nameof(EmailDetails), "EmailDetails")]
public partial class EmailDetailsPageViewModel : BaseViewModel
{
    private EmailBody _emailDetails;
    public EmailBody EmailDetails
    {
        get { return _emailDetails; }
        set
        {
            _emailDetails = value;
            AttachmentNames = GetAttachmentNames(_emailDetails);
            OnPropertyChanged();
        }
    }

    private List<string> _attachmentNames;
    public List<string> AttachmentNames
    {
        get { return _attachmentNames; }
        set
        {
            _attachmentNames = value;
            OnPropertyChanged();
        }
    }

    public EmailDetailsPageViewModel()
    {
    }

    private List<string> GetAttachmentNames(EmailBody email)
    {
        List<string> attachmentNames = new List<string>();
        foreach (var attachment in email.Attachments)
        {
            attachmentNames.Add(attachment.ContentDisposition.FileName);
        }
        return attachmentNames;
    }

    [ICommand]
    public async Task SaveFileAsync(string filename)
    {
        var path = FileSystem.Current.AppDataDirectory;
            var fullPath = Path.Combine(path, filename);
            var fileStream = File.Create(fullPath);

            foreach (var attachment in EmailDetails.Attachments.Where(
                         att => att.ContentDisposition.FileName == filename))
            {
                if (attachment is MimePart)
                {
                    await ((MimePart)attachment).Content.DecodeToAsync(fileStream);
                }
            }

            await fileStream.DisposeAsync();
    }
    [ICommand]
    public async Task ReplayAsync(EmailBody emailDetails)
    {
        await Shell.Current.GoToAsync($"{nameof(CreateEmailPage)}", true, new Dictionary<string, object>
        {
            { "EmailDetails", emailDetails }
        });
    }
}