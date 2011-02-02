using System.Web.Mvc;
using Castle.Core.Logging;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using ToBeSeen.Services;
using DotNetOpenAuth.Messaging;

namespace ToBeSeen.Controllers
{
	[HandleError]
	public class AccountController : Controller
	{
		public virtual IFormsAuthenticationService FormsService { get; set; }

		public virtual ILogger Logger { get; set; }
		
		public virtual ActionResult LogOn()
		{
			return View();
		}

		public virtual ActionResult Authenticate(string openid_provider)
		{
			var openid = new OpenIdRelyingParty();
			var response = openid.GetResponse();

			Logger.InfoFormat("Trying to log-on user using OpenId Identifier: {0} .", openid_provider);

			if (response == null)
			{
				var request = openid.CreateRequest(openid_provider);

				var claim = new ClaimsRequest
				{
					Email = DemandLevel.Require,
					Nickname = DemandLevel.Require,
					FullName = DemandLevel.Request,
				};

				var fetch = new FetchRequest();
				fetch.Attributes.AddRequired(WellKnownAttributes.Name.First);
				fetch.Attributes.AddRequired(WellKnownAttributes.Name.Last);

				request.AddExtension(claim);
				request.AddExtension(fetch);

				return request.RedirectingResponse.AsActionResult();
			}
			
			if (response.Status == AuthenticationStatus.Authenticated)
			{
				var claim = response.GetExtension<ClaimsResponse>();
				var fetch = response.GetExtension<FetchResponse>();
				var nick = response.FriendlyIdentifierForDisplay;
				var email = string.Empty;

				if (claim != null)
				{
					nick = string.IsNullOrEmpty(claim.Nickname) ? claim.FullName : claim.Nickname;
					email = claim.Email;
				}

				if (string.IsNullOrEmpty(nick) && fetch != null && 
					fetch.Attributes.Contains(WellKnownAttributes.Name.First) && 
					fetch.Attributes.Contains(WellKnownAttributes.Name.Last))
				{
					nick = fetch.GetAttributeValue(WellKnownAttributes.Name.First) + " " +
						   fetch.GetAttributeValue(WellKnownAttributes.Name.Last);
				}

				var user = string.Format("{0} <{1}>", nick, email);

				FormsService.SignIn(user);

				Logger.InfoFormat("User {0} authenticated successfully.", user);

				return RedirectToAction("Index", "Home");
			}

			Logger.Warn("User attempted login but password validation failed.");
			ModelState.AddModelError("", "Authentication failed.");

			return View("LogOn");
		}

		public virtual ActionResult LogOff()
		{
			FormsService.SignOut();

			return RedirectToAction("Index", "Home");
		}
	}
}
