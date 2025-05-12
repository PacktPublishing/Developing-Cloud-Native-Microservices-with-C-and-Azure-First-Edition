using SharedMessages.BasicTypes;
using SharedMessages.RouteNegotiation;
using System.Collections.Concurrent;

namespace CarRequests.SampleDB
{
    /// <summary>
    /// This will be a sample Database class.
    /// </summary>
    public class SampleDatabase
    {
        #region Singleton
        private static SampleDatabase _instance = null;

        private static readonly object _lock = new object();

        public static SampleDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SampleDatabase();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        ConcurrentDictionary<string, RouteRequestMessage> _carRequests = new ConcurrentDictionary<string, RouteRequestMessage>();
        /// <summary>
        /// This will be a sample Database class.
        /// </summary>
        public SampleDatabase()
        {
            // This is a sample database constructor.
            // You can initialize your database connection here.
        }
        /// <summary>
        /// This will be a sample method to add a user to the database.
        /// </summary>
        /// <param name="carRequest">The user to add.</param>
        internal bool AddRequest(RouteRequestMessage carRequest)
        {
            // This is a sample method to add a car request to the database.
            // You can implement your database logic here.
            if (carRequest.Id  == Guid.Empty)
            {
                carRequest.Id = Guid.NewGuid(); // Generate a new GUID if not provided
                _carRequests.TryAdd(carRequest.Id.ToString(), carRequest);
                return true;
            }
            else
                return false;
        }

        internal List<RouteRequestMessage> GetMyRequests(Guid userId)
        {
            List<RouteRequestMessage> routeRequestMessagesResult = _carRequests.Values.Where(routeRequestMessages => routeRequestMessages.User != null && routeRequestMessages.User.Id == userId).ToList();
            return routeRequestMessagesResult;
        }
    }
}
