using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMediaClient.Models
{
	public class SettingViewModel
	{
		public int ID { get; set; }
		public int OwnerID { get; set; }
		[Required]
		[Display(ResourceType = typeof(Resources), Name = "OwnerType")]
		public SettingType OwnerType { get; set; }
		[Required]
		[Display(ResourceType = typeof(Resources), Name = "Publicity")]
		public PublicityType Publicity { get; set; }
	}
}
