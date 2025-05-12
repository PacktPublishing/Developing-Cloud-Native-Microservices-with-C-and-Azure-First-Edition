using SharedMessages.BasicTypes;
using System.Collections.Concurrent;

namespace Authorization.SampleDB
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

        ConcurrentDictionary<string, UserBasicInfoMessage> _userDatabase = new ConcurrentDictionary<string, UserBasicInfoMessage>();
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
        /// <param name="user">The user to add.</param>
        internal bool AddUser(UserBasicInfoMessage user)
        {
            // This is a sample method to add a user to the database.
            // You can implement your database logic here.
            if (user.Id == Guid.Empty)
            {
                user.Id = Guid.NewGuid(); // Generate a new GUID if not provided
                _userDatabase.TryAdd(user.Id.ToString(), user);
                return true;
            }
            else
                return false;
        }

        internal UserBasicInfoMessage? GetUser(Guid id)
        {
            UserBasicInfoMessage user = _userDatabase.GetValueOrDefault(id.ToString());
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        internal bool UpdateUser(UserBasicInfoMessage user)
        {
            return _userDatabase.TryUpdate(user.Id.ToString(), user, _userDatabase.GetValueOrDefault(user.Id.ToString()));
        }

        internal UserBasicInfoMessage ValidateUser(string email, string password)
        {
            UserBasicInfoMessage result = null;
            _userDatabase.ToList().ForEach(x =>
            {
                if (x.Value.Email == email && x.Value.Password == password)
                {
                    result = x.Value;
                }
            });
            return result;
        }
    }
}
