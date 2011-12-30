using System.Collections.Generic;
using System.Linq;

namespace ToBeSeen.Plumbing
{
	public class Page<T>
	{
		public Page(IEnumerable<T> items, int pageNumber, int totalItemsCount, int pageSize)
		{
			Items = items.ToArray();
			PageNumber = pageNumber;
			TotalItemsCount = totalItemsCount;
			PageSize = pageSize;
		}

		public T[] Items { get; private set; }
		public int PageNumber { get; private set; }
		public int PageSize { get; private set; }
		public int TotalItemsCount { get; private set; }
	}
}