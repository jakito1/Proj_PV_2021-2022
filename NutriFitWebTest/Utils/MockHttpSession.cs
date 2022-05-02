﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NutriFitWebTest.Utils
{
    public class MockHttpSession : ISession
    {
        private readonly Dictionary<string, object> sessionStorage = new Dictionary<string, object>();
        public object this[string name]
        {
            get => sessionStorage[name];
            set => sessionStorage[name] = value;
        }

        string ISession.Id => throw new NotImplementedException();
        bool ISession.IsAvailable => throw new NotImplementedException();
        IEnumerable<string> ISession.Keys => sessionStorage.Keys;
        void ISession.Clear()
        {
            sessionStorage.Clear();
        }
        Task ISession.CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        Task ISession.LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        void ISession.Remove(string key)
        {
            sessionStorage.Remove(key);
        }

        void ISession.Set(string key, byte[] value)
        {
            sessionStorage[key] = value;
        }

        bool ISession.TryGetValue(string key, out byte[] value)
        {
            if (sessionStorage[key] != null)
            {
                value = Encoding.ASCII.GetBytes(sessionStorage[key].ToString());
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}
