namespace EvricaApi.Tools
{
    public class TypeMappingConverterWithNotEmptyArrays<TType, TImplementation> :
        TypeMappingConverter<TType, TImplementation> where TImplementation : TType
    {
        public override bool AllowEmptyArrays { get; set; } = false;
    }
}
