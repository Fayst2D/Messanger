using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.BusinessLogic.Models
{
    public class TokenPair
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
