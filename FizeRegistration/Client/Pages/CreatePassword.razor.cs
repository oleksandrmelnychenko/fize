namespace FizeRegistration.Client.Pages
{
    public partial class CreatePassword
    {
        private string Password;
        private string ConfirmPassword;

        public async Task SetPassword()
        {
            if (Password == ConfirmPassword)
            {
                //Some Logic
            }
        }
    }
}
