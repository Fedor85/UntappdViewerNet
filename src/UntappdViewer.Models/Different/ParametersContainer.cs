using System.Collections.Generic;

namespace UntappdViewer.Models.Different
{
    public class ParametersContainer
    {
        private readonly Dictionary<string, object> parameters = new();

        public void Add(string name, object value)
        {
            if (parameters.ContainsKey(name))
                parameters[name] = value;
            else
                parameters.Add(name, value);
        }

        public bool Contains(string name)
        {
            return parameters.ContainsKey(name);
        }

        public T Get<T>(string name)
        {
            return Contains(name) ? (T) parameters[name] : default(T);
        }
    }
}