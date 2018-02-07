using System;

namespace MLaw.Idp.Mvc.Services
{
    public class CacheKeyCreator
    {
        public string CreateKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
