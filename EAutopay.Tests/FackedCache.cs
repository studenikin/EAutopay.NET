﻿using System;
using System.Collections.Generic;

namespace EAutopay.Tests
{
    class FackedCache : ICache
    {
        private Dictionary<string, object> _cache;

        public FackedCache()
        {
            _cache = new Dictionary<string, object>();
        }

        public object Get(string key)
        {
            return _cache[key];
        }

        public void Set(string key, object data)
        {
            if (_cache.ContainsKey(key))
            {
                _cache[key] = data;
            }
            else
            {
                _cache.Add(key, data);
            }
        }
    }
}