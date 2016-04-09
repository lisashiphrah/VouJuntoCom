using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VouJuntoCom.Helpers
{
	public enum NotificationsEnum
	{
		[DescriptionAttribute(" aceitou sua solicitação de amizade.")]
		FriendAccepted,

		[DescriptionAttribute(" aceitou sua carona para ")]
		RideAccepted,

		[DescriptionAttribute(" cancelou a carona para ")]
		RideCancelled
	}
}