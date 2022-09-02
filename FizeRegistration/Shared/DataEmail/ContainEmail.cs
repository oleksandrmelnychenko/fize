using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FizeRegistration.Shared.DataEmail
{
    public class ContainEmail
    {
        private string Email { get; set; } = string.Empty;


        public async void SetData(string email)
        {
            Email = email;
        }
        public async Task<string> GetData()
        {
            //if (!initialized)
            //{
            //    // emulate a async data get from a Db
            //    await Task.Delay(1000);
            //    Email = $"Got data at {DateTime.Now.ToLongTimeString()}";
            //    this.initialized = true;
            //}
            return Email;
        }
    }
}
