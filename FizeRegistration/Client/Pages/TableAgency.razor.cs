using AntDesign;
using AntDesign.TableModels;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using System.Threading;

namespace FizeRegistration.Client.Pages
{
    public partial class TableAgency
    {
        [Inject] private IFizeHttpService HttpClient { get; set; }
        [Inject] private NavigationManager Navigate { get; set; }
        public List<AgencyDataContract> AgencyInformation { get; set; } = new List<AgencyDataContract>();
        private TableFilterContract tableFilterContract = new TableFilterContract();
        [Parameter] public string TextValidate { get; set; }
        public List<string> AgencyChangeNames { get; set; } = new List<string>();
        private bool isLoading;
        private List<IBrowserFile> loadedFiles = new();
        private int maxAllowedFiles = 3;
        private string extensionname = "default";
        private string base64data = "";
        private ITable table;
        private async Task OnLinkPictureFilesChange(InputFileChangeEventArgs e)
        {
            loadedFiles.Clear();
            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                try
                {
                    loadedFiles.Add(file);

                    extensionname = Path.GetExtension(file.Name);

                    var imagefiletypes = new List<string>() {
                    ".png",".jpg",".jpeg"
                };
                    if (imagefiletypes.Contains(extensionname))
                    {
                        var resizedFile = await file.RequestImageFileAsync(file.ContentType, 640, 480);
                        var buf = new byte[resizedFile.Size];
                        using (var stream = resizedFile.OpenReadStream())
                        {
                            await stream.ReadAsync(buf);
                        }
                        base64data = "data:image/png;base64," + Convert.ToBase64String(buf);
                        //сюда треба відправку на файз
                        //_agencyInformation.Picture = e.File;
                        isLoading = true;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await GetAgency();
        }
        private void Update(int id)
        {
            Navigate.NavigateTo($"/app/agency/change/{id}");
        }
        private void CreateAgency()
        {
            Navigate.NavigateTo($"/app/agency/new");
        }

        private async Task GetAgency()
        {
            var responce = await HttpClient.GetAgency();
            var stringAgency = responce.Message;
            AgencyInformation = JsonConvert.DeserializeObject<List<AgencyDataContract>>(stringAgency);
        }
        private async Task Delete(string agencionId)
        {
            var formDataLogo = new MultipartFormDataContent();
            var fileLogo = new StringContent(agencionId);
            formDataLogo.Add(fileLogo, "agencyId");
            await HttpClient.DeleteAgency(formDataLogo);
            await GetAgency();
        }

        public async Task DeleteListAgency()
        {
            var listDeleteAgencion = AgencyInformation.Where(e => e.IsDelete).ToList();
            var formDataLogo = new MultipartFormDataContent();
            var tableFilter = JsonConvert.SerializeObject(listDeleteAgencion);
            var fileLogo = new StringContent(tableFilter);
            formDataLogo.Add(fileLogo, "deleteList");
            await HttpClient.DeleteListAgency(formDataLogo);
            await GetAgency();
        }

        void OnRowClick(AgencyDataContract row)
        {
            if (row.IsDelete)
            {
                row.IsDelete = false;
                row.BackgroundColor = null;
            }
            else
            {
                row.IsDelete = true;
                row.BackgroundColor = "background:#e6f7ff";
            }
        }

        private async Task OnTextValidateChanged(ChangeEventArgs e)
        {
            TextValidate = e.Value.ToString();
            if (!string.IsNullOrEmpty(TextValidate))
            {
                tableFilterContract.ImputText = TextValidate;
                var formDataLogo = new MultipartFormDataContent();

                var tableFilter = JsonConvert.SerializeObject(tableFilterContract);

                var stringContentAgency = new StringContent(tableFilter);
                formDataLogo.Add(stringContentAgency, "tableFilterContract");
                var responce = await HttpClient.GetFilterAgency(formDataLogo);
                var stringAgency = responce.Message;
                AgencyInformation = JsonConvert.DeserializeObject<List<AgencyDataContract>>(stringAgency);
            }
            else
            {
                var responce = await HttpClient.GetAgency();
                var stringAgency = responce.Message;
                AgencyInformation = JsonConvert.DeserializeObject<List<AgencyDataContract>>(stringAgency);
            }
        }
    }
}
