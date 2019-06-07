using SONEPAR.SAPMIGRATION.WSS.HELPER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SONEPAR.SAPMIGRATION.WSS.EXCEPTION
{
    public class MailException : Exception
    {
        public ApplicationErrorType ApplicationErrorType { get; set; }

        public MailException(ApplicationErrorType type, Exception inner, String message = null) : base((message ?? inner.Message), inner) { ApplicationErrorType = type; }
    }
}
