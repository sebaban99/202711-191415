using System;
using System.Collections.Generic;

namespace IMMRequest.Domain
{
    public enum FieldType
    {
        Fecha,
        Texto,
        Entero
    }
    public class AdditionalField
    {
        public Guid Id {get; set;}
        public string Name {get; set;}
        public FieldType FieldType { get; set;}
        public Type Type {get; set;}
        public List<Range> Range {get; set;}
        public string Value {get; set;}
    }
}