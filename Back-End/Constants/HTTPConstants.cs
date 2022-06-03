namespace AnL.Constants
{
    public static class HTTPConstants
    {
        public static readonly int OK = 200;
        public static readonly int PARTIAL_SUCCESS = 203;
        public static readonly int NO_RESPONSE = 204;
        public static readonly int BAD_REQUEST = 400;
        public static readonly int UNAUTHORIZED = 401;
        public static readonly int FORBIDDEN = 403;
        public static readonly int NOT_FOUND = 404;
        public static readonly int CONFLICT = 409;
        public static readonly int UN_SUPPORTED_MEDIA_TYPE = 415;
        public static readonly int INTERNAL_SERVER_ERROR = 500;
        public static readonly int NOT_IMPLEMENTED = 501;
        public static readonly int SERVICE_OVERLOADED = 502;
        public static readonly int DB_CONNECT_ERROR = 507;
        public static readonly int Unprocessable_Entity = 422;
    }
}
