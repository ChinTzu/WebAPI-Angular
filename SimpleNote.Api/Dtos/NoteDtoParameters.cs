using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleNote.Api.Dtos
{
    public class NoteDtoParameters
    {
        public string Username { get; set; }
        public string Search { get; set; }

        private const int MaxPageSize = 20; 
        private int _pageSize = 5; 
        public int PageNumber { get; set; } = 1; 

        public string OrderBy { get; set; } = "Id";
        public string Fields { get; set; } 

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
