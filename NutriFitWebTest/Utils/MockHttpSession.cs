using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NutriFitWebTest.Utils
***REMOVED***
    public class MockHttpSession : ISession
    ***REMOVED***
        Dictionary<string, object> sessionStorage = new Dictionary<string, object>();
        public object this[string name]
        ***REMOVED***
            get ***REMOVED*** return sessionStorage[name]; ***REMOVED***
            set ***REMOVED*** sessionStorage[name] = value; ***REMOVED***
    ***REMOVED***

        string ISession.Id
        ***REMOVED***
            get
            ***REMOVED***
                throw new NotImplementedException();
        ***REMOVED***
    ***REMOVED***
        bool ISession.IsAvailable
        ***REMOVED***
            get
            ***REMOVED***
                throw new NotImplementedException();
        ***REMOVED***
    ***REMOVED***
        IEnumerable<string> ISession.Keys
        ***REMOVED***
            get ***REMOVED*** return sessionStorage.Keys; ***REMOVED***
    ***REMOVED***
        void ISession.Clear()
        ***REMOVED***
            sessionStorage.Clear();
    ***REMOVED***
        Task ISession.CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        ***REMOVED***
            throw new NotImplementedException();
    ***REMOVED***

        Task ISession.LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
        ***REMOVED***
            throw new NotImplementedException();
    ***REMOVED***

        void ISession.Remove(string key)
        ***REMOVED***
            sessionStorage.Remove(key);
    ***REMOVED***

        void ISession.Set(string key, byte[] value)
        ***REMOVED***
            sessionStorage[key] = value;
    ***REMOVED***

        bool ISession.TryGetValue(string key, out byte[] value)
        ***REMOVED***
            if (sessionStorage[key] != null)
            ***REMOVED***
                value = Encoding.ASCII.GetBytes(sessionStorage[key].ToString());
                return true;
        ***REMOVED***
            else
            ***REMOVED***
                value = null;
                return false;
        ***REMOVED***
    ***REMOVED***
***REMOVED***
***REMOVED***
