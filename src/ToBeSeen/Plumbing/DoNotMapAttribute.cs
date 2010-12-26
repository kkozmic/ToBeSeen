using System;

namespace ToBeSeen.Plumbing
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class DoNotMapAttribute : Attribute
	{
	}
}