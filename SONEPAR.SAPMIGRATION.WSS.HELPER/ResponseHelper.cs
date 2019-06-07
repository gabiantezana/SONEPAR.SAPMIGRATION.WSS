using System;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace SONEPAR.SAPMIGRATION.WSS.HELPER
{

    public class ResponseHelper
    {
        public static ApplicationResponse CreateResponseSuccess(String message)
        {
            ApplicationResponse response = new ApplicationResponse()
            {
                ResponseCount = 1,
                ResponseStatus = ResponseStatus.Success,
                ResponseType = ResponseType.Message,
                Response = new HELPER.Response
                {
                    Code = 0,
                    Message = new HELPER.Message
                    {
                        Object = null,
                        Text = message ?? ConstantHelper.SuccessMessage
                    }
                }
            };
            return response;
        }

        public static ApplicationResponse CreateResponseSuccessWithObject(Object objectResponse)
        {
            ApplicationResponse response = new ApplicationResponse()
            {
                ResponseCount = 1,
                ResponseStatus = ResponseStatus.Success,
                ResponseType = ResponseType.Object,
                Response = new HELPER.Response
                {
                    Code = null,
                    Message = new HELPER.Message
                    {
                        Object = CreateObjectTag(objectResponse),
                        Text = null
                    }
                }
            };
            return response;
        }

        public static ApplicationResponse CreateResponseSuccessDocEntry(String docEntry)
        {
            ApplicationResponse response = new ApplicationResponse()
            {
                ResponseCount = 1,
                ResponseStatus = ResponseStatus.Success,
                ResponseType = ResponseType.Object,
                Response = new HELPER.Response
                {
                    Code = null,
                    Message = new HELPER.Message
                    {
                        Object = CreateTag(docEntry, "DocEntry"),
                        Text = null
                    }
                }
            };
            return response;
        }

        /*public static ApplicationResponse CreateResponseDocEntry(String docEntry)
        {
            try
            {
                ApplicationResponse response = new ApplicationResponse
                {
                    ResponseStatus = ResponseStatus.Success,
                    ResponseType = ResponseType.Message,
                    Response = new Response
                    {
                        Message = new Message
                        {
                            Object = CreateTag(docEntry, "DocEntry")
                        }
                    }
                };
                return response;
            }
            catch (Exception ex)
            {
                return GeneralError(ex);
            }
        }

        public static ApplicationResponse CreateResponseWithObject(Object model)
        {
            try
            {
                ApplicationResponse response = new ApplicationResponse
                {
                    ResponseStatus = ResponseStatus.Success,
                    ResponseType = ResponseType.Message,
                    Response = new Response
                    {
                        Message = new Message
                        {
                            Object = CreateObjectTag(model)
                        }

                    }
                };

                return response;
            }
            catch (Exception ex)
            {
                return GeneralError(ex);
            }

        }*/

        public static ApplicationResponse CreateResponseError(String errorMessage, Int32? codeException = null, Object objectToShow = null)
        {
            ApplicationResponse response = new ApplicationResponse
            {
                ResponseStatus = ResponseStatus.Error,
                ResponseType = ResponseType.Message,
                Response = new Response
                {
                    Code = codeException,
                    Message = new Message
                    {
                        Object = objectToShow!= null? CreateObjectTagJson(objectToShow) : null,
                        Text = errorMessage,
                    }
                }
            };
            return response;
        }

        /*
        public static ApplicationResponse CreateResponseError(SapException ex)
        {
            String _errorMessage = ex.SapErrorMessage;
            Int32 _errorCode = ex.SapErrorCode;

            if (ex.InnerException != null)
                _errorMessage += " - " + ex.InnerException.Message;

            try
            {
                ApplicationResponse response = new ApplicationResponse
                {
                    ResponseStatus = ResponseStatus.Error,
                    ResponseType = ResponseType.Message,
                    Response = new Response
                    {
                        Code = _errorCode,
                        Message = new Message
                        {
                            Text = _errorMessage,
                        }
                    }
                };
                return response;
            }
            catch (Exception exc)
            {
                return GeneralError(exc);
            }

        }*/

        private static ApplicationResponse CreateResponseError(Exception ex)
        {
            String message = ex.Message + ex.InnerException;
            return CreateResponseError(message);
        }

        private static XElement CreateTag(String o, String tagName)
        {
            XElement test = new XElement(tagName, o);
            return test;
        }

        private static XElement CreateObjectTag(Object o)
        {

            string s = new JavaScriptSerializer().Serialize(o);
            XElement element = o.ToXml(o.GetType().Name);
            return element;
        }

        private static XElement CreateObjectTagJson(Object o)
        {
            string s = new JavaScriptSerializer().Serialize(o);
            XElement test = new XElement(o.GetType().Name, s);
            return test;
        }
    }
}
