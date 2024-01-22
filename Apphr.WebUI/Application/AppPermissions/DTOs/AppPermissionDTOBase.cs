using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.WebUI.Application.AppPermissions.DTOs
{
	public class AppPermissionDTOBase
	{
		[Required]
		[Display(Name = "Id")]
		public int PermissionId { get; set; }

		[Required, StringLength(100)]
		[Display(Name = "Nombre")]		
		public String Name { get; set; }
	}
}
