﻿using System.Net;

namespace HotelComercy_WebAPI.Model
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSucess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}
