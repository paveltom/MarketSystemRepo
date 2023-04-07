using System;

namespace Market_System.ServiceLayer
{
    ///<summary>This class extends <c>Response</c> and represents the result of a call to a non-void function. 
    ///In addition to the behavior of <c>Response</c>, the class holds the value of the returned value in the variable <c>Value</c>.</summary>
    ///<typeparam name="T">The type of the returned value of the function, stored by the list.</typeparam>
    public class Response<T> : Response
    {
        public readonly T Value;
        private Response(T value, string msg) : base(msg)//changed from private to public
        {
            this.Value = value;
        }

        public static Response<T> FromValue(T value)
        {
            return new Response<T>(value, null);
        }

/*        public object Select(Func<object, object, MessageModel> p)
        {
            throw new NotImplementedException();
        }
*/
        internal static Response<T> FromError(string msg)
        {
            return new Response<T>(default(T), msg);
        }
    }
}

