using Microsoft.AspNetCore.SignalR.Infrastructure;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace TaskManager.Core.Api.Utils
{
    public class SignalRContractResolver : IContractResolver
    {

        private readonly Assembly assembly;
        private readonly IContractResolver camelCaseContractResolver;
        private readonly IContractResolver defaultContractSerializer;

        public SignalRContractResolver()
        {
            defaultContractSerializer = new DefaultContractResolver();
            camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
            assembly = typeof(Connection).GetTypeInfo().Assembly;
        }

        public JsonContract ResolveContract(Type type)
        {
            if (type.GetTypeInfo().Assembly.Equals(assembly))
            {
                return defaultContractSerializer.ResolveContract(type);

            }

            return camelCaseContractResolver.ResolveContract(type);
        }

    }
}