using SONEPAR.SAPMIGRATION.WSS.HELPER;
using System;

namespace SONEPAR.SAPMIGRATION.WSS.EXCEPTION
{
    public class CustomException : Exception
    {
        public ApplicationErrorType ApplicationErrorType { get; set; }

        public CustomException(ApplicationErrorType type, Exception inner, String message = null) : base((message ?? inner.Message), inner) { ApplicationErrorType = type; }
    }
}
