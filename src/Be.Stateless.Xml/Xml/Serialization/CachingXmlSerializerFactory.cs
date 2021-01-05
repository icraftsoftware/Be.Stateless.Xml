#region Copyright & License

// Copyright © 2012 - 2020 François Chabot
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Be.Stateless.Xml.Serialization
{
	/// <summary>
	/// Cache dynamically generated assemblies.
	/// </summary>
	/// <remarks>
	/// <para>
	/// To increase performance, the XML serialization infrastructure dynamically generates assemblies to serialize and
	/// deserialize specified types. The infrastructure finds and reuses those assemblies. This behavior occurs only when using
	/// the following constructors:
	/// <list type="bullet">
	/// <item><see cref="XmlSerializer(Type)"/></item>
	/// <item><see cref="XmlSerializer(Type,string)"/></item>
	/// </list>
	/// </para>
	/// <para>
	/// If you use any of the other constructors, multiple versions of the same assembly are generated and never unloaded, which
	/// results in a memory leak and poor performance.
	/// </para>
	/// <para>
	/// This class takes care of caching the dynamically generated assemblies should the serialization infrastructure not do it.
	/// </para>
	/// </remarks>
	/// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/system.xml.serialization.xmlserializer#dynamically-generated-assemblies">Dynamically Generated Assemblies</seealso>
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
	public static class CachingXmlSerializerFactory
	{
		public static XmlSerializer Create(Type type)
		{
			return new XmlSerializer(type);
		}

		public static XmlSerializer Create(Type type, XmlRootAttribute root)
		{
			return CachedCreate(type, () => new XmlSerializer(type, root));
		}

		public static XmlSerializer Create(Type type, XmlAttributeOverrides overrides)
		{
			return CachedCreate(type, () => new XmlSerializer(type, overrides));
		}

		[SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
		[SuppressMessage("ReSharper", "InvertIf")]
		private static XmlSerializer CachedCreate(Type type, Func<XmlSerializer> factory)
		{
			if (!_cache.TryGetValue(type, out var serializer))
			{
				lock (_cache)
				{
					if (!_cache.TryGetValue(type, out serializer))
					{
						serializer = factory();
						_cache.Add(type, serializer);
					}
				}
			}
			return serializer;
		}

		private static readonly Dictionary<Type, XmlSerializer> _cache = new Dictionary<Type, XmlSerializer>();
	}
}
