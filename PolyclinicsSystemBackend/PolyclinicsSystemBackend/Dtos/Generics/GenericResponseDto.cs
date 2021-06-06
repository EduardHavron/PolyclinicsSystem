using System.Collections.Generic;

namespace PolyclinicsSystemBackend.Dtos.Generics
{
    public class GenerisResult<T, TE>
    {
        public bool IsSuccess { get; set; }
        
        public IEnumerable<T>? Errors { get; set; }
        
        public TE? Result { get; set; }
    }
}