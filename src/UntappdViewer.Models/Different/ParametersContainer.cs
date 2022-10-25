using System.Collections.Generic;

namespace UntappdViewer.Models.Different
{
    public class ParametersContainer
    {
        private Dictionary<string, object> parameters;

        public ParametersContainer()
        {
            parameters = new Dictionary<string, object>();
        }

        public void Add(string name, object value)
        {
            if (parameters.ContainsKey(name))
                parameters[name] = value;
            else
                parameters.Add(name, value);
        }

        public T Get<T>(string name)
        {
            return parameters.ContainsKey(name) ? (T) parameters[name] : default(T);
        }
    }
}