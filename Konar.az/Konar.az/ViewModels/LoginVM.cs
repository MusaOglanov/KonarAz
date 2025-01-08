using Konar.az.Models;
using System.ComponentModel.DataAnnotations;

namespace Konar.az.ViewModels
{
	public class LoginVM
	{
       
		public string Username { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public bool IsRemember { get; set; }
        public BackPhoto BackPhoto { get; set; }

    }
}
