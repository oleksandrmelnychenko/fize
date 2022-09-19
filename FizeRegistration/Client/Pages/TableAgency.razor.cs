using AntDesign;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace FizeRegistration.Client.Pages
{
    public partial class TableAgency
    {

        [Inject] private IFizeHttpService HttpClient { get; set; }
        [Inject] private NavigationManager Navigate { get; set; }
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
