using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace FizeRegistration.Client.Shared
{
    public partial class ProperyColumnAnt
    {
        [Inject] private IFizeHttpService HttpClient { get; set; }

        [Parameter] public string ColumnName { get; set; }
        [Parameter] public string TableName { get; set; }


        [Parameter] public EventCallback<string> ColumnNameChanged { get; set; }

        [Parameter] public string Id { get; set; }


        [Parameter] public AgencyDataContract AgencyInformation { get; set; } = new AgencyDataContract();

        private string editId;
        private List<string> AgencyChangeNames { get; set; } = new List<string>();



        async Task OnClick(string agencyName)
        {
            ChangeValueTableContract changeValue = new ChangeValueTableContract();

            ColumnName = agencyName;
            changeValue.ColumnName = TableName;
            changeValue.ValueInTable = ColumnName;
            changeValue.Id = AgencyInformation.Id;
            var formDataLogo = new MultipartFormDataContent();

            var tableFilter = JsonConvert.SerializeObject(changeValue);

            var stringContentAgency = new StringContent(tableFilter);
            formDataLogo.Add(stringContentAgency, "changeContract");
            var responce = await HttpClient.ChangeColumnValue(formDataLogo);
        }

        void startEdit(string id, string agencyName)
        {
            editId = id;
            if (!AgencyChangeNames.Any())
            {
                AgencyChangeNames.Add(agencyName);
            }
        }

        void stopEdit()
        {
            editId = null;
            foreach (var agency in AgencyChangeNames)
            {
                if (agency.Contains(ColumnName))
                {
                    return;
                }
            }

            if (ColumnName != null)
            {
                AgencyChangeNames.Add(ColumnName);
            }

        }

        private  Task OnColumnNameChanged(ChangeEventArgs e)
        {

            ChangeValueTableContract change = new ChangeValueTableContract();
            ColumnName = e.Value.ToString();
            change.ColumnName = TableName;
            change.ValueInTable = ColumnName;
            change.Id = AgencyInformation.Id;
            var formDataLogo = new MultipartFormDataContent();

            var tableFilter = JsonConvert.SerializeObject(change);

            var stringContentAgency = new StringContent(tableFilter);
            formDataLogo.Add(stringContentAgency, "changeContract");
            var responce = HttpClient.ChangeColumnValue(formDataLogo);
            return ColumnNameChanged.InvokeAsync(ColumnName);

        }
    }
}
