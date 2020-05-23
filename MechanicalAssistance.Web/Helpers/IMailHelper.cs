using MechanicalAssistance.Common.Models;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body);

        Response SendMultipleEmails(InternetAddressList to, string subject, string body);
    }
}
