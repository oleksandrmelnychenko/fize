using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;

namespace FizeRegistration.Client.Shared
{
    public partial class PropertyColumnFIle
    {

        [Inject] private IFizeHttpService HttpClient { get; set; }

        [Parameter] public string LinkPictureUser { get; set; }

        [Parameter] public string Id { get; set; }

        private string _newLinkPictureUser;
        private List<IBrowserFile> loadedFiles = new();
        private int maxAllowedFiles = 3;
        private string _extensionName = "default";
        private string _base64Data = "";

        private async Task OnLinkPictureFilesChange(InputFileChangeEventArgs e)
        {
            loadedFiles.Clear();
            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                try
                {
                    loadedFiles.Add(file);
                    _extensionName = Path.GetExtension(file.Name);

                    var imagefiletypes = new List<string>() {
                    ".png",".jpg",".jpeg"
                };
                    if (imagefiletypes.Contains(_extensionName))
                    {
                        var resizedFile = await file.RequestImageFileAsync(file.ContentType, 640, 480);
                        var buf = new byte[resizedFile.Size];
                        using (var stream = resizedFile.OpenReadStream())
                        {
                            await stream.ReadAsync(buf);
                        }
                        _base64Data = "data:image/png;base64," + Convert.ToBase64String(buf);

                        ChangeValueTableContract change = new ChangeValueTableContract();

                        change.ColumnName = "LinkPictureUser";
                        //change.ValueInTable = ColumnName;
                        change.Id = Id;
                        var formDataLogo = new MultipartFormDataContent();

                        var tableFilter = JsonConvert.SerializeObject(change);

                        var stringContentAgency = new StringContent(tableFilter);
                        formDataLogo.Add(stringContentAgency, "changeContract");

                        StreamContent filePictire = new StreamContent(e.File.OpenReadStream());

                        formDataLogo.Add(filePictire, "fileLogo", Guid.NewGuid().ToString());
                        var responce = await HttpClient.ChangeFile(formDataLogo);
                        _newLinkPictureUser = responce.Message;
                        //HttpClient.ChangeFile();
                        //сюда треба відправку на файз
                        //_agencyInformation.Picture = e.File;
                        //isLoading = true;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
