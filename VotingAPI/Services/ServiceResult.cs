﻿using System.Text.Json.Serialization;

namespace VotingAPI.Services
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }
    }
}
