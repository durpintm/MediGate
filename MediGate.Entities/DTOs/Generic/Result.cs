using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediGate.Entities.DTOs.Errors;

namespace MediGate.Entities.DTOs.Generic
{
    public class Result<T> // Single item return
    {
        public T Content { get; set; }
        public Error Error { get; set; }

        public bool IsSuccess => Error == null;
        public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
    }
}