using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EvricaApi.Tools
{
    public class TypeMappingConverter<TType, TImplementation> : JsonConverter<TType>
  where TImplementation : TType
    {
        public virtual bool AllowEmptyArrays { get; set; } = true;


        [return: MaybeNull]
        public override TType Read(
          ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = JsonSerializer.Deserialize<TImplementation>(ref reader, options);
            if (!AllowEmptyArrays && result is Array)
            {
                if ((result as Array)!.Length == 0)
                    throw new Model.Exceptions.InvalidEntityException("Array was been empty");
            }
            return result;
        }

        public override void Write(
          Utf8JsonWriter writer, TType value, JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, (TImplementation)value!, options);
    }
}
