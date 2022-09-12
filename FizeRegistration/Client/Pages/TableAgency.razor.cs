using AntDesign;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace FizeRegistration.Client.Pages
{
    public partial class TableAgency
    {
        [Inject] IFizeHttpService HttpClient { get; set; }
        [Inject] NavigationManager Navigate { get; set; }
        public List<AgencyDataContract> AgencyInformation { get; set; } = new List<AgencyDataContract>();
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
    }
}
