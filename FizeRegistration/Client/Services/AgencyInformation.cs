using FizeRegistration.Client.Helpers.Validation;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FizeRegistration.Client.Services
{
    public sealed class AgencyInformation
    {
        [Required]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Color { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string AgencyName { get; set; }

        [Required]
        [RegularExpression(@"^(http|http(s)?://)?([\w-]+\.)+[\w-]+[.com|.in|.org]+(\[\?%&=]*)?", ErrorMessage = "Characters are not allowed")]
        public string WebSite { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string LastName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Link { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$", ErrorMessage = "Characters are not allowed")]
        public string PhoneNumbers { get; set; }

        [Required]
        [MaxFileSize(3 * 1024 * 1024)]
        public IBrowserFile Logo { get; set; }
        [Required]
        [MaxFileSize(3 * 1024 * 1024)]
        public IBrowserFile Picture { get; set; }
    }

}
