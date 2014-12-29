using System;

namespace Lanit.Platform.Entity
{
    /// <summary>
    /// When applied to the member of a type, specifies that the member is part of a data contract and is serializable by the <see cref="T:System.Runtime.Serialization.DataContractSerializer"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class DataMemberAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a data member name.
        /// </summary>
        /// 
        /// <returns>
        /// The name of the data member. The default is the name of the target that the attribute is applied to.
        /// </returns>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the order of serialization and deserialization of a member.
        /// </summary>
        /// 
        /// <returns>
        /// The numeric order of serialization or deserialization.
        /// </returns>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets a value that instructs the serialization engine that the member must be present when reading or deserializing.
        /// </summary>
        /// 
        /// <returns>
        /// true, if the member is required; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">the member is not present.</exception>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets a value that specifies whether to serialize the default value for a field or property being serialized.
        /// </summary>
        /// 
        /// <returns>
        /// true if the default value for a member should be generated in the serialization stream; otherwise, false. The default is true.
        /// </returns>
        public bool EmitDefaultValue { get; set; }
    }

    /// <summary>
    /// Specifies that the type defines or implements a data contract and is serializable by a serializer, such as the <see cref="T:System.Runtime.Serialization.DataContractSerializer"/>. To make their type serializable, type authors must define a data contract for their type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class DataContractAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value that indicates whether to preserve object reference data.
        /// </summary>
        /// 
        /// <returns>
        /// true to keep object reference data using standard XML; otherwise, false. The default is false.
        /// </returns>
        public bool IsReference { get; set; }
        /// <summary>
        /// Gets or sets the namespace for the data contract for the type.
        /// </summary>
        /// 
        /// <returns>
        /// The namespace of the contract.
        /// </returns>
        public string Namespace { get; set; }
        /// <summary>
        /// Gets or sets the name of the data contract for the type.
        /// </summary>
        /// 
        /// <returns>
        /// The local name of a data contract. The default is the name of the class that the attribute is applied to.
        /// </returns>
        public string Name { get; set; }
    }

    /// <summary>
    /// Первичный ключ
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
    }

    /// <summary>
    /// Table attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Name { get; set; }

        public TableAttribute(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Column attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        public ColumnAttribute(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Auto increment attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoIncrementAttribute : Attribute
    {
    }

    /// <summary>
    /// Indexed attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IndexedAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Lanit.Platform.Entity.IndexedAttribute"/> is unique.
        /// </summary>
        /// <value><c>true</c> if unique; otherwise, <c>false</c>.</value>
        public virtual bool Unique { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lanit.Platform.Entity.IndexedAttribute"/> class.
        /// </summary>
        public IndexedAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lanit.Platform.Entity.IndexedAttribute"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="order">Order.</param>
        public IndexedAttribute(string name, int order)
        {
            Name = name;
            Order = order;
        }
    }

    /// <summary>
    /// Ignore attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }

    /// <summary>
    /// Unique attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueAttribute : IndexedAttribute
    {
        public override bool Unique
        {
            get { return true; }
            set { /* throw?  */ }
        }
    }

    /// <summary>
    /// Max length attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLengthAttribute : Attribute
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lanit.Platform.Entity.MaxLengthAttribute"/> class.
        /// </summary>
        /// <param name="length">Length.</param>
        public MaxLengthAttribute(int length)
        {
            Value = length;
        }
    }

    /// <summary>
    /// Collation attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CollationAttribute : Attribute
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lanit.Platform.Entity.CollationAttribute"/> class.
        /// </summary>
        /// <param name="collation">Collation.</param>
        public CollationAttribute(string collation)
        {
            Value = collation;
        }
    }
}
