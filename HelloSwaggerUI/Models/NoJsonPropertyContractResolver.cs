using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HelloSwaggerUI.Models
{
    public class NoJsonPropertyContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.PropertyName = property.UnderlyingName;
            return property;
        }
    }
}