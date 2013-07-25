using System.Collections.Generic;

namespace AKQ.Domain.Infrastructure.Mvc
{
    public class PagedResult<T>
    {
        public PagedResult()
        {
            PagingInfo = new PagingInfo();
        }

        public List<T> Items { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
