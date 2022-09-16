using AntDesign;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace FizeRegistration.Client.Pages
{
    public partial class TableAgency
    {

        private static string[] param = new string[] { "Id", "AgencyName", "Color", "FirstName", "LastName", "Link", "WebSite", };

        [Inject] IFizeHttpService HttpClient { get; set; }
        [Inject] NavigationManager Navigate { get; set; }
        public List<AgencyDataContract> AgencyInformation { get; set; } = new List<AgencyDataContract>();

        private TableFilterContract tableFilterContract = new TableFilterContract();
        ITable table;
        protected override async Task OnInitializedAsync()
        {

            var responce = await HttpClient.GetAgency();
            var stringAgency = responce.Message;
            AgencyInformation = JsonConvert.DeserializeObject<List<AgencyDataContract>>(stringAgency);

        }
        private void Update(int id)
        {
            Navigate.NavigateTo($"/app/agency/change/{id}");
        }
        private void CreateAgency()
        {
            Navigate.NavigateTo($"/app/agency/new");

        }

        private void SetFiltre(string filtre)
        {
            tableFilterContract.ColumnName = filtre;
           
        }

        private async Task OnTextValidateChanged(ChangeEventArgs e)
        {
            TextValidate = e.Value.ToString();
            //return TextValidateChanged.InvokeAsync(TextValidate);
            if (!string.IsNullOrEmpty(TextValidate) && tableFilterContract.ColumnName != null)
            {
                tableFilterContract.ImputText = TextValidate;
                var formDataLogo = new MultipartFormDataContent();
                //var output = new MemoryStream();
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
        public async Task IsVisible()
        {
         
            if (!string.IsNullOrEmpty(Filter) && tableFilterContract.ColumnName!= null)
            {
                tableFilterContract.ImputText = Filter;
                var formDataLogo = new MultipartFormDataContent();
                //var output = new MemoryStream();
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
