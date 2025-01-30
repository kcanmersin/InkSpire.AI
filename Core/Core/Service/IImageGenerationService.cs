using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkSpire.Application.Abstractions
{
    public interface IImageGenerationService
    {
        Task<byte[]> GenerateImageAsync(string prompt);

    }
}
