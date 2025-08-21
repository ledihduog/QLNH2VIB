namespace QuanLyNhaHang
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Role
        {
            private const string Base = $"{ApiBase}/Role";
            public const string GetAll = Base;
            public const string Create = $"{Base}/Create";
            public const string Get = $"{Base}/{{id}}";
            public const string Update = $"{Base}/Update/{{id:int}}";
            public const string Delete = $"{Base}/Delete/{{id}}";
            public const string DeleteBatch = $"{Base}/DeleteBatch";
        }

        public static class UserRole
        {
            private const string Base = $"{ApiBase}/UserRole";
            public const string GetAll = Base;
            public const string Create = $"{Base}/Create";
            public const string Get = $"{Base}/{{id}}";
            public const string Update = $"{Base}/Update/{{id:int}}";
            public const string Delete = $"{Base}/Delete/{{id}}";
            public const string DeleteBatch = $"{Base}/DeleteBatch";
        }

        public static class UserLogin
        {
            private const string Base = $"{ApiBase}/UserLogin";
            public const string Create = $"{Base}/Create";
            public const string Get = $"{Base}/{{idOrSlug}}";
            public const string GetAll = Base;
            public const string SignIn = $"{Base}/SignIn";
            public const string SignUp = $"{Base}/SignUp";
            public const string Update = $"{Base}/Update/{{id}}";
            public const string Delete = $"{Base}/Delete";
            public const string DeleteBatch = $"{Base}/DeleteBatch";
            public const string UserRole = $"{Base}/UserRole";

        }

        public static class Auth
        {
            private const string Base = $"{ApiBase}/Auth";
            public const string GetAll = Base;
            public const string SignIn = $"{Base}/SignIn";
            public const string ChangPass = $"{Base}/ChangPass";
            public const string SignUp = "SignUp";
            public const string Create = $"{Base}/Create";
            public const string Update = $"{Base}/Update/{{id:int}}";
        }

        public static class Product
        {
            private const string Base = $"{ApiBase}/Product";
            public const string GetAll = Base;
            public const string Create = $"{Base}/Create";
            public const string Get = $"{Base}/{{id}}";
            public const string Update = $"{Base}/Update/{{id:int}}";
            public const string Delete = $"{Base}/Delete/{{id}}";
            public const string DeleteBatch = $"{Base}/DeleteBatch";
        }

        public static class Order
        {
            private const string Base = $"{ApiBase}/Order";
            public const string GetAll = Base;
            public const string Create = $"{Base}/Create";
            public const string Get = $"{Base}/{{id}}";
            public const string Update = $"{Base}/Update/{{id:int}}";
            public const string Delete = $"{Base}/Delete/{{id}}";
            public const string DeleteBatch = $"{Base}/DeleteBatch";
        }

        public static class Sumary
        {
            private const string Base = $"{ApiBase}/Sumary";
            public const string GetAll = Base;
            public const string Delete = $"{Base}/Delete/{{id}}";
            public const string DeleteBatch = $"{Base}/DeleteBatch";
        }
    }
}
