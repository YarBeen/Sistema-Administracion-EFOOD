using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Servicios
{
    public  interface IStorageService

    {
        Task<string> UploadImageAsync(Stream imageStream, string containerName, string folderName, string fileName);
        
    }
}
