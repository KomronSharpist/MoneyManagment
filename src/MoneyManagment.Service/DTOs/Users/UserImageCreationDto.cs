using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagment.Service.DTOs.Users;

public class UserImageCreationDto
{
    public IFormFile Image { get; set; }
}
