namespace Common
{
    public class Constant
    {
        public const string Authentication_Scheme_Bearer = "Bearer";
        public const string Content_Type_Json = "application/json";
        public const string Identity_Api_Key = "IdentityApiKey";
        public const string Ocelot_Json_File_Name = "ocelot.json";
        public const string Response_Type = "code id_token";
        public const string Client_Id_Policy = "ClientIdPolicy";

        public const string Connection_String_Identity_Server_Key = "IdentityServerDatabase";
        public const string Connection_String_Client_Key = "ClientDatabase";

        public const string Http_Client_Movies_Api = "MovieApiClient";
        public const string Http_Client_Idp = "IdpClient";

        public const string Movies_Client_Id_Key = "client_id";
        public const string Movies_Client_Id_Value = "movies_mvc_client";
        public const string Movies_Client_Name = "Movies MVC Web App";
        public const string Movies_Client_Secret = "secret";

        public const string Role_Admin = "admin";

        public const string Scope_Address = "address";
        public const string Scope_Email = "email";
        public const string Scope_Movie_Api_Value = "movieApi";
        public const string Scope_Movie_Api_Text = "Movie Api";
        public const string Scope_Open_Id = "openid";
        public const string Scope_Profile = "profile";
        public const string Scope_Role_Value = "role";
        public const string Scope_Role_Text = "Role";

        public const string Movies_Api_Route_Name = "api/[controller]";
        public const string Movies_Api_Route_Id = "{id}";
        public const string Movies_Api_Action_Get_Movie = "GetMovie";

        public const string Movies_Controller_Action_Index = "Index";
        public const string Movies_Controller_Action_Delete = "Delete";
        public const string Movies_Controller_Bind_Attribute = "Id,Title,Genre,Rating,ReleaseDate,ImageUrl,Owner";
    }
}
