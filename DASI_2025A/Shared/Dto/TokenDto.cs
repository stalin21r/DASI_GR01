using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class TokenDto
    {
        public string AccessToken { get; set; } = "";

        public DateTime Expires{ get; set; }
    }
}
