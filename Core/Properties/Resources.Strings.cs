//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Globalization;

namespace NuPattern.Core.Properties
{
	///	<summary>
	///	Provides access to string resources.
	///	</summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("netfx-System.Strings", "1.0.0.0")]
	[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
	static partial class Strings
	{
		///	<summary>
		///	Provides access to string resources.
		///	</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("netfx-System.Strings", "1.0.0.0")]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public static partial class Collection
		{
			/// <summary>
			/// Looks up a localized string similar to: 
			///	An item with the same name '{name}' already exists.
			/// </summary>
			public static string DuplicateItemName(object name)
			{
				return Resources.Collection_DuplicateItemName.FormatWith(new 
				{
					name = name,
				});
			}
		}
		
		///	<summary>
		///	Provides access to string resources.
		///	</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("netfx-System.Strings", "1.0.0.0")]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public static partial class ComponentSchema
		{
			/// <summary>
			/// Looks up a localized string similar to: 
			///	A property with the same name '{name}' already exists.
			/// </summary>
			public static string DuplicatePropertyName(object name)
			{
				return Resources.ComponentSchema_DuplicatePropertyName.FormatWith(new 
				{
					name = name,
				});
			}
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Cannot create property named 'Name' since it is a reserved property that always exists for components.
			/// </summary>
			public static string NamePropertyReserved { get { return Resources.ComponentSchema_NamePropertyReserved; } }
		}
		
		///	<summary>
		///	Provides access to string resources.
		///	</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("netfx-System.Strings", "1.0.0.0")]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public static partial class Component
		{
			/// <summary>
			/// Looks up a localized string similar to: 
			///	A property with the same name '{name}' already exists.
			/// </summary>
			public static string DuplicatePropertyName(object name)
			{
				return Resources.Component_DuplicatePropertyName.FormatWith(new 
				{
					name = name,
				});
			}
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Cannot create property named 'Name' since it is a reserved property that always exists for components.
			/// </summary>
			public static string NamePropertyReserved { get { return Resources.Component_NamePropertyReserved; } }
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Component '{name}' has existing schema '{existingSchemaId}' which does not match the specified schema value with id '{newSchemaId}'.
			/// </summary>
			public static string SchemaMismatch(object name, object existingSchemaId, object newSchemaId)
			{
				return Resources.Component_SchemaMismatch.FormatWith(new 
				{
					name = name,
					existingSchemaId = existingSchemaId,
					newSchemaId = newSchemaId,
				});
			}
		}
		
		///	<summary>
		///	Provides access to string resources.
		///	</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("netfx-System.Strings", "1.0.0.0")]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public static partial class Container
		{
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Cannot create component because a property named '{propertyName}' already exists on '{containerName}'.
			/// </summary>
			public static string ComponentNameMatchesProperty(object propertyName, object containerName)
			{
				return Resources.Container_ComponentNameMatchesProperty.FormatWith(new 
				{
					propertyName = propertyName,
					containerName = containerName,
				});
			}
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	A component with the same name '{name}' already exists.
			/// </summary>
			public static string DuplicateComponentName(object name)
			{
				return Resources.Container_DuplicateComponentName.FormatWith(new 
				{
					name = name,
				});
			}
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Cannot rename component '{oldName}' to '{newName}' because its parent '{container}' already has a component with the same name.
			/// </summary>
			public static string RenamedDuplicateComponent(object oldName, object newName, object container)
			{
				return Resources.Container_RenamedDuplicateComponent.FormatWith(new 
				{
					oldName = oldName,
					newName = newName,
					container = container,
				});
			}
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Cannot rename component '{oldName}' to '{newName}' because its parent '{container}' already has a property with the same name.
			/// </summary>
			public static string RenamedDuplicateProperty(object oldName, object newName, object container)
			{
				return Resources.Container_RenamedDuplicateProperty.FormatWith(new 
				{
					oldName = oldName,
					newName = newName,
					container = container,
				});
			}
		}
		
		///	<summary>
		///	Provides access to string resources.
		///	</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("netfx-System.Strings", "1.0.0.0")]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public static partial class JsonProductSerializer
		{
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Invalid document. Expected '$format' property with a valid version string (i.e. '2.0').
			/// </summary>
			public static string InvalidFormat { get { return Resources.JsonProductSerializer_InvalidFormat; } }
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Invalid document. Expected '$items' object property to contain collection elements.
			/// </summary>
			public static string InvalidItems { get { return Resources.JsonProductSerializer_InvalidItems; } }
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Invalid document. Expected '$format' property with the document version.
			/// </summary>
			public static string MissingFormat { get { return Resources.JsonProductSerializer_MissingFormat; } }
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Invalid document. Expected '$schema' property with component schema definition identifier.
			/// </summary>
			public static string MissingSchema { get { return Resources.JsonProductSerializer_MissingSchema; } }
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Invalid document. Expected '$toolkit' property with the owning toolkit information.
			/// </summary>
			public static string MissingToolkit { get { return Resources.JsonProductSerializer_MissingToolkit; } }
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Invalid document. Expected '$id' property with the owning toolkit identifier.
			/// </summary>
			public static string MissingToolkitId { get { return Resources.JsonProductSerializer_MissingToolkitId; } }
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Invalid document. Expected '$version' property with the owning toolkit version.
			/// </summary>
			public static string MissingToolkitVersion { get { return Resources.JsonProductSerializer_MissingToolkitVersion; } }
		}
		
		///	<summary>
		///	Provides access to string resources.
		///	</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("netfx-System.Strings", "1.0.0.0")]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public static partial class ProductStore
		{
			/// <summary>
			/// Looks up a localized string similar to: 
			///	A product with the same name '{name}' already exists in the store '{store}'.
			/// </summary>
			public static string DuplicateProductName(object name, object store)
			{
				return Resources.ProductStore_DuplicateProductName.FormatWith(new 
				{
					name = name,
					store = store,
				});
			}
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	No product schema with identifier '{id}' was found.
			/// </summary>
			public static string ProductSchemaNotFound(object id)
			{
				return Resources.ProductStore_ProductSchemaNotFound.FormatWith(new 
				{
					id = id,
				});
			}
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Cannot rename product '{oldName}' to '{newName}' because the store '{store}' already has a product with the same name.
			/// </summary>
			public static string RenamedDuplicateProduct(object oldName, object newName, object store)
			{
				return Resources.ProductStore_RenamedDuplicateProduct.FormatWith(new 
				{
					oldName = oldName,
					newName = newName,
					store = store,
				});
			}
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	No toolkit with identifier '{id}' was found.
			/// </summary>
			public static string ToolkitNotFound(object id)
			{
				return Resources.ProductStore_ToolkitNotFound.FormatWith(new 
				{
					id = id,
				});
			}
		}
		
		///	<summary>
		///	Provides access to string resources.
		///	</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("netfx-System.Strings", "1.0.0.0")]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public static partial class SchemaBuilder
		{
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Product model can only contain interfaces and primitive types. Invalid type '{type}'.
			/// </summary>
			public static string ModelMustBeInterfaces(object type)
			{
				return Resources.SchemaBuilder_ModelMustBeInterfaces.FormatWith(new 
				{
					type = type,
				});
			}
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Reserved property 'Name' must be of type string.
			/// </summary>
			public static string NamePropertyMustBeString { get { return Resources.SchemaBuilder_NamePropertyMustBeString; } }
		}
		
		///	<summary>
		///	Provides access to string resources.
		///	</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("netfx-System.Strings", "1.0.0.0")]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public static partial class SmartCast
		{
			/// <summary>
			/// Looks up a localized string similar to: 
			///	The specified instance is not a product interface.
			/// </summary>
			public static string InstanceNotProductInterface { get { return Resources.SmartCast_InstanceNotProductInterface; } }
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Smart casting only works with interfaces.
			/// </summary>
			public static string InterfaceRequired { get { return Resources.SmartCast_InterfaceRequired; } }
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Only property get/set calls can be performed on a product interface.
			/// </summary>
			public static string NotPropertyAccess { get { return Resources.SmartCast_NotPropertyAccess; } }
		
			/// <summary>
			/// Looks up a localized string similar to: 
			///	Smart casting requires the given component '{component}' to have an associated schema.
			/// </summary>
			public static string SchemaRequired(object component)
			{
				return Resources.SmartCast_SchemaRequired.FormatWith(new 
				{
					component = component,
				});
			}
		}
		
		///	<summary>
		///	Provides access to string resources.
		///	</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("netfx-System.Strings", "1.0.0.0")]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public static partial class ToolkitCatalog
		{
			/// <summary>
			/// Looks up a localized string similar to: 
			///	A toolkit with identifier '{id}' already exists in the catalog.
			/// </summary>
			public static string DuplicateSchema(object id)
			{
				return Resources.ToolkitCatalog_DuplicateSchema.FormatWith(new 
				{
					id = id,
				});
			}
		}
	}
}

