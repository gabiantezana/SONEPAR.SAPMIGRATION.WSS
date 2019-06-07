using System;
using System.Linq.Expressions;
using System.Xml;

namespace SONEPAR.SAPMIGRATION.WSS.HELPER
{
    public static class ConvertHelper
    {
        public static string GetLogErrorConcatened(string newError, string lastError)
        {
            return "<" + DateTime.Now.ToString(ConstantHelper.LOG_DATE_FORMAT) + ": " + newError + "> " + lastError;
        }
        public static void ConvertToObject(ref Object _object, Type type)
        {

        }


        public static String ToSafeString(this object val)
        {
            return (val ?? String.Empty).ToString();
        }

        public static string GetVariableName<T>(Expression<Func<T>> expr)
        {
            var body = (MemberExpression)expr.Body;

            return body.Member.Name;
        }

        public static Int32 ToInt32(this Object value)
        {
            Int32 response = default(Int32);
            try
            {
                response = Convert.ToInt32(value);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Parse error value: " + value.ToSafeString());
            }
        }

        public static DateTime ToDate(String value)
        {
            try
            {
                return DateTime.ParseExact(value.ToSafeString(), ConstantHelper.DateFormat, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Date parse error. Value: " + value.ToSafeString());
            }

        }
    }
}
