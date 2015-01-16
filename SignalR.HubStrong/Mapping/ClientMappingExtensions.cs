namespace SignalR.HubStrong.Mapping
{
    using Microsoft.AspNet.SignalR.Client;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ClientMappingExtensions
    {
        private delegate void InvokeMethod(params object[] parameters);

        public static void MapClientMethods<T>(this IHubProxy hub, object hubClient)
        {
            if (!typeof(T).IsInterface)
            {
                throw new InvalidOperationException("The generic type parameter T must be an interface when mapping client methods.");
            }
            if (!(hubClient is T))
            {
                throw new ArgumentException(String.Format("The hubClient must implement the interface from the generic type parameter: {0}", typeof(T)));
            }

            var methods = typeof(T).GetMethods();
            hub.MapClientMethods(hubClient, methods);
        }

        public static void MapClientMethods(this IHubProxy hub, object hubClient)
        {
            var methods = hubClient.GetType().GetMethods().Where(m => m.CustomAttributes.Any(a => a.AttributeType == typeof(HubClientMethodAttribute)));
            hub.MapClientMethods(hubClient, methods);
        }

        private static void MapClientMethods(this IHubProxy hub, object hubClient, IEnumerable<MethodInfo> methods)
        {
            foreach (var methodInfo in methods)
            {
                var method = methodInfo;
                var eventName = method.Name;
                InvokeMethod onData = parameters => Invoke(hubClient, method, parameters);
                var parameterCount = method.GetParameters().Count();
                switch (parameterCount)
                {
                    case 0:
                        hub.On(eventName, () => onData());
                        break;
                    case 1:
                        hub.On<dynamic>(eventName, p1 => onData(p1));
                        break;
                    case 2:
                        hub.On<dynamic, dynamic>(eventName, (p1, p2) => onData(p1, p2));
                        break;
                    case 3:
                        hub.On<dynamic, dynamic, dynamic>(eventName, (p1, p2, p3) => onData(p1, p2, p3));
                        break;
                    case 4:
                        hub.On<dynamic, dynamic, dynamic, dynamic>(eventName, (p1, p2, p3, p4) => onData(p1, p2, p3, p4));
                        break;
                    case 5:
                        hub.On<dynamic, dynamic, dynamic, dynamic, dynamic>(eventName, (p1, p2, p3, p4, p5) => onData(p1, p2, p3, p4, p5));
                        break;
                    case 6:
                        hub.On<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic>(eventName, (p1, p2, p3, p4, p5, p6) => onData(p1, p2, p3, p4, p5, p6));
                        break;
                    case 7:
                        hub.On<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic>(eventName, (p1, p2, p3, p4, p5, p6, p7) => onData(p1, p2, p3, p4, p5, p6, p7));
                        break;
                }
            }
        }

        private static object Invoke(object hubClient, MethodInfo method, object[] parameters)
        {
            var typedParameters = new object[parameters.Length];
            var methodParameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();

            for (var i = 0; i < typedParameters.Length; i++)
            {
                var targetType = methodParameterTypes[i];
                var serialized = JsonConvert.SerializeObject(parameters[i]);
                typedParameters[i] = JsonConvert.DeserializeObject(serialized, targetType);
            }

            return method.Invoke(hubClient, typedParameters);
        }
    }
}
