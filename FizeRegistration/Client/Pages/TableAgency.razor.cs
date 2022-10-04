using AntDesign;
using AntDesign.TableModels;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using System.Threading;

namespace FizeRegistration.Client.Pages
{
    public partial class TableAgency
    {
        [Parameter] public string AgencyNameValidate { get; set; }
        [Inject] private IFizeHttpService HttpClient { get; set; }
        [Inject] private NavigationManager Navigate { get; set; }
        public List<AgencyDataContract> AgencyInformation { get; set; } = new List<AgencyDataContract>();

        private TableFilterContract tableFilterContract = new TableFilterContract();
        ITable table;
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
        void stopEdit()
        {
            var editedData = AgencyInformation.FirstOrDefault(x => x.Id == editId);
            if (AgencyNameValidate != null)
            {
                AgencyChangeNames.Add(AgencyNameValidate);
                AgencyNameValidate = null;
            }

            //Console.WriteLine(JsonSerializer.Serialize(editedData));
            editId = null;
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
            var listDeleteAgencion =  AgencyInformation.Where(e => e.IsDelete).ToList();
            var formDataLogo = new MultipartFormDataContent();
            var tableFilter = JsonConvert.SerializeObject(listDeleteAgencion);
            var fileLogo = new StringContent(tableFilter);
            formDataLogo.Add(fileLogo, "deleteList");
            await HttpClient.DeleteListAgency(formDataLogo);
            await GetAgency();
        }

        void OnClick(string agencyName,AgencyDataContract agency)
        {
             //_vievAgencyTable = agencyName;
            agency.AgencyName = agencyName;
        }

        void OnBlur()
        {

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


        void startEdit(string id, string agencyName)
        {
            editId = id;
            if (!AgencyChangeNames.Any())
            {
                AgencyChangeNames.Add(agencyName);
            }
            
        }

        private async Task OnAgencyNameValidateChanged(ChangeEventArgs e)
        {
            AgencyNameValidate = e.Value.ToString();
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
