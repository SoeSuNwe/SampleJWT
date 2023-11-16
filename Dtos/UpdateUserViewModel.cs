using System.ComponentModel.DataAnnotations;

namespace SampleJWT.Dtos
{
    public class UpdateUserViewModel
    {
        [EmailAddress]
        public string Email { get; set; }

        // Add other properties you want to update
    }
}
