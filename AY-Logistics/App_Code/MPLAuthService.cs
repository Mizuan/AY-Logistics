using RestSharp; // RestSharp Library
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Linq;

namespace AYLogistics.Services
{
    public class MPLAuthService
    {
        private static string _token;
        private static DateTime? _tokenExpiryTime;
        private static readonly RestClient _client = new RestClient("https://my.port.mv/api/");
        private static string _username;
        private static string _password;

        public static string GetToken()
        {
            if (IsTokenExpired())
            {
                Console.WriteLine("Token expired or about to expire. Re-authenticating...");
                Authenticate(_username, _password);
            }

            return _token;
        }

        public static string Authenticate(string username, string password)
        //public static void Authenticate(string username, string password)
        {
            _username = username;
            _password = password;
            var request = new RestRequest("login", Method.POST);
            request.AddParameter("username", username); // Replace with your username
            request.AddParameter("password", password); // Replace with your password

            IRestResponse response = _client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response.Content);
                _token = loginResponse.Token;
                _tokenExpiryTime = GetTokenExpiryTime(_token);

                if (_tokenExpiryTime.HasValue)
                {
                    Console.WriteLine("Token received. Expires at: " + _tokenExpiryTime.Value);
                }
                else
                {
                    Console.WriteLine("Token does not contain an expiry time or is invalid.");
                }
            }
            else
            {
                Console.WriteLine("Error during authentication: " + response.ErrorMessage);
            }
            return response.StatusCode.ToString();
        }

        private static bool IsTokenExpired()
        {
            if (!_tokenExpiryTime.HasValue)
            {
                return true;
            }

            return DateTime.UtcNow >= _tokenExpiryTime.Value.AddMinutes(-5);
        }

        private static DateTime? GetTokenExpiryTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken != null)
            {
                var expClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "exp");
                long expSeconds = 0;
                if (expClaim != null && long.TryParse(expClaim.Value, out expSeconds))
                {
                    var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    DateTime expiryDate = unixEpoch.AddSeconds(expSeconds);
                    return expiryDate;
                }
            }

            return null;
        }

        // New method to fetch active sessions
        public static List<ActiveSession> GetActiveSessions()
        {
            var request = new RestRequest("active_sessions", Method.GET);
            request.AddHeader("Authorization", "Bearer " + GetToken()); // Ensure you have a valid token

            IRestResponse response = _client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the JSON response to a list of ActiveSession objects
                return JsonConvert.DeserializeObject<List<ActiveSession>>(response.Content);
            }
            else
            {
                Console.WriteLine("Failed to fetch active sessions: " + response.ErrorMessage);
                return new List<ActiveSession>(); // Return an empty list if the request fails
            }
        }

        // Method to fetch active User Detail
        public static UserDetail GetUserDetail()
        {
            var request = new RestRequest("user_details", Method.GET);
            request.AddHeader("Authorization", "Bearer " + GetToken()); // Ensure you have a valid token

            IRestResponse response = _client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the JSON response to a UserDetail object
                return JsonConvert.DeserializeObject<UserDetail>(response.Content);
            }
            else
            {
                Console.WriteLine("Failed to fetch user details: " + response.ErrorMessage);
                return null; // Return null if the request fails
            }
        }

        // New method to fetch active sessions By Name (Name is the clearing Date)
        public static List<ActiveSession> GetActiveSessionsByName(string name)
        {
            var request = new RestRequest("active_sessions", Method.GET);
            request.AddHeader("Authorization", "Bearer " + GetToken()); // Ensure you have a valid token

            // Make the request
            IRestResponse response = _client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the response to a list of ActiveSession objects
                var sessions = JsonConvert.DeserializeObject<List<ActiveSession>>(response.Content);

                // Filter sessions by the specified name
                return sessions.FindAll(session => session.Name == name);
            }
            else
            {
                Console.WriteLine("Failed to fetch active sessions: " + response.ErrorMessage);
                return null; // Return null if the request fails
            }
        }

        // New method to fetch active session Id By Name (Name is the clearing Date)
        public static List<int> GetActiveSessionIdByName(string name)
        {
            var request = new RestRequest("active_sessions", Method.GET);
            request.AddHeader("Authorization", "Bearer " + GetToken()); // Ensure you have a valid token

            // Make the request
            IRestResponse response = _client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the response to a list of ActiveSession objects
                var sessions = JsonConvert.DeserializeObject<List<ActiveSession>>(response.Content);

                // Filter sessions by the specified name and select only their IDs
                var sessionIds = sessions
                    .Where(session => session.Name == name)
                    .Select(session => session.Id)
                    .ToList();

                return sessionIds;
            }
            else
            {
                Console.WriteLine("Failed to fetch active sessions: " + response.ErrorMessage);
                return null; // Return null if the request fails
            }
        }

        // Method to fetch Requested Conatiners (Shift request Screen my bandharu-clearing)
        public static ShiftRequestResponse GetRequestedContainerByDate(int sessionId)
        {
            var request = new RestRequest("session/"+sessionId+"/already_requested", Method.GET);
            //request.AddParameter("sessionId", sessionId);
            request.AddHeader("Authorization", "Bearer " + GetToken()); // Ensure you have a valid token

            // Make the request
            IRestResponse response = _client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the response into a ShiftRequestResponse object
                var shiftRequestResponse = JsonConvert.DeserializeObject<ShiftRequestResponse>(response.Content);
                return shiftRequestResponse;
            }
            else
            {
                Console.WriteLine("Failed to fetch shift request data: " + response.ErrorMessage);
                return null; // Return null if the request fails
            }
        }

        // Method to fetch specific Conatiner By Number
        public static Container GetContainerDetail(string containerNumber)
        {
            _client.BaseUrl = new Uri(_client.BaseUrl.ToString().Replace("api/", ""));
            var request = new RestRequest("dashboard/web_dashboard/search_web_containers?query=" + containerNumber, Method.GET);
            request.AddHeader("Authorization", "Bearer " + GetToken()); // Ensure you have a valid token
            // Add the bill_of_lading_number as a query parameter
            request.AddParameter("container_no", containerNumber);
            // Make the request
            IRestResponse response = _client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the response into a list of BillOfLadingDocument objects
                var Container = JsonConvert.DeserializeObject<List<Container>>(response.Content);
                // Return the first document that matches the bill_of_lading_number, or null if not found
                return Container.Find(CT => CT.ContainerNo == containerNumber);
            }
            else
            {
                Console.WriteLine("Failed to fetch Bill of Lading documents: " + response.ErrorMessage);
                return null; // Return null if the request fails
            }
        }

        //Method to Fetch List of containers (this list status should not identified yet)
        public static List<Container> GetContainerDetail()
        {
            _client.BaseUrl = new Uri(_client.BaseUrl.ToString().Replace("api/", ""));
            var request = new RestRequest("dashboard/web_dashboard/search_web_containers", Method.GET);
            request.AddHeader("Authorization", "Bearer " + GetToken()); // Ensure you have a valid token
            // Make the request
            IRestResponse response = _client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the response into a list of BillOfLadingDocument objects
                var ct = JsonConvert.DeserializeObject<List<Container>>(response.Content);
                return ct;
            }
            else
            {
                Console.WriteLine("Failed to fetch Bill of Lading documents: " + response.ErrorMessage);
                return null; // Return null if the request fails
            }
        }

        //Method to fetch All BLs in Display List of Consignments screen of my.port.mv
        public static List<BillOfLadingDocument> GetBillOfLadingDocument()
        {
            var request = new RestRequest("bill_of_lading_documents", Method.GET);
            request.AddHeader("Authorization", "Bearer " + GetToken()); // Ensure you have a valid token
            // Make the request
            IRestResponse response = _client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the response into a list of BillOfLadingDocument objects
                var bls = JsonConvert.DeserializeObject<List<BillOfLadingDocument>>(response.Content);
                return bls;
            }
            else
            {
                Console.WriteLine("Failed to fetch Bill of Lading documents: " + response.ErrorMessage);
                return null; // Return null if the request fails
            }
        }

        //Method to fetch BLs from Display List of Consignments screen of my.port.mv and filter specific BL from it by BL number
        /*public static BillOfLadingDocument GetBillOfLadingDocumentByNumber(string billOfLadingNumber)
        {
            var request = new RestRequest("bill_of_lading_documents", Method.GET);
            request.AddHeader("Authorization", "Bearer " + GetToken()); // Ensure you have a valid token
            // Add the bill_of_lading_number as a query parameter
            request.AddParameter("bill_of_lading_number", billOfLadingNumber);
            // Make the request
            IRestResponse response = _client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the response into a list of BillOfLadingDocument objects
                var blDocuments = JsonConvert.DeserializeObject<List<BillOfLadingDocument>>(response.Content);
                // Return the first document that matches the bill_of_lading_number, or null if not found
                return blDocuments.Find(BL => BL.bill_of_lading_number == billOfLadingNumber);
            }
            else
            {
                Console.WriteLine("Failed to fetch Bill of Lading documents: " + response.ErrorMessage);
                return null; // Return null if the request fails
            }
        }

        //Method to fetch BLs from Display List of Consignments screen of my.port.mv and filter specific BL Id from it by BL number
        public static int? GetBillOfLadingDocumentIdByNumber(string billOfLadingNumber)
        {
            // Create the request and set the endpoint
            var request = new RestRequest("bill_of_lading_documents", Method.GET);
            // Add the authorization header with the bearer token
            request.AddHeader("Authorization", "Bearer " + GetToken());
            // Add the bill_of_lading_number as a query parameter
            request.AddParameter("bill_of_lading_number", billOfLadingNumber);
            // Execute the request and get the response
            IRestResponse response = _client.Execute(request);
            // Check if the response is successful
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the response into a list of BillOfLadingDocument objects
                var blDocuments = JsonConvert.DeserializeObject<List<BillOfLadingDocument>>(response.Content);
                // Find the document matching the bill_of_lading_number
                var document = blDocuments.Find(BL => BL.bill_of_lading_number == billOfLadingNumber);
                // Return the document's id if found, otherwise return null
                return document.Id;
            }
            else
            {
                // Log the error message and return null if the request fails
                Console.WriteLine("Failed to fetch Bill of Lading documents: " + response.ErrorMessage);
                return null;
            }
        }*/

        // Method to fetch the specific BL (if even not in Consignment Details list of my.pport.mv (eg: already cleared BLs)
        public static BillOfLadingDocument GetBillOfLadingDocumentByNumber(string billOfLadingNumber)
        {
            var request = new RestRequest("bill_of_lading_documents?query=" + billOfLadingNumber, Method.GET);
            request.AddHeader("Authorization", "Bearer " + GetToken()); // Ensure you have a valid token
            // Add the bill_of_lading_number as a query parameter
            request.AddParameter("bill_of_lading_number", billOfLadingNumber);
            // Make the request
            IRestResponse response = _client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the response into a list of BillOfLadingDocument objects
                var blDocuments = JsonConvert.DeserializeObject<List<BillOfLadingDocument>>(response.Content);
                // Return the first document that matches the bill_of_lading_number, or null if not found
                return blDocuments.Find(BL => BL.bill_of_lading_number == billOfLadingNumber);
            }
            else
            {
                Console.WriteLine("Failed to fetch Bill of Lading documents: " + response.ErrorMessage);
                return null; // Return null if the request fails
            }
        }

        // Method to fetch the specific BL (if even not in Consignment Details list of my.pport.mv (eg: already cleared BLs) and filter its ID
        public static int? GetBillOfLadingDocumentIdByNumber(string billOfLadingNumber)
        {
            var request = new RestRequest("bill_of_lading_documents?query=" + billOfLadingNumber, Method.GET);
            request.AddHeader("Authorization", "Bearer " + GetToken()); // Ensure you have a valid token
            // Add the bill_of_lading_number as a query parameter
            request.AddParameter("bill_of_lading_number", billOfLadingNumber);
            // Make the request
            IRestResponse response = _client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the response into a list of BillOfLadingDocument objects
                var blDocuments = JsonConvert.DeserializeObject<List<BillOfLadingDocument>>(response.Content);
                // Return the first document that matches the bill_of_lading_number, or null if not found
                var document = blDocuments.Find(BL => BL.bill_of_lading_number == billOfLadingNumber);
                // Return the document's id if found, otherwise return null
                return document.Id;
            }
            else
            {
                Console.WriteLine("Failed to fetch Bill of Lading documents: " + response.ErrorMessage);
                return null; // Return null if the request fails
            }
        }

        //Method To Get full Detail of specific BL By its ID
        public static BillOfLadingDocumentDetail GetBillOfLadingDocumentDetail(int id)
        {
            // Create the request and set the endpoint, using the provided Id
            var request = new RestRequest("bill_of_lading_documents/"+id, Method.GET);
            // Add the authorization header with the bearer token
            request.AddHeader("Authorization", "Bearer " + GetToken());
            // Execute the request and get the response
            IRestResponse response = _client.Execute(request);
            // Check if the response is successful
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the JSON response into the BillOfLadingDocumentDetail  object
                var blDocument = JsonConvert.DeserializeObject<BillOfLadingDocumentDetail>(response.Content);
                // Return the deserialized BillOfLadingDocumentDetail 
                return blDocument;
            }
            else
            {
                // Log the error message and return null if the request fails
                Console.WriteLine("Failed to fetch Bill of Lading document: " + response.ErrorMessage);
                return null;
            }
        }

        //Method to get Invoice By BL number COntainer or InvoiceNumber
        public static Invoice GetInvoice(string DocumentNumber) // InvoiceNumbe
        {
            var request = new RestRequest("invoices?page=1&query=" + DocumentNumber, Method.GET);
            request.AddHeader("Authorization", "Bearer " + GetToken()); // Ensure you have a valid token
            // Make the request
            IRestResponse response = _client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the response into a list of Invoice objects
                return JsonConvert.DeserializeObject<Invoice>(response.Content);
            }
            else
            {
                Console.WriteLine("Failed to fetch Bill of Lading documents: " + response.ErrorMessage);
                return null; // Return null if the request fails
            }
        }



        // Class to represent the Login
        public class LoginResponse
        {
            [JsonProperty("token")]
            public string Token { get; set; }
        }
        // Class to represent the Active Session
        public class ActiveSession
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("start_time")]
            public string StartTime { get; set; }

            [JsonProperty("end_time")]
            public string EndTime { get; set; }

            [JsonProperty("published")]
            public int Published { get; set; }

            [JsonProperty("is_active")]
            public int IsActive { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            [JsonProperty("type")]
            public int Type { get; set; }

            [JsonProperty("dhoani")]
            public object Dhoani { get; set; }

            [JsonProperty("location_id")]
            public object LocationId { get; set; }

            [JsonProperty("express_type")]
            public object ExpressType { get; set; }

            [JsonProperty("user_teu_left")]
            public int UserTeuLeft { get; set; }

            [JsonProperty("sp_teu_left")]
            public SpTeuLeft SpTeuLeft { get; set; }

            [JsonProperty("teu_left")]
            public TeuLeft TeuLeft { get; set; }

            [JsonProperty("settings")]
            public Settings Settings { get; set; }
        }
        public class SpTeuLeft
        {
            [JsonProperty("left")]
            public int Left { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }
        }
        public class TeuLeft
        {
            [JsonProperty("male")]
            public int Male { get; set; }

            [JsonProperty("hulhumale")]
            public int Hulhumale { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }
        }
        public class Settings
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("clearing_session_id")]
            public int ClearingSessionId { get; set; }

            [JsonProperty("reservable_start_at")]
            public string ReservableStartAt { get; set; }

            [JsonProperty("reservable_end_at")]
            public string ReservableEndAt { get; set; }

            [JsonProperty("cancellable_start_at")]
            public string CancellableStartAt { get; set; }

            [JsonProperty("cancellable_end_at")]
            public string CancellableEndAt { get; set; }

            [JsonProperty("quota_id")]
            public int QuotaId { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            [JsonProperty("total_teu")]
            public int TotalTeu { get; set; }

            [JsonProperty("sp_start_at")]
            public string SpStartAt { get; set; }

            [JsonProperty("sp_end_at")]
            public string SpEndAt { get; set; }

            [JsonProperty("morning_teu")]
            public object MorningTeu { get; set; }

            [JsonProperty("afternoon_teu")]
            public object AfternoonTeu { get; set; }

            [JsonProperty("hulhumale_teu")]
            public int HulhumaleTeu { get; set; }

            [JsonProperty("hulhumale_dhoani_teu")]
            public object HulhumaleDhoaniTeu { get; set; }

            [JsonProperty("male_dhoani_teu")]
            public int MaleDhoaniTeu { get; set; }

            [JsonProperty("male_teu")]
            public int MaleTeu { get; set; }

            [JsonProperty("quota")]
            public Quota Quota { get; set; }
        }
        public class Quota
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("quota_per_customer")]
            public int QuotaPerCustomer { get; set; }

            [JsonProperty("reservable_quota")]
            public object ReservableQuota { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }
        }
        // Class to represent the User Details
        public class UserDetail
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("profile_id")]
            public object ProfileId { get; set; }

            [JsonProperty("type")]
            public int Type { get; set; }

            [JsonProperty("domain")]
            public object Domain { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("deleted_at")]
            public object DeletedAt { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("photo")]
            public object Photo { get; set; }

            [JsonProperty("active")]
            public int Active { get; set; }

            [JsonProperty("phone")]
            public long Phone { get; set; }

            [JsonProperty("new_profile_id")]
            public int NewProfileId { get; set; }

            [JsonProperty("profile_view")]
            public ProfileView ProfileView { get; set; }
        }
        public class ProfileView
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("alias")]
            public object Alias { get; set; }

            [JsonProperty("identifier")]
            public string Identifier { get; set; }

            [JsonProperty("taxpayer_identification_no")]
            public string TaxpayerIdentificationNo { get; set; }

            [JsonProperty("profile_type")]
            public string ProfileType { get; set; }

            [JsonProperty("employee_no")]
            public object EmployeeNo { get; set; }

            [JsonProperty("designation")]
            public object Designation { get; set; }

            [JsonProperty("department_id")]
            public object DepartmentId { get; set; }

            [JsonProperty("unit")]
            public object Unit { get; set; }

            [JsonProperty("gender")]
            public object Gender { get; set; }

            [JsonProperty("date_of_birth")]
            public object DateOfBirth { get; set; }

            [JsonProperty("country_id")]
            public int CountryId { get; set; }

            [JsonProperty("address_line1")]
            public string AddressLine1 { get; set; }

            [JsonProperty("address_line2")]
            public object AddressLine2 { get; set; }

            [JsonProperty("city_id")]
            public int CityId { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("phone")]
            public long Phone { get; set; }

            [JsonProperty("profile_id")]
            public int ProfileId { get; set; }

            [JsonProperty("profileable_id")]
            public object ProfileableId { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            [JsonProperty("bank_account_no")]
            public string BankAccountNo { get; set; }

            [JsonProperty("bank_account_name")]
            public string BankAccountName { get; set; }

            [JsonProperty("bank_account_type")]
            public string BankAccountType { get; set; }

            [JsonProperty("bank_name")]
            public string BankName { get; set; }

            [JsonProperty("name_with_id")]
            public string NameWithId { get; set; }

            [JsonProperty("address")]
            public string Address { get; set; }

            [JsonProperty("name_with_employee_no")]
            public object NameWithEmployeeNo { get; set; }

            [JsonProperty("name_with_address")]
            public string NameWithAddress { get; set; }

            [JsonProperty("name_with_id_and_emp")]
            public string NameWithIdAndEmp { get; set; }
        }
        // Class to represent ShiftRequest
        public class ShiftRequestResponse
        {
            [JsonProperty("already_requested")]
            public List<int> AlreadyRequested { get; set; }

            [JsonProperty("requested_me")]
            public List<ShiftRequest> RequestedMe { get; set; }
        }
        public class ShiftRequest
        {
            public int Id { get; set; }
            [JsonProperty("inward_list_id")]
            public int InwardListId { get; set; }
            public int container_id { get; set; }
            public int session_id { get; set; }
            public int user_id { get; set; }
            public string Remarks { get; set; }
            public int Status { get; set; }
            public int payment_status { get; set; }
            public int bill_of_lading_document_id { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
            public int Type { get; set; }
            public string c_mode { get; set; }
            public string c_time { get; set; }
            public int Moved { get; set; }
            public string zone_id { get; set; }
            public long mobile_number { get; set; }
            public string clearing_location { get; set; }
            public int with_container { get; set; }
            public string Shifted { get; set; }
            public string shifted_at { get; set; }
            public string bill_of_lading_number { get; set; }
            public string container_no { get; set; }
            public string Session { get; set; }
            public int container_size { get; set; }
            public string container_type { get; set; }
            public int container_term { get; set; }
            public string Voyage { get; set; }
            public string voyage_number { get; set; }
            public int voyage_id { get; set; }
            public string voyage_name { get; set; }
            public string Eta { get; set; }
            public string Consignee { get; set; }
            public string registration_number { get; set; }
            public int profile_version_id { get; set; }
            public string operator_local { get; set; }
            public string do_exp { get; set; }
            public int broker_id { get; set; }
            public bool is_reefer { get; set; }
            public string clearence_mode { get; set; }
            public string clearence_time { get; set; }
            public int Reshifting { get; set; }
        }
        public class Container
        {
            [JsonProperty("container_no")]
            public string ContainerNo { get; set; }

            [JsonProperty("detail_name")]
            public string DetailName { get; set; }

            [JsonProperty("eta")]
            public string ETA { get; set; }

            [JsonProperty("voyage_name")]
            public string VoyageName { get; set; }

            [JsonProperty("tallied_date")]
            public string TalliedDate { get; set; }  // Nullable, as it can be null

            [JsonProperty("loading_port")]
            public string LoadingPort { get; set; }

            [JsonProperty("status")]
            public int Status { get; set; }
        }
        // Class to represent BillOfLadingDocument
        public class BillOfLadingDocument
        {
            public int Id { get; set; }
            public int voyage_id { get; set; }
            public string created_at { get; set; }
            public int Approved { get; set; }
            public string approved_at { get; set; }
            public int profile_version_id { get; set; }
            public int Status { get; set; }
            public int entry_status { get; set; }
            public int file_id { get; set; }
            public string mark_nos { get; set; }
            public string bill_of_lading_number { get; set; }
            public string consignee_name { get; set; }
            public string voyage_name { get; set; }
            public string BlVoyage { get; set; }
            public int cargo_count { get; set; }
            public double total_volume { get; set; }
            public string eta { get; set; }
            public string arrival_at { get; set; }
            public int? broker_id { get; set; }
            public string do_exp_date { get; set; }
            public string clearing_date { get; set; }
            public int? country_of_origin { get; set; }
            public string date_received { get; set; }
            public int? remaining_quantity { get; set; }
            public int invoice_raised { get; set; }
            public string Voyage { get; set; }
            public string formated_name { get; set; }
            public string bl_voyage_name { get; set; }
            public bool is_sea_to_air { get; set; }
        }
        // Class to represent BillOfLadingDocumentDetail
        public class BillOfLadingDocumentDetail
        {
            [JsonProperty("remarks")]
            public List<Remark> Remarks { get; set; }

            [JsonProperty("no")]
            public string No { get; set; }

            [JsonProperty("voyage")]
            public string Voyage { get; set; }

            [JsonProperty("cargos")]
            public List<Cargo> Cargos { get; set; }

            [JsonProperty("gate_passes")]
            public List<GatePass> GatePasses { get; set; }

            public Broker Broker { get; set; }
            public PaymentStatus PaymentStatus { get; set; }

            [JsonProperty("cleared_status")]
            public int ClearedStatus { get; set; }

            public Storage Storage { get; set; }
            public Ammendment Ammendment { get; set; }
            public Power Power { get; set; }
            public Remeasurements Remeasurements { get; set; }

            [JsonProperty("tallied_status")]
            public int TalliedStatus { get; set; }

            public Document Document { get; set; }
        }
        public class Remark
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("remarkble_type")]
            public string RemarkableType { get; set; }

            [JsonProperty("remarkble_id")]
            public int RemarkableId { get; set; }

            [JsonProperty("remarks")]
            public string Remarks { get; set; }

            [JsonProperty("status")]
            public int Status { get; set; }

            [JsonProperty("user_id")]
            public int UserId { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            [JsonProperty("erp_status")]
            public string ErpStatus { get; set; }
        }
        public class Cargo
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("cargo_type_id")]
            public int CargoTypeId { get; set; }

            [JsonProperty("commodity_id")]
            public int CommodityId { get; set; }

            [JsonProperty("bill_of_lading_document_id")]
            public int BillOfLadingDocumentId { get; set; }

            [JsonProperty("quantity")]
            public int Quantity { get; set; }

            [JsonProperty("tallied_qty")]
            public int? TalliedQty { get; set; }

            [JsonProperty("container_tallied_qty")]
            public int ContainerTalliedQty { get; set; }

            [JsonProperty("cleared_qty")]
            public string ClearedQty { get; set; }

            [JsonProperty("balance")]
            public int Balance { get; set; }

            [JsonProperty("in_container")]
            public int InContainer { get; set; }

            [JsonProperty("claim_cargo_id")]
            public int? ClaimCargoId { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("cubic_meter")]
            public double CubicMeter { get; set; }

            [JsonProperty("gross_weight")]
            public double? GrossWeight { get; set; }

            [JsonProperty("freight_tonnage")]
            public double FreightTonnage { get; set; }

            [JsonProperty("inward_list_id")]
            public int InwardListId { get; set; }

            [JsonProperty("container_id")]
            public int ContainerId { get; set; }

            [JsonProperty("held")]
            public int Held { get; set; }

            [JsonProperty("voyage_id")]
            public int VoyageId { get; set; }

            [JsonProperty("cargo_type")]
            public string CargoType { get; set; }

            [JsonProperty("package_type_id")]
            public int PackageTypeId { get; set; }

            [JsonProperty("packed_type_id")]
            public int? PackedTypeId { get; set; }

            [JsonProperty("packed_quantity")]
            public int? PackedQuantity { get; set; }

            [JsonProperty("held_by_agent")]
            public bool? HeldByAgent { get; set; }

            [JsonProperty("held_by_mpl")]
            public int HeldByMpl { get; set; }

            [JsonProperty("package_type")]
            public string PackageType { get; set; }

            [JsonProperty("packed_type")]
            public string PackedType { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("bill_of_lading_number")]
            public string BillOfLadingNumber { get; set; }

            [JsonProperty("consignee_id")]
            public int? ConsigneeId { get; set; }

            [JsonProperty("bl_date")]
            public string BlDate { get; set; }

            [JsonProperty("mark_nos")]
            public string MarkNos { get; set; }

            [JsonProperty("profileable_id")]
            public int? ProfileableId { get; set; }

            [JsonProperty("profile_version_id")]
            public int ProfileVersionId { get; set; }

            [JsonProperty("bl_file_id")]
            public int BlFileId { get; set; }

            [JsonProperty("container_no")]
            public string ContainerNo { get; set; }

            [JsonProperty("size")]
            public int Size { get; set; }

            [JsonProperty("arrival_date")]
            public string ArrivalDate { get; set; }

            [JsonProperty("vessel_name")]
            public string VesselName { get; set; }

            [JsonProperty("container_type")]
            public string ContainerType { get; set; }

            [JsonProperty("container_term")]
            public int ContainerTerm { get; set; }

            [JsonProperty("remarks")]
            public List<Remark> Remarks { get; set; }

            [JsonProperty("cargo_locations")]
            public List<string> CargoLocations { get; set; }
        }
        public class GatePass
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("seriel_no")]
            public string SerielNo { get; set; }

            [JsonProperty("approved_by")]
            public string ApprovedBy { get; set; }

             [JsonProperty("approved_date")]
            public string ApprovedDate { get; set; }

            [JsonProperty("transport_mode_name")]
            public string TransportModeName { get; set; }

            [JsonProperty("mndf_approved_by")]
            public string MNDFApprovedBy { get; set; }

            [JsonProperty("mfda_approved_by")]
            public string MFDAApprovedBy { get; set; }

            [JsonProperty("r_form_no")]
            public string RFormNo { get; set; }

            [JsonProperty("deleted_at")]
            public string DeletedAt { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            [JsonProperty("status")]
            public int Status { get; set; }

            [JsonProperty("remarks")]
            public string Remarks { get; set; }

            [JsonProperty("location_id")]
            public int LocationId { get; set; }

            [JsonProperty("customs_append_note")]
            public string CustomsAppendNote { get; set; }

            [JsonProperty("quarantine_released_by")]
            public string QuarantineReleasedBy { get; set; }

            [JsonProperty("pharmaceutical_approved_by")]
            public string PharmaceuticalApprovedBy { get; set; }

            [JsonProperty("released_vehicles")]
            public string ReleasedVehicles { get; set; }

            [JsonProperty("vehicle_request")]
            public List<VehicleRequest> VehicleRequest { get; set; }
        }
        public class Broker
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("profile_id")]
            public int? ProfileId { get; set; }

            [JsonProperty("deleted_at")]
            public string DeletedAt { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("new_profile_id")]
            public int NewProfileId { get; set; }

            [JsonProperty("is_public")]
            public int IsPublic { get; set; }

            [JsonProperty("profile")]
            public Profile Profile { get; set; }
        }
        public class PaymentStatus
        {
            [JsonProperty("code")]
            public int Code { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("invoice_id")]
            public List<int> InvoiceId { get; set; }
        }
        public class Storage
        {
             [JsonProperty("status")]
            public string Status { get; set; }

             [JsonProperty("payment_status")]
             public string PaymentStatus { get; set; }

             [JsonProperty("storage")]
             public List<string> storageList { get; set; }
        }
        public class Ammendment
        {
            [JsonProperty("status")]
            public string Status { get; set; }
            [JsonProperty("payment_status")]
            public string PaymentStatus { get; set; }
        }
        public class Power
        {
            [JsonProperty("status")]
            public string Status { get; set; }
            [JsonProperty("payment_status")]
            public string PaymentStatus { get; set; }
        }
        public class Remeasurements
        {
            [JsonProperty("status")]
            public string Status { get; set; }
            [JsonProperty("payment_status")]
            public string PaymentStatus { get; set; }
        }
        public class Document
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("file_id")]
            public int FileId { get; set; }

            [JsonProperty("bill_of_lading_number")]
            public string BillOfLadingNumber { get; set; }

            [JsonProperty("transit")]
            public int Transit { get; set; }

            [JsonProperty("personal_effects")]
            public int PersonalEffects { get; set; }

            [JsonProperty("port_of_loading")]
            public int PortOfLoading { get; set; }

            [JsonProperty("bill_of_lading_date")]
            public string BillOfLadingDate { get; set; }

            [JsonProperty("voyage_id")]
            public int VoyageId { get; set; }

            [JsonProperty("country_of_origin")]
            public int? CountryOfOrigin { get; set; }

            [JsonProperty("shipper")]
            public int Shipper { get; set; }

            [JsonProperty("consignee_id")]
            public int? ConsigneeId { get; set; }

            [JsonProperty("mark_nos")]
            public string MarkNos { get; set; }

            [JsonProperty("date_received")]
            public string DateReceived { get; set; }

            [JsonProperty("is_clear")]
            public int IsClear { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            [JsonProperty("user_id")]
            public int? UserId { get; set; }

            [JsonProperty("status")]
            public int Status { get; set; }

            [JsonProperty("open")]
            public int Open { get; set; }

            [JsonProperty("deleted_at")]
            public string DeletedAt { get; set; }

            [JsonProperty("profileable_id")]
            public int? ProfileableId { get; set; }

            [JsonProperty("entry_status")]
            public int EntryStatus { get; set; }

            [JsonProperty("do_exp_date")]
            public string DoExpDate { get; set; }

            [JsonProperty("broker_id")]
            public int BrokerId { get; set; }

            [JsonProperty("clearing_date")]
            public string ClearingDate { get; set; }

            [JsonProperty("approved")]
            public int Approved { get; set; }

            [JsonProperty("approved_at")]
            public string ApprovedAt { get; set; }

            [JsonProperty("profile_version_id")]
            public int ProfileVersionId { get; set; }

            [JsonProperty("new_user_id")]
            public int NewUserId { get; set; }

            [JsonProperty("hw_payment_details")]
            public PaymentStatus HwPaymentDetails { get; set; } /***sub class PaymentStatus is using in multiple classes*****/

            [JsonProperty("total_volume")]
            public double TotalVolume { get; set; }

            [JsonProperty("cargo_count")]
            public int CargoCount { get; set; }

            [JsonProperty("ref_voyage_payload")]
            public object RefVoyagePayload { get; set; }

            [JsonProperty("bl_type")]
            public string BlType { get; set; }

            [JsonProperty("auction_session_shipment_id")]
            public object AuctionSessionShipmentId { get; set; }

            [JsonProperty("transhipment_port_id")]
            public object TranshipmentPortId { get; set; }

            [JsonProperty("tallied_quantity")]
            public int TalliedQuantity { get; set; }

            [JsonProperty("do_exp")]
            public string DoExp { get; set; }

            [JsonProperty("is_sea_to_air")]
            public bool IsSeaToAir { get; set; }

            [JsonProperty("remarkbles")]
            public List<Remark> Remarkbles { get; set; } /***sub class Remark is using in multiple classes*****/

            [JsonProperty("voyage_view")]
            public VoyageView VoyageView { get; set; }

            [JsonProperty("cargo_view")]
            public List<CargoView> CargoView { get; set; }

            [JsonProperty("broker")]
            public Broker Broker { get; set; } /***sub class Broker is using in multiple classes*****/

            [JsonProperty("transhipment_port")]
            public object TranshipmentPort { get; set; }

            [JsonProperty("delivery_order")]
            public DeliveryOrder DeliveryOrder { get; set; }
        }
        public class VoyageView
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("vessel_name")]
            public string VesselName { get; set; }

            [JsonProperty("vessel_id")]
            public int VesselId { get; set; }

            [JsonProperty("vessel_type_id")]
            public int VesselTypeId { get; set; }

            [JsonProperty("gross_register_tonnage")]
            public double GrossRegisterTonnage { get; set; }

            [JsonProperty("voyage_name")]
            public string VoyageName { get; set; }

            [JsonProperty("arrival_at")]
            public string ArrivalAt { get; set; }

            [JsonProperty("departed_at")]
            public string DepartedAt { get; set; }
        }
        public class CargoView
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("cargo_type_id")]
            public int CargoTypeId { get; set; }

            [JsonProperty("commodity_id")]
            public int CommodityId { get; set; }

            [JsonProperty("bill_of_lading_document_id")]
            public int BillOfLadingDocumentId { get; set; }

            [JsonProperty("quantity")]
            public int Quantity { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("container_no")]
            public string ContainerNo { get; set; }

            [JsonProperty("size")]
            public int Size { get; set; }
        }
        public class DeliveryOrder
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("delivery_order_number")]
            public string DeliveryOrderNumber { get; set; }

            [JsonProperty("expirty_date")]
            public string ExpiryDate { get; set; }

            [JsonProperty("bill_of_lading_document_id")]
            public int BillOfLadingDocumentId { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            [JsonProperty("assignments")]
            public List<Assignment> Assignments { get; set; }
        }
        public class VehicleRequest
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("transport_request_id")]
            public int TransportRequestId { get; set; }

            [JsonProperty("delivery_order_assignment_id")]
            public int DeliveryOrderAssignmentId { get; set; }

            [JsonProperty("delivery_order_assignment_gatepass_id")]
            public int DeliveryOrderAssignmentGatepassId { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            [JsonProperty("active")]
            public int Active { get; set; }
        }
        public class LatestVersion
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("alias")]
            public string Alias { get; set; }

            [JsonProperty("identifier")]
            public string Identifier { get; set; }

            [JsonProperty("taxpayer_identification_no")]
            public string TaxpayerIdentificationNo { get; set; }

            [JsonProperty("profile_type")]
            public string ProfileType { get; set; }

            [JsonProperty("employee_no")]
            public string EmployeeNo { get; set; }

            [JsonProperty("designation")]
            public string Designation { get; set; }

            [JsonProperty("department_id")]
            public int? DepartmentId { get; set; }

            [JsonProperty("unit")]
            public string Unit { get; set; }

            [JsonProperty("gender")]
            public string Gender { get; set; }

            [JsonProperty("date_of_birth")]
            public string DateOfBirth { get; set; }

            [JsonProperty("country_id")]
            public int CountryId { get; set; }

            [JsonProperty("address_line1")]
            public string AddressLine1 { get; set; }

            [JsonProperty("address_line2")]
            public string AddressLine2 { get; set; }

            [JsonProperty("city_id")]
            public int CityId { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("phone")]
            public long Phone { get; set; }

            [JsonProperty("profile_id")]
            public int ProfileId { get; set; }

            [JsonProperty("profileable_id")]
            public int? ProfileableId { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            [JsonProperty("bank_account_no")]
            public string BankAccountNo { get; set; }

            [JsonProperty("bank_account_name")]
            public string BankAccountName { get; set; }

            [JsonProperty("bank_account_type")]
            public string BankAccountType { get; set; }

            [JsonProperty("bank_name")]
            public string BankName { get; set; }

            [JsonProperty("name_with_id")]
            public string NameWithId { get; set; }

            [JsonProperty("address")]
            public string Address { get; set; }

            [JsonProperty("name_with_employee_no")]
            public string NameWithEmployeeNo { get; set; }

            [JsonProperty("name_with_address")]
            public string NameWithAddress { get; set; }

            [JsonProperty("name_with_id_and_emp")]
            public string NameWithIdAndEmp { get; set; }
        }
        public class Profile
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("identifier")]
            public string Identifier { get; set; }

            [JsonProperty("customs_no")]
            public string CustomsNo { get; set; }

            [JsonProperty("types")]
            public List<string> Types { get; set; }

            [JsonProperty("latest_version_id")]
            public int LatestVersionId { get; set; }

            [JsonProperty("profile_type")]
            public string ProfileType { get; set; }

            [JsonProperty("type_breakdown")]
            public string TypeBreakdown { get; set; }

            [JsonProperty("parent_id")]
            public int? ParentId { get; set; }

            [JsonProperty("responsible_party_id")]
            public int? ResponsiblePartyId { get; set; }

            [JsonProperty("old_profile_id")]
            public int? OldProfileId { get; set; }

            [JsonProperty("reviewed")]
            public int Reviewed { get; set; }

            [JsonProperty("verified")]
            public int Verified { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            [JsonProperty("account_no")]
            public string AccountNo { get; set; }

            [JsonProperty("blacklisted")]
            public bool Blacklisted { get; set; }

            [JsonProperty("government")]
            public bool Government { get; set; }

            [JsonProperty("erp_status")]
            public string ErpStatus { get; set; }

            [JsonProperty("erp_synced_at")]
            public string ErpSyncedAt { get; set; }

            [JsonProperty("source")]
            public string Source { get; set; }

            [JsonProperty("latest_version")]
            public LatestVersion LatestVersion { get; set; }

            [JsonProperty("name_with_id")]
            public string NameWithId { get; set; }

            [JsonProperty("address")]
            public string Address { get; set; }
        }
        public class Assignment
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("delivery_order_id")]
            public int DeliveryOrderId { get; set; }

            [JsonProperty("staff_id")]
            public int? StaffId { get; set; }

            [JsonProperty("clearing_agent_id")]
            public int? ClearingAgentId { get; set; }

            [JsonProperty("referral_assignment_id")]
            public int? ReferralAssignmentId { get; set; }

            [JsonProperty("site_id")]
            public int SiteId { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("remarks")]
            public string Remarks { get; set; }

            [JsonProperty("deleted_at")]
            public string DeletedAt { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            [JsonProperty("direct_delivery")]
            public int DirectDelivery { get; set; }

            [JsonProperty("progress_status")]
            public int ProgressStatus { get; set; }

            [JsonProperty("location_id")]
            public int LocationId { get; set; }

            [JsonProperty("reassignable")]
            public string Reassignable { get; set; }

            [JsonProperty("new_staff_id")]
            public int? NewStaffId { get; set; }

            [JsonProperty("new_clearing_agent_id")]
            public int? NewClearingAgentId { get; set; }

            [JsonProperty("referrals")]
            public List<int> Referrals { get; set; }

            [JsonProperty("payment_status")]
            public List<object> PaymentStatus { get; set; }// Assuming payment status is a list, adjust if necessary.
        }
        //Class to represent Invoice
        public class Invoice
        {
            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("per_page")]
            public int PerPage { get; set; }

            [JsonProperty("current_page")]
            public int CurrentPage { get; set; }

            [JsonProperty("last_page")]
            public int LastPage { get; set; }

            [JsonProperty("next_page_url")]
            public string NextPageUrl { get; set; }

            [JsonProperty("prev_page_url")]
            public string PrevPageUrl { get; set; }

            [JsonProperty("from")]
            public int From { get; set; }

            [JsonProperty("to")]
            public int To { get; set; }

            [JsonProperty("data")]
            public List<InvoiceData> Data { get; set; }
        }
        public class InvoiceData
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("identifier")]
            public string Identifier { get; set; }

            [JsonProperty("invoice_number")]
            public string InvoiceNumber { get; set; }

            [JsonProperty("data_id")]
            public int DataId { get; set; }

            [JsonProperty("profileable_id")]
            public int? ProfileableId { get; set; }

            [JsonProperty("details")]
            public string Details { get; set; }

            [JsonProperty("amount")]
            public decimal Amount { get; set; }

            [JsonProperty("finance_checked")]
            public int FinanceChecked { get; set; }

            [JsonProperty("type")]
            public int Type { get; set; }

            [JsonProperty("status")]
            public int Status { get; set; }

            [JsonProperty("deleted_at")]
            public string DeletedAt { get; set; }

            [JsonProperty("created_at")]
            public string CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; }

            [JsonProperty("voyage_id")]
            public int VoyageId { get; set; }

            [JsonProperty("tampered")]
            public int Tampered { get; set; }

            [JsonProperty("gst")]
            public decimal Gst { get; set; }

            [JsonProperty("surcharge")]
            public decimal Surcharge { get; set; }

            [JsonProperty("location_id")]
            public int LocationId { get; set; }

            [JsonProperty("currency_id")]
            public int CurrencyId { get; set; }

            [JsonProperty("discount")]
            public decimal Discount { get; set; }

            [JsonProperty("pilotage_id")]
            public int? PilotageId { get; set; }

            [JsonProperty("profile_version_id")]
            public int ProfileVersionId { get; set; }

            [JsonProperty("new_description")]
            public string NewDescription { get; set; }

            [JsonProperty("credit_notes")]
            public int CreditNotes { get; set; }

            [JsonProperty("total")]
            public decimal Total { get; set; }

            [JsonProperty("balance")]
            public decimal Balance { get; set; }

            [JsonProperty("tariff_id")]
            public int TariffId { get; set; }

            [JsonProperty("credit_fine")]
            public decimal CreditFine { get; set; }

            [JsonProperty("credit_days")]
            public int CreditDays { get; set; }

            [JsonProperty("credit_ends_at")]
            public string CreditEndsAt { get; set; }

            [JsonProperty("credit_fine_waived_days")]
            public int? credit_fine_waived_days { get; set; }

            [JsonProperty("accounted_at")]
            public string AccountedAt { get; set; }

            [JsonProperty("transaction_type")]
            public string TransactionType { get; set; }

            [JsonProperty("created_by")]
            public int? CreatedBy { get; set; }

            [JsonProperty("checked_by")]
            public int? checked_by { get; set; }

            [JsonProperty("approved_by")]
            public int? approved_by { get; set; }

            [JsonProperty("document_number")]
            public int? document_number { get; set; }

            [JsonProperty("erp_status")]
            public string ErpStatus { get; set; }

            [JsonProperty("formatted_amount")]
            public string FormattedAmount { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("invoiced_at")]
            public string invoiced_at { get; set; }

            [JsonProperty("formatted_description")]
            public string formatted_description { get; set; }

            [JsonProperty("status_name")]
            public string status_name { get; set; }

            [JsonProperty("payable_total")]
            public decimal PayableTotal { get; set; }

            [JsonProperty("total_in_words")]
            public string TotalInWords { get; set; }

            [JsonProperty("receipts")]
            public List<Receipt> Receipts { get; set; }

            [JsonProperty("invoice_request")]
            public string invoice_request { get; set; }

            [JsonProperty("profile")]
            public InvoiceProfile InvoiceProfile { get; set; }

            [JsonProperty("currency")]
            public Currency Currency { get; set; }
        }
        public class Receipt
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("receipt_number")]
            public string ReceiptNumber { get; set; }

            [JsonProperty("payment_date")]
            public string PaymentDate { get; set; }

            [JsonProperty("remarks")]
            public string remarks { get; set; }

            [JsonProperty("account_number")]
            public string account_number { get; set; }

            [JsonProperty("cheque_number")]
            public string cheque_number { get; set; }

            [JsonProperty("card_name")]
            public string card_name { get; set; }

            [JsonProperty("bank_name")]
            public string bank_name { get; set; }

            [JsonProperty("type_name")]
            public string type_name { get; set; }

            // Additional properties for receipts...
        }
        public class InvoiceProfile
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("identifier")]
            public string identifier { get; set; }

            [JsonProperty("name_with_id")]
            public string name_with_id { get; set; }

            [JsonProperty("address")]
            public string address { get; set; }

            [JsonProperty("name_with_employee_no")]
            public string name_with_employee_no { get; set; }

            // Additional properties for profile...
        }
        public class Currency
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            // Additional properties for currency...
        }


    }
}
